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
    }
}
