using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace VecherVKomnatu
{
    public partial class EditSmetaWindow : Window
    {
        public Smeta Smeta { get; private set; }
        public List<Dogovor> DogovorList { get; private set; }
        public List<Mat> MatList { get; private set; }
        public List<Uslugi> UslugiList { get; private set; }
        private readonly ВечерВКвартируEntities _db;
        public EditSmetaWindow(ВечерВКвартируEntities db, Smeta smeta = null)
        {
            InitializeComponent();
            _db = db;
            Smeta = smeta ?? new Smeta
            {
                dog = 0,
                uslugi = 0,
                mat = 0,
                kolMat = 0
            };
            LoadData();
            DataContext = this;
        }

        public EditSmetaWindow(Smeta smeta) 
        {
            if (smeta != null)
            {
                Smeta = smeta;
                // Проверяем на 0 (аналог null для int)
                if (Smeta.dog < 1) Smeta.dog = 0;
                if (Smeta.uslugi < 1) Smeta.uslugi = 0;
                if (Smeta.mat < 1) Smeta.mat = 0;
            }
        }

        private void LoadData()
        {
            try
            {
                // Вариант 1: Если у вас есть доступ к экземпляру MainWindow
                var mainWindow = Application.Current.MainWindow as MainWindow;
                if (mainWindow != null)
                {
                    DogovorList = mainWindow.db.Dogovor.ToList();
                    MatList = mainWindow.db.Mat.Include(m => m.EdIzmer).ToList();
                    UslugiList = mainWindow.db.Uslugi.ToList();
                }
                else
                {
                    // Вариант 2: Создаем новый контекст, если не можем получить доступ к MainWindow
                    using (var db = new ВечерВКвартируEntities())
                    {
                        DogovorList = db.Dogovor.ToList();
                        MatList = db.Mat.Include(m => m.EdIzmer).ToList();
                        UslugiList = db.Uslugi.ToList();
                    }
                }

                if (DogovorList == null || DogovorList.Count == 0)
                {
                    MessageBox.Show("Нет доступных договоров", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // Проверка обязательных полей
            if (Smeta.dog == 0 || Smeta.uslugi == 0 || Smeta.mat == 0)
            {
                MessageBox.Show("Заполните все обязательные поля: договор, услуга, материал",
                              "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (Smeta.kolMat <= 0)
            {
                MessageBox.Show("Количество материалов должно быть больше 0",
                              "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                // Получаем экземпляр главного окна
                var mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                if (mainWindow == null)
                {
                    MessageBox.Show("Не удалось получить доступ к базе данных",
                                  "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Работа с контекстом БД
                using (var transaction = mainWindow.db.Database.BeginTransaction())
                {
                    try
                    {
                        // Проверяем существование связанных записей
                        if (!mainWindow.db.Dogovor.Any(d => d.id == Smeta.dog) ||
                            !mainWindow.db.Uslugi.Any(u => u.id == Smeta.uslugi) ||
                            !mainWindow.db.Mat.Any(m => m.id == Smeta.mat))
                        {
                            MessageBox.Show("Один из выбранных элементов не существует в базе данных",
                                          "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        // Добавляем или обновляем смету
                        if (Smeta.id == 0) // Новая смета
                        {
                            mainWindow.db.Smeta.Add(Smeta);
                        }
                        else // Редактирование существующей
                        {
                            mainWindow.db.Entry(Smeta).State = EntityState.Modified;
                        }

                        mainWindow.db.SaveChanges();
                        transaction.Commit();

                        DialogResult = true;
                        Close();
                    }
                    catch (DbEntityValidationException ex)
                    {
                        transaction.Rollback();
                        string errorMessage = "Ошибка валидации:\n";
                        foreach (var validationErrors in ex.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                errorMessage += $"- {validationError.PropertyName}: {validationError.ErrorMessage}\n";
                            }
                        }
                        MessageBox.Show(errorMessage, "Ошибка валидации",
                                      MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"Ошибка сохранения: {ex.InnerException?.Message ?? ex.Message}",
                                      "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Критическая ошибка: {ex.Message}",
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