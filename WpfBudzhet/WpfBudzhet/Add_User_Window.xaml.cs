using Budzet.Domain;
using Budzet.EFData;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using UserBudzet.Application.ViewModels;
using Path = System.IO.Path;

namespace WpfBudzhet
{
   
    /// <summary>
    /// Логика взаимодействия для Add_User_Window.xaml
    /// </summary>
    public partial class Add_User_Window : Window
    {
        private readonly ObservableCollection<UserVM> _users;
        private EFDataContext _context = new EFDataContext();
        private readonly UserViewModel us = new UserViewModel();
        public string FileName { get; set; }
        private bool _debit_kredit { get; set; }
        public Add_User_Window(ObservableCollection<UserVM> users)
        {
            InitializeComponent();
            _users = users;
            us.EnableValidation = true;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rbtn = (RadioButton)sender;
            _debit_kredit = Convert.ToBoolean(rbtn.Tag);
        }

        private void btnSelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                FileName = openFileDialog.FileName;
            }
        }

        private void Save_User_Click(object sender, RoutedEventArgs e)
        {
            us.EnableValidation = true;
            if (string.IsNullOrEmpty(us.Error))
            {
                var extension = Path.GetExtension(FileName);
            var imageName = Path.GetRandomFileName() + extension;
            var dir = Directory.GetCurrentDirectory();
            var saveDir = Path.Combine(dir, "images");
            if (!Directory.Exists(saveDir))
                Directory.CreateDirectory(saveDir);
            var fileSave = Path.Combine(saveDir, imageName);
            File.Copy(FileName, fileSave);

            var user =
                    new AppUser
                    {
                        Name = tbName.Text,
                        DebitKredit = _debit_kredit,
                        Tranіaction = (DateTime)dpDate.SelectedDate,
                        Details = tbDetails.Text,
                        Image = fileSave
                    };
            user.AppTranzactionPrices = new List<AppTranzactionPrice>
            {
                new AppTranzactionPrice
                {
                    UserId=user.Id,
                            DateCreate=DateTime.Now,
                          Price=decimal.Parse(this.tbPrice.Text)
                }
            };
            _context.Add(user);
            _context.SaveChanges();

            _users.Add(new UserVM
            {
                Id = user.Id,
                Name = user.Name,
                Tranіaction = user.Tranіaction,
                Details = user.Details,
                Image = user.Image

            });
            this.Close();
            }
            else
                MessageBox.Show(us.Error);
        }
    }
}
