using System;
using System.Windows;

namespace VecherVKomnatu
{
    public partial class EditUslugiWindow : Window
    {
        private readonly ВечерВКвартируEntities _db;
        public Uslugi Uslugi { get; private set; }

        public EditUslugiWindow(ВечерВКвартируEntities db, Uslugi uslugi = null)
        {
            InitializeComponent();
            _db = db;
            Uslugi = uslugi ?? new Uslugi();
            DataContext = this;
        }

        public EditUslugiWindow(Uslugi uslugi)
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