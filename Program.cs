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
            var files = 0;
            var skipped = 0;
            var di = new DirectoryInfo(path);
            if (di.Exists)
            {
                foreach (var fi in di.GetFiles())
                {
                    var srcFile = fi.FullName;
                    var date = GetDateTakenFromImage(srcFile);
                    if (date == DateTime.MaxValue)
                    {
                        skipped++;
                    }
                    else
                    {
                        var dstPath = String.Format(@"{0}\{1}\{2:00}\{3:00}", path, date.Year, date.Month, date.Day);
                        Directory.CreateDirectory(dstPath);
                        var dstFile = String.Format(@"{0}\{1}", dstPath, fi.Name);
                        if (File.Exists(dstFile))
                        {
                            Console.WriteLine(String.Format("File Exists: {0}!", dstFile));
                        }
                        else
                        {
                            if (move)
                            {
                                Console.WriteLine(String.Format("Moving: {0}", dstFile));
                                // File.Move(srcFile, dstFile);
                            }
                            else
                            {
                                Console.WriteLine(String.Format("Copying: {0}", dstFile));
                                // File.Copy(srcFile, dstFile);
                            }
                            files++;
                        }
                    }
                }
            }
            Console.WriteLine($"Files copied/moved: {files}");
            Console.WriteLine($"Files skipped: {skipped}");
        }

        public static void Main(string[] args)
        {
            ProcessFolder(@"D:\00-NO_SYNC\FOTOS\2020-2021\106APPLE\", false);
            Console.WriteLine("Done!");
            Console.ReadKey();
        }
    }
}