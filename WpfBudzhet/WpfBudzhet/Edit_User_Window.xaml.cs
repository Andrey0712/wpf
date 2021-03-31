using Budzet.EFData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UserBudzet.Application;

namespace WpfBudzhet
{
    /// <summary>
    /// Логика взаимодействия для Edit_User_Window.xaml
    /// </summary>
    public partial class Edit_User_Window : Window
    {
        private readonly ObservableCollection<UserVM> _users;
        private EFDataContext _context = new EFDataContext();
        public Edit_User_Window(System.Collections.ObjectModel.ObservableCollection<UserBudzet.Application.UserVM> users)
        {
            InitializeComponent();
            _users = users;
        }
    }
}
