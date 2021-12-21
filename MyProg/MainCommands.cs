using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyProg
{
    public static class MainCommands
    {
        static MainCommands()
        {
            InputGestureCollection input = new InputGestureCollection();
            input.Add(new KeyGesture(Key.N, ModifierKeys.Control|ModifierKeys.Shift,"Ctrl+Shift+N"));
            NewWindow = new RoutedUICommand("AddNewWindowOfMainProgram", "NewWindow", typeof(MainCommands), input);
        }
        public static RoutedCommand NewWindow { get; private set; }
    }
}
