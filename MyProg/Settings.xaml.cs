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
    /// Логика взаимодействия для Vid.xaml
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
                MainWindow.DataNotepad.SaveSizeAndStyleIntoObject(Convert.ToInt32(Size.Text), ((ComboBoxItem)FamilySelect.SelectedItem).Content.ToString());
            }
            catch
            {
                MessageBox.Show("Сохранение размера и стиля не было завершено, так как введено некорректное значение в поле", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                MainWindow.DataNotepad.ImagePath = ImagePath.Text.ToString();
                MainWindow.DataNotepad.SaveSettings();
            }
        }

        private void LoadImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog loadimage = new OpenFileDialog
            {
                Title = "Выбор картинки",
                Filter = "Фон(.png) | *.png | Фон(.jpg) | *.jpg | Все фоны(.*) | *.*",
                FilterIndex = 1,
                DefaultExt = "*.png",
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
            try
            {
                for (int i = 0; i < FamilySelect.Items.Count; i++)
                {
                    if (FamilySelect.Items[i].ToString() == MainWindow.DataNotepad.FontFamily) FamilySelect.SelectedItem = FamilySelect.Items[i];
                }
            }
            catch
            {
                MessageBox.Show("Значение стиля не было установлено. Будет использоваться по умолчанию.", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
            }           
        }
    }
}
