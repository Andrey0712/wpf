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

using System.Threading;
using System.IO;
using System.Drawing;

namespace DataParallelismWithForEach
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CancellationTokenSource cancelToken = new CancellationTokenSource();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            cancelToken.Cancel();
        }
        private void cmdProcess_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() => ProcessFiles());
        }
        private void ProcessFiles()
        {
            // Использовать экземпляр ParallelOptions для хранения CancellationToken.
            ParallelOptions parOpts = new ParallelOptions();
            parOpts.CancellationToken = cancelToken.Token;
            parOpts.MaxDegreeOfParallelism = System.Environment.ProcessorCount;
            // Загрузить все файлы *.jpg и создать новый каталог
            // для модифицированных данных.
            string[] files = Directory.GetFiles(@".\TestPictures", "*.jpg", SearchOption.AllDirectories);
            string newDir = @".\ModifledPictures";
            Directory.CreateDirectory(newDir);
            try
            {
                // Обработать данные изображений в блокирующей манере.
                //foreach (string currentFile in files)
                // Обработать данные изображений в параллельном режиме!
                Parallel.ForEach(files, currentFile =>
            {
                string filename = System.IO.Path.GetFileName(currentFile);
                using (Bitmap bitmap = new Bitmap(currentFile))
                {
                    bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    bitmap.Save(System.IO.Path.Combine(newDir, filename));
                    // Вывести идентификатор потока, обрабатывающего текущее изображение,
                    //this.Title = $"Processing { filename} on thread { Thread.CurrentThread.ManagedThreadId}";
                    this.Dispatcher.Invoke((Action)delegate
                    {
                        this.Title = $"Processing {filename} on thread { Thread.CurrentThread.ManagedThreadId}";
                    });
                }
            });
                this.Dispatcher.Invoke((Action)delegate
                {
                    this.Title = "Done!"; // Готово!
                });

            }
            catch (OperationCanceledException ex)
            {
                this.Dispatcher.Invoke((Action)delegate
                {
                    this.Title = ex.Message;
                });


            }
        }
    }
}
