using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace kursivaya
{
    class Client
    {
        /*bool alive = false;
        UdpClient client;
        int Port;
        //string LOCALPORT; // порт для приема сообщений
        //string REMOTEPORT; // порт для отправки сообщений
        const int TTL = 20;
        string IP; // хост для групповой рассылки
        IPAddress groupAddress; // адрес для групповой рассылки
        private MainWindow Mainwindow;
        string userName;

        public Client(string ip, string port, MainWindow mainwin, string username)
        {
            IP = ip;
            groupAddress = IPAddress.Parse(IP);
            Port = Convert.ToInt32(port);
            Mainwindow = mainwin;
            userName = username;
        }

        public string Connect()
        {
            try
            {
                client = new UdpClient(Port);
                // присоединяемся к групповой рассылке
                client.JoinMulticastGroup(groupAddress, TTL);

                // запускаем задачу на прием сообщений
                Task receiveTask = new Task(ReceiveMessages);
                receiveTask.Start();

                // отправляем первое сообщение о входе нового пользователя
                string message ="Пользователь " + userName + " вошел в чат";
                byte[] data = Encoding.Unicode.GetBytes(message);
                client.Send(data, data.Length, IP, Port);

                //loginButton.Enabled = false;
                //logoutButton.Enabled = true;
                //sendButton.Enabled = true;

                return "Подключено";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private void ReceiveMessages()
        {
            alive = true;
            try
            {
                while (alive)
                {
                    IPEndPoint remoteIp = null;
                    byte[] data = client.Receive(ref remoteIp);
                    string message = Encoding.Unicode.GetString(data);
                    Mainwindow.ShowNewMessage(message);
                }
            }
            catch (ObjectDisposedException)
            {
                if (!alive)
                    return;
                throw;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }


        public void SendMessage(string message)
        {
            try
            {
                //DateTime date = new DateTime();
                string fullMessage = String.Format("{0}\n{1}: {2}",DateTime.Now.ToShortTimeString (), userName,message);
                byte[] data = Encoding.Unicode.GetBytes(fullMessage);
                client.Send(data, data.Length, IP, Port);
                //messageTextBox.Clear();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }
        // обработчик нажатия кнопки logoutButton
        //private void logoutButton_Click(object sender, EventArgs e)
        //{
           // ExitChat();
        //}
        // выход из чата
        private void ExitChat()
        {
            string message = userName + " покидает чат";
            byte[] data = Encoding.Unicode.GetBytes(message);
            client.Send(data, data.Length, IP, Port);
            client.DropMulticastGroup(groupAddress);

            alive = false;
            client.Close();

            //loginButton.Enabled = true;
            //logoutButton.Enabled = false;
            //sendButton.Enabled = false;
        }
        // обработчик события закрытия формы
        //private void Form1_FormClosing(object sender, FormClosingEventArgs e)
       // {
            //if (alive)
               // ExitChat();
        //}
    }
    /*public string ServerIPAddress { get; set;}
    public string ServerPort { get; set; }
    private IPEndPoint ipPoint;
    private Socket socket;
    private MainWindow Mainwindow;

    public Client(string ip, string port, MainWindow mainwin)
    {
        ServerIPAddress = ip;
        ServerPort = port;
        Mainwindow = mainwin;
    }



    }
    public void SendMessage(string message)//, User user)
    {
        byte[] data = Encoding.Unicode.GetBytes(message);
        socket.Send(data);
    }

    private void ReceiveMessage()
    {
        while (true)
        {
            byte [] data = new byte[256]; // буфер для ответа
            StringBuilder builder = new StringBuilder();
            int bytes = 0; // количество полученных байт

            do
            {
                bytes = socket.Receive(data, data.Length, 0);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (socket.Available > 0);
            //Console.WriteLine("ответ сервера: " + builder.ToString());
            if (builder.ToString()!="")
                Mainwindow.ShowNewMessage(builder.ToString());

        }
    }*/
    }
}
//}


