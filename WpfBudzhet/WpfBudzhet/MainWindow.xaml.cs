using Budzet.EFData;
using Budzet.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public static int newUsers;
        //bool abort = false;
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
            //int addUsers = int.Parse(txtNewUsers.Text);
            //newUsers = addUsers;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Add_User_Window addUser = new Add_User_Window(this._users);
            addUser.Show();
            Window_Loaded(sender, e);
        }

        private void btnAddRange_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Thread id : {0}", Thread.CurrentThread.ManagedThreadId);
            //ShowMessage();
            btnAddRange.IsEnabled = false;
            thread1 = new Thread(ShowMessage);
            thread1.Start();
        }
        private void btnCancelAddRange_Click(object sender, RoutedEventArgs e)
        { 
            btnAddRange.IsEnabled = true;
            //ShowMessage();
             thread1.Abort();
            //abort = true;
           
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
            userService.InsertUser(100);

            //for (int i = 0; i < 10; i++)
            //{
            //    if (abort)
            //    {
            //        Application.Current.Dispatcher.Invoke(//диспетчер обновляет UI путем
            //        new Action(() =>//Создается делегат что указывает на анонимный метод
            //        {
            //            btnAddRange.IsEnabled = true;

            //        }));

            //        abort = false;
            //        break;
            //    }
            //    Thread.Sleep(200);
            //    Application.Current.Dispatcher.Invoke(//диспетчер обновляет UI путем
            //        new Action(() =>//Создается делегат что указывает на анонимный метод
            //        {
            //            UpdateUIAsync(i+1);
            //        }));

            //}
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

        
    }
}
