using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace VecherVKomnatu
{
    public partial class MainWindow : Window
    {
        public ВечерВКвартируEntities db = new ВечерВКвартируEntities();
        private string currentView;

        public MainWindow()
        {
            InitializeComponent();
            LoadData("Договоры"); // Загружаем договоры по умолчанию
        }

        private void LoadData(string viewName)
        {
            currentView = viewName;
            tbTitle.Text = viewName;
            lvData.ItemsSource = null;
            lvData.View = new GridView();

            try
            {
                switch (viewName)
                {
                    case "Клиенты":
                        ConfigureClientView();
                        lvData.ItemsSource = db.Klient.ToList();
                        break;

                    case "Бригады":
                        ConfigureBrigadaView();
                        lvData.ItemsSource = db.Brigada.ToList();
                        break;

                    case "Работники":
                        ConfigureRabView();
                        lvData.ItemsSource = db.Rab.Include(r => r.Brigada1).ToList();
                        break;

                    case "Договоры":
                        ConfigureDogovorView();
                        lvData.ItemsSource = db.Dogovor
                            .Include(d => d.Klient1)
                            .Include(d => d.Brigada1)
                            .ToList();
                        break;

                    case "Сметы":
                        ConfigureSmetaView();
                        lvData.ItemsSource = db.Smeta
                            .Include(s => s.Dogovor)
                            .Include(s => s.Uslugi1)
                            .Include(s => s.Mat1)
                            .ToList();
                        break;

                    case "Материалы":
                        ConfigureMatView();
                        lvData.ItemsSource = db.Mat.Include(m => m.EdIzmer).ToList();
                        break;

                    case "Единицы измерения":
                        ConfigureEdIzmerView();
                        lvData.ItemsSource = db.EdIzmer.ToList();
                        break;

                    case "Услуги":
                        ConfigureUslugiView();
                        lvData.ItemsSource = db.Uslugi.ToList();
                        break;

                    case "Оплаты":
                        ConfigureOplataView();
                        lvData.ItemsSource = db.Oplata
                            .Include(o => o.Dogovor1)
                            .ToList();
                        break;

                    default:
                        lvData.ItemsSource = null;
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

            UpdateButtonStates();
        }

        private void ConfigureClientView()
        {
            var gridView = new GridView();
            gridView.Columns.Add(new GridViewColumn { Header = "ID", DisplayMemberBinding = new Binding("id"), Width = 50 });
            gridView.Columns.Add(new GridViewColumn { Header = "ФИО", DisplayMemberBinding = new Binding("FIO"), Width = 200 });
            gridView.Columns.Add(new GridViewColumn { Header = "Телефон", DisplayMemberBinding = new Binding("tel"), Width = 120 });
            gridView.Columns.Add(new GridViewColumn { Header = "Дата рождения", DisplayMemberBinding = new Binding("dateR") { StringFormat = "dd.MM.yyyy" },
                Width = 100
            });
            gridView.Columns.Add(new GridViewColumn { Header = "Серия", DisplayMemberBinding = new Binding("seria"), Width = 150 });
            gridView.Columns.Add(new GridViewColumn { Header = "Номер", DisplayMemberBinding = new Binding("nomer"), Width = 150 });
            lvData.View = gridView;
        }

        private void ConfigureBrigadaView()
        {
            var gridView = new GridView();
            gridView.Columns.Add(new GridViewColumn { Header = "ID", DisplayMemberBinding = new Binding("id"), Width = 50 });
            gridView.Columns.Add(new GridViewColumn { Header = "Название бригады", DisplayMemberBinding = new Binding("name"), Width = 300 });
            gridView.Columns.Add(new GridViewColumn { Header = "Кол-во работников", DisplayMemberBinding = new Binding("Rab.Count"), Width = 100 });
            lvData.View = gridView;
        }

        private void ConfigureRabView()
        {
            var gridView = new GridView();
            gridView.Columns.Add(new GridViewColumn { Header = "ID", DisplayMemberBinding = new Binding("id"), Width = 50 });
            gridView.Columns.Add(new GridViewColumn { Header = "ФИО", DisplayMemberBinding = new Binding("FIO"), Width = 200 });
            gridView.Columns.Add(new GridViewColumn { Header = "Телефон", DisplayMemberBinding = new Binding("tel"), Width = 120 });
            gridView.Columns.Add(new GridViewColumn { Header = "Дата рождения", DisplayMemberBinding = new Binding("dateR") { StringFormat = "dd.MM.yyyy" },
                Width = 100
            });
            gridView.Columns.Add(new GridViewColumn { Header = "Оклад", DisplayMemberBinding = new Binding("salary") { StringFormat = "{0:N2} ₽" }, Width = 100 });
            gridView.Columns.Add(new GridViewColumn { Header = "Бригада", DisplayMemberBinding = new Binding("Brigada1.name"), Width = 150 });
            gridView.Columns.Add(new GridViewColumn { Header = "Серия", DisplayMemberBinding = new Binding("seria"), Width = 150 });
            gridView.Columns.Add(new GridViewColumn { Header = "Номер", DisplayMemberBinding = new Binding("nomer"), Width = 150 }); lvData.View = gridView;
        }

        private void ConfigureDogovorView()
        {
            var gridView = new GridView();
            gridView.Columns.Add(new GridViewColumn { Header = "№", DisplayMemberBinding = new Binding("id"), Width = 50 });
            gridView.Columns.Add(new GridViewColumn { Header = "Дата составления", DisplayMemberBinding = new Binding("dateSostav") { StringFormat = "dd.MM.yyyy" }, Width = 100 });
            gridView.Columns.Add(new GridViewColumn { Header = "Клиент", DisplayMemberBinding = new Binding("Klient1.FIO"), Width = 200 });
            gridView.Columns.Add(new GridViewColumn { Header = "Бригада", DisplayMemberBinding = new Binding("Brigada1.name"), Width = 150 });
            gridView.Columns.Add(new GridViewColumn { Header = "Дата начала", DisplayMemberBinding = new Binding("dateN") { StringFormat = "dd.MM.yyyy" }, Width = 100 });
            gridView.Columns.Add(new GridViewColumn { Header = "Дата конца", DisplayMemberBinding = new Binding("dateK") { StringFormat = "dd.MM.yyyy" }, Width = 100 });
            lvData.View = gridView;
        }

        private void ConfigureSmetaView()
        {
            var gridView = new GridView();
            gridView.Columns.Add(new GridViewColumn { Header = "ID", DisplayMemberBinding = new Binding("id"), Width = 50 });
            gridView.Columns.Add(new GridViewColumn { Header = "Договор №", DisplayMemberBinding = new Binding("dog"), Width = 80 });
            gridView.Columns.Add(new GridViewColumn { Header = "Услуга", DisplayMemberBinding = new Binding("Uslugi1.name"), Width = 200 });
            gridView.Columns.Add(new GridViewColumn { Header = "Материал", DisplayMemberBinding = new Binding("Mat1.name"), Width = 150 });
            gridView.Columns.Add(new GridViewColumn { Header = "Кол-во", DisplayMemberBinding = new Binding("kolMat"), Width = 80 });
            lvData.View = gridView;
        }

        private void ConfigureMatView()
        {
            var gridView = new GridView();
            gridView.Columns.Add(new GridViewColumn { Header = "ID", DisplayMemberBinding = new Binding("id"), Width = 50 });
            gridView.Columns.Add(new GridViewColumn { Header = "Наименование", DisplayMemberBinding = new Binding("name"), Width = 250 });
            gridView.Columns.Add(new GridViewColumn { Header = "Ед. измерения", DisplayMemberBinding = new Binding("EdIzmer.edIzmer1"), Width = 120 });
            gridView.Columns.Add(new GridViewColumn { Header = "Цена", DisplayMemberBinding = new Binding("price") { StringFormat = "{0:N2} ₽" }, Width = 100 });
            lvData.View = gridView;
        }

        private void ConfigureEdIzmerView()
        {
            var gridView = new GridView();
            gridView.Columns.Add(new GridViewColumn { Header = "ID", DisplayMemberBinding = new Binding("id"), Width = 50 });
            gridView.Columns.Add(new GridViewColumn { Header = "Единица измерения", DisplayMemberBinding = new Binding("edIzmer1"), Width = 200 });
            lvData.View = gridView;
        }

        private void ConfigureUslugiView()
        {
            var gridView = new GridView();
            gridView.Columns.Add(new GridViewColumn { Header = "ID", DisplayMemberBinding = new Binding("id"), Width = 50 });
            gridView.Columns.Add(new GridViewColumn { Header = "Наименование", DisplayMemberBinding = new Binding("name"), Width = 250 });
            gridView.Columns.Add(new GridViewColumn { Header = "Цена", DisplayMemberBinding = new Binding("price") { StringFormat = "{0:N2} ₽" }, Width = 100 });
            gridView.Columns.Add(new GridViewColumn { Header = "Описание", DisplayMemberBinding = new Binding("opisanie"), Width = 300 });
            lvData.View = gridView;
        }

        private void ConfigureOplataView()
        {
            var gridView = new GridView();
            gridView.Columns.Add(new GridViewColumn { Header = "ID", DisplayMemberBinding = new Binding("id"), Width = 50 });
            gridView.Columns.Add(new GridViewColumn { Header = "Дата оплаты", DisplayMemberBinding = new Binding("dateOplat") { StringFormat = "dd.MM.yyyy" }, Width = 100 });
            gridView.Columns.Add(new GridViewColumn { Header = "Договор №", DisplayMemberBinding = new Binding("Dogovor1.id"), Width = 80 });
            gridView.Columns.Add(new GridViewColumn { Header = "Клиент", DisplayMemberBinding = new Binding("Dogovor1.Klient1.FIO"), Width = 200 });
            gridView.Columns.Add(new GridViewColumn { Header = "Сумма", DisplayMemberBinding = new Binding("sum") { StringFormat = "{0:N2} ₽" }, Width = 100 });
            lvData.View = gridView;
        }

        private void NavigationButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                LoadData(button.Content.ToString());
            }
        }

        private void lvData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonStates();

            // Показываем детали для сметы
            if (currentView == "Сметы" && lvData.SelectedItem != null)
            {
                var smeta = lvData.SelectedItem as Smeta;
                if (smeta != null)
                {
                    detailsBorder.Visibility = Visibility.Visible;
                    detailsRow.Height = new GridLength(200);

                    // Получаем все услуги договора, к которому относится эта смета
                    lvServices.ItemsSource = db.Smeta
                        .Where(s => s.dog == smeta.dog) // Все сметы этого договора
                        .Select(s => s.Uslugi1)         // Услуги из этих смет
                        .Distinct()                    // Уникальные услуги
                        .ToList();
                }
            }
            else
            {
                detailsBorder.Visibility = Visibility.Collapsed;
                detailsRow.Height = new GridLength(0);
            }
        }

        private void UpdateButtonStates()
        {
            btnEdit.IsEnabled = lvData.SelectedItem != null;
            btnDelete.IsEnabled = lvData.SelectedItem != null;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (currentView)
                {
                    case "Клиенты":
                        var klientWindow = new EditKlientWindow(db);
                        if (klientWindow.ShowDialog() == true)
                        {
                            db.Klient.Add(klientWindow.Klient);
                            db.SaveChanges();
                            LoadData(currentView);
                        }
                        break;


                    case "Бригады":
                        var brigadaWindow = new EditBrigadaWindow(db);
                        if (brigadaWindow.ShowDialog() == true)
                        {
                            db.Brigada.Add(brigadaWindow.Brigada);
                            db.SaveChanges();
                            LoadData(currentView);
                        }
                        break;

                    case "Работники":
                        var rabWindow = new EditRabWindow(db);
                        if (rabWindow.ShowDialog() == true)
                        {
                            db.Rab.Add(rabWindow.Rab);
                            db.SaveChanges();
                            LoadData(currentView);
                        }
                        break;

                    case "Договоры":
                        var dogovorWindow = new EditDogovorWindow(db);
                        if (dogovorWindow.ShowDialog() == true)
                        {
                            db.Dogovor.Add(dogovorWindow.Dogovor);
                            db.SaveChanges();
                            LoadData(currentView);
                        }
                        break;

                    case "Сметы":
                        var smetaWindow = new EditSmetaWindow(db);
                        if (smetaWindow.ShowDialog() == true)
                        {
                            if (db.Dogovor.Any(d => d.id == smetaWindow.Smeta.dog) &&
                                db.Uslugi.Any(u => u.id == smetaWindow.Smeta.uslugi) &&
                                db.Mat.Any(m => m.id == smetaWindow.Smeta.mat))
                            {
                                db.Smeta.Add(smetaWindow.Smeta);
                                db.SaveChanges();
                                LoadData(currentView);
                            }
                        }
                        break;

                    case "Материалы":
                        var matWindow = new EditMatWindow(db);
                        if (matWindow.ShowDialog() == true)
                        {
                            if (db.EdIzmer.Any(x => x.id == matWindow.Mat.idIzmer))  // Заменили e на x
                            {
                                db.Mat.Add(matWindow.Mat);
                                db.SaveChanges();
                                LoadData(currentView);
                            }
                        }
                        break;

                    case "Единицы измерения":
                        var edIzmerWindow = new EditEdIzmerWindow(db);
                        if (edIzmerWindow.ShowDialog() == true)
                        {
                            db.EdIzmer.Add(edIzmerWindow.EdIzmer);
                            db.SaveChanges();
                            LoadData(currentView);
                        }
                        break;

                    case "Услуги":
                        var uslugiWindow = new EditUslugiWindow(db);
                        if (uslugiWindow.ShowDialog() == true)
                        {
                            db.Uslugi.Add(uslugiWindow.Uslugi);
                            db.SaveChanges();
                            LoadData(currentView);
                        }
                        break;

                    case "Оплаты":
                        var oplataWindow = new EditOplataWindow(db);
                        if (oplataWindow.ShowDialog() == true)
                        {
                            if (db.Dogovor.Any(d => d.id == oplataWindow.Oplata.dogovor))
                            {
                                db.Oplata.Add(oplataWindow.Oplata);
                                db.SaveChanges();
                                LoadData(currentView);
                            }
                        }
                        break;

                    default:
                        MessageBox.Show($"Добавление для раздела '{currentView}' не реализовано",
                            "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (lvData.SelectedItem == null) return;

            try
            {
                switch (currentView)
                {
                    case "Клиенты":
                        var klient = lvData.SelectedItem as Klient;
                        var klientWindow = new EditKlientWindow(db, klient);
                        if (klientWindow.ShowDialog() == true)
                        {
                            db.Entry(klient).CurrentValues.SetValues(klientWindow.Klient);
                            db.SaveChanges();
                            LoadData(currentView);
                        }
                        break;

                    case "Бригады":
                        var brigada = lvData.SelectedItem as Brigada;
                        var brigadaWindow = new EditBrigadaWindow(db, brigada);
                        if (brigadaWindow.ShowDialog() == true)
                        {
                            db.Entry(brigada).CurrentValues.SetValues(brigadaWindow.Brigada);
                            db.SaveChanges();
                            LoadData(currentView);
                        }
                        break;

                    case "Работники":
                        var rab = lvData.SelectedItem as Rab;
                        var rabWindow = new EditRabWindow(db, rab);
                        if (rabWindow.ShowDialog() == true)
                        {
                            db.Entry(rab).CurrentValues.SetValues(rabWindow.Rab);
                            db.SaveChanges();
                            LoadData(currentView);
                        }
                        break;

                    case "Договоры":
                        var dogovor = lvData.SelectedItem as Dogovor;
                        var dogovorWindow = new EditDogovorWindow(db, dogovor);  // Используем правильный конструктор
                        if (dogovorWindow.ShowDialog() == true)
                        {
                            db.Entry(dogovor).CurrentValues.SetValues(dogovorWindow.Dogovor);
                            db.SaveChanges();
                            LoadData(currentView);
                        }
                        break;

                    case "Сметы":
                        var smeta = lvData.SelectedItem as Smeta;
                        var smetaWindow = new EditSmetaWindow(db, smeta);
                        if (smetaWindow.ShowDialog() == true)
                        {
                            db.Entry(smeta).CurrentValues.SetValues(smetaWindow.Smeta);
                            db.SaveChanges();
                            LoadData(currentView);
                        }
                        break;

                    case "Материалы":
                        var mat = lvData.SelectedItem as Mat;
                        var matWindow = new EditMatWindow(db, mat);
                        if (matWindow.ShowDialog() == true)
                        {
                            db.Entry(mat).CurrentValues.SetValues(matWindow.Mat);
                            db.SaveChanges();
                            LoadData(currentView);
                        }
                        break;

                    case "Единицы измерения":
                        var edIzmer = lvData.SelectedItem as EdIzmer;
                        var edIzmerWindow = new EditEdIzmerWindow(db, edIzmer);
                        if (edIzmerWindow.ShowDialog() == true)
                        {
                            db.Entry(edIzmer).CurrentValues.SetValues(edIzmerWindow.EdIzmer);
                            db.SaveChanges();
                            LoadData(currentView);
                        }
                        break;

                    case "Услуги":
                        var uslugi = lvData.SelectedItem as Uslugi;
                        var uslugiWindow = new EditUslugiWindow(db, uslugi);
                        if (uslugiWindow.ShowDialog() == true)
                        {
                            db.Entry(uslugi).CurrentValues.SetValues(uslugiWindow.Uslugi);
                            db.SaveChanges();
                            LoadData(currentView);
                        }
                        break;

                    case "Оплаты":
                        var oplata = lvData.SelectedItem as Oplata;
                        var oplataWindow = new EditOplataWindow(db, oplata);
                        if (oplataWindow.ShowDialog() == true)
                        {
                            db.Entry(oplata).CurrentValues.SetValues(oplataWindow.Oplata);
                            db.SaveChanges();
                            LoadData(currentView);
                        }
                        break;

                    default:
                        MessageBox.Show($"Редактирование для раздела '{currentView}' не реализовано",
                            "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при редактировании: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lvData.SelectedItem == null) return;

            var result = MessageBox.Show("Удалить выбранную запись?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    switch (currentView)
                    {
                        case "Клиенты":
                            var klient = lvData.SelectedItem as Klient;
                            db.Klient.Remove(klient);
                            break;

                        case "Бригады":
                            var brigada = lvData.SelectedItem as Brigada;
                            db.Brigada.Remove(brigada);
                            break;

                        case "Работники":
                            var rab = lvData.SelectedItem as Rab;
                            db.Rab.Remove(rab);
                            break;

                        case "Договоры":
                            var dogovor = lvData.SelectedItem as Dogovor;
                            db.Dogovor.Remove(dogovor);
                            break;

                        case "Сметы":
                            var smeta = lvData.SelectedItem as Smeta;
                            db.Smeta.Remove(smeta);
                            break;

                        case "Материалы":
                            var mat = lvData.SelectedItem as Mat;
                            db.Mat.Remove(mat);
                            break;

                        case "Единицы измерения":
                            var edIzmer = lvData.SelectedItem as EdIzmer;
                            db.EdIzmer.Remove(edIzmer);
                            break;

                        case "Услуги":
                            var uslugi = lvData.SelectedItem as Uslugi;
                            db.Uslugi.Remove(uslugi);
                            break;

                        case "Оплаты":
                            var oplata = lvData.SelectedItem as Oplata;
                            db.Oplata.Remove(oplata);
                            break;

                        default:
                            MessageBox.Show($"Удаление для раздела '{currentView}' не реализовано",
                                "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
                    }

                    db.SaveChanges();
                    LoadData(currentView);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Функция формирования отчета будет реализована позже", 
                "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            db.Dispose();
        }

        private void ShowValidationErrors(DbEntityValidationException ex)
        {
            string errorMessage = "Ошибка валидации:\n";
            foreach (var eve in ex.EntityValidationErrors)
            {
                foreach (var ve in eve.ValidationErrors)
                {
                    errorMessage += $"- {ve.PropertyName}: {ve.ErrorMessage}\n";
                }
            }
            MessageBox.Show(errorMessage, "Ошибка валидации", 
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}