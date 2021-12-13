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
        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog
            {
                Title = "Открытие файла",
                Filter = "Файл(.txt) | *.txt",
                DefaultExt = ".txt",
            };
            if (open.ShowDialog() == true)
            {
                OwnText.Text = WorkNotepad.OpenFile(open.FileName);
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog
            {
                Title = "Сохранение файла",
                Filter = "Файл(.txt) | *.txt",
                DefaultExt = ".txt",
            };
            if (save.ShowDialog() == true)
            {
                WorkNotepad.SaveFile(save.FileName, OwnText.Text);
            }
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
       }

        private void AboutProgram_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Name of the developer must be here? :D\n" +
                "Ok. Hapro is developer :D\n" +
                "For pleasure :3");
        }

        private void OwnWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DispatcherTimer time = new DispatcherTimer();
            time.Tick += Time_Tick;
            time.Interval = new TimeSpan(0,0,1);
            time.IsEnabled = true;
        }

        private void Time_Tick(object sender, EventArgs e)
        {
            DateTime data = DateTime.Now;
            Time.Text = data.ToString("HH:mm");
            Date.Text = data.ToString("dd.MM.yyyy");
        }
    }
}
