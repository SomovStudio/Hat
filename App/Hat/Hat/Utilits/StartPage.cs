using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Hat
{
    public static class StartPage
    {
		public static string fileStartPage = Directory.GetCurrentDirectory() + "/readme.html"; // стартовая html страница


		public static void createStartPage()
		{
			try
			{
				if (!File.Exists(fileStartPage))
				{
					string content =
@"
<!DOCTYPE html>
<html lang=""en"">
<head>
	<title>Приветствие</title>
	<meta charset=""UTF-8"" />
	<meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
	<meta http-equiv=""X-UA-Compatible"" content=""ie=edge"" />
	<style type=""text/css"">
		html { margin: 0; padding: 0; border: 0;}
		body {
			background-color: #A1A1A1;
		    font-family: ""Source Sans Pro"", Helvetica, sans-serif;
            font-size: 9pt;
        }
		#info {
			border: 1px solid #F2F2F2; 
			border-radius: 10px; 
			padding: 15px; 
			position: relative; 
			min-width: 400px;
			max-width: 400px; 
			margin-left: auto;
    		margin-right: auto;
    		margin-top: 5em;
    		text-align: center;
    		background-color: #F2F2F2;
    		box-shadow: 0 0 10px rgba(80,80,80,0.8);
    		color: #404040;
		}
		p{ text-align: left; }
		.a_link{ text-decoration: none; }
	</style>
</head>
<body>
	<div id=""info"">
        <svg version=""1.1"" id=""Layer_1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" x=""0px"" y=""0px"" width=""64px"" height=""64px"" viewBox=""0 0 64 64"" enable-background=""new 0 0 64 64"" xml:space=""preserve"">
        <image id=""image0"" width=""64"" height=""64"" x=""0"" y=""0"" href=""data: image / png; base64,iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAAABGdBTUEAALGPC / xhBQAAACBjSFJN
		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
		AAAASUVORK5CYII="" />
		</svg>
		<h2>Browser Hat</h2>
        <p>
			Браузер со встроенной технологией автоматизированного тестирования Web приложений.
			<br>
            Особенность браузера в том что автотесты напрямую выполняются в браузере без Selenium и WebDriver.
			<br><br>
			Встроенный фреймворк HatFramework содержит достаточное количество методов необходимых для выполнения основных задач автоматизации тестирования. Для описания скриптов автотестов используется язык программирования C# и встроенный редактор кода. Так же в качестве редактора можно воспользоваться Visual Studio. Удобный интерфейс браузера отображает все шаги выполнения теста с подробным описанием событий. Результат проверки формируется в отчет и отправляются на указаную почту. Запуск автотестов возможен из командной строки операционной системы Windows это пригодится при использовании автотестов в популярных средствах непрерывной интеграции таких как: Jenkins, TeamCity, GitLab CI/CD.
		</p>
		<p>
			Программа разработана при поддержке компании <a href=""https://zionec.ru/"">Зионек</a>
		</p>

		<a href=""https://somovstudio.github.io/"" class=""a_link"">
        <svg version=""1.1"" id=""Layer_2"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" x=""0px"" y=""0px"" width=""166px"" height=""42px"" viewBox=""0 0 166 42"" enable-background=""new 0 0 166 42"" xml:space=""preserve"">
		<image id=""image2"" width = ""166"" height = ""42"" x = ""0"" y = ""0"" href = ""
		data:image/png;base64,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
		"" />
		</svg>
		</a>

		<a href=""https://zionec.ru/"" class=""a_link"">
		<svg version = ""1.1"" id = ""Layer_3"" xmlns = ""http://www.w3.org/2000/svg"" xmlns: xlink = ""http://www.w3.org/1999/xlink"" x = ""0px"" y = ""0px"" width = ""166px"" height = ""42px"" viewBox = ""0 0 166 42"" enable - background = ""new 0 0 166 42"" xml: space = ""preserve"" >
		<image id = ""image3"" width = ""166"" height = ""42"" x = ""0"" y = ""0"" href = ""
		data:image/png;base64,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
		RK5CYII=
		"" />
		</ svg >
		</a>

		<p>
			Благодарим вас за использование нашего приложения.
		</p>
		<h4>
			Copyright © 2022 Somov Studio. All Rights Reserved.
		</h4>
	</div>
</body>
</html>
";
					StreamWriter writer;
					writer = new StreamWriter(fileStartPage, false, new UTF8Encoding(true));
					writer.Write(content);
					writer.Close();
				}
			}
			catch (Exception ex)
			{
				Config.browserForm.consoleMsgError(ex.ToString());
			}
		}



	}
}
