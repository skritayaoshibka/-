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

namespace kursivaya
{
    /// <summary>
    /// Логика взаимодействия для Info.xaml
    /// </summary>
    public partial class InfoWindow : Window
    {
        public InfoWindow()
        {
            InitializeComponent();
        }

        private void HowToUse_TextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            HowToUse_TextBlock.TextDecorations = TextDecorations.Underline;
        }

        private void HowToUse_TextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            HowToUse_TextBlock.TextDecorations = null;
        }

        private void Algorithms_TextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            Algorithms_TextBlock.TextDecorations = TextDecorations.Underline;
        }

        private void Algorithms_TextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            Algorithms_TextBlock.TextDecorations = null;
        }

        private void RC4_TextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            RC4_TextBlock.TextDecorations = TextDecorations.Underline;
        }

        private void RC4_TextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            RC4_TextBlock.TextDecorations = null;
        }

        private void A5_TextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            A5_TextBlock.TextDecorations = TextDecorations.Underline;
        }

        private void A5_TextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            A5_TextBlock.TextDecorations = null;
        }

        private void About_TextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            About_TextBlock.TextDecorations = TextDecorations.Underline;
        }

        private void About_TextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            About_TextBlock.TextDecorations = null;
        }

        private void HowToUse_TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Info_TextBlock.Text = "                             Как пользоваться программой?\n\n   Для того, чтобы отправлять сообщения другим пользователям сообщения необходимо:\n" +
                "   1.   Настроить IP-адрес и порт сервера. По умолчанию IP-адрес имеет значение 127.0.0.1, а порт-8005. Если у сервера, к которому вы хотите подключиться, другой адрес или порт" +
                "необходимо нажать кнопку с шестеренкой(кнопка для открытия окна настроек), в открывшемся окне ввести нужный IP-адрес и порт и нажать \"Применить\"," +
                "также в открывшемся окне можно нажать кнопку \"localhost\" для установки настроек по умолчанию.\n" +
                "   2.   Нажать кнопку \"Подключиться\". Появится окно авторизации, в нем необходимо ввести свой логин, который будет отображаться в чате, и нажать кнопку \"Авторизоваться\", окно авторизации автоматически закроется. Если подключение прошло успешно, в окне программы вы увидете сообщение \"Добро пожаловать ...\". Теперь вы готовы отправлять сообщения.\n" +
                "   3.   Ввести сообщение в поле для сообщений, для отправки нажать клавишу Enter.\n" +
                "   4.   Если вам необходимо выйти из чата нажмите кнопку \"Отключиться\", появится сообщение \"Соединение прекращено\"";
        }

        private void About_TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Info_TextBlock.Text = "                                    О программе\n\n Программу разработали:\n Михалев Дмитрий, студент группы ПТ-30917\n Евгений Екимов, студент группы ПТ-30917.\n\n\n                                        2019 год";
        }

        private void Algorithms_TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Info_TextBlock.Text = "                                  Алгоритмы шифрования\n   Все передающиеся сообщения зашифрованы.\n" +
                "   Для шифрования использовались два поточных шифра: RC4 и A5/1.\n" +
                "   Шифрование происходит на стороне пользователя, отправляющего сообщение, расшифрование происходит на стороне получателя, т.е. все сообщения, приходящие от отправителя на сервер зашифрованы.";
        }

        private void RC4_TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Info_TextBlock.Text = "                                             RC4\n   RC4-потоковый шифр, разработанный компанией RSA Security, широко применяющийся в различных системах защиты информации в компьютерных сетях (протоколы SSl, TLS)\n" +
                "   Алгоритм шифрования состоит из трех основных этапов:\n" +
                "   1.   Инициализация S-блока\n" +
                "   2.   Генерация псевдослучайного байта K\n" +
                "   3.   Сложение по модулю 2 байта K с байтом открытого текста\n" +
                "   Дешифрование происходит по точно такому же алгоритму\n\n\n";
            Run link = new Run("Подробнее");
            Hyperlink hyperlink = new Hyperlink(link);
            hyperlink.NavigateUri = new Uri("https://ru.wikipedia.org/wiki/RC4");
            hyperlink.RequestNavigate += LinkOnRequestNavigate;
            Info_TextBlock.Inlines.Add(hyperlink);

        }


        private void LinkOnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.ToString());
        }
    }
}
