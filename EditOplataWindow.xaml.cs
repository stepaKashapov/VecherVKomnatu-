using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace VecherVKomnatu
{
    public partial class EditOplataWindow : Window
    {
        public Oplata Oplata { get; private set; }
        public List<Dogovor> DogovorList { get; private set; }

        public EditOplataWindow()
        {
            InitializeComponent();
            Oplata = new Oplata { dateOplat = DateTime.Today };
            DogovorList = MainWindow.db.Dogovor.ToList();
            DataContext = this;
        }

        public EditOplataWindow(Oplata oplata) : this()
        {
            Oplata = oplata;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (Oplata.dogovor == 0)
            {
                MessageBox.Show("Выберите договор", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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