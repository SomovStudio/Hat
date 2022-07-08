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
                string mailFrom = Config.dataMail[0];
                string userFrom = Config.dataMail[1];
                string passFrom = Config.dataMail[2];
                string mailsTo = Config.dataMail[3];
                string smtpServer = Config.dataMail[4];
                string portServer = Config.dataMail[5];
                string ssl = Config.dataMail[6];

                /*
                Config.browserForm.consoleMsg($"mailFrom: {mailFrom}");
                Config.browserForm.consoleMsg($"userFrom: {userFrom}");
                Config.browserForm.consoleMsg($"passFrom: {passFrom}");
                Config.browserForm.consoleMsg($"mailsTo: {mailsTo}");
                Config.browserForm.consoleMsg($"smtpServer: {smtpServer}");
                Config.browserForm.consoleMsg($"portServer: {portServer}");
                Config.browserForm.consoleMsg($"ssl: {ssl}");

                Config.browserForm.consoleMsg($"subject: {subject}");
                Config.browserForm.consoleMsg($"body: {body}");
                */

                string[] mails = mailsTo.Split(' ');
                int count = mails.Length;

                // отправитель и получатель
                MailAddress from = new MailAddress(mailFrom, "Browser Hat");    // отправитель
                MailAddress to = new MailAddress(mails[0]);                     // получатель

                // создаем объект сообщения
                MailMessage message = new MailMessage(from, to);
                message.Subject = subject;                                      // тема письма
                message.Body = body;                                            // текст письма
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
                
                // логин и пароль
                smtp.Credentials = new NetworkCredential(userFrom, passFrom);
                if (ssl == "true") smtp.EnableSsl = true;
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
