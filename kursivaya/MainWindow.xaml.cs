using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace kursivaya
{

    public partial class MainWindow : Window
    {
        private Thread receiveThread;                     //Поток, запускаемый для прослушки на наличие входящих сообщений
        private RC4 rc4 = new RC4();                      //Объект класса, используемый для шифрования/дешифрования
        private User user;                                //Объект класса, в котором содержится информация о пользователе
        private TcpClient client;                         //Клиент, который подключается к серверу по протоколу TCP
        private NetworkStream stream;                     //Поток для обмена информацией с сервером
        
        public string IP { get; private set; }            //IP-адрес сервера
        public string Port { get; private set; }          //Порт для подключения к серверу
        
        public bool IsUserAuthorized { get; private set; }//Флаг авторизованности пользователя
        public bool IsConnected { get; private set; }     //Флаг подключения к серверу
        


        /*
         * При первой загрузке формы устанавливается адрес сервера и порт по умолчанию(localhost)
         * Устанавливаются значения флагов, означающих, что клиент не авторизован и не подключен к серверу
         */
        public MainWindow()
        {
            InitializeComponent();
            IP = "127.0.0.1";
            Port = "8005";
            IsUserAuthorized = false;
            IsConnected = false;
        }

        /*
         * Устанавливает значение IP-адреса сервера и его порта 
         * Флаг подключения изменяется на false, так как пользователь только сменил адрес сервера, но не подключался к нему
         * Вызывается из AuthorizationWindow
         */
        public void SetServerAddress(string ip, string port)
        {
            IP = ip;
            Port = port;
            IsConnected = false;
        }

        /*
         * Устанавливается логнин пользователя
         * Вызывается из AuthorizationWindow
         */
        public void SetUser(string login)
        {
            user = new User(login);
        }

        /*
         * Обработка нажатия на кнопку "Настройки"
         * Вызывает форму SettingsWindow
         */
        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingswin = new SettingsWindow(this);
            settingswin.Show();
        }

        /*
        * Обработка нажатия на кнопку "Информация"
        * Вызывает форму InfoWindow
        */
        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            InfoWindow infowin = new InfoWindow();
            infowin.Show();
        }

        /*
        * Обработка нажатия на кнопку "Подключиться/Отключиться"
        */
        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {

            AuthorizationWindow authwin = new AuthorizationWindow(this);
            //если пользователь не подключен к серверу, выполняется подключение по протоколу TCP
            if (!IsConnected)
            {
                //если пользователь не авторизован, вызываем окно AuthorizationWindow
                //в случае закрытия окна авторизации без успешной авторизации, прекращаем процесс подключения
                if (!IsUserAuthorized)
                {
                    if (authwin.ShowDialog().Value == false)
                        return;
                    IsUserAuthorized = true;
                }


                client = new TcpClient();//создаем нового Tcp клиента

                try
                {
                    client.Connect(IP, Convert.ToInt32(Port));//подключаем к серверу по указанному IP-адресу и порту
                    stream = client.GetStream();//получаем поток, по которому происходит обмен информацией между сервером и клиентом

                    //отправляем серверу логин пользователя
                    string message = user.Login;
                    byte[] data = Encoding.Unicode.GetBytes(message);
                    stream.Write(data, 0, data.Length);

                    //создаем новый поток для постоянного прослушивания потока на наличие сообщений
                    receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                    receiveThread.Start();
                    messagesListBox.Items.Add("Добро пожаловать, " + user.Login);//на экран приветствующая информация

                }
                catch (Exception ex)
                {
                    messagesListBox.Items.Add(ex);//если что-то пошло не так, выводим информацию об ошибке на экран
                }

                //если все успешно изменяем кнопку "Подключиться" на "Отключиться", значения флага подключения устанавливается в true
                connectButton.Style = (Style)connectButton.FindResource("disconnectButtonStyle");
                IsConnected = true;
            }
            //пользователь нажал кнопку отключиться
            else
            {
                //вызываем функцию отключения
                Disconnect();
                //если все успешно изменяем кнопку "Отключиться" на "Подключиться", значения флага подключения устанавливается в false
                connectButton.Style = (Style)connectButton.FindResource("connectButton");
                IsConnected = false;
            }
        }

        /*
        * Обработка ввода текста в поле MessageTextBox(поле ввода отправляемого сообщения)
        */
        private void MessageTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            //при нажатии Enter отправляется сообщение
            if (e.Key == Key.Enter)
            {
                //невозможно отправить пустое сообщение
                if (messageTextBox.Text == "")
                    return;

                //выводится сообщение о том, что пользователь не подключен
                if (!IsConnected)
                {
                    MessageBox.Show("Вы не подключены", "Сообщение не отправлено", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                //выводится сообщение о том, что пользователь не авторизован
                if (!IsUserAuthorized)
                {
                    MessageBox.Show("Вы не авторизованы", "Сообщение не отправлено", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                SendMessage(messageTextBox.Text);//функция для отправки сообщения на сервер

                int test = messageTextBox.Text.Length;
                messageTextBox.Text = "";
            }
        }

        /*
         * Функция для шифрования и отправки сообщения на сервер
         * В качестве аргумента - отправляемое сообщение
         * Отправка осуществляется записью массива из байтов сообщения в поток
         */
        private void SendMessage(string message)
        {
            message = String.Format("{0}\n{1}: {2}", DateTime.Now.ToString("hh:mm:ss"), user.Login, message);//к сообщению добавляется текущее время и логин пользователя
            byte[] data = Encoding.Unicode.GetBytes(message);

            data = rc4.Encrypt(data);
            stream.Write(data, 0, data.Length);

            //отправленное сообщение добавляется в поле со всеми сообщениями
            messagesListBox.Items.Add(message);
            messagesListBox.SelectedIndex = messagesListBox.Items.Count - 1;
            messagesListBox.ScrollIntoView(messagesListBox.SelectedItem);
        }

        /*
         * Функция для получения сообщений от сервера и их дешифровке при необходимости
         * Если поток не пуст, получаем из него информацию
         * Если сообщение начинается с "<" и заканчивается ">", то это сообщение-ключ
         * Если сообщение начинается с "<server>", то это сообщение-сообщение от сервера, а не от другого клиента, его дешифровать не нужно
         */
        private void ReceiveMessage()
        {
            while (true)
            {
                try
                {
                    byte[] data = new byte[64];
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    string message = builder.ToString();

                    if (message[0] == '<' && message[message.Length - 1] == '>')
                    {
                        message = message.Remove(0, 1);
                        message = message.Remove(message.Length - 1, 1);
                        rc4.SetKey(Encoding.Unicode.GetBytes(message));
                        continue;
                    }
                    if (message.Substring(0, 8) == "<server>")
                    {
                        Dispatcher.BeginInvoke((Action)(delegate { messagesListBox.Items.Add(message.Substring(8, message.Length - 8)); }));
                        Dispatcher.BeginInvoke((Action)(delegate { messagesListBox.SelectedIndex = messagesListBox.Items.Count - 1; }));
                        Dispatcher.BeginInvoke((Action)(delegate { messagesListBox.ScrollIntoView(messagesListBox.SelectedItem); }));
                        continue;
                    }

                    byte[] messageBytes = Encoding.Unicode.GetBytes(message);
                    messageBytes = rc4.Decrypt(messageBytes);
                    message = Encoding.Unicode.GetString(messageBytes);


                    Dispatcher.BeginInvoke((Action)(delegate { messagesListBox.Items.Add(message); }));
                    Dispatcher.BeginInvoke((Action)(delegate { messagesListBox.SelectedIndex = messagesListBox.Items.Count - 1; }));
                    Dispatcher.BeginInvoke((Action)(delegate { messagesListBox.ScrollIntoView(messagesListBox.SelectedItem); }));
                }
                catch
                {
                    Dispatcher.BeginInvoke((Action)(delegate { this.messagesListBox.Items.Add("Соединение прекращено"); }));
                    Dispatcher.BeginInvoke((Action)(delegate { messagesListBox.SelectedIndex = messagesListBox.Items.Count - 1; }));
                    Dispatcher.BeginInvoke((Action)(delegate { messagesListBox.ScrollIntoView(messagesListBox.SelectedItem); }));
                    Disconnect();
                    break;
                }
            }
        }

        /*
         * Функция для закрытия соединения с сервером путем отключения потока и клиента
         */
        private void Disconnect()
        {
            if (stream != null)
                stream.Close();//отключение потока
            if (client != null)
                client.Close();//отключение клиента
        }

    }
}
