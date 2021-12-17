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
        private void SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow.DataNotepad.SaveSizeAndStyleIntoObject(Convert.ToInt32(Size.Text), FamilySelect.Text);
                MainWindow.DataNotepad.ImagePath = ImagePath.Text.ToString();
                MainWindow.DataNotepad.FontStyleItalic = (bool)Cursive.IsChecked;
                MainWindow.DataNotepad.FontWeightBold = (bool)Bold.IsChecked;
                MainWindow.DataNotepad.SaveSettings();
                MessageBox.Show("Сохранение выполнено успешно", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                MessageBox.Show("Сохранение не было завершено, так как введены некорректно значения размера шрифта", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
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
        }

        private void SettingsWin_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!MainWindow.DataNotepad.IsSavedCfg)
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите выйти без сохранения изменений",
                    "Выход", MessageBoxButton.YesNoCancel, MessageBoxImage.Information);
                if (result == MessageBoxResult.Yes)
                {
                    RoutedEventArgs aEvent = new RoutedEventArgs();
                    SaveSettings_Click(sender, aEvent);
                }
                if (result == MessageBoxResult.Cancel) e.Cancel = true;
            }
        }
    }
}
