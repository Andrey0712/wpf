using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using FilePath = System.IO.Path;

namespace Resurs
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            //var directory = FilePath.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //Pict.Source = new BitmapImage(new Uri(FilePath.Combine(directory, "uploads/2015/logo.jpg")));
            //myImage. = new BitmapImage(new Uri("image/2.jpg", UriKind.Relative));
        }

        

        // public object myImage { get; private set; }

        private void btnClickMe_Click(object sender, RoutedEventArgs e)
        {
    //        System.Windows.Resources.StreamResourceInfo res =
    //Application.GetResourceStream(new Uri("image/2.jpg", UriKind.Relative));
            lbResult.Items.Add(Application.Current.FindResource("strApp").ToString());
            lbResult.Items.Add(pnlMain.FindResource("strPanel").ToString());
            lbResult.Items.Add(this.FindResource("strWindow").ToString());
            
        }
    }
}
