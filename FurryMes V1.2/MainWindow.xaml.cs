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

namespace FurryMes_V1._2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _stopLoop;
        public MainWindow()
        {
            InitializeComponent();
            FileService.createDirectory();
            FileService.createDataFiles();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            _stopLoop = false;
            while (!_stopLoop)
            {
                await Task.Run(() => NetworkService.startPoint());
            }
            
        }

        private async void  Button_Click_1(object sender, RoutedEventArgs e)
        {
            string subjectText = MessageTextSubject.Text;
            string messageText = MessageTextMain.Text;
            await Task.Run(() => NetworkService.SendMessage(subjectText, messageText));

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            NetworkService.flag = false;
            _stopLoop = true;
            NetworkService.driver.Quit();
        }
    }
}
