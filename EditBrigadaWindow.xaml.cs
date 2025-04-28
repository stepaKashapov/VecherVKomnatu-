using System;
using System.Windows;

namespace VecherVKomnatu
{
    public partial class EditBrigadaWindow : Window
    {
        public Brigada Brigada { get; private set; }

        public EditBrigadaWindow()
        {
            InitializeComponent();
            Brigada = new Brigada();
            DataContext = this;
        }

        public EditBrigadaWindow(Brigada brigada) : this()
        {
            Brigada = brigada;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Brigada.name))
            {
                MessageBox.Show("Введите название бригады", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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