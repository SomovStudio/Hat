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
		fmbajN4ZKDtlKpNVuMGmLZhVKMsQVCzcmfRv3igFi4nP+dq4DmmaCxO2r0MRdo1xLIgSGRczTmer
		f9hGvRLdjLS++QL8kbkHBz/HGVdf2jmmfZYqM9Pc22xLULC1H62rqWWPRsT7BDuX2oDI8iQPITUl
		dpDWZDVG1JOQ7/ZGROsDaHeeQHEO2lsDE5cSn9mwfbAoR8urTHoEf+W5PccNKLc/JaJdAYvEJ558
		r49pn7bIwx7D7h8mT+vwF7SMByY7nrNqve3tTpQHbwkUkGl7n39M8ra3/kceBWVF+6wMlb+djJ5h
		8R4s24wCj7DCjY40mqX3iK+scZCPbGqd10nmG3dDVV0nAke5x2pRPekG4otLcH9XX4qpv+34hdVE
		X4qtZQ3FD7wdTkpKyXGcHcBeV54qKda6OwnP64N8fw/lQEWYYHZCPs2wNJM7SLgHPbLwvtbJX0PY
		9hBWgb0XqdRSsi5ZUIWyEvlrjts4oh4N2IZKuqqbaS5Po5rOfaUOlON/H3HPrLyKyuFfaOJ5PIFy
		53GZlxxtBEkepqpGHOZCsr+TKAz7Ue59LNHOcY42hqRP+e1C7yMagcjmUn3AAqJphqK0W0vTUzla
		GdK+6notetnWKFRTmdYfrEOlTsMRP5elcihHG0CWNwQ3oOi5CqWTTkWZouNQwUEPxAXuR/n31xB3
		uN791KW/ZI62Bif/D6hytEb8F9CKXy+KmJCxAAAAJXRFWHRkYXRlOmNyZWF0ZQAyMDIyLTExLTA5
		VDE2OjU2OjU0KzAxOjAwCIK1DgAAACV0RVh0ZGF0ZTptb2RpZnkAMjAyMi0xMS0wOVQxNjo1Njo1
		NCswMTowMHnfDbIAAAA3dEVYdGljYzpjb3B5cmlnaHQAQ29weXJpZ2h0IDE5OTkgQWRvYmUgU3lz
		dGVtcyBJbmNvcnBvcmF0ZWQxbP9tAAAAIHRFWHRpY2M6ZGVzY3JpcHRpb24AQWRvYmUgUkdCICgx
		OTk4KbC66vYAAAAASUVORK5CYII=
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
