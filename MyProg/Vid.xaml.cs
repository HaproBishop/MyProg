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
using WorkNotepad;

namespace MyProg
{
    /// <summary>
    /// Логика взаимодействия для Vid.xaml
    /// </summary>
    public partial class Vid : Window
    {
        public Vid()
        {
            InitializeComponent();
        }

        private void LImpact_Selected(object sender, RoutedEventArgs e)
        {
          
        }
        public static void VidClick()
        {
            
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            Class_WorkNotepad.SaveSettings(FontChangeSize_TextBox.Text, StyleSelect.Text);
            
        }

        private void StyleSegoeUI_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void TimesNewRoman_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void StyleArial_ComboBox_Selected(object sender, RoutedEventArgs e)
        {

        }
    }
}
