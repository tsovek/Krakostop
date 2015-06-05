using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Krakostop.Models;
using System.Net.Mail;
using System.Net;

namespace Krakostop.Mails
{
    public static class Mailing
    {
        public static void SendEmailWithoutHtml(string email, string subject, string body)
        {
            SendEmail("krakostop@gmail.com", email, subject, body, false, "Krakostop");
        }

        public static void SendEmail(string email, string subject, string body)
        {
            SendEmail("krakostop@gmail.com", email, subject, body, true, "Krakostop");
        }

        public static void SendEmail(string from, string to, 
            string sbj, string body, bool isHtml, string name)
        {
            MailMessage mail = new MailMessage(from, to, sbj, body);
            mail.From = new MailAddress(from, name);
            mail.IsBodyHtml = isHtml;

            NetworkCredential credential = new NetworkCredential("krakostop@gmail.com", "bondziorno");
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = credential;
            try
            {
                smtp.Send(mail);
            }
            catch
            { 
                
            }
        }

        public static string UserNameFactory(Person p1, Person p2)
        {
            return char.ToUpper(p1.Name[0]) + 
                   p1.Name.Substring(1).ToLower() + 
                   char.ToUpper(p1.Surname[0]) + 
                   "_" + 
                   char.ToUpper(p2.Name[0]) +
                   p2.Name.Substring(1).ToLower() +
                   char.ToUpper(p2.Surname[0]);
        }

        public static string NamesToDisplay(List<Person> people, 
            string userName, bool withUserName)
        {
            string namesToDisplay = string.Empty;
            people.ForEach(p =>
                   {
                        namesToDisplay += (p.Name + " ");
                        namesToDisplay += (p.Surname) + " i ";
                   });
            namesToDisplay = namesToDisplay.Substring(0, 
                namesToDisplay.LastIndexOf('i') - 1);
            if (withUserName)
            {
                namesToDisplay += " (" + userName + ")";
            }
            return namesToDisplay;
        }
    }
}