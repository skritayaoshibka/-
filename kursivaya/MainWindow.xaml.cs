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
            IP = "235.5.5.11";
            Port = "8001";
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

            //Thread myThread = new Thread(Connect);
            //myThread.Start(); // запускаем поток
            client = new Client(IP, Port, this, user.Login);
            test.Content = client.Connect();
            client.SendMessage("hello");
        }

        //private void Connect()
        //{
           // client = new Client(IP, Port, this);
            //test.Content = client.Connect();
            //client.SendMessage("hello");
        //}

        public void ShowNewMessage(string message)
        {
            Dispatcher.BeginInvoke((Action)(delegate { this.messagesListBox.Items.Add(message); }));
            Dispatcher.BeginInvoke((Action)(delegate { this.UpdateLayout(); }));
        }

        private void MessageTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key ==Key.Enter)
            {
                if (messageTextBox.Text.Length <=200)
                client.SendMessage(messageTextBox.Text);
                else
                {
                    int count = 0;
                    while (true)
                    {
                        string text;
                        int count2 = messageTextBox.Text.Length - count;
                        if (count2 >= 200)
                        {
                            text = messageTextBox.Text.Substring(count, 200);
                            count += 200;
                        }
                        else
                        {
                            text = messageTextBox.Text.Substring(count, count2);
                            break;
                        }

                        client.SendMessage(text);

                    }
                }
                messageTextBox.Text = "";
            }
        }
    }
}
