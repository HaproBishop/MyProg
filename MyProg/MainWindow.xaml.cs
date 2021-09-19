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
using WorkNotepad;

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
                OwnText.Text = Class_WorkNotepad.OpenFile(open.FileName);
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
                Class_WorkNotepad.SaveFile(save.FileName, OwnText.Text);
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Vid_Click(object sender, RoutedEventArgs e)
        {
            Vid windowvid = new Vid();
            if (windowvid.ShowDialog() == true) 
            {
                OwnText.Text = "AHAHA";
            }
        }

        private void AbProg_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
