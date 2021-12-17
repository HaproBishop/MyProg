using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WorkNotepadLibrary;

namespace MyProg
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }
        bool _savedCfg;
        private void SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow.DataNotepad.SaveSizeAndStyleIntoObject(Convert.ToInt32(Size.Text), FamilySelect.Text);
            }
            catch
            {
                MessageBox.Show("Сохранение размера и стиля не было завершено, так как введено некорректное значение в поле", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                _savedCfg = false;
            }
            finally
            {
                MainWindow.DataNotepad.ImagePath = ImagePath.Text.ToString();
                MainWindow.DataNotepad.FontStyleItalic = (bool)Cursive.IsChecked;
                MainWindow.DataNotepad.FontWeightBold = (bool)Bold.IsChecked;
                MainWindow.DataNotepad.SaveSettings();
                MessageBox.Show("Сохранение выполнено успешно", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void LoadImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog loadimage = new OpenFileDialog
            {
                Title = "Выбор картинки",
                Filter = "Фон(.png) | *.png | Фон(.jpg) | *.jpg | Все фоны(*.*) | *.*",
                FilterIndex = 1,
                DefaultExt = ".png",
            };
            if (loadimage.ShowDialog() == true)
            {
                ImagePath.Text = MainWindow.DataNotepad.ImagePath = loadimage.FileName;
                _savedCfg = false;
            }
        }

        private void SettingsWin_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindow.DataNotepad.LoadSettings();
            Size.Text = MainWindow.DataNotepad.FontSize.ToString();
            ImagePath.Text = MainWindow.DataNotepad.ImagePath;
            Cursive.IsChecked = MainWindow.DataNotepad.FontStyleItalic;
            Bold.IsChecked = MainWindow.DataNotepad.FontWeightBold;
            if(MainWindow.DataNotepad.FontFamily != "") FamilySelect.Text = MainWindow.DataNotepad.FontFamily;
            else MessageBox.Show("Значение стиля не было установлено. Будет использоваться по умолчанию.", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
            _savedCfg = true;
        }

        private void SettingsWin_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы ");
        }

        private void Size_TextChanged(object sender, TextChangedEventArgs e)
        {
            _savedCfg = false;
        }
    }
}
