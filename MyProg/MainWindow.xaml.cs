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
            TransparentChanger.Value = 95;
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
            if (DataNotepad.GetFileName() == "" || DataNotepad.GetFileName() == null || e.Source != SaveAs)
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
                    OwnWindow.Title.Replace(OwnWindow.Title, save.FileName + " - " + "Notepad");
                }
            }
            else
            {
                DataNotepad.SaveFile(OwnText.Text);
                OwnWindow.Title = OwnWindow.Title.Replace("*", "");
            }
        }

        private void FontSettings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingswindow = new SettingsWindow
            {
                Owner = this
            };            
            settingswindow.ShowDialog();
                OwnText.FontSize = DataNotepad.FontSize;
                OwnText.FontFamily = new FontFamily(DataNotepad.FontFamily);
                SetBoldAndCursive();
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
        private void SetBoldAndCursive()
        {
            if (DataNotepad.FontStyleItalic) OwnText.FontStyle = FontStyles.Italic;
            else OwnText.FontStyle = FontStyles.Normal;
            if (DataNotepad.FontWeightBold) OwnText.FontWeight = FontWeights.Bold;
            else OwnText.FontWeight = FontWeights.Normal;
        }
        private void AboutProgram_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Name of the developer must be here? :D\n" +
                "Ok. Hapro is developer :D\n" +
                "For pleasure :3", "О программе", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private readonly RoutedCommand _newWindow = new RoutedCommand();//Новый эвент без добавление в xaml (Для этого создан отдельный класс, если понадобится шаблон(будет разработана отдельная шаблонная библиотека в будущем)))
        //Упрощенный способ добавления горячих клавиш. Спасибо автору plaasmeisie на сайте askdev.ru за ответ(набрал лишь 2 лайка, а оказался во много раз полезней, в плане простоты использования command)
        private void OwnWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _newWindow.InputGestures.Add(new KeyGesture(Key.N, ModifierKeys.Control|ModifierKeys.Shift));//Добавление горячих клавиш
            OwnWindow.CommandBindings.Add(new CommandBinding(_newWindow, NewWindow_Click));//Добавление в свойство CommandsBindings новых сведений о горяей клавише и ее действии
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
            try
            {
                OwnText.FontSize = DataNotepad.FontSize;
            }
            catch
            {
                DataNotepad.FontSize = 12;
            }
            finally
            {
                if(DataNotepad.FontFamily != "")
                OwnText.FontFamily = new FontFamily(DataNotepad.FontFamily);
                SetBoldAndCursive();
                if (DataNotepad.IsWrap) OwnText.TextWrapping = TextWrapping.Wrap;
                else OwnText.TextWrapping = TextWrapping.NoWrap;
                if (!DataNotepad.IsStatusBar)
                {
                    _reservedHeight = Status.Height;
                    Status.Height = 0;
                    OwnWindow.MinWidth = 300;
                    ShowStatus.IsChecked = false;
                }
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
            if (!DataNotepad.IsSavedOwnText)
            {
                QuestionClose(sender, e);
                _wasCancel = false;
            }
            else
            {
                DefaultClearData();
            }
        }
        private void DefaultClearData()
        {
            OwnText.Clear();
            OwnWindow.Title = "Без имени - " + "Notepad";
            DataNotepad.Clear();
        }

        private void NewWindow_Click(object sender, RoutedEventArgs e)
        {
            MainWindow newwindow = new MainWindow();
            newwindow.Show();
        }

        private void All_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if(e.Command == CCreate.Command) Create_Click(sender, new RoutedEventArgs());
            if(e.Command == COpen.Command) Open_Click(sender, new RoutedEventArgs());
            if (e.Command == CSave.Command) Save_Click(sender, new RoutedEventArgs());
            if (e.Command == CSaveAs.Command) Save_Click(sender, new RoutedEventArgs());
        }

        private void CanExecuteHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CloseCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }
        bool _wasCancel;//Используется для проверки на наличие ответа "Cancel"
        private void OwnWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!DataNotepad.IsSavedOwnText)
            {
                QuestionClose(sender, new RoutedEventArgs());
                if (_wasCancel)
                {
                    e.Cancel = _wasCancel;
                    _wasCancel = false;
                }
                else
                {
                    DataNotepad.SaveSettings();
                }
            }
        }
        private void QuestionClose(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Хотите сохранить текущие изменения?", "Сохранение изменений", MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                Save_Click(sender, e);
                DefaultClearData();
            }
            if (result == MessageBoxResult.No) DefaultClearData();
            if (result == MessageBoxResult.Cancel) _wasCancel = true;
        }
        private void OwnText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (OwnText.Text != "" && DataNotepad.IsSavedOwnText)
            {
                DataNotepad.IsSavedOwnText = false;
                OwnWindow.Title = "*" + OwnWindow.Title;
            }
            else if (OwnText.Text == "")
            {
                DataNotepad.IsSavedOwnText = true;
                OwnWindow.Title = OwnWindow.Title.Replace("*", "");
            }
        }
        double _reservedHeight;
        private void ShowStatus_Click(object sender, RoutedEventArgs e)
        {
            if (ShowStatus.IsChecked == true)
            {
                DataNotepad.IsStatusBar = true;
                Status.Height = _reservedHeight;
                OwnWindow.MinWidth = 750;
            }
            else
            {
                DataNotepad.IsStatusBar = false;
                _reservedHeight = Status.Height;
                Status.Height = 0;
                OwnWindow.MinWidth = 300;
            }
        }

        private void WrapSwitcher_Click(object sender, RoutedEventArgs e)
        {
            if (WrapSwitcher.IsChecked == true)
            {
                OwnText.TextWrapping = TextWrapping.Wrap;
                DataNotepad.IsWrap = true;
            }
            else
            {
                OwnText.TextWrapping = TextWrapping.NoWrap;
                DataNotepad.IsWrap = false;
            }
        }
        private void TransparentChanger_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
              TransparentPercent.Text = TransparentChanger.Value.ToString();
              OwnText.Background.Opacity = TransparentChanger.Value / 100;
        }
    }
}
