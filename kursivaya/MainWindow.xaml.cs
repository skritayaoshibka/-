using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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
        private User user;
        public string IP { get; private set; }
        public string Port { get; private set; }
        static TcpClient client;
        static NetworkStream stream;
        public bool IsUserAuthorized { get; private set; }


        public MainWindow()
        {
            InitializeComponent();
            IP = "127.0.0.1";
            Port = "8005";
            IsUserAuthorized = false;
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

            if (!IsUserAuthorized)
            {
                if (authwin.ShowDialog().HasValue == false)
                    return;
                IsUserAuthorized = true;
            }


            client = new TcpClient();
            try
            {
                client.Connect(IP, Convert.ToInt32(Port)); //подключение клиента
                stream = client.GetStream(); // получаем поток

                string message = user.Login;
                byte[] data = Encoding.Unicode.GetBytes(message);
                stream.Write(data, 0, data.Length);

                // запускаем новый поток для получения данных
                Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                receiveThread.Start(); //старт потока
                messagesListBox.Items.Add("Добро пожаловать, " + user.Login);

            }
            catch (Exception ex)
            {
                messagesListBox.Items.Add(ex);
            }
        }

        private void MessageTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (messageTextBox.Text.Length <= 200)
                    SendMessage(messageTextBox.Text);
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
                            SendMessage(text);
                            break;
                        }

                        SendMessage(text);
                    }
                }
                messageTextBox.Text = "";
            }
        }

        // отправка сообщений
        private void SendMessage(string message)
        {
            message = String.Format("{0}\n{1}: {2}", DateTime.Now.ToShortTimeString(), user.Login, message);
            byte[] data = Encoding.Unicode.GetBytes(message);
            stream.Write(data, 0, data.Length);
            messagesListBox.Items.Add(message);
        }

        // получение сообщений
        private void ReceiveMessage()
        {
            while (true)
            {
                try
                {
                    byte[] data = new byte[64]; // буфер для получаемых данных
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    string message = builder.ToString();
                    Dispatcher.BeginInvoke((Action)(delegate { this.messagesListBox.Items.Add(message); }));
                }
                catch
                {
                    Dispatcher.BeginInvoke((Action)(delegate { this.messagesListBox.Items.Add("Соединение прекращено"); }));
                }
            }
        }


        private void Disconnect()
        {
            if (stream != null)
                stream.Close();//отключение потока
            if (client != null)
                client.Close();//отключение клиента
            Dispatcher.BeginInvoke((Action)(delegate { this.Close(); })); //завершение процесса
        }

    }
}
