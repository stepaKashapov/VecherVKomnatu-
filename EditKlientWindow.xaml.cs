using System;
using System.Windows;

namespace VecherVKomnatu
{
    public partial class EditKlientWindow : Window
    {
        public Klient Klient { get; private set; }

        public EditKlientWindow()
        {
            InitializeComponent();
            Klient = new Klient
            {
                dateR = DateTime.Today // Устанавливаем текущую дату по умолчанию
            };
            DataContext = this;
        }

        public EditKlientWindow(Klient klient) : this()
        {
            if (klient != null)
            {
                Klient.FIO = klient.FIO;
                Klient.tel = klient.tel;
                Klient.dateR = klient.dateR;
                Klient.seria = klient.seria;
                Klient.nomer = klient.nomer;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Klient.FIO))
            {
                MessageBox.Show("Введите ФИО клиента", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DialogResult = true;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}