═══════════════════════════════════════════════════════════════════════════════
HatFramework Skill for AI Test Automation
Версия фреймворка: 1.5.2.4
Цель: Написание и поддержка автотестов на C# с использованием HatFramework.
═══════════════════════════════════════════════════════════════════════════════

СОДЕРЖАНИЕ (для навигации в файле):

Общие принципы и правила

Структура проекта

Правила описания XPath локаторов

Класс Tester – Основной API

Классы CommonPage и CommonSteps – PageObject + StepsObject

Пример полноценного автотеста

Чек-лист для ИИ при генерации тестов

═══════════════════════════════════════════════════════════════════════════════

ОБЩИЕ ПРИНЦИПЫ И ПРАВИЛА

Никогда не используйте интерполяцию строк $"..." – она НЕ ПОДДЕРЖИВАЕТСЯ фреймворком.

Для подстановки значений используйте:

Конкатенацию: "(" + CommonPage.Element + ")[1]"

string.Format(): string.Format("//div[@class='{0}']", className)

Метод ToString() для чисел: count.ToString()

Вывод сообщений – перед SendMessage или SendMessageDebug НЕ НУЖНО писать await.

Не проверяйте ошибки в консоли браузера (метод AssertNoErrorsAsync не рекомендуется без крайней необходимости).

Обязательно используйте паттерны:

CommonPage – хранилище XPath локаторов (только строки).

CommonSteps – методы, выполняющие действия и проверки.

Всегда работайте через хранилище локаторов: сначала AddLocator, затем GetLocator.

Методы ожидания:

WaitVisibleElementAsync – ждёт видимость элемента В ОБЛАСТИ ПРОСМОТРА (если элемент за пределами экрана – не сработает, нужно предварительно ScrollToElementAsync).

FindElementAsync – аналогично, ищет только в DOM, но для видимости тоже нужен скролл.

WaitElementInDomAsync – проверяет наличие элемента в DOM без учёта видимости.

Для работы с числами всегда вызывайте .ToString() при конкатенации в XPath или сообщениях.

═══════════════════════════════════════════════════════════════════════════════

СТРУКТУРА ПРОЕКТА

Проект/
├── support/
│ ├── Helper.cs
│ └── PageObjects/
│ │ └── CommonPage.cs # XPath локаторы
│ └── StepObjects/
│ └── CommonSteps.cs # методы действий и проверок
└── tests/
└── Test_AuthPage.cs # автотест

Helper.cs – общие переменные

using System;
using HatFramework;

namespace Hat
{
public static class Helper
{
public static string URL = @"https://somovstudio.github.io/test.html";
}
}

CommonPage.cs – локаторы (только строки)

namespace Hat
{
public static class CommonPage
{
// Поле ввода логина
public static string AuthPageInputLogin = "//input[@id='login']";
// Поле ввода пароля
public static string AuthPageInputPass = "//input[@id='pass']";
// Кнопка войти
public static string AuthPageButtonLogin = "//input[@id='buttonLogin']";
// Окно результата
public static string AuthPageResult = "//div[@id='result']";
}
}

CommonSteps.cs – шаги (наследуются от Tester)

using HatFramework;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace Hat
{
public class CommonSteps : Tester
{
public CommonSteps(Form browserWindow) : base(browserWindow) { }

public void InitAuthLocators(bool clear = true)
{
if (clear) ClearLocators();
AddLocator("AuthPageInputLogin", Tester.BY_XPATH, CommonPage.AuthPageInputLogin, "Поле ввода логина");
AddLocator("AuthPageInputPass", Tester.BY_XPATH, CommonPage.AuthPageInputPass, "Поле ввода пароля");
AddLocator("AuthPageButtonLogin", Tester.BY_XPATH, CommonPage.AuthPageButtonLogin, "Кнопка войти");
AddLocator("AuthPageResult", Tester.BY_XPATH, CommonPage.AuthPageResult, "Результат авторизации");
SendMessage("InitAuthLocators", Tester.PASSED, "Локаторы инициализированы");
}

public async Task FillLoginForm(string login, string password)
{
await WaitVisibleElementAsync(GetLocator("AuthPageInputLogin"), 15);
await SetValueInElementAsync(GetLocator("AuthPageInputLogin"), login);
await SetValueInElementAsync(GetLocator("AuthPageInputPass"), password);
}
}
}

═══════════════════════════════════════════════════════════════════════════════

ПРАВИЛА ОПИСАНИЯ XPATH ЛОКАТОРОВ

Избегайте интерполяции строк – только конкатенация или string.Format.

Именование локаторов: ИмяСтраницы + Блок + ТипЭлемента.
Пример: AuthPageInputLogin, MainPageButtonSubmit.

