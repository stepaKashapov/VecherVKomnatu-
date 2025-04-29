using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;

namespace VecherVKomnatu
{
    public partial class EditOplataWindow : Window
    {
        private readonly ВечерВКвартируEntities _db;
        public Oplata Oplata { get; set; }
        public System.Collections.Generic.List<Dogovor> DogovorList { get; set; }

        public EditOplataWindow(ВечерВКвартируEntities db, Oplata oplata = null)
        {
            InitializeComponent();
            _db = db;
            Oplata = oplata ?? new Oplata(){ dateOplat = DateTime.Today };

            DataContext = this;
        }
        private void LoadData()
        {
            try
            {
                DogovorList = _db.Dogovor.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки договоров: {ex.Message}",
                              "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (Oplata.dogovor == 0)
            {
                MessageBox.Show("Выберите договор", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                _db.Oplata.Add(Oplata);
                _db.SaveChanges();
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения оплаты: {ex.Message}",
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