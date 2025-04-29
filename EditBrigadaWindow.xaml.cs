using System;
using System.Windows;

namespace VecherVKomnatu
{
    public partial class EditBrigadaWindow : Window
    {
        private readonly ВечерВКвартируEntities _db;
        public Brigada Brigada { get; private set; }

        public EditBrigadaWindow(ВечерВКвартируEntities db, Brigada brigada = null)
        {
            InitializeComponent();
            _db = db;
            Brigada = brigada ?? new Brigada();
            DataContext = this;
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