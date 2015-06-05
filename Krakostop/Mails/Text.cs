using Krakostop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Krakostop.Mails
{
    public class Text
    {
        public static string RegisterTitle = "Potwierdzenie rejestracji";
        public static string PaymentPairTitle = "Potwierdzenie płatności za parę";

        public static string GenerateJoinerEmail(string name, string email, string body)
        { 
            return @"
                Ten email został wysłany z formularza na stronie krakostop.pl!!!
                NIE ODPISUJ NA NIEGO! 
            
                " + @"
                Od: " + name + " (" + email + ")" +
                @"
                Treść:
                " + body;
        }

        public static string GetNames(KrakostopUser user)
        { 
            if (user.Pair == null)
            { 
                return "";
            }
            var people = user.Pair.Persons.ToList();
            return people[0].Name + " " + people[0].Surname + ", " 
                 + people[1].Name + " " + people[1].Surname;
        }

        public static string RegisterBody(string user, string titleAccount, KrakostopUser names)
        {
            return @"
        <b>Dziękujemy za rejestrację!</b><br><br>

        Wspaniale, że dołączyliście do wyścigu. Następnym krokiem jaki musicie zrobić to 
        przelew bankowy na kwotę 70zł na poniższe dane: <br><br>

        <b>Dawid Kmieć</b><br>
        71 1600 1013 1844 0174 4000 0001<br>
        ul. Kaczkowskiego 3/3, 33-100 Tarnów<br>" +
        "Tytuł przelewu: <b>Krakostop, Para nr " + titleAccount + "," + GetNames(names) + "</b>.<br>" +
        @"<br>
        Wasze dane możecie zweryfikować / poprawić logując się na stronie
        <a href='http://krakostop.pl'>Krakostopu</a>. Jako login możesz podać
        email którejś z osób lub Wasz unikatowy identyfikator: 
        <b>" + user + @"</b>. Tam znajdziecie również powiadomienia o wpłatach i 
        inne społecznościowe funkcje. 
        W razie problemów prosimy o kontakt na ten email.<br><br>
        Pozdrawiamy,<br>
        Ekipa Krakostop";
        }

        public static string RegisterJoiner(string user)
        { 
            return @"
        <b>Dziękujemy za rejestrację!</b><br><br>

        Twóje dane zostały opublikowane w indeksie Parozłączki. 
        Możesz je edytować logując się na stronie www.krakostop.pl. 
        Jako login wykorzystaj swój email: " + user + 
        @"<br><br>W razie problemów prosimy o kontakt na ten email.<br><br>
        Pozdrawiamy,<br>
        Ekipa Krakostop";
        }

        public static string PaymentPairBody(string user)
        {
            return @"
        Informujemy, że Wasza wpłata została zaksięgowana na naszym koncie.<br><br>
        
        Wasze dane możecie zweryfikować / poprawić logując się na stronie
        <a href='http://krakostop.pl'>Krakostopu</a>. Jako login możesz podać
        email którejś z osób lub Wasz unikatowy login: <b>
        " + user + @"</b>. Tam znajdziecie również powiadomienia o wpłatach i 
        inne społecznościowe funkcje. 
        W razie problemów prosimy o kontakt na ten email.<br><br>
        Pozdrawiamy,<br>
        Ekipa Krakostop";
        }
    }
}