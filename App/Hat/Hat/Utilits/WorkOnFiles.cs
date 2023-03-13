using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using HatFramework;

namespace Hat
{
    public class WorkOnFiles
    {
        public const string DEFAULT = "DEFAULT";
        public const string UTF_8 = "UTF-8";
        public const string UTF_8_BOM = "UTF-8 BOM";
        public const string WINDOWS_1251 = "WINDOWS-1251";

        public WorkOnFiles()
        {

        }

        public string readFile(string encoding, string filename)
        {
            string content = "";
            try
            {
                StreamReader reader;
                if (encoding == DEFAULT)
                {
                    reader = new StreamReader(filename, Encoding.Default);
                }
                else if (encoding == UTF_8)
                {
                    reader = new StreamReader(filename, new UTF8Encoding(false));
                }
                else if (encoding == UTF_8_BOM)
                {
                    reader = new StreamReader(filename, new UTF8Encoding(true));
                }
                else if (encoding == WINDOWS_1251)
                {
                    reader = new StreamReader(filename, Encoding.GetEncoding("Windows-1251"));
                }
                else
                {
                    reader = new StreamReader(filename, Encoding.Default);
                }
                content = reader.ReadToEnd();
                reader.Close();
            }
            catch (Exception ex)
            {
                Config.browserForm.ConsoleMsgError(ex.ToString());
            }
            return content;
        }

        public List<string> readFileLines(string encoding, string filename, int count)
        {
            List<string> lines = new List<string>();
            try
            {
                StreamReader reader;
                if (encoding == DEFAULT)
                {
                    reader = new StreamReader(filename, Encoding.Default);
                }
                else if (encoding == UTF_8)
                {
                    reader = new StreamReader(filename, new UTF8Encoding(false));
                }
                else if (encoding == UTF_8_BOM)
                {
                    reader = new StreamReader(filename, new UTF8Encoding(true));
                }
                else if (encoding == WINDOWS_1251)
                {
                    reader = new StreamReader(filename, Encoding.GetEncoding("Windows-1251"));
                }
                else
                {
                    reader = new StreamReader(filename, Encoding.Default);
                }

                for (int i = 0; i < count; i++)
                {
                    lines.Add(reader.ReadLine());
                }
                
                reader.Close();
            }
            catch (Exception ex)
            {
                Config.browserForm.ConsoleMsgError(ex.ToString());
            }
            return lines;
        }


        public void writeFile(string content, string encoding, string filename)
        {
            try
            {
                StreamWriter writer;
                if (encoding == DEFAULT)
                {
                    writer = new StreamWriter(filename, false, Encoding.Default);
                }
                else if (encoding == UTF_8)
                {
                    writer = new StreamWriter(filename, false, new UTF8Encoding(false));
                }
                else if (encoding == UTF_8_BOM)
                {
                    writer = new StreamWriter(filename, false, new UTF8Encoding(true));
                }
                else if (encoding == WINDOWS_1251)
                {
                    writer = new StreamWriter(filename, false, Encoding.GetEncoding("Windows-1251"));
                }
                else
                {
                    writer = new StreamWriter(filename, false, Encoding.Default);
                }
                writer.Write(content);
                writer.Close();
            }
            catch (Exception ex)
            {
                //Config.browserForm.ConsoleMsgError(ex.ToString());
                Config.browserForm.ConsoleMsg("Сохранение файла: " + filename + " - неудалось сохранить из за ошибки: " + ex.Message);
                Config.browserForm.ReportAddMessage(Tester.WARNING, "Сохранение файла: " + filename, "Неудалось сохранить файл из за ошибки: " + ex.Message);
            }
        }

        public bool folderCreate(string path, string folderName)
        {
            try
            {
                if (Directory.Exists(path) == true)
                {
                    if (path[path.Count() - 1].ToString() != "/") path += "/";
                    Directory.CreateDirectory(path + folderName);
                    if (Directory.Exists(path + folderName) == true) return true;
                    else return false;
                }
            }
            catch (Exception ex)
            {
                Config.browserForm.ConsoleMsgError(ex.ToString());
            }
            return false;
        }

        public bool folderDelete(string path)
        {
            try
            {
                if (Directory.Exists(path) == true)
                {
                    DialogResult dialogResult = MessageBox.Show($"Вы действительно хотите удалить папку {Config.selectName} со всем её содержимым ?", "Вопрос", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        if (path[path.Count() - 1].ToString() != "/") path += "/";
                        Directory.Delete(path, true);
                        if (Directory.Exists(path) == false) return true;
                        else return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Config.browserForm.ConsoleMsgError(ex.ToString());
            }
            return false;
        }

        public bool folderRename(string path, string folderName)
        {
            try
            {
                if (Directory.Exists(path) == true)
                {
                    if (path[path.Count() - 1].ToString() != "/") path += "/";
                    // не реализовано
                }
            }
            catch (Exception ex)
            {
                Config.browserForm.ConsoleMsgError(ex.ToString());
            }
            return false;
        }

 

    }
}
