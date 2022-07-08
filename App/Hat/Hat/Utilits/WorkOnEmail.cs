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

namespace Hat
{
    public class WorkOnEmail
    {
        /* Работа с электронной почтой Отправка почты. SmtpClient
         * Источник: https://metanit.com/sharp/net/8.1.php
         */
        public static void SendMail_BAD()
        {
            try
            {
                // отправитель - устанавливаем адрес и отображаемое в письме имя
                MailAddress from = new MailAddress("sep@zionec.ru", "SEP");
                // кому отправляем
                MailAddress to = new MailAddress("raven-84@mail.ru");
                // создаем объект сообщения
                MailMessage m = new MailMessage(from, to);
                // тема письма
                m.Subject = "Тестовое письмо из браузера HAt";
                // текст письма
                m.Body = "<h2>Письмо-тест работы smtp-клиента</h2>";
                // письмо представляет код html
                m.IsBodyHtml = true;
                // адрес smtp-сервера и порт, с которого будем отправлять письмо
                SmtpClient smtp = new SmtpClient("smtp.yandex.ru", 465);
                // логин и пароль
                smtp.Credentials = new NetworkCredential("sep@zionec.ru", "800jNT6zwMQrfl");
                smtp.EnableSsl = true;
                //ОШИБКА smtp.Send(m);
                Console.Read();
                Config.browserForm.consoleMsg("Тест: Письмо отправлено");
            }
            catch (Exception ex)
            {
                Config.browserForm.consoleMsgError(ex.ToString());
            }
        }

        /* Не отправляется письмо через smtp mail - время ожидания операции истекло
         * Источник: https://ru.stackoverflow.com/questions/652352/%D0%9D%D0%B5-%D0%BE%D1%82%D0%BF%D1%80%D0%B0%D0%B2%D0%BB%D1%8F%D0%B5%D1%82%D1%81%D1%8F-%D0%BF%D0%B8%D1%81%D1%8C%D0%BC%D0%BE-%D1%87%D0%B5%D1%80%D0%B5%D0%B7-smtp-mail-%D0%B2%D1%80%D0%B5%D0%BC%D1%8F-%D0%BE%D0%B6%D0%B8%D0%B4%D0%B0%D0%BD%D0%B8%D1%8F-%D0%BE%D0%BF%D0%B5%D1%80%D0%B0%D1%86%D0%B8%D0%B8-%D0%B8%D1%81%D1%82%D0%B5%D0%BA%D0%BB%D0%BE
         */

        public static void MessageSend_BAD()
        {
            try
            {
                Config.browserForm.consoleMsg("Тест: Начинается отправка письма");
                MailMessage message = new MailMessage();
                SmtpClient client =
                new SmtpClient("smtp.yandex.ru", 465) // сервер,порт
                {
                    Credentials = new NetworkCredential("sep@zionec.ru", "800jNT6zwMQrfl"),
                    EnableSsl = true // обязательно!
                };
                message.From = new MailAddress("sep@zionec.ru"); // от кого
                message.To.Add(new MailAddress("raven-84@mail.ru")); // кому
                message.Subject = "Тестовое письмо из браузера Hat";
                message.SubjectEncoding = Encoding.UTF8;
                message.Body = "Тестовое сообщение";
                message.BodyEncoding = Encoding.UTF8; // кодировка 
                //string fileName = @"C:\Resume.txt"; // какой-нибудь файл
                //Attachment item = new Attachment(fileName);
                //message.Attachments.Add(item);// добавляем файл к сообщению
                client.Send(message); // отправка сообщения
                Config.browserForm.consoleMsg("Тест: Письмо отправлено");
            }
            catch (Exception ex)
            {
                Config.browserForm.consoleMsgError(ex.ToString());
            }
        }

        /* send email using implicit ssl
         * Источник: 
         */
        
        public static void SendEmail()
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                Config.browserForm.consoleMsgError(ex.ToString());
            }
        }

        

    }
}
