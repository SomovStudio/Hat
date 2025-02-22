﻿using System;
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
					string content_rus =
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
		data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAKYAAAAqCAYAAAA5+iDUAAAABGdBTUEAALGOfPtRkwAAACBjSFJN
		AAB6JQAAgIMAAPn/AACA6QAAdTAAAOpgAAA6mAAAF2+SX8VGAAAABmJLR0QA/wD/AP+gvaeTAAAA
		CXBIWXMAAAsTAAALEwEAmpwYAAAX00lEQVR42u1ceVxUVft/zj333oFhCHRA0BRElNeVTCUL9SX3
		FVf4RWSmJmkuIbmQRZILiqRCJKlvaKXmr6xEEQw3SF9cU3pRQQVZB1mHHWa72+8PuTTiDMxgZr0/
		vp/P/WPmnmc797nnPM9znhkkCAJ0oAN/NRDPWoEOdMAQOhyzA39J/OGOuT7pe49VJw6MetaGdeDv
		jT/UMT8/fxKdV2THZipLIw5eSUHP2rgO/H3xhzpmfl3l69UalXtRQ/XL54uylj9r4zrw98Uf5pif
		Jh/v9osiayuBEGBEwM2KB6EHLqd0e9YGduDviT/MMa+V5IfoONaJQAgIhEDNMvLTBZkfPGsDO/D3
		xB/imOFn4/6RW6ucR6Lf2ZGIgMzKkoUR546PfNZGduDvB/JJGcRePE2dK7wbwXCcFUn87pgIIWB5
		3io+Jz3a1cbupTnDRrDPwkA/Pz9iwIABjgqFwrZbt246lUpVGhER0fAsdDGGwMBAiZ2dnWNhYaGV
		g4MDo1Qqy/bs2VP3rPV6lkBPevKzPG7/vAtF2d+QBDZ4nxcEGObo9G6sz+I9f6Zh7733nmNDQ8Ok
		27dvL6iurh4AADKEEE/TdKmjo+NPzs7OP+3bt++KIdqoqKhJmZmZYyiKEr86GRMT80tr8lasWOHL
		cZwHAIBEIkmPjIz8trXxmzdvJi9evDicZdnx1dXVM2pra/sAAEYICQRBlPXt2/ffNTU1R3v37p0a
		GxurbI3Xhg0bXi4vL58NAMDzPNjY2PwaHh7+g7Hxy5YtexEAXgcA4DgOunbtmhkaGvq13v1eALAY
		AEyqrDAMAwMGDDgbGBh4GgBg8eLFvUiS1Ke/HRMTc8AQbXh4uFVhYeEyALBDCAHP89ja2vrsEzlm
		ZEpCl6PZaRfrdJreGBmOCngQgOcF1YKBnq8GjZ72a7uFmYHJkyf7lpSUhCuVyl4EQQBCCBBCINoq
		CAIghLQDBw7cmZiY+GFL+nfeeSf68OHDK6ysrMSv1peVlW0yJu+LL774x7Zt235rbGy0RAiBra3t
		iezs7OnGxvv5+fVVKBRfFBcXe7EsS4j66evIsiyQJAlWVlY5bm5u644dO2bU0UaMGBF4//79KAAA
		jUYDXl5ecfHx8bONjZ8zZ87i1NTUPYIgAMMwMGjQoJQLFy6MEe87ODiMBoBkU+e7sbER3njjjZ17
		9+5dBQBgY2Mz2sLCQp/+VFlZ2SRDtKNGjfo6KyvrLUEQgOM4kEgkzNtvv/3iE23lqQ/ub67RqHtT
		GBsdQwACFnjp5eKc1UEArz2JPFMwf/78CRkZGfsFQZCJKx7P88Dz/EN9CAIIggCe5yU3b95c5+/v
		T8+YMWPta6+9xos8KIrSWFlZgVQqFb/SGJMXFRUlP3LkSAzP85bW1tbA8zxYWlo2Ghu/cOFCv+vX
		r4drtVpnjDGQJAmCIIAgCMDzfLOD0jQNAAANDQ2uaWlp/ztr1qyXPT09N65Zs6a2JU9LS0utqCvG
		GCiKUrU2RxhjnVQqBUEQgGVZsLCweCS0kUqlLADwoJeDtLaACYIAFEVpxc9WVlasRCLRpzc4H0uW
		LAlQKBRvWVpagiAIQBBEnbe396JNmzZltDv5Wf/z94NzaireJHHbLCgCQ1Z1ufen5457tleeKdi0
		aVOvq1evxgKATP+BW1paFj///PO/2NnZpSGEeI7jmh304sWLq1JTU19ur8xjx46F5OfnjyXJtt/x
		uXPnBicnJ3+r0+mcSZIEhBBotQ+fp4WFhUomk9VJpdJ6jDHHsixwHAcYYyAIAl+9evX948ePxyUm
		Jlo+zTk0BJ7nxV3G4NUEs3xp7ty5M0+cOBED8DAfUavVMHbs2E+io6N/AGhn8vNR0vdW10ryohpZ
		nQVlILakCQzE7woDAgBeECwTcm99LqMtpr07amLJ05hAjUbTDQAaOI4TJ6zO1dV17dChQ+Nyc3Mr
		bG1tSZIkX0lLS4sqKyt7EWMMOp0OSkpKvAHgkrnygoOD59y/f3+RuLq1hoULF45MSUlZT5IkgTGG
		Jh01w4cP30/T9Jm+ffveHDx4cG1ZWRkdHx/fS6fTja2srJxXXl7uStM00DQNOTk5o48dO7Zq6tSp
		m5/G/BkCy7Lg5OSU6eHhEaZWqwWCIB4L/xiGQX369EkXPxNE6z66b98++99++y0MY0whhECn08HI
		kSO/nj59+m5xTLsckxLguUEO3U8Pcuh+Bok6omZDrNIrHsyp1jS66cedmCCgXFU/JLOy+F0AWP80
		JjEsLCz11q1bYysrK3eUlpZO7dq166bk5OS9+nMIABcWLFgQU1ZWFgvwcOtTKBReU6ZMwSdPnuRM
		lbV9+3a7hISESISQTG/VMIqcnJwQhJCUIAjgOA6sra3Le/bs+W5cXNzRlmOXLFlSAgAX161bdyA1
		NfVAbm7uKNE5U1JSVgcFBR2LjIy8/TTmsCU4jgO5XF4UFRV12FQavaTxMezfv5+OjY3dXldX158k
		SWBZFqRSadaUKVOCpk+f3hwymb2Vfx27T1J2LDmgIe4XsvHYedRwvOmKO49qf0qh3Uu0O4d16rZc
		yz/+jC0wCZeL81aEnf7J42lNZHx8fMnFixf9x48f7zlx4sRoQ2N69uxZThBE8/ak1Wol1dXVZp3t
		JyQkbKmtre1hyha+fPnyBXl5eePE8IIkyYZp06b5JyYmHm2NbuvWrflvvfWWj6OjYybDMEAQBGg0
		GpurV69ui46O/tN6EViWfaJchGV/rxQePXp0g0KhmEeSJPA8DxRF1UyYMGHZokWLavRpzBZ49peU
		NaeSTm3AmADQWyk4jgNbW9sHtp077dq+ZPuZih/27E0rLVisv9UjhEDD6mwvFedu/f7XC1Nf8/in
		1lz5pmLPnj0Zxu5lZWUNY1kWKIoCrVYLbm5uV48cOWJynXXt2rVvZWdnL9BProxtXzt37kTp6elv
		8jyPxdDB3d392LZt286ZImvRokXlJSUlH8TGxv4oCAKNMQalUumlUqmcAKDAEA1BEK2u/ARB6MyZ
		S5Ikn6imKDpmSEjI3PT09NW4KVlmGAYmTZr0QUxMzNnHZJojYP78+ePPnz8faiWz0g96oans0Dhu
		3Djf7du3VwAAjHfqt/ZeZcl4Dcv0IvS2dBqTkFtbMTa9vGjOawAmbw/tRUBAAJ2enk7V1dWBm5ub
		fU1NzXuFhYVvUxQFPM8DTdMwcODA71vjoR9Tbdq0ySkuLm4HxphsqruBpaWljmEYWsz89cdzHGej
		VCr7YozFsQ2jR4/eYY4NH3/88YmkpKTUwsLCMU3ObZWTkzMEDDgmQRBQW1vrsHHjxiEsyyJoUYuk
		KIolCMKttZepJT+NRiPx8fGxvnLlitBym+Z5Htna2vJ9+vRR/fjjj485MM/zIJfL6wMDA91OnTr1
		OUKIxBhDY2MjDBo06Nu9e/fuNSTXZMeMiIigMzIyPhIEgWwZUzEMAx4eHvt27959Wfxu7nCvusvF
		Obt+UWTtlLTI3GlMwqWS3FU7U+KT3h89vcqch2Qu6uvrP1QqlQswxpCRkWHNsmwnUX+CIDQuLi7f
		SKXS1NZ4iG98WFjYc4mJiZ+p1Wo5RVHAMAw8//zziS+99NKJn3/+ebcgCI9tr1lZWd1Zlu0sOrFc
		Ls/FGN8y1w4HB4f/5OXljRHDgZs3bzqJ9/SfB0VRkJ2dPaGgoGCMIT4IIYFlWWyKU+rxe0mn0901
		kuQhAMgBgPFgpKwmlUqHXrhw4ZBKpbLFGAPDMNCnT58Ls2fPft+YXJMds6CgYHZxcbFXS+V4ngeJ
		RNLo7u6+qyXNeKd+X+bWKKcqGqrH0npbOkYElDXWD/lPeVEIALzfpvAngE6n66JWq50sLCzEWhkA
		PAw9HBwcLicnJy9piwfHcQIAQHZ2dnB+fv5MS0tL4HkeMMa148aNC6IoyoXjuGbv0HeUc+fOdRIE
		gUIIAcuyYGdnV7Fq1SqTkywRRUVFD8R4Vq1Wg4eHh1z/GbQA0mg0RjOQFmUeU0DTNN1ap5ja2A2M
		MZSUlAwEAP2aLefp6fnR0qVLy43RmfTahISEeJw9eza6ZaDfVBQWRo8eHbJx48bslnQzh77SMNbp
		H4ESgqzjW5QYLEgSbiuLl36afGy4OTNkLnieJzmOA5Zlgef55tUPYwwVFRUjPT094w4cOGBnjJ7j
		OOjWrZtq9erVzpcuXXpXIpGItvOTJk0KCgsLy9bpdNbGCtA0TfMIPaxdIISA4zjjpxGtwMXFhdbX
		XalUNsfELZ2M53lgGMboxXHmvRc8z4NOpwOtVmvw0ul0JMMwRufP3t6+7LnnnqsVDxAAAN+9e3de
		azLbXDFDQ0NRSkrK2oaGBvuWq2XTqnNu3759UcboV4+dkZFdU77r4oOcD2n8uzgECBiek5zNvxvR
		/ep579eHez2VpgV7e/sLTk5OFjRNcwzD0Bhj1+Li4iEIIYogCCo/P39mbGxsTadOnQK8vb0NJkAW
		FhZdL126FKlSqTrRNA1arRYcHR3P7N279ytD4/WddPz48dVJSUkswzAYYwyVlZX2GzduxOvXrzfL
		OziOa16xJBIJ3L9/v/kESN8xm87Ky52dnW81FcZbxpicUqnsXlRU1M+U7ZzneZDJZDXOzs5pCCGh
		JT9BEMDa2lrh7OzMG6LnOA46d+58eujQoScOHjx4hKZpoCgKrl+/HrBw4cI7+/fvjzRE16ZjFhQU
		BBQUFMxu6ZRNpRbNyJEjt7bFY7xz/0/uVpWNqtaoRul3IFEEBkVDzT9Ti3Pefh28Itvi0x58+eWX
		hwDgkPh5zZo1ZFZW1tjCwsJ9VVVVz9M0DQUFBfOTk5OPent7n2hJjzGG8vLy5TqdzpaiKGBZFmQy
		WcHEiRNXmSJfLpcrJBJJuU6n60EQBFRUVPTmeX4MAJwx1YYtW7YQeXl5L+nvWIMHD76r/yxEMAwD
		vXv3Pnv06NE3jPELCAhYoFAo9pvSJ8EwDDg7O6edPHlybHvmn6IouHPnjuXZs2d/yMrK2nb9+vVg
		kiSBoihITk7evHHjxvT169c/di7f6iuzZcsWhxs3bnxEGHi1eJ6HIUOGxERFRbV52O8zbAQzoWf/
		CADgBXh0MigCw62KB8t2X0jq3B7DzcWnn37KHj9+/NSgQYN2MQzzsISl0UBdXd0QYzQMw9jqTQE/
		Y8aM97Zs2WK0HKW/gn3yySf1Xbp0+Y1lWUAIAcMwkvPnz68xR2elUjm/tLR0qJjZSySSGldXV6MJ
		FEKIb41fVVUVj7FZEcUTlYtE/4mLi/vAxcUlQafTQdNBg/Tw4cP7IyMjez5G0xrDjIyMBZWVlU4t
		jeA4DmQyWdGIESO2mKrckC49ErtbdzqtaxHfYISgUtPoeqOs4L0nMV4fkyZNem/GjBkHWxtTVVVV
		Ku4CBEFAfX293NhY0dE0Gg04OzsfjIiIiDdHnxdeeCGWIAhWEASgaRqys7PHLFu27E1TaD/77LPe
		SUlJ4Rg/jINYlgW5XJ6wbt264lbIWs1sMDahwcEMfm1B33+mTp0aKJVKiziOA5Ikoa6uzjkxMfGx
		3dKogkuXLh1+5cqV9w2VCBiGgWHDhm1fs2aNyaWeyS94CN6ug5bZW8ruc8KjLzRNYLheVhi88dSP
		U9tr/I4dO9CcOXPGe3l5HcvMzPwsLS1tbkBAwApDY8PDw7FGo/EWkwlBEEAikdS3xp9lWejatev1
		iRMnmrXaAQD0798/YcCAAYk63cO6NkEQOCUlZeeUKVMWh4aGGn0GK1eu9Pzuu+8O1tXV2YurpY2N
		TfHQoUPD2ztP7QFJkmY1ebesEujvIMHBwbn+/v4LAEAtvqhZWVnTly1btuARmYYYBwcHE9euXduu
		1WrtWxZUGYYBNze38+PGjfvSXAPfHTUpt6i+ZsdP2b/tlpKPdrtzPGdxvihr7U/XL52eM8yTMZf3
		qlWrhKSkpHcyMzNnyGQyEAQBkpKSdowZMwa8vLyOX7p0SSkIArzwwgtdMzIylt67d2+2aBtFUVBd
		XW307FlsSZsxY0ZoSEhIhbm6BQQECEqlcnNGRsZElmUtKIqChoYGu/T09D1arXbKrFmzDjg5Od3u
		379//YMHD+i7d+/2qqqqmqRQKJY3NjZaUhQFgiCATqeDyZMnR0RHR2eYq0N70VQB6L506VJ/juMM
		NnHwPI/kcnnu1q1bLzd9brV4v2HDhrP37t37V0pKSqBUKgWEEBEfH/+Zm5tbRmBg4DUAI45ZWlrq
		W1ZWNsJAlR8wxtpZs2YFzps3r9WeP2Po29nhmz6dukzNr1VO0z+uJAkMFaqGf/6UnfbRnGGen7SH
		t4+PT3B5ebl7ZWWlG0VRgDGmcnNzox88eBBGEIQSACA/P99eq9XKxO1Fp9NBz549z02bNu2kMb4M
		w8DAgQMjQ0NDT5qoymNYt27d9dra2gVxcXGfq1QqO5IkAWMM9+7dm06S5PSMjAz16dOnOZ7nEcMw
		VmJMKp5QCYIAHh4eu6ZOnbq7vTq0ByRJQnFxcf/i4uJvjdU+OY6D7t27fwsAlwHa7i4CAPDz8wtV
		KpXOd+7cmUnTNPA8b33o0KE9crl82ty5c4sf4xAREdHnxo0b25ARLfr16/evlStXprcp2QjefHm0
		erqre7CUoitbvnkEQpBdU7485sLPru3hvXjx4lx/f/81GGNGbH1rSm6s1Wq1i1qtdtFqtTKxq51l
		WbCxsSn28fFZtmTJklpDPBmGgR49elyYMmXK6vbaLCI8PPy7V1555Q2apvNYlhUbbKGp9ctSpVLJ
		NBqNVdMC0Kwjz/OaYcOGbY6Pj1/h7e1t1jn3HwHRFcT+ViOXWeWvmTNn1r766qvv2tjYFIrxZlFR
		0Yv79+/fde7cOeKxFTMlJeXDqqoqZ7GQLKKpUHp1yJAhHy9fvpwWHzxNkhAVHa0LTjhEcwAEetj5
		bBAPM3IEdpayO71s7ffdrniwlkK/r5oEQqBmdPLkwnvBy2DyO+2ZxODg4Pj09PRJN2/ejKypqXEX
		f64gdhMRBNFcZO/Ro0ecr69vcFBQ0COHAwzDWDY2Noo7hHro0KGrAwMDjdrFcRylUqmaHx5FUVbG
		xn799den/fz8RtTW1m6+e/fuWyzLYvEF1e94EufXxcXlkru7+5qYmBij/aJqtVoiytdoNMAwjFH5
		AABarVaiUqnEHgdgGEamf1+lUpFgRucZy7Kg0Wik+vQtKjkyQ3QhISGlCoVibmJi4mm1Wm2BEIIb
		N27MOnLkSNAjjrlu3Tr3e/fu+RpKeAiCgMbGRtczZ84kI4Sa6TiWhYB3AnbIJ4+8/J+akkMgCHRb
		KZwAwGtZ1tbQD9goAkNOTYX/hlM/JIRO9DUr+xVx+PDh5NmzZ4/v1KnTlMzMzFclEomTWq2WY4wF
		tVpd+txzz+WxLJvq5ub2Y1BQ0GMdTv3794/39/evI0kSbGxsrm/durXV3yo5Ojqm+/r6bgF46JhW
		VlY3Wxv/3XfflQDA2zNnzoxtaGjw0ul0gwHAWa1WW9E0zXIc96Bz5845ffr0Se3Wrdvp4ODgVg8f
		JkyYcHnw4MFbAJoL7DdaGz9ixIhr3bt316+oPBJf+/j45AFAGJiYjfM8D507d04TP/v6+uYhhPTp
		jcbEe/fu/fcHH3wwr7a2dnBTCQnJ5fL65kA2MjLSIiEh4cc7d+5MbblaihCTAH1wHAdy20535ywP
		ePFXG/6bPGXZ/5AmxBgI0CNd7vpgeQ46W1rdmtf/5ZcWeo7TtMmsDezatQtlZ2dTMpkMwsLC/vSt
		0BRs374d5+TkkF26dOE3bNhgdvL334Zmx/Tz81uZkpISqfcDLJOhUavB1++1BS4zx5369s6vd1iO
		syHMaxJ4DAzPw8tdXT750idgw7OepA78+SAAAA4ePOiUmZm50thK2RYwScJv135dEfSqd0k/+26H
		Gd7s5pnHQCIEV0vz3o9MSXjlWU9SB/58YFdXV/TVV18dUCgUw1v7rUarTDCGcqWya072fWbCq2O2
		3awqnqdlWWszW6seAUIIWIGXqBidE81yh/t1der4T+7/R8DOzs6jrly5soYgCAYhpAWAdl0IIW1p
		aYmTx0D33UhuI7lfXT6EJIh28wMALUaEtkJd72BnKbvs1XtAgUkWdeC/Ak/8FzEd6MDTQMd/sHfg
		L4kOx+zAXxIdjtmBvyQ6HLMDf0n8H+dMCpLYusouAAAAJXRFWHRkYXRlOmNyZWF0ZQAyMDIzLTAy
		LTA2VDA2OjA4OjEzKzAwOjAwXLEhjQAAACV0RVh0ZGF0ZTptb2RpZnkAMjAyMy0wMi0wNlQwNjow
		ODoxMyswMDowMC3smTEAAAAASUVORK5CYII=
		"" />
		</ svg >
		</a>

		<p>
			Благодарим вас за использование нашего приложения.
		</p>
		<h4>
			Copyright © 2024 Somov Studio. All Rights Reserved.
		</h4>
	</div>
