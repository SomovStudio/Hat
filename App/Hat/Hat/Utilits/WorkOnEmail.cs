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
        /*
         * Отправка почты
         * Источник: https://metanit.com/sharp/net/8.1.php
         */
        public static void SendEmail(string subject, string body)
        {
            try
            {
                // почта получателя (подготовка)
                string dataMails = Config.dataMail[3];                              
                string[] mails = dataMails.Split(' ');
                int count = mails.Length;

                // отправитель и получатель
                MailAddress from = new MailAddress(Config.dataMail[0], "Browser Hat");  // отправитель
                MailAddress to = new MailAddress(mails[0]);                             // получатель

                // создаем объект сообщения
                MailMessage message = new MailMessage(from, to);
                message.Subject = subject;                                              // тема письма
                message.Body = body;                                                    // текст письма
                message.IsBodyHtml = true;                                              // письмо представляет код html

                // копии письма
                if(count > 1)
                {
                    for (int i = 1; i < mails.Length; i++)
                    {
                        message.CC.Add(new MailAddress(mails[i]));
                    }
                }                             
                
                // адрес smtp-сервера и порт, с которого будем отправлять письмо
                SmtpClient smtp = new SmtpClient(Config.dataMail[4], Convert.ToInt32(Config.dataMail[5]));
                
                // логин и пароль
                smtp.Credentials = new NetworkCredential(Config.dataMail[1], Config.dataMail[2]);
                if (Config.dataMail[2] == "true") smtp.EnableSsl = true;
                else smtp.EnableSsl = false;

                // отправка письма
                smtp.Send(message);

                Config.browserForm.consoleMsg($"Отправлено писем: {count}");
            }
            catch (Exception ex)
            {
                Config.browserForm.consoleMsgError(ex.ToString());
            }
        }

        

    }
}
