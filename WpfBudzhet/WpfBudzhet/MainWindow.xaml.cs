using Budzet.EFData;
using Budzet.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UserBudzet.Application;
using UserBudzet.Application.Interfaces;

namespace WpfBudzhet
{
    delegate void UpdateProgressBarDelegate(DependencyProperty dp, object value);
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static int chang_User;//id юзера для корекции данных
        private ObservableCollection<UserVM> _users = new ObservableCollection<UserVM>();//обновление ui
        private EFDataContext _context = new EFDataContext();
        //private Thread thread1 = null;
        //private BackgroundWorker worker = null;
        public static int newUsers;//кол-во юзеров для добавления
        private IUserService userService = new UserService();
        ManualResetEvent _mrse = new ManualResetEvent(false);//для приостановки потока(пауза)
        bool i = true;//флаг для паузы
        

        


        public MainWindow()
        {
            InitializeComponent();
            userService.EventInsertItem += UpdateUIAsync;
            //Seed.SeedUser(_context);

            

        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lblInfoStatus.Text = "Подключаємся к БД-----";
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();//подсчет времени инициализация подключения

            await Task.Run(() =>
            {
                _context.Users.Count(); //инициализация подключения
            });

            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            //Debug.WriteLine("Сідер 1 закінчив свою роботу: " + elapsedTime);
            lblCursorPosition.Text = elapsedTime;//время подключения
            lblInfoStatus.Text = "Підключення до БД успішно";

            await Seed.SeedDataAsync(_context);//асинхронно провереям не пустая ли БД и сидим

            stopWatch = new Stopwatch();//замеряем время на загрузку данных
            stopWatch.Start();
            //Debug.WriteLine("Thread id : {0}", Thread.CurrentThread.ManagedThreadId);
            var list = _context.Users.Select(x => new UserVM()//запрос на получение данных
            {
                
            Id =x.Id,
                Name = x.Name,
                Tranіaction = x.Tranіaction,
                Details = x.Details,
                Image = x.Image,
                Price = x.AppTranzactionPrices
                        .OrderByDescending(x => x.DateCreate)
                        .FirstOrDefault().Price
            })
                //.OrderBy(x => x.Name)
                //.Skip(0)
                //.Take(20)
                .ToList();

            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            ts = stopWatch.Elapsed;//общее затраченое время в милисек

            // Format and display the TimeSpan value.
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            //Debug.WriteLine("Сідер 1 закінчив свою роботу: " + elapsedTime);
            lblCursorPosition.Text = elapsedTime;// время на чтение данных
            lblInfoStatus.Text = "Читання даних із БД успішно";
            _users = new ObservableCollection<UserVM>(list);//обновляем юайку
            dgSimple.ItemsSource = _users;
            
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Add_User_Window addUser = new Add_User_Window(this._users);
            addUser.Show();
            Window_Loaded(sender, e);
        }
        private void btnDell_Click(object sender, RoutedEventArgs e)
        {
            if (dgSimple.SelectedItem != null)
            {
                if (dgSimple.SelectedItem is UserVM)
                {
                    var userDell = dgSimple.SelectedItem as UserVM;//Выбираем по строке грида
                    int id = int.Parse((userDell.Id).ToString());
                    var user = _context.Users.SingleOrDefault(c => c.Id == id);
                    _context.Users.Remove(user);
                    _context.SaveChanges();
                }
            }
            Window_Loaded(sender, e);
        }
        private void btnEdt_Click(object sender, RoutedEventArgs e)
        {
            if (dgSimple.SelectedItem != null)
            {
                if (dgSimple.SelectedItem is UserVM)
                {
                    var userDell = dgSimple.SelectedItem as UserVM;//Выбираем по строке грида
                    int id = int.Parse((userDell.Id).ToString());
                    chang_User = id;
                    Edit_User_Window editUser = new Edit_User_Window(this._users);
                    editUser.Show();
                    //if (editUser.ShowDialog() == true)
                    //{
                    //    Window_Loaded(sender, e);
                    //}

                }
            }
        }
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Window_Loaded(sender, e);
           
        }

        /// <summary>
        /// ВАРИАНТ РАБОТЫ С Thread  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        #region Thread
        //private void btnAddRange_Click(object sender, RoutedEventArgs e)
        //{
        //    Debug.WriteLine("Thread id : {0}", Thread.CurrentThread.ManagedThreadId);
        //    newUsers = int.Parse(txtNewUsers.Text);
        //    pbZagruzka.Maximum = 100;
        //    btnAddRange.IsEnabled = false;
        //    thread1 = new Thread(ShowMessage);
        //    thread1.Start();

        //}
        //private void btnCancelAddRange_Click(object sender, RoutedEventArgs e)
        //{
        //    btnAddRange.IsEnabled = true;

        //    thread1.Abort();

        //}


        //private void ShowMessage()
        //{

        //    Dispatcher.Invoke(new Action(() =>
        //            {
        //                btnAddRange.IsEnabled = false;

        //            }));

        //    IUserService userService = new UserService();
        //    userService.EventInsertItem += UpdateUIAsync;
        //    userService.InsertUser(newUsers);
        //    Dispatcher.Invoke(new Action(() =>
        //    {
        //        btnAddRange.IsEnabled = true;
        //        btnAddRange.Content = "Добавить ЮЗЕРОВ";

        //    }));

        //}
        //void UpdateUIAsync(int i)
        //{


        //    Application.Current.Dispatcher.Invoke(//диспетчер обновляет UI путем
        //            new Action(() =>//Создается делегат что указывает на анонимный метод
        //            {
        //                btnAddRange.Content = $"{i}";
        //                pbZagruzka.Value = Convert.ToInt32((double)i * 100 / newUsers);
        //                Debug.WriteLine("Thread id : {0}", Thread.CurrentThread.ManagedThreadId);

        //            }));


        //}
        #endregion

        #region Task
        private async void btnAddRange_Click(object sender, RoutedEventArgs e)
        {
            //using (var transaction = _context.Database.BeginTransaction())
            //{
            //    try
            //    {
                    Debug.WriteLine("Thread id : {0}", Thread.CurrentThread.ManagedThreadId);//в дебагесмотрим какой поток 
                    newUsers = int.Parse(txtNewUsers.Text);//кол-во добавляемых юзеров
                    pbZagruzka.Maximum = 100;// 100% прогресбар
                    btnAddRange.IsEnabled = false;//деактивируем кнопку пока грузим юзеров
                    Resume();//НЕ пауза
                    userService.EventInsertItem += UpdateUIAsync;//подисываемся на обновление UI
                    await userService.InsertUserAsync(newUsers, _mrse);//добавляем юзеров
                    btnAddRange.IsEnabled = true;//активируем кнопку
                    btnAddRange.Content = "добавить ЮЗЕРОВ";//меняем название с бегуших цифр
                //    if (!userService.CanselAsyngMetod)
                //    {
                //        //  Метод, який зберігає усі зміни БД
                //        transaction.Commit();
                //    }

                    
                //}
                //catch (Exception ex)
                //{
                //    transaction.Rollback();
                //}
                //Action action = ShowMessage;
                //Task task = new Task(action);
                //Task task = new Task(()=> ShowMessage());//тоже самое через анонимный метод action
                //task.Start();
                //Task.Run(() => ShowMessage()); //тоже самое через анонимный метод



            //}
        }
        private void ShowMessage()
        {

            Dispatcher.Invoke(new Action(() =>
            {
                btnAddRange.IsEnabled = false;

            }));

            IUserService userService = new UserService();
            userService.EventInsertItem += UpdateUIAsync;
            userService.InsertUser(newUsers, _mrse);
            Dispatcher.Invoke(new Action(() =>
            {
                btnAddRange.IsEnabled = true;
                btnAddRange.Content = "Добавить ЮЗЕРОВ";

            }));

        }
        private void btnCancelAddRange_Click(object sender, RoutedEventArgs e)
        {
            btnAddRange.IsEnabled = true;
            btnAddRange.Content = "добавить ЮЗЕРОВ";
            userService.CanselAsyngMetod = true;//останавливает добавление путем изменения CanselAsyngMetod на true в инсерте
            

        }

        public void Resume() => _mrse.Set();//отжать паузу
        public void Pause() => _mrse.Reset();//пауза
        private void btnPauseAddRange_Click(object sender, RoutedEventArgs e)
        {
            pause_play(ref i);
            
        }

        public void pause_play(ref bool i)
        {
            if (i)//нажать паузу, изменить флаг
            {
                btnPause.Content = " >> ";
                this.Pause();
            }
            else//наоборот
            {
                btnPause.Content = " || ";
                this.Resume();
            }
            i = !i;
        }
        
        void UpdateUIAsync(int i)
        {


            Application.Current.Dispatcher.Invoke(//диспетчер обновляет UI путем
                    new Action(() =>//Создается делегат что указывает на анонимный метод
                    {
                        btnAddRange.Content = $"{i}";
                        pbZagruzka.Value = Convert.ToInt32((double)i * 100 / newUsers);
                        Debug.WriteLine("Thread id : {0}", Thread.CurrentThread.ManagedThreadId);

                    }));


        }
        #endregion


        /// <summary>
        /// ВАРИАНТ РАБОТЫ С BackgroundWorker с https://www.wpf-tutorial.com/ не коректно добавляет юзеров но класно обновляет ProgressBar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        #region BackgroundWorker
        //private void btnAddRange_Click(object sender, RoutedEventArgs e)
        //{
        //    pbZagruzka.Value = 0;
        //    worker = new BackgroundWorker();
        //    newUsers = int.Parse(txtNewUsers.Text);
        //    worker.WorkerReportsProgress = true;
        //    worker.DoWork += worker_DoWork;
        //    worker.ProgressChanged += worker_ProgressChanged;

        //    worker.RunWorkerAsync();
        //}


        //private void btnCancelAddRange_Click(object sender, RoutedEventArgs e)
        //{
        //    if (worker.IsBusy)
        //    {
        //        worker.CancelAsync();
        //        pbZagruzka.Value = 0;
        //    }
        //}
        ////тут чето не то
        //private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        //{
        //    pbZagruzka.Value = e.ProgressPercentage;
        //}

        //private void worker_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    IUserService userService = new UserService();

        //    for (int i = 1; i <= newUsers; i++)
        //    {

        //        if (worker.CancellationPending)
        //        {
        //            e.Cancel = true;
        //            break;
        //        }

        //        userService.InsertUser(i);
        //        (sender as BackgroundWorker).ReportProgress(Convert.ToInt32((double)i * 100 / newUsers));

        //    }
        //}
        #endregion



    }
}
