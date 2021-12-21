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
        const int _defaultFontSize = 12;
        int _fontSize;
        public int FontSize { get => _fontSize; set => _fontSize = ProveValue(value) ? value : throw new Exception("Неудовлетворительное число для данного свойства"); }
        public string FontFamily { get; set; }
        public bool FontStyleItalic { get; set; }
        public bool FontWeightBold { get; set; }
        public string ImagePath { get; set; }
        private string FileName { get; set; }
        public bool IsSavedOwnText { get; set; }
        public WorkNotepad()
        {
            try
            {
                LoadSettings();
            }
            catch//Задавать стиль для текста в блокноте нужно по умолчанию через интерефейс :3
            {
                _fontSize = _defaultFontSize;               
                SaveSettings();
            }
        }
        public bool ProveValue(int value)
        {
            if (value >= 8 && value <= 72) return true;
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
        public void SaveFile(in string owntext)
        {
            StreamWriter infile = new StreamWriter(FileName);
            infile.WriteLine(owntext);
            infile.Close();
        }
        public void SaveFile(string filename, in string owntext)
        {
            FileName = filename;
            SaveFile(in owntext);
        }
        public void SaveSizeAndStyleIntoObject(int fontsize, string fontfamily)
        {            
            FontSize = fontsize;
            FontFamily = fontfamily;
        }
        public void SaveSettings()
        {
            var savefile = new StreamWriter("config.ini");
            savefile.WriteLine(_fontSize);
            savefile.WriteLine(FontFamily);
            savefile.WriteLine(ImagePath);
            savefile.WriteLine(FontStyleItalic);
            savefile.WriteLine(FontWeightBold);
            savefile.Close();
            IsSavedOwnText = true;
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
                FontStyleItalic = Convert.ToBoolean(savefile.ReadLine());
                FontWeightBold = Convert.ToBoolean(savefile.ReadLine());
                savefile.Close();
                IsSavedOwnText = true;
            }
        }
        public WorkNotepad Clone()
        {
            return (WorkNotepad)MemberwiseClone();
        }
        public void Clear()
        {
            FileName = "";
            IsSavedOwnText = true;
        }
    }
}