</body>
</html>
";

                    string content_eng =
@"
<!DOCTYPE html>
<html lang=""en"">
<head>
	<title>Greeting</title>
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
		.text_center {text-align: center;}
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
			A browser with built-in technology for automated testing of Web applications.
			<br>
            The peculiarity of the browser is that autotests are performed directly in the browser without Selenium and WebDriver.
			<br><br>
			The built-in Net Framework contains a sufficient number of methods necessary to perform the basic tasks of test automation. The C# programming language and the built-in code editor are used to describe autotest scripts. You can also use Visual Studio as an editor. The user-friendly browser interface displays all the steps of the test with the corresponding description of sales. The verification result is generated in a report and sent to the specified email address. Autotests can be run from the command line of the Windows operating system. This happens when using autotests in popular continuous integration environments such as: Jenkins, TeamCity, GitLab CI/CD.
		</p>
		<p>
			The program was developed with the support of the company <a href=""https://zionec.ru/"">Zionec</a>
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
		data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAKYAAAAqCAYAAAA5+iDUAAAABGdBTUEAALGOfPtRkwAAACBjSFJN
		AAB6JQAAgIMAAPn/AACA6QAAdTAAAOpgAAA6mAAAF2+SX8VGAAAABmJLR0QA/wD/AP+gvaeTAAAA
		CXBIWXMAAAsTAAALEwEAmpwYAAAX00lEQVR42u1ceVxUVft/zj333oFhCHRA0BRElNeVTCUL9SX3
		FVf4RWSmJmkuIbmQRZILiqRCJKlvaKXmr6xEEQw3SF9cU3pRQQVZB1mHHWa72+8PuTTiDMxgZr0/
		vp/P/WPmnmc797nnPM9znhkkCAJ0oAN/NRDPWoEOdMAQOhyzA39J/OGOuT7pe49VJw6MetaGdeDv
		jT/UMT8/fxKdV2THZipLIw5eSUHP2rgO/H3xhzpmfl3l69UalXtRQ/XL54uylj9r4zrw98Uf5pif
		Jh/v9osiayuBEGBEwM2KB6EHLqd0e9YGduDviT/MMa+V5IfoONaJQAgIhEDNMvLTBZkfPGsDO/D3
		xB/imOFn4/6RW6ucR6Lf2ZGIgMzKkoUR546PfNZGduDvB/JJGcRePE2dK7wbwXCcFUn87pgIIWB5
		3io+Jz3a1cbupTnDRrDPwkA/Pz9iwIABjgqFwrZbt246lUpVGhER0fAsdDGGwMBAiZ2dnWNhYaGV
		g4MDo1Qqy/bs2VP3rPV6lkBPevKzPG7/vAtF2d+QBDZ4nxcEGObo9G6sz+I9f6Zh7733nmNDQ8Ok
		27dvL6iurh4AADKEEE/TdKmjo+NPzs7OP+3bt++KIdqoqKhJmZmZYyiKEr86GRMT80tr8lasWOHL
		cZwHAIBEIkmPjIz8trXxmzdvJi9evDicZdnx1dXVM2pra/sAAEYICQRBlPXt2/ffNTU1R3v37p0a
		GxurbI3Xhg0bXi4vL58NAMDzPNjY2PwaHh7+g7Hxy5YtexEAXgcA4DgOunbtmhkaGvq13v1eALAY
		AEyqrDAMAwMGDDgbGBh4GgBg8eLFvUiS1Ke/HRMTc8AQbXh4uFVhYeEyALBDCAHP89ja2vrsEzlm
		ZEpCl6PZaRfrdJreGBmOCngQgOcF1YKBnq8GjZ72a7uFmYHJkyf7lpSUhCuVyl4EQQBCCBBCINoq
		CAIghLQDBw7cmZiY+GFL+nfeeSf68OHDK6ysrMSv1peVlW0yJu+LL774x7Zt235rbGy0RAiBra3t
		iezs7OnGxvv5+fVVKBRfFBcXe7EsS4j66evIsiyQJAlWVlY5bm5u644dO2bU0UaMGBF4//79KAAA
		jUYDXl5ecfHx8bONjZ8zZ87i1NTUPYIgAMMwMGjQoJQLFy6MEe87ODiMBoBkU+e7sbER3njjjZ17
		9+5dBQBgY2Mz2sLCQp/+VFlZ2SRDtKNGjfo6KyvrLUEQgOM4kEgkzNtvv/3iE23lqQ/ub67RqHtT
		GBsdQwACFnjp5eKc1UEArz2JPFMwf/78CRkZGfsFQZCJKx7P88Dz/EN9CAIIggCe5yU3b95c5+/v
		T8+YMWPta6+9xos8KIrSWFlZgVQqFb/SGJMXFRUlP3LkSAzP85bW1tbA8zxYWlo2Ghu/cOFCv+vX
		r4drtVpnjDGQJAmCIIAgCMDzfLOD0jQNAAANDQ2uaWlp/ztr1qyXPT09N65Zs6a2JU9LS0utqCvG
		GCiKUrU2RxhjnVQqBUEQgGVZsLCweCS0kUqlLADwoJeDtLaACYIAFEVpxc9WVlasRCLRpzc4H0uW
		LAlQKBRvWVpagiAIQBBEnbe396JNmzZltDv5Wf/z94NzaireJHHbLCgCQ1Z1ufen5457tleeKdi0
		aVOvq1evxgKATP+BW1paFj///PO/2NnZpSGEeI7jmh304sWLq1JTU19ur8xjx46F5OfnjyXJtt/x
		uXPnBicnJ3+r0+mcSZIEhBBotQ+fp4WFhUomk9VJpdJ6jDHHsixwHAcYYyAIAl+9evX948ePxyUm
		Jlo+zTk0BJ7nxV3G4NUEs3xp7ty5M0+cOBED8DAfUavVMHbs2E+io6N/AGhn8vNR0vdW10ryohpZ
		nQVlILakCQzE7woDAgBeECwTcm99LqMtpr07amLJ05hAjUbTDQAaOI4TJ6zO1dV17dChQ+Nyc3Mr
		bG1tSZIkX0lLS4sqKyt7EWMMOp0OSkpKvAHgkrnygoOD59y/f3+RuLq1hoULF45MSUlZT5IkgTGG
		Jh01w4cP30/T9Jm+ffveHDx4cG1ZWRkdHx/fS6fTja2srJxXXl7uStM00DQNOTk5o48dO7Zq6tSp
		m5/G/BkCy7Lg5OSU6eHhEaZWqwWCIB4L/xiGQX369EkXPxNE6z66b98++99++y0MY0whhECn08HI
		kSO/nj59+m5xTLsckxLguUEO3U8Pcuh+Bok6omZDrNIrHsyp1jS66cedmCCgXFU/JLOy+F0AWP80
		JjEsLCz11q1bYysrK3eUlpZO7dq166bk5OS9+nMIABcWLFgQU1ZWFgvwcOtTKBReU6ZMwSdPnuRM
		lbV9+3a7hISESISQTG/VMIqcnJwQhJCUIAjgOA6sra3Le/bs+W5cXNzRlmOXLFlSAgAX161bdyA1
		NfVAbm7uKNE5U1JSVgcFBR2LjIy8/TTmsCU4jgO5XF4UFRV12FQavaTxMezfv5+OjY3dXldX158k
		SWBZFqRSadaUKVOCpk+f3hwymb2Vfx27T1J2LDmgIe4XsvHYedRwvOmKO49qf0qh3Uu0O4d16rZc
		yz/+jC0wCZeL81aEnf7J42lNZHx8fMnFixf9x48f7zlx4sRoQ2N69uxZThBE8/ak1Wol1dXVZp3t
		JyQkbKmtre1hyha+fPnyBXl5eePE8IIkyYZp06b5JyYmHm2NbuvWrflvvfWWj6OjYybDMEAQBGg0
		GpurV69ui46O/tN6EViWfaJchGV/rxQePXp0g0KhmEeSJPA8DxRF1UyYMGHZokWLavRpzBZ49peU
		NaeSTm3AmADQWyk4jgNbW9sHtp077dq+ZPuZih/27E0rLVisv9UjhEDD6mwvFedu/f7XC1Nf8/in
		1lz5pmLPnj0Zxu5lZWUNY1kWKIoCrVYLbm5uV48cOWJynXXt2rVvZWdnL9BProxtXzt37kTp6elv
		8jyPxdDB3d392LZt286ZImvRokXlJSUlH8TGxv4oCAKNMQalUumlUqmcAKDAEA1BEK2u/ARB6MyZ
		S5Ikn6imKDpmSEjI3PT09NW4KVlmGAYmTZr0QUxMzNnHZJojYP78+ePPnz8faiWz0g96oans0Dhu
		3Djf7du3VwAAjHfqt/ZeZcl4Dcv0IvS2dBqTkFtbMTa9vGjOawAmbw/tRUBAAJ2enk7V1dWBm5ub
		fU1NzXuFhYVvUxQFPM8DTdMwcODA71vjoR9Tbdq0ySkuLm4HxphsqruBpaWljmEYWsz89cdzHGej
		VCr7YozFsQ2jR4/eYY4NH3/88YmkpKTUwsLCMU3ObZWTkzMEDDgmQRBQW1vrsHHjxiEsyyJoUYuk
		KIolCMKttZepJT+NRiPx8fGxvnLlitBym+Z5Htna2vJ9+vRR/fjjj485MM/zIJfL6wMDA91OnTr1
		OUKIxBhDY2MjDBo06Nu9e/fuNSTXZMeMiIigMzIyPhIEgWwZUzEMAx4eHvt27959Wfxu7nCvusvF
		Obt+UWTtlLTI3GlMwqWS3FU7U+KT3h89vcqch2Qu6uvrP1QqlQswxpCRkWHNsmwnUX+CIDQuLi7f
		SKXS1NZ4iG98WFjYc4mJiZ+p1Wo5RVHAMAw8//zziS+99NKJn3/+ebcgCI9tr1lZWd1Zlu0sOrFc
		Ls/FGN8y1w4HB4f/5OXljRHDgZs3bzqJ9/SfB0VRkJ2dPaGgoGCMIT4IIYFlWWyKU+rxe0mn0901
		kuQhAMgBgPFgpKwmlUqHXrhw4ZBKpbLFGAPDMNCnT58Ls2fPft+YXJMds6CgYHZxcbFXS+V4ngeJ
		RNLo7u6+qyXNeKd+X+bWKKcqGqrH0npbOkYElDXWD/lPeVEIALzfpvAngE6n66JWq50sLCzEWhkA
		PAw9HBwcLicnJy9piwfHcQIAQHZ2dnB+fv5MS0tL4HkeMMa148aNC6IoyoXjuGbv0HeUc+fOdRIE
		gUIIAcuyYGdnV7Fq1SqTkywRRUVFD8R4Vq1Wg4eHh1z/GbQA0mg0RjOQFmUeU0DTNN1ap5ja2A2M
		MZSUlAwEAP2aLefp6fnR0qVLy43RmfTahISEeJw9eza6ZaDfVBQWRo8eHbJx48bslnQzh77SMNbp
		H4ESgqzjW5QYLEgSbiuLl36afGy4OTNkLnieJzmOA5Zlgef55tUPYwwVFRUjPT094w4cOGBnjJ7j
		OOjWrZtq9erVzpcuXXpXIpGItvOTJk0KCgsLy9bpdNbGCtA0TfMIPaxdIISA4zjjpxGtwMXFhdbX
		XalUNsfELZ2M53lgGMboxXHmvRc8z4NOpwOtVmvw0ul0JMMwRufP3t6+7LnnnqsVDxAAAN+9e3de
		azLbXDFDQ0NRSkrK2oaGBvuWq2XTqnNu3759UcboV4+dkZFdU77r4oOcD2n8uzgECBiek5zNvxvR
		/ep579eHez2VpgV7e/sLTk5OFjRNcwzD0Bhj1+Li4iEIIYogCCo/P39mbGxsTadOnQK8vb0NJkAW
		FhZdL126FKlSqTrRNA1arRYcHR3P7N279ytD4/WddPz48dVJSUkswzAYYwyVlZX2GzduxOvXrzfL
		OziOa16xJBIJ3L9/v/kESN8xm87Ky52dnW81FcZbxpicUqnsXlRU1M+U7ZzneZDJZDXOzs5pCCGh
		JT9BEMDa2lrh7OzMG6LnOA46d+58eujQoScOHjx4hKZpoCgKrl+/HrBw4cI7+/fvjzRE16ZjFhQU
		BBQUFMxu6ZRNpRbNyJEjt7bFY7xz/0/uVpWNqtaoRul3IFEEBkVDzT9Ti3Pefh28Itvi0x58+eWX
		hwDgkPh5zZo1ZFZW1tjCwsJ9VVVVz9M0DQUFBfOTk5OPent7n2hJjzGG8vLy5TqdzpaiKGBZFmQy
		WcHEiRNXmSJfLpcrJBJJuU6n60EQBFRUVPTmeX4MAJwx1YYtW7YQeXl5L+nvWIMHD76r/yxEMAwD
		vXv3Pnv06NE3jPELCAhYoFAo9pvSJ8EwDDg7O6edPHlybHvmn6IouHPnjuXZs2d/yMrK2nb9+vVg
		kiSBoihITk7evHHjxvT169c/di7f6iuzZcsWhxs3bnxEGHi1eJ6HIUOGxERFRbV52O8zbAQzoWf/
		CADgBXh0MigCw62KB8t2X0jq3B7DzcWnn37KHj9+/NSgQYN2MQzzsISl0UBdXd0QYzQMw9jqTQE/
		Y8aM97Zs2WK0HKW/gn3yySf1Xbp0+Y1lWUAIAcMwkvPnz68xR2elUjm/tLR0qJjZSySSGldXV6MJ
		FEKIb41fVVUVj7FZEcUTlYtE/4mLi/vAxcUlQafTQdNBg/Tw4cP7IyMjez5G0xrDjIyMBZWVlU4t
		jeA4DmQyWdGIESO2mKrckC49ErtbdzqtaxHfYISgUtPoeqOs4L0nMV4fkyZNem/GjBkHWxtTVVVV
		Ku4CBEFAfX293NhY0dE0Gg04OzsfjIiIiDdHnxdeeCGWIAhWEASgaRqys7PHLFu27E1TaD/77LPe
		SUlJ4Rg/jINYlgW5XJ6wbt264lbIWs1sMDahwcEMfm1B33+mTp0aKJVKiziOA5Ikoa6uzjkxMfGx
		3dKogkuXLh1+5cqV9w2VCBiGgWHDhm1fs2aNyaWeyS94CN6ug5bZW8ruc8KjLzRNYLheVhi88dSP
		U9tr/I4dO9CcOXPGe3l5HcvMzPwsLS1tbkBAwApDY8PDw7FGo/EWkwlBEEAikdS3xp9lWejatev1
		iRMnmrXaAQD0798/YcCAAYk63cO6NkEQOCUlZeeUKVMWh4aGGn0GK1eu9Pzuu+8O1tXV2YurpY2N
		TfHQoUPD2ztP7QFJkmY1ebesEujvIMHBwbn+/v4LAEAtvqhZWVnTly1btuARmYYYBwcHE9euXduu
		1WrtWxZUGYYBNze38+PGjfvSXAPfHTUpt6i+ZsdP2b/tlpKPdrtzPGdxvihr7U/XL52eM8yTMZf3
		qlWrhKSkpHcyMzNnyGQyEAQBkpKSdowZMwa8vLyOX7p0SSkIArzwwgtdMzIylt67d2+2aBtFUVBd
		XW307FlsSZsxY0ZoSEhIhbm6BQQECEqlcnNGRsZElmUtKIqChoYGu/T09D1arXbKrFmzDjg5Od3u
		379//YMHD+i7d+/2qqqqmqRQKJY3NjZaUhQFgiCATqeDyZMnR0RHR2eYq0N70VQB6L506VJ/juMM
		NnHwPI/kcnnu1q1bLzd9brV4v2HDhrP37t37V0pKSqBUKgWEEBEfH/+Zm5tbRmBg4DUAI45ZWlrq
		W1ZWNsJAlR8wxtpZs2YFzps3r9WeP2Po29nhmz6dukzNr1VO0z+uJAkMFaqGf/6UnfbRnGGen7SH
		t4+PT3B5ebl7ZWWlG0VRgDGmcnNzox88eBBGEIQSACA/P99eq9XKxO1Fp9NBz549z02bNu2kMb4M
		w8DAgQMjQ0NDT5qoymNYt27d9dra2gVxcXGfq1QqO5IkAWMM9+7dm06S5PSMjAz16dOnOZ7nEcMw
		VmJMKp5QCYIAHh4eu6ZOnbq7vTq0ByRJQnFxcf/i4uJvjdU+OY6D7t27fwsAlwHa7i4CAPDz8wtV
		KpXOd+7cmUnTNPA8b33o0KE9crl82ty5c4sf4xAREdHnxo0b25ARLfr16/evlStXprcp2QjefHm0
		erqre7CUoitbvnkEQpBdU7485sLPru3hvXjx4lx/f/81GGNGbH1rSm6s1Wq1i1qtdtFqtTKxq51l
		WbCxsSn28fFZtmTJklpDPBmGgR49elyYMmXK6vbaLCI8PPy7V1555Q2apvNYlhUbbKGp9ctSpVLJ
		NBqNVdMC0Kwjz/OaYcOGbY6Pj1/h7e1t1jn3HwHRFcT+ViOXWeWvmTNn1r766qvv2tjYFIrxZlFR
		0Yv79+/fde7cOeKxFTMlJeXDqqoqZ7GQLKKpUHp1yJAhHy9fvpwWHzxNkhAVHa0LTjhEcwAEetj5
		bBAPM3IEdpayO71s7ffdrniwlkK/r5oEQqBmdPLkwnvBy2DyO+2ZxODg4Pj09PRJN2/ejKypqXEX
		f64gdhMRBNFcZO/Ro0ecr69vcFBQ0COHAwzDWDY2Noo7hHro0KGrAwMDjdrFcRylUqmaHx5FUVbG
		xn799den/fz8RtTW1m6+e/fuWyzLYvEF1e94EufXxcXlkru7+5qYmBij/aJqtVoiytdoNMAwjFH5
		AABarVaiUqnEHgdgGEamf1+lUpFgRucZy7Kg0Wik+vQtKjkyQ3QhISGlCoVibmJi4mm1Wm2BEIIb
		N27MOnLkSNAjjrlu3Tr3e/fu+RpKeAiCgMbGRtczZ84kI4Sa6TiWhYB3AnbIJ4+8/J+akkMgCHRb
		KZwAwGtZ1tbQD9goAkNOTYX/hlM/JIRO9DUr+xVx+PDh5NmzZ4/v1KnTlMzMzFclEomTWq2WY4wF
		tVpd+txzz+WxLJvq5ub2Y1BQ0GMdTv3794/39/evI0kSbGxsrm/durXV3yo5Ojqm+/r6bgF46JhW
		VlY3Wxv/3XfflQDA2zNnzoxtaGjw0ul0gwHAWa1WW9E0zXIc96Bz5845ffr0Se3Wrdvp4ODgVg8f
		JkyYcHnw4MFbAJoL7DdaGz9ixIhr3bt316+oPBJf+/j45AFAGJiYjfM8D507d04TP/v6+uYhhPTp
		jcbEe/fu/fcHH3wwr7a2dnBTCQnJ5fL65kA2MjLSIiEh4cc7d+5MbblaihCTAH1wHAdy20535ywP
		ePFXG/6bPGXZ/5AmxBgI0CNd7vpgeQ46W1rdmtf/5ZcWeo7TtMmsDezatQtlZ2dTMpkMwsLC/vSt
		0BRs374d5+TkkF26dOE3bNhgdvL334Zmx/Tz81uZkpISqfcDLJOhUavB1++1BS4zx5369s6vd1iO
		syHMaxJ4DAzPw8tdXT750idgw7OepA78+SAAAA4ePOiUmZm50thK2RYwScJv135dEfSqd0k/+26H
		Gd7s5pnHQCIEV0vz3o9MSXjlWU9SB/58YFdXV/TVV18dUCgUw1v7rUarTDCGcqWya072fWbCq2O2
		3awqnqdlWWszW6seAUIIWIGXqBidE81yh/t1der4T+7/R8DOzs6jrly5soYgCAYhpAWAdl0IIW1p
		aYmTx0D33UhuI7lfXT6EJIh28wMALUaEtkJd72BnKbvs1XtAgUkWdeC/Ak/8FzEd6MDTQMd/sHfg
		L4kOx+zAXxIdjtmBvyQ6HLMDf0n8H+dMCpLYusouAAAAJXRFWHRkYXRlOmNyZWF0ZQAyMDIzLTAy
		LTA2VDA2OjA4OjEzKzAwOjAwXLEhjQAAACV0RVh0ZGF0ZTptb2RpZnkAMjAyMy0wMi0wNlQwNjow
		ODoxMyswMDowMC3smTEAAAAASUVORK5CYII=
		"" />
		</ svg >
		</a>

		<p class=""text_center"">
			Thank you for using this application.
		</p>
		<h4>
			Copyright © 2024 Somov Studio. All Rights Reserved.
		</h4>
	</div>
</body>
</html>
";

                    StreamWriter writer;
					writer = new StreamWriter(fileStartPage, false, new UTF8Encoding(true));
					
                    if (HatSettings.language == HatSettings.RUS) writer.Write(content_rus);
                    else writer.Write(content_eng);

                    writer.Close();
				}
			}
			catch (Exception ex)
			{
				Config.browserForm.ConsoleMsgError(ex.ToString());
			}
		}



	}
}
