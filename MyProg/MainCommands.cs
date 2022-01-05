﻿using System;
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
            InputGestureCollection input1 = new InputGestureCollection()
            {
                new KeyGesture(Key.OemPlus, ModifierKeys.Control, "Ctrl+Плюс(+)") 
            };
            CommandScalePlus = new RoutedUICommand("CommandScalePlus", "MainCommands", typeof(MainCommands), input1);
            InputGestureCollection input2 = new InputGestureCollection()
            {
                new KeyGesture(Key.OemMinus, ModifierKeys.Control, "Ctrl+Минус(-)")
            };
            CommandScaleMinus = new RoutedUICommand("CommandScaleMinus", "MainCommands", typeof(MainCommands), input2);
            InputGestureCollection input3 = new InputGestureCollection()
            {
                new KeyGesture(Key.D9|Key.D0, ModifierKeys.Control, "Ctrl+()")
            };
            CommandDefaultScale = new RoutedUICommand("CommandDefaultScale", "MainCommands", typeof(MainCommands), input3);
            InputGestureCollection input4 = new InputGestureCollection()
            {
                new KeyGesture(Key.F5, ModifierKeys.None, "F5")
            };
            CommandCurrentDateAndTime = new RoutedUICommand("CommandCurrentDateAndTime", "MainCommands", typeof(MainCommands), input4);
        }
        public static RoutedCommand CommandScalePlus { get; private set; }
        public static RoutedCommand CommandScaleMinus { get; private set; }
        public static RoutedCommand CommandDefaultScale { get; private set; }
        public static RoutedCommand CommandCurrentDateAndTime { get; private set; }
    }
}
