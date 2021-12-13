using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WorkNotepadLibrary
{
    public class WorkNotepad
    {
        int _size;
        public int Size { get => _size; set => _size = ProveValue(value) ? value : throw new Exception("Неудовлетворительное число для данного свойства"); }
        public object Style { get; set; }
        public string ImagePath { get; set; }
        public WorkNotepad()
        {
            try
            {
                LoadSettings();
            }
            catch//Задавать стиль для текста в блокноте нужно по умолчанию через интерефейс :3
            {
                _size = 12;               
                SaveSettings();
            }
        }
        public bool ProveValue(int value)
        {
            if (value >= 2) return true;
            return false;
        }
        public static string OpenFile(string filename) 
        {
            var outfile = new StreamReader(filename);
            string owntext = Convert.ToString(outfile.ReadToEnd());
            outfile.Close();
            return owntext;
        }
        public static void SaveFile(string filename, in string text)
        {
            StreamWriter infile = new StreamWriter(filename);
            infile.WriteLine(text);
            infile.Close();
        }
        public void SaveSizeAndStyleIntoObject(int size, object style)
        {            
            Size = size;
            Style = style;
        }
        public void SaveSettings()
        {
            var savefile = new StreamWriter("config.ini");
            savefile.WriteLine(_size);
            savefile.WriteLine(Style);
            savefile.WriteLine(ImagePath);
            savefile.Close();
        }
        public void LoadSettings()
        {
            var savefile = new StreamReader("config.ini");
            try
            {
                Size = Convert.ToInt32(savefile.ReadLine());
                Style = savefile.ReadLine();
                ImagePath = savefile.ReadLine();
                savefile.Close();
            }
            catch
            {
                savefile.Close();
            }
        }
    }
}