Для индексов используйте:
string xpath = CommonPage.SomeElement + "[" + index.ToString() + "]";
или
string.Format("{0}[{1}]", CommonPage.SomeElement, index);

Текстовые локаторы:

Частичное совпадение: [contains(text(), 'текст')]

Точное совпадение: [text()='текст']

Игнорирование пробелов: [normalize-space()='текст'] или [contains(normalize-space(), 'текст')]

Всегда комментируйте локаторы в CommonPage.

В CommonSteps создавайте метод инициализации локаторов (например, InitAuthLocators), который добавляет их через AddLocator.

═══════════════════════════════════════════════════════════════════════════════

КЛАСС Tester – ОСНОВНОЙ API

Конструктор:
Tester tester = new Tester(Form browserWindow);

Глобальные константы:

Типы локаторов: Tester.BY_CSS, Tester.BY_XPATH

Статусы: Tester.PASSED, Tester.FAILED, Tester.PROCESS, Tester.COMPLETED, Tester.STOPPED, Tester.WARNING, Tester.DEBUG

Изображения: Tester.IMAGE_STATUS_PASSED (1), IMAGE_STATUS_FAILED (2), IMAGE_STATUS_PROCESS (0), IMAGE_STATUS_MESSAGE (3), IMAGE_STATUS_WARNING (4), IMAGE_STATUS_DEBUG (5)

Кодировки: Tester.DEFAULT, Tester.UTF8, Tester.UTF8BOM, Tester.WINDOWS1251

Работа с локаторами:

AddLocator(string name, string type, string value, string description) – добавляет локатор

GetLocator(string name) – возвращает объект Locator

GetLocatorValue(string name) – возвращает строку XPath/CSS

GetCountLocators() – количество локаторов

ClearLocators() – очищает хранилище

RemoveLocator(string name) – удаляет локатор, возвращает bool

Класс Locator:
public class Locator {
public string name { get; set; }
public string description { get; set; }
public string type { get; set; }
public string value { get; set; }
}

Управление тестом:

Task TestBeginAsync() – обязательный старт теста

Task TestEndAsync() – обязательное завершение (сохраняет отчёт, отправляет письма)

Task TestStopAsync() – принудительная остановка

bool DefineTestStop() – проверка, остановлен ли тест

string GetTestResult() – возвращает PASSED / FAILED / PROCESS

void Description(string text) – описание для отчёта

void DisableDebugInReport() – отключить отладочные сообщения в отчёте

Браузерные методы:

Навигация и размеры:

Task GoToUrlAsync(string url, int sec, bool abortLoadAfterTime = false)

Task GoToUrlBaseAuthAsync(string url, string login, string pass, int sec, bool abortLoadAfterTime = false)

Task LoadPageAsync(string url, int sec, bool abortLoadAfterTime = false) – игнорирует ошибки загрузки

Task BrowserGoBackAsync(int sec, bool abortLoadAfterTime = false)

Task BrowserGoForwardAsync(int sec, bool abortLoadAfterTime = false)

Task BrowserPageReloadAsync(int sec, bool abortLoadAfterTime = false)

Task BrowserSizeAsync(int width, int height)

Task BrowserFullScreenAsync()

Task BrowserCloseAsync()

Task<string> GetUrlAsync()

Task<string> GetTitleAsync()

Task<List<string>> GetListRedirectUrlAsync()

Работа с User-Agent и куки:

Task BrowserSetUserAgentAsync(string value)

Task<string> BrowserGetUserAgentAsync()

Task<List<string>> GetCookiesAsync()

Скриншоты:

Task<string> BrowserScreenshotAsync(string filename) – возвращает путь к сохранённому скриншоту

Работа с консолью браузера и network:

Task<List<string>> BrowserGetErrorsAsync()

Task<string> BrowserGetNetworkAsync()

Task<string> BrowserClearNetworkAsync()

Другое:

Task BrowserBasicAuthenticationAsync(string user, string pass)

Task BrowserEnableSendMailAsync(bool byFailure = true, bool bySuccess = true)

Ожидания и поиск элементов:

Все методы ожидания и поиска перегружены: принимают либо (string by, string locator), либо (Locator locator).

Ожидания видимости / невидимости:

Task WaitVisibleElementAsync(string by, string locator, int sec)

Task WaitVisibleElementByIdAsync(string id, int sec)

Task WaitVisibleElementByClassAsync(string _class, int index, int sec)

Task WaitVisibleElementByNameAsync(string name, int index, int sec)

Task WaitVisibleElementByTagAsync(string tag, int index, int sec)
Аналогично WaitNotVisibleElement...

Ожидание появления/исчезновения в DOM:

Task WaitElementInDomAsync(string by, string locator, int sec)

Task WaitElementNotDomAsync(...)

Поиск (возвращает bool без ожидания видимости):

