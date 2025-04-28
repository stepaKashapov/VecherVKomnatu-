using System;
using System.Windows;

namespace VecherVKomnatu
{
    public partial class EditEdIzmerWindow : Window
    {
        public EdIzmer EdIzmer { get; private set; }

        public EditEdIzmerWindow()
        {
            InitializeComponent();
            EdIzmer = new EdIzmer();
            DataContext = this;
        }

        public EditEdIzmerWindow(EdIzmer edIzmer) : this()
        {
            EdIzmer = edIzmer;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EdIzmer.edIzmer1))
            {
                MessageBox.Show("Введите единицу измерения", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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