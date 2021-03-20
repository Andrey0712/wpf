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

namespace TryCatch
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
        /// <summary>
        ///     Подія яка оброблює клік кнопки
        /// </summary>
        /// <param name="sender">Кнопка яка згенерувала подію</param>
        /// <param name="e">Параметри</param>
        private void btnException_Click(object sender, RoutedEventArgs e)
        {
            //  Ініціалізація нової строки і присвоєння їй NULL
            string s = null;
            try
            {
                //  Виконування операції над елементом який дорівнює null
                //  Вивлети Exception, яка обробиться локальним обробником try... catch..
                s.Trim();
            }
            //  Відловлювання помилки
            catch (Exception ex)
            {
                //  Виведення вікна, яке сповіщає про помилку яка сталася у коді,
                //  використовуючи обєкт помилки. У вікні є лише одна кнопка і тип зображення вікна змінено на
                //  MessageBoxImage.Warning
                MessageBox.Show("A handled exception just occurred: " +
                    ex.Message, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            //  Виконування операції над елементом, якому присвоєно NULL
            s.Trim();
        }
    }
}
