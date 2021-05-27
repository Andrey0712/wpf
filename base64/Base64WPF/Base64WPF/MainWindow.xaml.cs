using System;
using System.Collections.Generic;
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

namespace Base64WPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Save_code_Click(object sender, RoutedEventArgs e)
        {
            string Text = tbText.Text;
            var TextBytes = Encoding.UTF8.GetBytes(Text);
            tbRezalt.Text = Convert.ToBase64String(TextBytes).ToString();

            
        }

        private void Save_base64_Click(object sender, RoutedEventArgs e)
        {
            string Text_base64 = tbBase64.Text;
            var TextBytes = Convert.FromBase64String(Text_base64);
            tbRezalt.Text = Encoding.UTF8.GetString(TextBytes);
        }
    }
}
