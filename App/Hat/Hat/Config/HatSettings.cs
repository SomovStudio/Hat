using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.Animation;

namespace Hat
{
    public static class HatSettings
    {
        public const string RUS = "RUS";
        public const string ENG = "ENG";
        public static string fileSettings = Directory.GetCurrentDirectory() + "/settings.cfg";
        public static string language = "";

        public static string systemCurrentLanguage()
        {
            try
            {
                if (CultureInfo.InstalledUICulture.Name == "ru-RU" && CultureInfo.CurrentUICulture.Name == "ru-RU" && CultureInfo.CurrentCulture.Name == "ru-RU")
                {
                    return HatSettings.RUS;
                }
                else
                {
                    return HatSettings.ENG;
                }
            }
            catch (Exception ex)
            {
                Config.browserForm.ConsoleMsgError(ex.ToString());
            }
            return HatSettings.ENG;
        }

        public static bool load()
        {
            try
            {
                string content = "";

                if (!File.Exists(HatSettings.fileSettings))
                {
                    content += "language=" + HatSettings.systemCurrentLanguage();
                    StreamWriter writer;
                    writer = new StreamWriter(HatSettings.fileSettings, false, new UTF8Encoding(true));
                    writer.Write(content);
                    writer.Close();
                }

                WorkOnFiles reader = new WorkOnFiles();
                content = reader.readFile(WorkOnFiles.UTF_8_BOM, HatSettings.fileSettings);
                if (content.Contains("language=RUS") == true) HatSettings.language = HatSettings.RUS;
                else HatSettings.language = HatSettings.ENG;

                return true;
            }
            catch (Exception ex)
            {
                Config.browserForm.ConsoleMsgError(ex.ToString());
            }
            return false;
        }

        public static bool save()
        {
            try
            {
                string content = "";
                content += "language=" + HatSettings.language;
                
                StreamWriter writer;
                writer = new StreamWriter(HatSettings.fileSettings, false, new UTF8Encoding(true));
                writer.Write(content);
                writer.Close();
                return true;
            }
            catch (Exception ex)
            {
                Config.browserForm.ConsoleMsgError(ex.ToString());
            }
            return false;
        }
    }
}
