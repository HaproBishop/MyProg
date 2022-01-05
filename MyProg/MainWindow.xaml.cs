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
                OwnWindow.Title.Replace(OwnWindow.Title, open.FileName + " - " + "Notepad");

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
            if (SettingsWindow._wasSave)
            {
                OwnText.FontSize = DataNotepad.FontSize * (Convert.ToDouble(CurrentScale.Text)/100);
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
                SettingsWindow._wasSave = false;
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
        DateTime data;
        private void Time_Tick(object sender, EventArgs e)
        {
            data = DateTime.Now;
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
        /// <summary>
        /// Событие для всех команд (сделано для удобства)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void All_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == CCreate.Command) Create_Click(sender, new RoutedEventArgs());
            if (e.Command == COpen.Command) Open_Click(sender, new RoutedEventArgs());
            if (e.Command == CSave.Command) Save_Click(sender, new RoutedEventArgs());
            if (e.Command == CSaveAs.Command) Save_Click(sender, new RoutedEventArgs());
            if (e.Command == CCurrentDateAndTime.Command) CurrentDateAndTime_Click(sender, new RoutedEventArgs());
            if (e.Command == CScalePlus.Command) ScalePlus_Click(sender, new RoutedEventArgs());
            if (e.Command == CScaleMinus.Command) ScaleMinus_Click(sender, new RoutedEventArgs());
            if (e.Command == CDefaultScale.Command) DefaultScale_Click(sender, new RoutedEventArgs());
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
            if (OwnText.Text == "")
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

        private void ScaleMinus_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentScale.Text != "10")
            {
                CurrentScale.Text = (Convert.ToInt32(CurrentScale.Text) - 10).ToString();
                OwnText.FontSize = DataNotepad.FontSize * (Convert.ToDouble(CurrentScale.Text) / 100); 
            }
        }

        private void ScalePlus_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentScale.Text != "500")
            {
                CurrentScale.Text = (Convert.ToInt32(CurrentScale.Text) + 10).ToString();
                OwnText.FontSize = DataNotepad.FontSize * (Convert.ToDouble(CurrentScale.Text) / 100);
            }
        }

        private void OwnText_LayoutUpdated(object sender, EventArgs e)
        {
            CurrentRow.Text = (OwnText.GetLineIndexFromCharacterIndex(OwnText.CaretIndex) + 1).ToString();
            CurrentColumn.Text = (OwnText.CaretIndex + 1).ToString();
        }

        private void DefaultScale_Click(object sender, RoutedEventArgs e)
        {
            OwnText.FontSize = DataNotepad.FontSize;
            CurrentScale.Text = "100";
        }

        private void OwnText_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (OwnText.Text == "" || DataNotepad.IsSavedOwnText)
            {
                DataNotepad.IsSavedOwnText = false;
                OwnWindow.Title = "*" + OwnWindow.Title;
            }
        }

        private void UndoMenu_Click(object sender, RoutedEventArgs e)
        {
            OwnText.Undo();
        }
        private void RedoMenu_Click(object sender, RoutedEventArgs e)
        {
            OwnText.Redo();
        }

        private void CutMenu_Click(object sender, RoutedEventArgs e)
        {
            OwnText.Cut();
        }

        private void CopyMenu_Click(object sender, RoutedEventArgs e)
        {
            OwnText.Copy();
        }

        private void PasteMenu_Click(object sender, RoutedEventArgs e)
        {
            OwnText.Paste();
        }

        private void DelMenu_Click(object sender, RoutedEventArgs e)
        {
            OwnText.Text = OwnText.Text.Replace(OwnText.SelectedText, "");
        }

        private void SelectAllMenu_Click(object sender, RoutedEventArgs e)
        {
            OwnText.SelectAll();
        }

        private void CurrentDateAndTime_Click(object sender, RoutedEventArgs e)
        {
            OwnText.Text += data.ToString("dd.MM.yyyy") + " " + data.ToString("HH:mm");
        }

    }
}
