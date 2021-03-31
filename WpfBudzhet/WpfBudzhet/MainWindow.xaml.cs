using Budzet.EFData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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

namespace WpfBudzhet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static int chang_User;
        private ObservableCollection<UserVM> _users = new ObservableCollection<UserVM>();
        private EFDataContext _context = new EFDataContext();
        public MainWindow()
        {
            InitializeComponent();
            Seed.SeedUser(_context);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var list = _context.Users.Select(x => new UserVM()
            {
                Id=x.Id,
                Name = x.Name,
                Tranіaction = x.Tranіaction,
                Details = x.Details,
                Image = x.Image

            }).ToList();
            _users = new ObservableCollection<UserVM>(list);
            dgSimple.ItemsSource = _users;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Add_User_Window addUser = new Add_User_Window(this._users);
            addUser.Show();
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

                }
            }
        }
    }
}
