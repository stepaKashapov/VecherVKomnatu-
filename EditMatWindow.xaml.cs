using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace VecherVKomnatu
{
    public partial class EditMatWindow : Window
    {
        public Mat Mat { get; private set; }
        public List<EdIzmer> EdIzmerList { get; private set; }

        public EditMatWindow()
        {
            InitializeComponent();
            Mat = new Mat();
            EdIzmerList = MainWindow.db.EdIzmer.ToList();
            DataContext = this;
        }

        public EditMatWindow(Mat mat) : this()
        {
            Mat = mat;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Mat.name))
            {
                MessageBox.Show("Введите наименование материала", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (Mat.idIzmer == 0)
            {
                MessageBox.Show("Выберите единицу измерения", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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