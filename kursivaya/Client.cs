using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace kursivaya
{
    class Client
    {
        public string ServerIPAddress { get; set;}
        public string ServerPort { get; set; }
        private IPEndPoint ipPoint;
        private Socket socket;


        public Client(string ip, string port)
        {
            ServerIPAddress = ip;
            ServerPort = port;
        }

        public string Connect()
        {
            try
            {
                ipPoint = new IPEndPoint(IPAddress.Parse(ServerIPAddress), Convert.ToInt32(ServerPort));
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(ipPoint);
                return "Подключено";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        public void SendMessage(string message)//, User user)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            socket.Send(data);
        }
    }
}
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