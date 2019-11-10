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
using System.Windows.Shapes;

namespace kursivaya
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private MainWindow mainWindow;

        public SettingsWindow()
        {
            InitializeComponent();
        }

        public SettingsWindow(MainWindow mainwin)
        {
            InitializeComponent();
            mainWindow = mainwin;
        }


        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            string ip = IPmaskedTextBox.Text;
            ip = ip.Replace(" ", "");
            string port = PortmaskedTextBox.Text;
            mainWindow.SetServerAddress(ip, port);
            //test = test.Replace(',', '.');
            //MessageBox.Show(test);
        }

        private void defaultSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            IPmaskedTextBox.Text = "127. 0 . 0 . 1 ";
            PortmaskedTextBox.Text = "8005";
        }
    }
}
