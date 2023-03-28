using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.ComponentModel;
using System.Web;
using System.Net.Mime;
using System.Text.RegularExpressions;
using System.Windows.Shapes;

namespace Hat
{
    public class WorkOnEmail
    {
        /*
         * Отправка почты
         * Источник: https://metanit.com/sharp/net/8.1.php
         */
        public static void SendEmail(string subject, string body, string filename, string addresses = "")
        {
            try
            {
                // почта получателя (подготовка)
                string mailFrom = Config.dataMail[0];
                string userFrom = Config.dataMail[1];
                string passFrom = Config.dataMail[2];
                string mailsTo = Config.dataMail[3];
                string smtpServer = Config.dataMail[4];
                string portServer = Config.dataMail[5];
                string ssl = Config.dataMail[6];

                Config.browserForm.ConsoleMsg($"Данные почты: {smtpServer} | {portServer} | {ssl} | {userFrom} | {passFrom}");
                if (Config.languageEngConsole == false) Config.browserForm.SystemConsoleMsg($"Данные почты: SMTP: {smtpServer} | Port: {portServer} | SSL: {ssl} | Login: {userFrom} | Pass: *******", default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                else Config.browserForm.SystemConsoleMsg($"Mail Data: SMTP: {smtpServer} | Port: {portServer} | SSL: {ssl} | Login: {userFrom} | Pass: *******", default, ConsoleColor.DarkGray, ConsoleColor.White, true);

                string[] mails;
                int count = 0;
                if (addresses == "")
                {
                    mails = mailsTo.Split(' ');
                    count = mails.Length;
                }
                else
                {
                    mails = addresses.Split(' ');
                    count = mails.Length;
                }

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                // отправитель и получатель
                MailAddress from = new MailAddress(mailFrom, "Browser Hat");    // отправитель
                MailAddress to = new MailAddress(mails[0]);                     // получатель

                // вложенный файл
                Attachment attachment = null;
                if (filename != "")
                {
                    string pattern = @"(/i)(.*)[^""\s/>]";
                    string file = Regex.Match(filename, pattern).Value;         // получим имя файла /image-22-51-2023-10-51-44.jpeg
                    file = file.Substring(1);                                   // коррекция имени файла
                    file = Report.FolderImagesName + file;                      // конечный путь к файлу
                    file = file.Replace("/", "\\");                             // коррекция пути
                    Config.browserForm.ConsoleMsg($"Вложенный файл в письме: {file}");

                    attachment = new Attachment(file, MediaTypeNames.Application.Octet);
                    ContentDisposition disposition = attachment.ContentDisposition;
                    disposition.CreationDate = System.IO.File.GetCreationTime(file);
                    disposition.ModificationDate = System.IO.File.GetLastWriteTime(file);
                    disposition.ReadDate = System.IO.File.GetLastAccessTime(file);
                }

                // создаем объект сообщения
                MailMessage message = new MailMessage(from, to);
                message.Subject = subject;                                      // тема письма
                message.Body = body;                                            // текст письма
                if (attachment != null) message.Attachments.Add(attachment);    // файл в письме
                message.IsBodyHtml = true;                                      // письмо представляет код html

                // копии письма
                if(count > 1)
                {
                    for (int i = 1; i < mails.Length; i++)
                    {
                        message.CC.Add(new MailAddress(mails[i]));
                    }
                }                             
                
                // адрес smtp-сервера и порт, с которого будем отправлять письмо
                SmtpClient smtp = new SmtpClient(smtpServer, Convert.ToInt32(portServer));
                smtp.UseDefaultCredentials = false;
                smtp.DeliveryFormat = SmtpDeliveryFormat.International;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                if (ssl == "true") smtp.EnableSsl = true;
                else smtp.EnableSsl = false;
                smtp.Timeout = 60000; // 60000 - 60 секунд

                // логин и пароль
                smtp.Credentials = new NetworkCredential(userFrom, passFrom);

                // отправка письма
                smtp.Send(message);

                Config.browserForm.ConsoleMsg($"Писем было отправлено: {count}");
                if (Config.languageEngConsole == false) Config.browserForm.SystemConsoleMsg("Писем было отправлено: " + count.ToString(), default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                else Config.browserForm.SystemConsoleMsg(count.ToString() + " emails were sent", default, ConsoleColor.DarkGray, ConsoleColor.White, true);
            }
            catch (Exception ex)
            {
                Config.browserForm.ConsoleMsgError(ex.ToString());
            }
        }

        

    }
}
