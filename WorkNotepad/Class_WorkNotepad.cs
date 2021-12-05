using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WorkNotepad
{
    public class Class_WorkNotepad
    {
        public static string OpenFile(string filename) 
        {
            string newtext;
            StreamReader outfile = new StreamReader(filename);
            newtext = Convert.ToString(outfile.ReadToEnd());
            outfile.Close();
            return newtext;
        }
        public static void SaveFile(string filename, in string text)
        {
            StreamWriter infile = new StreamWriter(filename);
            infile.WriteLine(text);
            infile.Close();
        }
        public static void SaveSettings(string size, string style)
        {
            StreamWriter savefile = new StreamWriter("config.ini");
            savefile.WriteLine(size);
            savefile.WriteLine(style);
            savefile.Close();
        }
    }
}
