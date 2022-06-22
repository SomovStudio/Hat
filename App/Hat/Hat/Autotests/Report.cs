using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hat
{
    public class Report
    {
        public const string PASSED = "Успешно";
        public const string FAILED = "Провально";
        public const string STOPPED = "Остановлено";
        public const string PROCESS = "Выполняется";
        public const string COMPLETED = "Выполнено";
        public const string WARNING = "Предупреждение";
        public const string ERROR = "ОШИБКА";

        public static string TestFileName;
        public static string FileName;
        public static string FolderName;
        public static int CountErrors;
        public static bool TestSuccess;
        public static List<string[]> Steps;

        public static void Init()
        {
            Report.TestFileName = Config.selectName;
            Report.FileName = $"Report-{Report.TestFileName}.html";
            Report.FileName = Report.FileName.Replace(".cs", "");
            Report.FolderName = Config.projectPath + "/reports/";
            Report.CountErrors = 0;
            Report.TestSuccess = false;
            Report.Steps = new List<string[]>();

            SaveReport();
        }
                
        public static void AddStep(string status, string action, string comment)
        {
            if (status == Report.FAILED || status == Report.ERROR) Report.CountErrors++;
            Report.Steps.Add(new string[] { status, action, comment });
        }

        public static void SaveReport()
        {
            try
            {
                if (!Directory.Exists(Report.FolderName))
                {
                    Directory.CreateDirectory(Report.FolderName);
                    Config.browserForm.consoleMsg("Создана папка для отчетов");
                    Config.browserForm.updateProjectTree();
                }
                if (Directory.Exists(Report.FolderName))
                {
                    WorkOnFiles writer = new WorkOnFiles();
                    if (!File.Exists(Report.FolderName + Report.FileName))
                    {
                        writer.writeFile(GetHead() + GetBody() + GetFooter(), WorkOnFiles.UTF_8_BOM, Report.FolderName + Report.FileName);
                        if (File.Exists(Report.FolderName + Report.FileName)) Config.browserForm.consoleMsg($"Создан файл отчета {Report.FileName}");
                        else Config.browserForm.consoleMsg($"Не удалось создать файл отчета {Report.FileName} по адресу {Report.FolderName}");
                        Config.browserForm.updateProjectTree();
                    }
                    else
                    {
                        File.Delete(Report.FolderName + Report.FileName);
                        writer.writeFile(GetHead() + GetBody() + GetFooter(), WorkOnFiles.UTF_8_BOM, Report.FolderName + Report.FileName);
                        if (File.Exists(Report.FolderName + Report.FileName)) Config.browserForm.consoleMsg($"Обновлен файл отчета {Report.FileName}");
                        else Config.browserForm.consoleMsg($"Отсутствует файл отчета {Report.FileName} по адресу {Report.FolderName}");
                    }
                }
                else
                {
                    Config.browserForm.consoleMsg($"Не удалось создать папку для отчетов по адресу {Report.FolderName}");
                }
            }
            catch (Exception ex)
            {
                Config.browserForm.consoleMsg(ex.ToString());
            }
        }

        public static string GetHead()
        {
            string content =
@"<!DOCTYPE html>
<html lang=""ru"">
<head>
	<title>Отчет</title>
	<meta charset=""UTF-8"" />
	<meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
	<meta http-equiv=""X-UA-Compatible"" content=""ie=edge"" />
    <style type=""text/css"">
        html { margin: 0; padding: 0; border: 0;}
		body { background-color: #F9F7FF; font-family: ""Source Sans Pro"", Helvetica, sans-serif; font-size: 9pt; }
        .wrapper { margin-left: auto; margin-right: auto; position: relative; max-width: 1440px; }
        header { display: block; position: fixed; top: 0px; background-color: #F9F7FF; border-bottom: 1px solid #eaeff2; min-width: 1400px; max-width: 1400px; z-index: 1000; }
		section { display: block; position: relative; min-width: 1400px; max-width: 1400px; }
		table { margin: 0px; min-width: 1400px; max-width: 1400px; }
		thead { background-color: #4d545d; color: #FFF; }
		.table { position: relative; top: 140px; z-index: 1; }
		.table-status { padding: 10px; min-width: 100px; max-width: 100px; }
		.table-action { padding: 10px; min-width: 450px; max-width: 450px; }
		.table-comment { padding: 10px; min-width: 700px; max-width: 700px; }
		.table-row { background-color: #FFF; border-bottom: 1px solid #eaeff2; }
		.status-passed { background-color: #98C900; color: #FFFFFF; }
		.status-failed { background-color: #E94B31; color: #FFFFFF; }
		.status-stopped { background-color: #858585; color: #FFFFFF; }
		.status-process { background-color: #FFFFFF; color: #222222; }
		.status-completed { background-color: #0094FF; color: #FFFFFF; }
		.status-warning { background-color: #FFE97F; color: #222222; }
		.status-error { background-color: #F4CCCC; color: #FF0000; }
		.result-passed { color: #007F0E; }
		.result-failed { color: #7F0000; }
	</style>
</head>";
            return content;
        }

        public static string GetFooter()
        {
            string content =
@"</html>";
            return content;
        }

        public static string GetBody()
        {
            string content = "<body>";
            content += "<div class=\"wrapper\">";
            content += "<header>";
            content += "<h2>Отчет о работе автотеста</h2>";
            content += $"<h3>Файл: {Report.TestFileName}</h3>";
            if (Report.TestSuccess == true) content += "<h3>Результат: <span class=\"result-passed\">Успешно</span></h3>";
            else content += "<h3>Результат: <span class=\"result-failed\">Провально</span></h3>";
            content += "<table>";
            content += "<thead>";
            content += "<tr>";
            content += "<th class=\"table-status\">Статус</th>";
            content += "<th class=\"table-action\">Действие</th>";
            content += "<th class=\"table-comment\">Комментарий</th>";
            content += "</tr> ";
            content += "</thead>";
            content += "</table>";
            content += "</header>";

            if (Report.Steps.Count > 0)
            {
                content += "<section>";
                content += "<table class=\"table\">";
                content += "<tbody>";

                try
                {
                    foreach (string[] step in Report.Steps)
                    {
                        content += "<tr>";
                        if (step[0] == Report.PASSED) content += $"<td class=\"table-status table-row status-passed\">{step[0]}</td>";
                        if (step[0] == Report.FAILED) content += $"<td class=\"table-status table-row status-failed\">{step[0]}</td>";
                        if (step[0] == Report.STOPPED) content += $"<td class=\"table-status table-row status-stopped\">{step[0]}</td>";
                        if (step[0] == Report.PROCESS) content += $"<td class=\"table-status table-row status-process\">{step[0]}</td>";
                        if (step[0] == Report.COMPLETED) content += $"<td class=\"table-status table-row status-completed\">{step[0]}</td>";
                        if (step[0] == Report.WARNING) content += $"<td class=\"table-status table-row status-warning\">{step[0]}</td>";
                        if (step[0] == Report.ERROR) content += $"<td class=\"table-status table-row status-error\">{step[0]}</td>";
                        content += $"<td class=\"table-action table-row\">{step[1]}</td> ";
                        content += $"<td class=\"table-comment table-row\">{step[2]}</td>";
                        content += "</tr>";
                    }
                }
                catch (Exception ex)
                {
                    Config.browserForm.consoleMsg(ex.ToString());
                }

                content += "</tbody>";
                content += "<tfoot>";
                content += "<td class=\"table-status\">Ошибок: </td>";
                content += $"<td class=\"table-action\">{Report.CountErrors}</td>";
                content += "<td class=\"table-comment\"></td>";
                content += "</tfoot>";
                content += "</table>";
                content += "</section>";
            }

            content += "</div>";
            content += "</body>";
            return content;
        }


    }
}

