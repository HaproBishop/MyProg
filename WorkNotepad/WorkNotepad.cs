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
        const int _defaultfontsize = 12;
        int _fontsize;
        public int FontSize { get => _fontsize; set => _fontsize = ProveValue(value) ? value : throw new Exception("Неудовлетворительное число для данного свойства"); }       
        public string FontFamily { get; set; }
        public string ImagePath { get; set; }
        private string FileName { get; set; }
        public WorkNotepad()
        {
            try
            {
                LoadSettings();
            }
            catch//Задавать стиль для текста в блокноте нужно по умолчанию через интерефейс :3
            {
                _fontsize = _defaultfontsize;               
                SaveSettings();
            }
        }
        public bool ProveValue(int value)
        {
            if (value >= 2 && value <= 72) return true;
            return false;
        }
        public string GetFileName()
        {
            return FileName;
        }
        public string OpenFile(string filename) 
        {
            FileName = filename;
            var outfile = new StreamReader(filename);
            string owntext = Convert.ToString(outfile.ReadToEnd());
            outfile.Close();
            return owntext;
        }
        public void SaveFile(string filename, in string text)
        {
            FileName = filename;
            StreamWriter infile = new StreamWriter(filename);
            infile.WriteLine(text);
            infile.Close();
        }
        public void SaveSizeAndStyleIntoObject(int size, string fontfamily)
        {            
            FontSize = size;
            FontFamily = fontfamily;
        }
        public void SaveSettings()
        {
            var savefile = new StreamWriter("config.ini");
            savefile.WriteLine(_fontsize);
            savefile.WriteLine(FontFamily);
            savefile.WriteLine(ImagePath);
            savefile.Close();
        }
        public void LoadSettings()
        {
            var savefile = new StreamReader("config.ini");
            try
            {
                FontSize = Convert.ToInt32(savefile.ReadLine());
            }
            finally
            {
                FontFamily = savefile.ReadLine();
                ImagePath = savefile.ReadLine();
                savefile.Close();
            }
        }
        public WorkNotepad Clone()
        {
            return (WorkNotepad)MemberwiseClone();
        }
    }
}
