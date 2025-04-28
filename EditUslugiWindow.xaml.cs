using System;
using System.Windows;

namespace VecherVKomnatu
{
    public partial class EditUslugiWindow : Window
    {
        public Uslugi Uslugi { get; private set; }

        public EditUslugiWindow()
        {
            InitializeComponent();
            Uslugi = new Uslugi();
            DataContext = this;
        }

        public EditUslugiWindow(Uslugi uslugi) : this()
        {
            Uslugi = uslugi;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Uslugi.name))
            {
                MessageBox.Show("Введите наименование услуги", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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