using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace VecherVKomnatu
{
    public partial class EditRabWindow : Window
    {
        public Rab Rab { get; private set; }
        public List<Brigada> BrigadaList { get; private set; }

        public EditRabWindow()
        {
            InitializeComponent();
            Rab = new Rab();
            BrigadaList = MainWindow.db.Brigada.ToList();
            DataContext = this;
        }

        public EditRabWindow(Rab rab) : this()
        {
            Rab = rab;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Rab.FIO))
            {
                MessageBox.Show("Введите ФИО работника", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (Rab.brigada == 0)
            {
                MessageBox.Show("Выберите бригаду", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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