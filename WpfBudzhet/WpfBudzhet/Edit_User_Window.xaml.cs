using Budzet.Domain;
using Budzet.EFData;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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
    /// Логика взаимодействия для Edit_User_Window.xaml
    /// </summary>
    public partial class Edit_User_Window : Window
    {
        private readonly ObservableCollection<UserVM> _users;
        private EFDataContext _context = new EFDataContext();
        private readonly UserViewModel us = new UserViewModel();
        
        public static string New_FileName { get; set; }
        //public string Foto_dell { get; set; } = null;

        public Edit_User_Window(System.Collections.ObjectModel.ObservableCollection<UserBudzet.Application.UserVM> users)
        {
            us.EnabledValidation = true;
            DataContext = us;
            InitializeComponent();
            _users = users;
        }

        

        private void btnSelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                New_FileName = openFileDialog.FileName;
            }
        }

        private void btnSaveChangs_Click(object sender, RoutedEventArgs e)
        {
            
            var usChangNew = _context.Users.SingleOrDefault(p => p.Id == MainWindow.chang_User);

            if (!string.IsNullOrEmpty(this.tbName.Text))
            {
                usChangNew.Name = tbName.Text;
                
            }
            if (!string.IsNullOrEmpty(this.tbDetails.Text))
            {
                usChangNew.Details = tbDetails.Text;
                
            }

            if (!string.IsNullOrEmpty(this.tbPrice.Text))
            {
                usChangNew.AppTranzactionPrices = new List<AppTranzactionPrice>
                    {
                      new AppTranzactionPrice
                      {
                          Id=usChangNew.Id,
                          DateCreate=DateTime.Now,
                          Price=decimal.Parse(this.tbPrice.Text)
                      }
                };
                _context.Add(usChangNew);

            }
            if (!string.IsNullOrEmpty(New_FileName))
            {
                var extension = Path.GetExtension(New_FileName);
                var imageName = Path.GetRandomFileName() + extension;
                var dir = Directory.GetCurrentDirectory();
                var saveDir = Path.Combine(dir, "images");
                if (!Directory.Exists(saveDir))
                    Directory.CreateDirectory(saveDir);
                
                var fileSave = Path.Combine(saveDir, imageName);
                File.Copy(New_FileName, fileSave);
                usChangNew.Image = fileSave;
            }


            us.EnableValidation = true;

            if (string.IsNullOrEmpty(us.Error))
            {
                _context.SaveChanges();
                MessageBox.Show("Ура, пошло");

            }

            else
                MessageBox.Show(us.Error);



            this.Close();

        }
    }
}
