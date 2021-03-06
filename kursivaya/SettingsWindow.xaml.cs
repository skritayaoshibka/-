﻿using System;
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
        private MainWindow mainWindow;//Объект главного окна, вызвавшего это окно авторизации

        public SettingsWindow()
        {
            InitializeComponent();
        }

        /*
         * Перегруженный конструктор, который сохраняет информацию о главном окне, открышем это окно
         */
        public SettingsWindow(MainWindow mainwin)
        {
            InitializeComponent();
            mainWindow = mainwin;
        }

        /*
         * Функция, обрабатывающая нажатие кнопки "Применить"
         * Из полей ввода IP-адреса и порта считывается информация
         * Вызывается функция SetServerAddress главного окна, которая устанавливает IP-адрес и порт, к которому будет подключен клиент
         */
        private void acceptButton_Click(object sender, RoutedEventArgs e)
        {
            if (IPmaskedTextBox.Text != "   .   .   .   " && PortmaskedTextBox.Text != "     ")
            {
                string ip = IPmaskedTextBox.Text;
                ip = ip.Replace(" ", "");
                string port = PortmaskedTextBox.Text;
                mainWindow.SetServerAddress(ip, port);
                MessageBox.Show("Настройки успешно сохранены", "Настройки", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show("Заполните поля", "Настройки", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /*
        * Функция, обрабатывающая нажатие кнопки "localhost"
        * Значения IP-адреса и порта устанавливаются по умолчанию для localhost(127.0.0.1:8005)
        */
        private void defaultSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            IPmaskedTextBox.Text = "127. 0 . 0 . 1 ";
            PortmaskedTextBox.Text = "8005";
        }

        /*
        * Функция, обрабатывающая нажатие кнопки "Выход"
        * Закрывает окно, не внося никаких изменений
        */
        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
