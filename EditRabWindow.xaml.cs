using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;

namespace VecherVKomnatu
{
    public partial class EditRabWindow : Window
    {
        private readonly ВечерВКвартируEntities _db;
        public Rab Rab { get; set; }
        public System.Collections.Generic.List<Brigada> BrigadaList { get; set; }

        public EditRabWindow(ВечерВКвартируEntities db, Rab rab = null)
        {
            InitializeComponent();
            _db = db;
            Rab = rab ?? new Rab();
            LoadData();
            DataContext = this;
        }

        private void LoadData()
        {
            try
            {
                BrigadaList = _db.Brigada.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки бригад: {ex.Message}",
                              "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Rab.FIO))
            {
                MessageBox.Show("Введите ФИО работника", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (Rab.brigada == 0)
            {
                MessageBox.Show("Выберите бригаду", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                if (Rab.id == 0)
                    _db.Rab.Add(Rab);

                _db.SaveChanges();
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения работника: {ex.Message}",
                              "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}