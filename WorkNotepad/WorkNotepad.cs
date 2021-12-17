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
        string _fontFamily;
        bool _fontStyleItalic;
        bool _fontWeightBold;
        string _imagePath;
        string _cancelledFontFamily;
        bool _cancelledFontStyleItalic;
        string _link;
        public int FontSize { get => _fontSize; set => _fontSize = ProveValue(value) ? value : throw new Exception("Неудовлетворительное число для данного свойства"); }
        public string FontFamily { get => _fontFamily; set { _link = "_fontFamily"; _fontFamily = value;  } }
        public bool FontStyleItalic 
        { get => _fontStyleItalic; 
            set 
            { 
                _link = "_fontStyleItalic"; 
                _fontStyleItalic = value; 
                IsSavedCfg = false; 
            } 
        }
        public bool FontWeightBold { get => _fontWeightBold; set { _link = "_fontWeightBold"; _fontWeightBold = value; IsSavedCfg = false; } }
        public string ImagePath { get => _imagePath; set { _imagePath = value; IsSavedCfg = false; } }
        private string FileName { get; set; }
        public bool IsSavedCfg { get; private set; }
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
            if (value >= 2 && value <= 72) return true;
            return false;
        }
        private bool ComparingPreviousAndCurrent<T>(T value)
        {
            try
            {
                bool Avalue = Convert.ToBoolean(value);
                if()
            }
            catch
            {

            }
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
            savefile.WriteLine(_fontSize);
            savefile.WriteLine(FontFamily);
            savefile.WriteLine(ImagePath);
            savefile.WriteLine(FontStyleItalic);
            savefile.WriteLine(FontWeightBold);
            savefile.Close();
            IsSavedCfg = true;
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
                _cancelledChangesOfBools[0] = FontStyleItalic = Convert.ToBoolean(savefile.ReadLine());
                _cancelledChangesOfBools[1] = FontWeightBold = Convert.ToBoolean(savefile.ReadLine());
                savefile.Close();
                IsSavedCfg = true;
            }
        }
        public WorkNotepad Clone()
        {
            return (WorkNotepad)MemberwiseClone();
        }
    }
}
