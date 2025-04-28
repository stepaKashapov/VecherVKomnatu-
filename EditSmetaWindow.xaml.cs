using System.Linq;
using System.Windows;

namespace VecherVKomnatu
{
    public partial class EditSmetaWindow : Window
    {
        public Smeta Smeta { get; private set; }
        public System.Collections.Generic.List<Dogovor> DogovorList { get; private set; }
        public System.Collections.Generic.List<Uslugi> UslugiList { get; private set; }

        public EditSmetaWindow(Smeta smeta)
        {
            InitializeComponent();

            // Загружаем списки договоров и услуг
            DogovorList = MainWindow.db.Dogovor.ToList();
            UslugiList = MainWindow.db.Uslugi.ToList();

            // Создаем копию редактируемой сметы
            Smeta = new Smeta
            {
                id = smeta.id,
                dog = smeta.dog,
                uslugi = smeta.uslugi,
                mat = smeta.mat,
                kolMat = smeta.kolMat
            };

            DataContext = this;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (Smeta.dog == null || Smeta.uslugi == null)
            {
                MessageBox.Show("Необходимо выбрать договор и услугу", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
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