using System;
using System.Windows;

namespace VecherVKomnatu
{
    public partial class EditEdIzmerWindow : Window
    {
        public EdIzmer EdIzmer { get; private set; }
        private readonly ВечерВКвартируEntities _db;
        public EditEdIzmerWindow(ВечерВКвартируEntities db, EdIzmer edIzmer = null)
        {
            InitializeComponent();
            _db = db;
            EdIzmer = edIzmer ?? new EdIzmer();
            DataContext = this;
        }

        public EditEdIzmerWindow(EdIzmer edIzmer)
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