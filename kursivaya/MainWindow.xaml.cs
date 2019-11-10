using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace kursivaya
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string IP { get; private set; }
        public string Port { get; private set; }
        public bool UserAuthorized { get; private set; }
        private Client client;
        private User user;
        

        public MainWindow()
        {
            InitializeComponent();
            IP = "127.0.0.1";
            Port = "8005";
            UserAuthorized = false;
        }

        public void SetServerAddress(string ip, string port)
        {
            IP = ip;
            Port = port;
        }

        public void SetUser(string login)
        {
            user = new User(login);
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingswin = new SettingsWindow(this);
            settingswin.Show();
        }

        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            InfoWindow infowin = new InfoWindow();
            infowin.Show();
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            AuthorizationWindow authwin = new AuthorizationWindow(this);

            if (!UserAuthorized)
                authwin.ShowDialog();
            
            client = new Client(IP, Port);
            test.Content = client.Connect();
            client.SendMessage("hello");
        }
    }
}
