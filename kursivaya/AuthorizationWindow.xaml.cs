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
    /// Логика взаимодействия для AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        private MainWindow mainwin;//Объект главного окна, вызвавшего это окно авторизации

        public AuthorizationWindow()
        {
            InitializeComponent();
        }

        /*
         * Перегруженный конструктор, который сохраняет информацию о главном окне, открышем это окно
         */
        public AuthorizationWindow(MainWindow main)
        {
            InitializeComponent();
            mainwin = main;
        }

        /*
         * Функция, обрабатывающая нажатие кнопки "Авторизоваться"
         * Если поле ввода логина не пустое, вызывается функция главного окна, записывающая логин
         * Открывается окно, вызвавшее текущее
         */
        private void AuthorizationButton_Click(object sender, RoutedEventArgs e)
        {
            if (loginTextBox.Text != "")
            {
                mainwin.SetUser(loginTextBox.Text);
                mainwin.Show();
                this.DialogResult = true;
            }
        }
    }
}
