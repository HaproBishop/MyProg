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
            ListSize.ItemsSource = new List<int>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
        }
        public static bool _wasSave;
        public static WorkNotepad DataNotepadSettings { get; set; }
        private void SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataNotepadSettings.SaveSizeAndStyleIntoObject(Convert.ToInt32(FontSizeValue.Text), FontFamilySelect.Text);
                DataNotepadSettings.ImagePath = ImagePath.Text.ToString();
                DataNotepadSettings.FontStyleItalic = (bool)FontStyleItalic.IsChecked;
                DataNotepadSettings.FontWeightBold = (bool)FontWeightBold.IsChecked;
                DataNotepadSettings.SaveSettings();
                _wasSave = true;
                _imageLoaded = false;
                MessageBox.Show("Сохранение выполнено успешно", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                MessageBox.Show("Сохранение не было завершено, так как введены некорректно значения размера шрифта", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        bool _imageLoaded;
        private void LoadImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog loadimage = new OpenFileDialog
            {
                Title = "Выбор картинки",
                InitialDirectory = @"C:\Users\" + Environment.UserName + @"\Pictures",
                Filter = "Фон(*.png, *.jpg, *.jpeg)| *.png;*.jpg;*.jpeg | Все фоны(*.*) | *.*",
                DefaultExt = "png",
            };
            if (loadimage.ShowDialog() == true)
            {
                ImagePath.Text = DataNotepadSettings.ImagePath = loadimage.FileName;
                _imageLoaded = true;
            }
        }

        private void SettingsWin_Loaded(object sender, RoutedEventArgs e)
        {
            FontSizeValue.Text = DataNotepadSettings.FontSize.ToString();
            ImagePath.Text = DataNotepadSettings.ImagePath;
            FontStyleItalic.IsChecked = DataNotepadSettings.FontStyleItalic;
            FontWeightBold.IsChecked = DataNotepadSettings.FontWeightBold;
            if (DataNotepadSettings.FontFamily != "") FontFamilySelect.Text = DataNotepadSettings.FontFamily;
            else
            {
                MessageBox.Show("Значение стиля не было установлено. Будет использоваться по умолчанию - Segou UI", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            if (FontFamilySelect.Text == "")
            {
                MessageBox.Show("Информация стиля была загружена некорректно, поэтому выбран стиль по умолчанию - Segou UI",
                    "Некорректность задания стиля", MessageBoxButton.OK, MessageBoxImage.Warning);
                FontFamilySelect.Text = DataNotepadSettings.FontFamily;
            }
            _wasSave = false;
        }

        private void SettingsWin_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DataNotepadSettings.FontFamily != FontFamilySelect.Text ||
                DataNotepadSettings.FontStyleItalic != FontStyleItalic.IsChecked ||
                DataNotepadSettings.FontWeightBold != FontWeightBold.IsChecked ||
                DataNotepadSettings.FontSize != Convert.ToInt32(FontSizeValue.Text) ||
                _imageLoaded)
            {
                MessageBoxResult result = MessageBox.Show("Изменения не были сохранены. Вы хотите сохранить их перед выходом?",
                    "Выход", MessageBoxButton.YesNoCancel, MessageBoxImage.Information);
                if (result == MessageBoxResult.Yes)
                {
                    RoutedEventArgs aEvent = new RoutedEventArgs();
                    SaveSettings_Click(sender, aEvent);
                }
                if (result == MessageBoxResult.No) _wasSave = false;
                if (result == MessageBoxResult.Cancel) e.Cancel = true;
            }
        }
        bool _firstChangeFamily = true;
        private void FontFamilySelect_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_firstChangeFamily)
            {
                FontExample.FontFamily = new FontFamily(FontFamilySelect.Text);
            }
            _firstChangeFamily = false;
        }

        private void ListFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FontFamilySelect.Text = ListFontFamily.SelectedValue.ToString();
        }

        private void ListSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FontSizeValue.Text = ListSize.SelectedValue.ToString();
        }
        bool _firstChangeSize = true;
        private void FontSizeValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_firstChangeSize)
            {
                FontExample.FontSize = Convert.ToInt32(FontSizeValue.Text);
            }
            _firstChangeSize = false;
        }
    }
}