Task<bool> FindElementAsync(string by, string locator, int sec)

Task<bool> FindVisibleElementAsync(...) – ищет видимый элемент

Простое ожидание:

Task WaitAsync(int sec)

Действия с элементами:

Клик:

Task ClickElementAsync(string by, string locator)

Task ClickElementByIdAsync(string id)

Task ClickElementByClassAsync(string _class, int index)

и т.д.

Ввод значений (value):

Task SetValueInElementAsync(string by, string locator, string value)

Task SetValueInElementByIdAsync(string id, string value)

и т.д.

Чтение значений (value):

Task<string> GetValueFromElementAsync(string by, string locator)

Текст (innerText / textContent):

Task SetTextInElementAsync(string by, string locator, string text)

Task<string> GetTextFromElementAsync(string by, string locator)

Атрибуты:

Task SetAttributeInElementAsync(string by, string locator, string attribute, string value)

Task<string> GetAttributeFromElementAsync(string by, string locator, string attribute)
Работа с группами элементов: GetAttributeFromElementsAsync, SetAttributeInElementsAsync и т.д.

Стили:

Task SetStyleInElementAsync(string by, string locator, string cssText)

Task<string> GetStyleFromElementAsync(string by, string locator, string property)

Task MakeElementVisibleAsync(string by, string locator, string visibility = "visible", int opacity = 1, int index = 1000)

Другое:

Task ScrollToElementAsync(string by, string locator, bool behaviorSmooth = false)

Task FocusElementAsync(string by, string locator)

Task<HTMLElement> GetElementAsync(string by, string locator) – возвращает объект с методами ClickAsync(), GetTextAsync() и т.д.

Task<FRAMEElement> GetFrameAsync(int index) – работа с фреймами

Выполнение JavaScript:

Task<string> ExecuteJavaScriptAsync(string script) – возвращает результат (строка)

REST запросы:

Task<string> RestGetAsync(string url, TimeSpan timeout, string charset = "UTF-8")

Task<string> RestGetBasicAuthAsync(string login, string pass, string url, TimeSpan timeout, string charset = "UTF-8")

Task<int> RestGetStatusCodeAsync(string url)

Task<string> RestPostAsync(string url, string json, TimeSpan timeout, string charset = "UTF-8")

Файлы:

Task<string> FileReadAsync(string encoding, string filename)

Task FileWriteAsync(string content, string encoding, string filename)

Task FileDownloadAsync(string fileURL, string filename, int waitingSec = 60)

Task<string> FileGetHashMD5Async(string filename)

Task<string> CreateHashMD5FromTextAsync(string text)

Отправка уведомлений:

Task SendMsgToMailAsync(string subject, string body, string filename = "", string addresses = "")

Task SendMsgToTelegramAsync(string botToken, string chatId, string text, string charset = "UTF-8", int timeHourFrom = 0, int timeHourBefore = 0)

Task<string> SendMsgToBitrixChatAsync(string webhookUrl, string chatId, string messageText, string system = "N")

Сообщения в лог/отчёт:

void ConsoleMsg(string messageRus, string messageEng)

void ConsoleMsgError(string message)

void ClearMessages()

void SendMessage(string action, string status, string comment)

void SendMessageDebug(string actionRus, string actionEng, string status, string commentRus, string commentEng, int image)

Проверки (Assert):
Все Assert* методы возвращают Task<bool> и при провале останавливают тест.

AssertEqualsAsync(dynamic expected, dynamic actual)

AssertNotEqualsAsync(dynamic expected, dynamic actual)

AssertTrueAsync(bool condition)

AssertFalseAsync(bool condition)

AssertNotNullAsync(dynamic obj)

AssertNullAsync(dynamic obj)

AssertNoErrorsAsync(bool showListErrors = false, string[] listIgnored = null) – использовать осторожно

AssertNetworkEventsAsync(bool presence, string[] events)

Замер времени:

Task<DateTime> TimerStart()

Task<TimeSpan> TimerStop(DateTime start)

Прочее:

string GetProjectPath() – возвращает путь к папке проекта

═══════════════════════════════════════════════════════════════════════════════

КЛАССЫ CommonPage И CommonSteps – PageObject + StepsObject

Правила написания CommonPage:

Только статические строковые поля – XPath или CSS.

Комментарий к каждому локатору.

Не использовать $"..."

Пример:
public static class CommonPage
{
// Заголовок формы авторизации
public static string AuthPageHeader = "//h2[contains(text(),'Авторизация')]";
}

Правила написания CommonSteps:

Наследовать от Tester.

Конструктор вызывает base(browserWindow).

Создать метод Init<Page>Locators(bool clear = true), который вызывает ClearLocators() (опционально) и AddLocator для каждого локатора.

