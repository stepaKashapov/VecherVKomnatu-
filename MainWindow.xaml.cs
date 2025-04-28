using System;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace VecherVKomnatu
{
    public partial class MainWindow : Window
    {
        public static ВечерВКвартируEntities db;
        private string currentView = "Договоры";

        public MainWindow()
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("ru-RU");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("ru-RU");

            InitializeComponent();

            try
            {
                db = new ВечерВКвартируEntities();
                db.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
                LoadData(currentView);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к базе данных: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }

        private void NavigationButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            currentView = button.Content.ToString();
            tbTitle.Text = currentView;
            lvData.SelectedItem = null;
            LoadData(currentView);
        }

        private void LoadData(string viewType)
        {
            try
            {
                lvData.ItemsSource = null;
                lvData.View = new GridView();
                GridView gridView = (GridView)lvData.View;
                gridView.Columns.Clear();

                detailsBorder.Visibility = Visibility.Collapsed;
                detailsRow.Height = new GridLength(0);

                switch (viewType)
                {
                    case "Договоры":
                        SetupDogovorView();
                        lvData.ItemsSource = db.Dogovor.Include(d => d.Klient1)
                                                  .Include(d => d.Brigada1)
                                                  .ToList()
                                                  .Select(d => new
                                                  {
                                                      d.id,
                                                      DateSostav = d.dateSostav.ToString("dd.MM.yyyy"),
                                                      KlientName = d.Klient1?.FIO,
                                                      DateN = d.dateN.ToString("dd.MM.yyyy"),
                                                      DateK = d.dateK.ToString("dd.MM.yyyy"),
                                                      BrigadaName = d.Brigada1?.name
                                                  }).ToList();
                        break;

                    case "Клиенты":
                        SetupKlientView();
                        lvData.ItemsSource = db.Klient.ToList()
                                                .Select(k => new
                                                {
                                                    k.id,
                                                    k.FIO,
                                                    k.tel,
                                                    DateR = k.dateR.ToString("dd.MM.yyyy"),
                                                    Passport = $"{k.seria} {k.nomer}"
                                                }).ToList();
                        break;

                    case "Бригады":
                        SetupBrigadaView();
                        lvData.ItemsSource = db.Brigada.ToList()
                                                  .Select(b => new
                                                  {
                                                      b.id,
                                                      b.name
                                                  }).ToList();
                        break;

                    case "Работники":
                        SetupRabView();
                        lvData.ItemsSource = db.Rab.Include(r => r.Brigada1)
                                               .ToList()
                                               .Select(r => new
                                               {
                                                   r.id,
                                                   r.FIO,
                                                   r.tel,
                                                   DateR = r.dateR.ToString("dd.MM.yyyy"),
                                                   r.salary,
                                                   BrigadaName = r.Brigada1?.name,
                                                   Passport = $"{r.seria} {r.nomer}"
                                               }).ToList();
                        break;

                    case "Материалы":
                        SetupMatView();
                        lvData.ItemsSource = db.Mat.Include(m => m.EdIzmer)
                                              .ToList()
                                              .Select(m => new
                                              {
                                                  m.id,
                                                  m.name,
                                                  EdizmerName = m.EdIzmer?.edIzmer1,
                                                  m.price
                                              }).ToList();
                        break;

                    case "Единицы измерения":
                        SetupEdizmerView();
                        lvData.ItemsSource = db.EdIzmer.ToList()
                                                  .Select(e => new
                                                  {
                                                      e.id,
                                                      e.edIzmer1
                                                  }).ToList();
                        break;

                    case "Услуги":
                        SetupUslugiView();
                        lvData.ItemsSource = db.Uslugi.ToList()
                                                  .Select(u => new
                                                  {
                                                      u.id,
                                                      u.name,
                                                      u.price,
                                                      u.opisanie
                                                  }).ToList();
                        break;

                    case "Оплаты":
                        SetupOplataView();
                        lvData.ItemsSource = db.Oplata.Include(o => o.Dogovor1)
                                                 .ToList()
                                                 .Select(o => new
                                                 {
                                                     o.id,
                                                     DateOplat = o.dateOplat.ToString("dd.MM.yyyy"),
                                                     o.sum,
                                                     DogovorNumber = $"Договор №{o.Dogovor1?.id}"
                                                 }).ToList();
                        break;

                    case "Сметы":
                        SetupSmetaView();
                        lvData.ItemsSource = db.Smeta.Include(s => s.Dogovor)
                                                .Include(s => s.Mat1)
                                                .Include(s => s.Uslugi1)
                                                .ToList()
                                                .Select(s => new
                                                {
                                                    s.id,
                                                    DogovorNumber = $"Договор №{s.Dogovor?.id}",
                                                    UslugaName = s.Uslugi1?.name,
                                                    MaterialName = s.Mat1?.name,
                                                    s.kolMat,
                                                    DogovorId = s.Dogovor?.id
                                                }).ToList();
                        break;
                }

                foreach (var column in gridView.Columns)
                {
                    column.Width = column.ActualWidth;
                    column.Width = double.NaN;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void lvData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnEdit.IsEnabled = lvData.SelectedItem != null;
            btnDelete.IsEnabled = lvData.SelectedItem != null;

            if (currentView == "Сметы" && lvData.SelectedItem != null)
            {
                try
                {
                    dynamic selectedSmeta = lvData.SelectedItem;
                    int? dogovorId = selectedSmeta.DogovorId;

                    if (dogovorId.HasValue)
                    {
                        var services = db.Smeta.Include(s => s.Uslugi1)
                                          .Where(s => s.Dogovor.id == dogovorId && s.Uslugi1 != null)
                                          .Select(s => s.Uslugi1)
                                          .Distinct()
                                          .ToList();

                        lvServices.ItemsSource = services;
                        detailsBorder.Visibility = Visibility.Visible;
                        detailsRow.Height = new GridLength(200);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки услуг: {ex.Message}", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                detailsBorder.Visibility = Visibility.Collapsed;
                detailsRow.Height = new GridLength(0);
            }
        }

        #region Методы настройки отображения таблиц
        private void SetupDogovorView()
        {
            AddGridViewColumn("ID", "id", 50);
            AddGridViewColumn("Дата составления", "DateSostav", 100);
            AddGridViewColumn("Клиент", "KlientName", 150);
            AddGridViewColumn("Дата начала", "DateN", 100);
            AddGridViewColumn("Дата окончания", "DateK", 100);
            AddGridViewColumn("Бригада", "BrigadaName", 120);
        }

        private void SetupKlientView()
        {
            AddGridViewColumn("ID", "id", 50);
            AddGridViewColumn("ФИО", "FIO", 200);
            AddGridViewColumn("Телефон", "tel", 120);
            AddGridViewColumn("Дата рождения", "DateR", 100);
            AddGridViewColumn("Паспорт", "Passport", 150);
        }

        private void SetupBrigadaView()
        {
            AddGridViewColumn("ID", "id", 50);
            AddGridViewColumn("Название", "name", 200);
        }

        private void SetupRabView()
        {
            AddGridViewColumn("ID", "id", 50);
            AddGridViewColumn("ФИО", "FIO", 180);
            AddGridViewColumn("Телефон", "tel", 100);
            AddGridViewColumn("Дата рождения", "DateR", 100);
            AddGridViewColumn("Оклад", "salary", 80, true);
            AddGridViewColumn("Бригада", "BrigadaName", 120);
            AddGridViewColumn("Паспорт", "Passport", 150);
        }

        private void SetupMatView()
        {
            AddGridViewColumn("ID", "id", 50);
            AddGridViewColumn("Наименование", "name", 150);
            AddGridViewColumn("Ед. измерения", "EdizmerName", 100);
            AddGridViewColumn("Цена", "price", 80, true);
        }

        private void SetupEdizmerView()
        {
            AddGridViewColumn("ID", "id", 50);
            AddGridViewColumn("Единица измерения", "edIzmer1", 150);
        }

        private void SetupUslugiView()
        {
            AddGridViewColumn("ID", "id", 50);
            AddGridViewColumn("Наименование", "name", 150);
            AddGridViewColumn("Цена", "price", 80, true);
            AddGridViewColumn("Описание", "opisanie", 250);
        }

        private void SetupOplataView()
        {
            AddGridViewColumn("ID", "id", 50);
            AddGridViewColumn("Дата оплаты", "DateOplat", 100);
            AddGridViewColumn("Сумма", "sum", 100, true);
            AddGridViewColumn("Договор", "DogovorNumber", 120);
        }

        private void SetupSmetaView()
        {
            AddGridViewColumn("ID", "id", 50);
            AddGridViewColumn("Договор", "DogovorNumber", 120);
            AddGridViewColumn("Услуга", "UslugaName", 200);
            AddGridViewColumn("Материал", "MaterialName", 150);
            AddGridViewColumn("Кол-во материалов", "kolMat", 120);
        }

        private void AddGridViewColumn(string header, string bindingPath, double? width = null, bool isMoney = false)
        {
            var gridView = (GridView)lvData.View;
            var binding = new Binding(bindingPath)
            {
                StringFormat = isMoney ? "{0:N2} ₽" : null
            };

            var column = new GridViewColumn
            {
                Header = header,
                DisplayMemberBinding = binding,
                Width = width ?? double.NaN
            };

            gridView.Columns.Add(column);
        }
        #endregion

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (currentView)
                {
                    case "Клиенты":
                        var klientWindow = new EditKlientWindow();
                        klientWindow.Owner = this;
                        klientWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;

                        if (klientWindow.ShowDialog() == true)
                        {
                            var newKlient = new Klient
                            {
                                FIO = klientWindow.Klient.FIO,
                                tel = klientWindow.Klient.tel,
                                dateR = klientWindow.Klient.dateR,
                                seria = klientWindow.Klient.seria,
                                nomer = klientWindow.Klient.nomer
                            };

                            db.Klient.Add(newKlient);
                            db.SaveChanges();
                            LoadData(currentView);
                        }
                        break;

                    case "Бригады":
                        var brigadaWindow = new EditBrigadaWindow();
                        brigadaWindow.Owner = this;
                        brigadaWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;

                        if (brigadaWindow.ShowDialog() == true)
                        {
                            var newBrigada = new Brigada
                            {
                                name = brigadaWindow.Brigada.name
                            };

                            db.Brigada.Add(newBrigada);
                            db.SaveChanges();
                            LoadData(currentView);
                        }
                        break;

                    case "Работники":
                        var rabWindow = new EditRabWindow();
                        rabWindow.Owner = this;
                        rabWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;

                        if (rabWindow.ShowDialog() == true)
                        {
                            var newRab = new Rab
                            {
                                FIO = rabWindow.Rab.FIO,
                                tel = rabWindow.Rab.tel,
                                dateR = rabWindow.Rab.dateR,
                                salary = rabWindow.Rab.salary,
                                brigada = rabWindow.Rab.brigada,
                                seria = rabWindow.Rab.seria,
                                nomer = rabWindow.Rab.nomer
                            };

                            db.Rab.Add(newRab);
                            db.SaveChanges();
                            LoadData(currentView);
                        }
                        break;

                    case "Договоры":
                        var dogovorWindow = new EditDogovorWindow();
                        dogovorWindow.Owner = this;
                        dogovorWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;

                        if (dogovorWindow.ShowDialog() == true)
                        {
                            var newDogovor = new Dogovor
                            {
                                dateSostav = dogovorWindow.Dogovor.dateSostav,
                                klient = dogovorWindow.Dogovor.klient,
                                dateN = dogovorWindow.Dogovor.dateN,
                                dateK = dogovorWindow.Dogovor.dateK,
                                brigada = dogovorWindow.Dogovor.brigada
                            };

                            db.Dogovor.Add(newDogovor);
                            db.SaveChanges();
                            LoadData(currentView);
                        }
                        break;

                    case "Сметы":
                        var smetaWindow = new EditSmetaWindow(new Smeta());
                        smetaWindow.Owner = this;
                        smetaWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;

                        if (smetaWindow.ShowDialog() == true)
                        {
                            var newSmeta = new Smeta
                            {
                                dog = smetaWindow.Smeta.dog,
                                uslugi = smetaWindow.Smeta.uslugi,
                                mat = smetaWindow.Smeta.mat,
                                kolMat = smetaWindow.Smeta.kolMat
                            };

                            db.Smeta.Add(newSmeta);
                            db.SaveChanges();
                            LoadData(currentView);
                        }
                        break;

                    case "Материалы":
                        var matWindow = new EditMatWindow();
                        matWindow.Owner = this;
                        matWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;

                        if (matWindow.ShowDialog() == true)
                        {
                            var newMat = new Mat
                            {
                                name = matWindow.Mat.name,
                                idIzmer = matWindow.Mat.idIzmer,
                                price = matWindow.Mat.price
                            };

                            db.Mat.Add(newMat);
                            db.SaveChanges();
                            LoadData(currentView);
                        }
                        break;

                    

                    case "Услуги":
                        var uslugiWindow = new EditUslugiWindow();
                        uslugiWindow.Owner = this;
                        uslugiWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;

                        if (uslugiWindow.ShowDialog() == true)
                        {
                            var newUslugi = new Uslugi
                            {
                                name = uslugiWindow.Uslugi.name,
                                price = uslugiWindow.Uslugi.price,
                                opisanie = uslugiWindow.Uslugi.opisanie
                            };

                            db.Uslugi.Add(newUslugi);
                            db.SaveChanges();
                            LoadData(currentView);
                        }
                        break;

                    case "Оплаты":
                        var oplataWindow = new EditOplataWindow();
                        oplataWindow.Owner = this;
                        oplataWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;

                        if (oplataWindow.ShowDialog() == true)
                        {
                            var newOplata = new Oplata
                            {
                                dateOplat = oplataWindow.Oplata.dateOplat,
                                sum = oplataWindow.Oplata.sum,
                                dogovor = oplataWindow.Oplata.dogovor
                            };

                            db.Oplata.Add(newOplata);
                            db.SaveChanges();
                            LoadData(currentView);
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
                        dynamic selectedKlient = lvData.SelectedItem;
                        var klient = db.Klient.Find(selectedKlient.id);
                        if (klient != null)
                        {
                            var dialog = new EditKlientWindow(klient);
                            if (dialog.ShowDialog() == true)
                            {
                                db.SaveChanges();
                                LoadData(currentView);
                            }
                        }
                        break;

                    case "Бригады":
                        dynamic selectedBrigada = lvData.SelectedItem;
                        var brigada = db.Brigada.Find(selectedBrigada.id);
                        if (brigada != null)
                        {
                            var dialog = new EditBrigadaWindow(brigada);
                            if (dialog.ShowDialog() == true)
                            {
                                db.SaveChanges();
                                LoadData(currentView);
                            }
                        }
                        break;

                    case "Работники":
                        dynamic selectedRab = lvData.SelectedItem;
                        var rab = db.Rab.Find(selectedRab.id);
                        if (rab != null)
                        {
                            var dialog = new EditRabWindow(rab);
                            if (dialog.ShowDialog() == true)
                            {
                                db.SaveChanges();
                                LoadData(currentView);
                            }
                        }
                        break;

                    case "Договоры":
                        dynamic selectedDogovor = lvData.SelectedItem;
                        var dogovor = db.Dogovor.Find(selectedDogovor.id);
                        if (dogovor != null)
                        {
                            var dialog = new EditDogovorWindow(dogovor);
                            if (dialog.ShowDialog() == true)
                            {
                                db.SaveChanges();
                                LoadData(currentView);
                            }
                        }
                        break;

                    case "Сметы":
                        dynamic selectedSmeta = lvData.SelectedItem;
                        var smeta = db.Smeta.Find(selectedSmeta.id);
                        if (smeta != null)
                        {
                            var dialog = new EditSmetaWindow(smeta);
                            if (dialog.ShowDialog() == true)
                            {
                                db.SaveChanges();
                                LoadData(currentView);
                            }
                        }
                        break;

                    case "Материалы":
                        dynamic selectedMat = lvData.SelectedItem;
                        var mat = db.Mat.Find(selectedMat.id);
                        if (mat != null)
                        {
                            var dialog = new EditMatWindow(mat);
                            if (dialog.ShowDialog() == true)
                            {
                                db.SaveChanges();
                                LoadData(currentView);
                            }
                        }
                        break;

                    

                    case "Услуги":
                        dynamic selectedUslugi = lvData.SelectedItem;
                        var uslugi = db.Uslugi.Find(selectedUslugi.id);
                        if (uslugi != null)
                        {
                            var dialog = new EditUslugiWindow(uslugi);
                            if (dialog.ShowDialog() == true)
                            {
                                db.SaveChanges();
                                LoadData(currentView);
                            }
                        }
                        break;

                    case "Оплаты":
                        dynamic selectedOplata = lvData.SelectedItem;
                        var oplata = db.Oplata.Find(selectedOplata.id);
                        if (oplata != null)
                        {
                            var dialog = new EditOplataWindow(oplata);
                            if (dialog.ShowDialog() == true)
                            {
                                db.SaveChanges();
                                LoadData(currentView);
                            }
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

            var result = MessageBox.Show("Вы уверены, что хотите удалить запись?",
                                       "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;

            try
            {
                switch (currentView)
                {
                    case "Договоры":
                        dynamic selectedDogovor = lvData.SelectedItem;
                        var dogovor = db.Dogovor.Find(selectedDogovor.id);
                        if (dogovor != null)
                        {
                            db.Dogovor.Remove(dogovor);
                            db.SaveChanges();
                            LoadData(currentView);
                        }
                        break;
                        // Аналогичные case для других сущностей...
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Функция отчета будет реализована в следующей версии",
                          "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            db?.Dispose();
        }
    }
}