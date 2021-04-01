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
        public static string New_FileName { get; set; }

        public Edit_User_Window(System.Collections.ObjectModel.ObservableCollection<UserBudzet.Application.UserVM> users)
        {
            InitializeComponent();
            _users = users;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            
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
            var us = _context.Users.SingleOrDefault(p => p.Id == MainWindow.chang_User);

            if (!string.IsNullOrEmpty(this.tbName.Text))
            {
                us.Name = tbName.Text;
            }
            if (!string.IsNullOrEmpty(this.tbDetails.Text))
            {
                us.Details = tbDetails.Text;
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
                us.Image = fileSave;
            }
            _context.SaveChanges();

           
            this.Close();

        }
    }
}