Группировать действия в осмысленные методы (например, LoginAsUser, FillOrderForm).

Внутри методов использовать GetLocator(...) для обращения к локаторам.

Пример:
public class CommonSteps : Tester
{
public CommonSteps(Form browserWindow) : base(browserWindow) { }

public void InitAuthLocators(bool clear = true)
{
if (clear) ClearLocators();
AddLocator("AuthPageHeader", Tester.BY_XPATH, CommonPage.AuthPageHeader, "Заголовок формы");
}

public async Task CheckHeaderText(string expectedText)
{
await WaitVisibleElementAsync(GetLocator("AuthPageHeader"), 10);
string actual = await GetTextFromElementAsync(GetLocator("AuthPageHeader"));
await AssertEqualsAsync(expectedText, actual);
}
}

═══════════════════════════════════════════════════════════════════════════════

ПРИМЕР ПОЛНОЦЕННОГО АВТОТЕСТА

Тест-кейс:

Открыть страницу Helper.URL

Ввести логин admin, пароль 0000

Нажать кнопку входа

Проверить, что появилось сообщение "Вы успешно авторизованы"

CommonPage.cs:

public static class CommonPage
{
public static string AuthPageInputLogin = "//input[@id='login']";
public static string AuthPageInputPass = "//input[@id='pass']";
public static string AuthPageButtonLogin = "//input[@id='buttonLogin']";
public static string AuthPageResultTextarea = "//textarea[@id='textarea']";
}

CommonSteps.cs:

public class CommonSteps : Tester
{
public CommonSteps(Form browserWindow) : base(browserWindow) { }

public void InitAuthLocators(bool clear = true)
{
if (clear) ClearLocators();
AddLocator("InputLogin", Tester.BY_XPATH, CommonPage.AuthPageInputLogin, "Поле логина");
AddLocator("InputPass", Tester.BY_XPATH, CommonPage.AuthPageInputPass, "Поле пароля");
AddLocator("ButtonLogin", Tester.BY_XPATH, CommonPage.AuthPageButtonLogin, "Кнопка войти");
AddLocator("ResultArea", Tester.BY_XPATH, CommonPage.AuthPageResultTextarea, "Результат");
}

public async Task PerformLogin(string login, string pass)
{
await WaitVisibleElementAsync(GetLocator("InputLogin"), 15);
await SetValueInElementAsync(GetLocator("InputLogin"), login);
await SetValueInElementAsync(GetLocator("InputPass"), pass);
await ClickElementAsync(GetLocator("ButtonLogin"));
}

public async Task AssertSuccessMessage(string expected)
{
await WaitVisibleElementAsync(GetLocator("ResultArea"), 15);
string actual = await GetValueFromElementAsync(GetLocator("ResultArea"));
await AssertEqualsAsync(expected, actual);
}
}

Test_AuthPage.cs:

public class Test_AuthPage
{
CommonSteps tester;

public async void Main(Form browserWindow)
{
tester = new CommonSteps(browserWindow);
await setUp();
await test();
await tearDown();
}

public async Task setUp()
{
tester.Description("Автотест проверяет успешную авторизацию");
tester.InitAuthLocators();
await tester.BrowserFullScreenAsync();
}

public async Task test()
{
await tester.TestBeginAsync();
await tester.GoToUrlAsync(Helper.URL, 5);
await tester.PerformLogin("admin", "0000");
await tester.AssertSuccessMessage("Вы успешно авторизованы");
await tester.TestEndAsync();
}

public async Task tearDown()
{
await tester.BrowserCloseAsync();
}
}

═══════════════════════════════════════════════════════════════════════════════

ЧЕК-ЛИСТ ДЛЯ ИИ ПРИ ГЕНЕРАЦИИ ТЕСТОВ

Проверить, что все строковые подстановки выполнены через конкатенацию или string.Format, а не интерполяцию.

Все локаторы вынесены в CommonPage и имеют комментарии.

В CommonSteps есть метод инициализации локаторов с вызовом AddLocator.

Действия разбиты на маленькие методы (не пишем длинный код прямо в test()).

Тест содержит:

setUp() – описание, инициализация локаторов, настройка браузера.

test() – TestBeginAsync, действия, проверки, TestEndAsync.

tearDown() – закрытие браузера.

Используем GetLocator везде, где нужен локатор.

Перед SendMessage нет await.

Для ожидания видимости элемента, который может быть вне экрана, сначала вызываем ScrollToElementAsync.

Для поиска элемента без проверки видимости используем FindElementAsync или WaitElementInDomAsync.

Все Assert возвращают bool, но мы не обязаны их присваивать – они сами останавливают тест при провале.

═══════════════════════════════════════════════════════════════════════════════
КОНЕЦ ДОКУМЕНТА
═══════════════════════════════════════════════════════════════════════════════