using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace VecherVKomnatu
{

    public partial class EditDogovorWindow : Window
    {
        public static ВечерВКвартируEntities db;
        public Dogovor Dogovor { get; private set; }
        public List<Klient> KlientList { get; private set; }
        public List<Brigada> BrigadaList { get; private set; }

        public EditDogovorWindow()
        {
            db = new ВечерВКвартируEntities();
            InitializeComponent();

            // Получаем данные из главного окна
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                KlientList = db.Klient.ToList();
                BrigadaList = db.Brigada.ToList();
            }
            else
            {
                KlientList = new List<Klient>();
                BrigadaList = new List<Brigada>();
            }

            Dogovor = new Dogovor
            {
                dateSostav = DateTime.Today,
                dateN = DateTime.Today,
                dateK = DateTime.Today.AddMonths(1)
            };

            DataContext = this;
        }

        public EditDogovorWindow(Dogovor dogovor) : this()
        {
            Dogovor = dogovor;
            DataContext = this;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (Dogovor.klient == 0)
            {
                MessageBox.Show("Выберите клиента", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (Dogovor.brigada == 0)
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