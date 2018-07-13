using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace DateTaken
{
    public class Program
    {
        private static Regex r = new Regex(":");

        private static DateTime GetDateTakenFromImage(string path)
        {
            var result = DateTime.MaxValue;
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                try
                {
                    using (Image myImage = Image.FromStream(fs, false, false))
                    {
                        PropertyItem propItem = myImage.GetPropertyItem(36867);
                        string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                        result = DateTime.Parse(dateTaken);
                    }
                }
                catch
                {
                }
            return result;
        }

        private static void ProcessFolder(string path, bool move)
        {
            var di = new DirectoryInfo(path);
            if (di.Exists)
            {
                foreach (var fi in di.GetFiles())
                {
                    var srcFile = fi.FullName;
                    var date = GetDateTakenFromImage(srcFile);
                    if (date != DateTime.MaxValue)
                    {
                        var dstPath = String.Format(@"{0}\{1}\{2:00}\{3:00}", path, date.Year, date.Month, date.Day);
                        Directory.CreateDirectory(dstPath);
                        var dstFile = String.Format(@"{0}\{1}", dstPath, fi.Name);
                        if (move)
                        {
                            File.Move(srcFile, dstFile);
                        }
                        else
                        {
                            File.Copy(srcFile, dstFile);
                        }
                        Console.WriteLine(dstFile);
                    }
                }
            }
        }

        public static void Main(string[] args)
        {
            ProcessFolder(@"C:\IMAGES\101APPLE\", false);
            Console.WriteLine("Done!");
            Console.ReadKey();
        }
    }
}