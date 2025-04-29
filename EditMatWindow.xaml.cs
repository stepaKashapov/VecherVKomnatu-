using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;

namespace VecherVKomnatu
{
    public partial class EditMatWindow : Window
    {
        private readonly ВечерВКвартируEntities _db;
        public Mat Mat { get; set; }
        public System.Collections.Generic.List<EdIzmer> EdIzmerList { get; set; }

        public EditMatWindow(ВечерВКвартируEntities db, Mat mat = null)
        {
            InitializeComponent();
            _db = db;
            Mat = mat ?? new Mat();
            LoadData();
            DataContext = this;
        }

        private void LoadData()
        {
            try
            {
                EdIzmerList = _db.EdIzmer.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки единиц измерения: {ex.Message}",
                              "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Mat.name))
            {
                MessageBox.Show("Введите наименование материала", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (Mat.idIzmer == 0)
            {
                MessageBox.Show("Выберите единицу измерения", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                if (Mat.id == 0)
                    _db.Mat.Add(Mat);

                _db.SaveChanges();
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения материала: {ex.Message}",
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