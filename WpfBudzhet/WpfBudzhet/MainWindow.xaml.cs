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
        public static int chang_User;
        private ObservableCollection<UserVM> _users = new ObservableCollection<UserVM>();
        private EFDataContext _context = new EFDataContext();
        private Thread thread1 = null;
        private BackgroundWorker worker = null;
        public static int newUsers;
        
        public MainWindow()
        {
            InitializeComponent();
            Seed.SeedUser(_context);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        { 
            Debug.WriteLine("Thread id : {0}", Thread.CurrentThread.ManagedThreadId);
            var list = _context.Users.Select(x => new UserVM()
            {
                
            Id =x.Id,
                Name = x.Name,
                Tranіaction = x.Tranіaction,
                Details = x.Details,
                Image = x.Image,
                Price = x.AppTranzactionPrices
                        .OrderByDescending(x => x.DateCreate)
                        .FirstOrDefault().Price
            }).ToList();
            _users = new ObservableCollection<UserVM>(list);
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
        /// ВАРИАНТ РАБОТЫ С Thread с https://habr.com/ru/sandbox/38787/ не коректно обновляет ProgressBar но класно добавляет юзеров 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        #region Thread
        private void btnAddRange_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Thread id : {0}", Thread.CurrentThread.ManagedThreadId);
            newUsers = int.Parse(txtNewUsers.Text);
            btnAddRange.IsEnabled = false;
            thread1 = new Thread(ShowMessage);
            thread1.Start();
        }
        private void btnCancelAddRange_Click(object sender, RoutedEventArgs e)
        {
            btnAddRange.IsEnabled = true;

            thread1.Abort();


        }


        private void ShowMessage()
        {

            Dispatcher.Invoke(new Action(() =>
                    {
                        btnAddRange.IsEnabled = false;

                    }));

            IUserService userService = new UserService();
            userService.EventInsertItem += UpdateUIAsync;
            //userService.InsertUser(int.Parse(txtNewUsers.Text));
            userService.InsertUser(newUsers);


        }
        void UpdateUIAsync(int i)
        {
            UpdateProgressBarDelegate updProgress = new UpdateProgressBarDelegate(pbZagruzka.SetValue);
            double value = 0;
            Application.Current.Dispatcher.Invoke(//диспетчер обновляет UI путем
                    new Action(() =>//Создается делегат что указывает на анонимный метод
                    {
                        btnAddRange.Content = $"{i}";
                        Debug.WriteLine("Thread id : {0}", Thread.CurrentThread.ManagedThreadId);

                    }));
            Dispatcher.Invoke(updProgress, new object[] { ProgressBar.ValueProperty, ++value });

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