/*bool alive = false; // будет ли работать поток для приема
UdpClient client;
const int LOCALPORT = 8001; // порт для приема сообщений
const int REMOTEPORT = 8001; // порт для отправки сообщений
const int TTL = 20;
const string HOST = "235.5.5.1"; // хост для групповой рассылки
IPAddress groupAddress; // адрес для групповой рассылки

string userName; // имя пользователя в чате
public Form1()
{
    InitializeComponent();

    loginButton.Enabled = true; // кнопка входа
    logoutButton.Enabled = false; // кнопка выхода
    sendButton.Enabled = false; // кнопка отправки
    chatTextBox.ReadOnly = true; // поле для сообщений

    groupAddress = IPAddress.Parse(HOST);
}
// обработчик нажатия кнопки loginButton
private void loginButton_Click(object sender, EventArgs e)
{
    userName = userNameTextBox.Text;
    userNameTextBox.ReadOnly = true;

    try
    {
        client = new UdpClient(LOCALPORT);
        // присоединяемся к групповой рассылке
        client.JoinMulticastGroup(groupAddress, TTL);

        // запускаем задачу на прием сообщений
        Task receiveTask = new Task(ReceiveMessages);
        receiveTask.Start();

        // отправляем первое сообщение о входе нового пользователя
        string message = userName + " вошел в чат";
        byte[] data = Encoding.Unicode.GetBytes(message);
        client.Send(data, data.Length, HOST, REMOTEPORT);

        loginButton.Enabled = false;
        logoutButton.Enabled = true;
        sendButton.Enabled = true;
    }
    catch (Exception ex)
    {
        MessageBox.Show(ex.Message);
    }
}
// метод приема сообщений
private void ReceiveMessages()
{
    alive = true;
    try
    {
        while (alive)
        {
            IPEndPoint remoteIp = null;
            byte[] data = client.Receive(ref remoteIp);
            string message = Encoding.Unicode.GetString(data);

            // добавляем полученное сообщение в текстовое поле
            this.Invoke(new MethodInvoker(() =>
            {
                string time = DateTime.Now.ToShortTimeString();
                chatTextBox.Text = time + " " + message + "\r\n" + chatTextBox.Text;
            }));
        }
    }
    catch (ObjectDisposedException)
    {
        if (!alive)
            return;
        throw;
    }
    catch (Exception ex)
    {
        MessageBox.Show(ex.Message);
    }
}
// обработчик нажатия кнопки sendButton
private void sendButton_Click(object sender, EventArgs e)
{
    try
    {
        string message = String.Format("{0}: {1}", userName, messageTextBox.Text);
        byte[] data = Encoding.Unicode.GetBytes(message);
        client.Send(data, data.Length, HOST, REMOTEPORT);
        messageTextBox.Clear();
    }
    catch (Exception ex)
    {
        MessageBox.Show(ex.Message);
    }
}
// обработчик нажатия кнопки logoutButton
private void logoutButton_Click(object sender, EventArgs e)
{
    ExitChat();
}
// выход из чата
private void ExitChat()
{
    string message = userName + " покидает чат";
    byte[] data = Encoding.Unicode.GetBytes(message);
    client.Send(data, data.Length, HOST, REMOTEPORT);
    client.DropMulticastGroup(groupAddress);

    alive = false;
    client.Close();

    loginButton.Enabled = true;
    logoutButton.Enabled = false;
    sendButton.Enabled = false;
}
// обработчик события закрытия формы
private void Form1_FormClosing(object sender, FormClosingEventArgs e)
{
    if (alive)
        ExitChat();
}
    } 
    */

/*try
            {
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);

Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
// подключаемся к удаленному хосту
socket.Connect(ipPoint);
                Console.Write("Введите сообщение:");
                string message = Console.ReadLine();
byte[] data = Encoding.Unicode.GetBytes(message);
socket.Send(data);
 
                // получаем ответ
                data = new byte[256]; // буфер для ответа
                StringBuilder builder = new StringBuilder();
int bytes = 0; // количество полученных байт
 
                do
                {
                    bytes = socket.Receive(data, data.Length, 0);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                }
                while (socket.Available > 0);
                Console.WriteLine("ответ сервера: " + builder.ToString());
 
                // закрываем сокет
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.Read();*/
