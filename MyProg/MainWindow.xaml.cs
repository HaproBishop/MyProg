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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WorkNotepadLibrary;
using System.Windows.Threading;

namespace MyProg
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();                
        }
        private static WorkNotepad _datanotepad = new WorkNotepad();
        public static WorkNotepad DataNotepad { get => _datanotepad; set => _datanotepad = value.Clone(); }
        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog
            {
                Title = "Открыть",
                Filter = "Текстовый документ(.txt) | *.txt",
                DefaultExt = ".txt",
            };
            if (open.ShowDialog() == true)
            {
                OwnText.Text = DataNotepad.OpenFile(open.FileName);
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (DataNotepad.GetFileName() == "")
            {
                SaveFileDialog save = new SaveFileDialog
                {
                    Title = "Сохранить как",
                    Filter = "Текстовый документ(.txt) | *.txt",
                    DefaultExt = ".txt",
                };
                if (save.ShowDialog() == true)
                {
                    DataNotepad.SaveFile(save.FileName, OwnText.Text);
                }
            }
            else DataNotepad.SaveFile(OwnText.Text);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingswindow = new SettingsWindow
            {
                Owner = this
            };            
            settingswindow.ShowDialog();
                OwnText.FontSize = DataNotepad.FontSize;
                OwnText.FontFamily = new FontFamily(DataNotepad.FontFamily);
            if (DataNotepad.FontStyleItalic) OwnText.FontStyle = FontStyles.Italic;
            else OwnText.FontStyle = FontStyles.Normal;
            if (DataNotepad.FontWeightBold) OwnText.FontWeight = FontWeights.Bold;
            else OwnText.FontWeight = FontWeights.Normal;
            if (DataNotepad.ImagePath != "")
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(DataNotepad.ImagePath);
                image.EndInit();
                BackgroundImage.Source = image;
                DefaultBackground.Background = Brushes.Black;
            }
       }

        private void AboutProgram_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Name of the developer must be here? :D\n" +
                "Ok. Hapro is developer :D\n" +
                "For pleasure :3", "О программе", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OwnWindow_Loaded(object sender, RoutedEventArgs e)
        {
            OwnWindow.Title = "Без имени - " + OwnWindow.Title;
            DispatcherTimer time = new DispatcherTimer();
            time.Tick += Time_Tick;
            time.Interval = new TimeSpan(0,0,1);
            time.IsEnabled = true;
            DataNotepad.LoadSettings();
            SetBeginSettings();
        }

        private void Time_Tick(object sender, EventArgs e)
        {
            DateTime data = DateTime.Now;
            Time.Text = data.ToString("HH:mm");
            Date.Text = data.ToString("dd.MM.yyyy");
        }
        private void SetBeginSettings()
        {
            OwnText.FontSize = DataNotepad.FontSize;
            try
            {
                OwnText.FontFamily = new FontFamily(DataNotepad.FontFamily);
            }
            catch
            {
                MessageBox.Show("Стиль текста не установлен, поэтому был установлен по умолчанию (Segou UI)","Уведомление",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (_datanotepad.ImagePath != "")
                {
                    try
                    {
                        BitmapImage image = new BitmapImage();
                        image.BeginInit();
                        image.UriSource = new Uri(DataNotepad.ImagePath);
                        image.EndInit();
                        BackgroundImage.Source = image;
                        DefaultBackground.Background = Brushes.Black;
                    }
                    catch
                    {

                        MessageBox.Show("Фон не был загружен для блокнота! Возможно файл был перемещен или " +
                            "поврежден!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
                        _datanotepad.ImagePath = "";
                        _datanotepad.SaveSettings();
                    }
                }
            }
        }
        private void Create_Click(object sender, RoutedEventArgs e)
        {
            if (!DataNotepad.IsSavedOwnText) Save_Click(sender, e);
            else MessageAboutSave();
        }

        private void OwnText_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            DataNotepad.IsSavedOwnText = false;
        }
        private void MessageAboutSave()
        {
            MessageBox.Show("");
        }
    }
}
