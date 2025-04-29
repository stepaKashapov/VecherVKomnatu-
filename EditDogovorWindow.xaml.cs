    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    namespace VecherVKomnatu
    {

        public partial class EditDogovorWindow : Window
        {
            public static ВечерВКвартируEntities _db;
            public Dogovor Dogovor { get; private set; }
            public List<Klient> KlientList { get; private set; }
            public List<Brigada> BrigadaList { get; private set; }

        public EditDogovorWindow(ВечерВКвартируEntities db, Dogovor dogovor = null)
        {
            InitializeComponent();
            _db = db;

            KlientList = db.Klient.ToList();
            BrigadaList = db.Brigada.ToList();

            Dogovor = dogovor ?? new Dogovor
            {
                dateSostav = DateTime.Today,
                dateN = DateTime.Today,
                dateK = DateTime.Today.AddMonths(1)
            };

            DataContext = this;
        }

        public EditDogovorWindow(Dogovor dogovor) 
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