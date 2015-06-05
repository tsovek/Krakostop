using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Krakostop.Models
{
    public enum Recipients
    {
        [Display(Name = "Wszystkim")]
        All = 1,

        [Display(Name = "Ludziom z autokarem")]
        Autocars = 2,

        [Display(Name = "Z autokarem bez zapłaty")]
        AutocarsWithoutPayments = 3,

        [Display(Name = "Z autokarem z zapłatą")]
        AutocarsWithPayments = 4,

        [Display(Name = "Parom bez zapłaty")]
        PairWithoutPayments = 5,

        [Display(Name = "Parom z zapłatą")]
        PairWithPayments = 6,

        [Display(Name = "Parozłączkowiczom")]
        PairJoiners = 7
    };

    public class MailToMany
    {
        KrakostopDbContext db = new KrakostopDbContext();  

        public List<string> ToMany { get; set; }

        [Required()]
        [Display(Name = "Komu wysłać?")]
        [Range(1, int.MaxValue, ErrorMessage = "Wybierz komu wysłać!!!")]
        public Recipients KindOfEmail { get; set; }

        [Required()]
        [Display(Name = "Temat wiadomości")]
        public string Title { get; set; }

        [Required()]
        [Display(Name = "Treść maila")]
        public string Body { get; set; }
        
        public MailToMany()
        { 
            ToMany = new List<string>();
        }

        public IEnumerable<string> GetAllEmails()
        {
            var emails = from pair in db.Pairs
                         from person in pair.Persons
                         select person.Email;

            return emails;
        }

        public IEnumerable<string> GetPairEmailsWithoutPayments()
        {
            var emails = from pair in db.Pairs
                         from person in pair.Persons
                         where !pair.Payments
                         select person.Email;

            return emails;
        }

        public IEnumerable<string> GetPairEmailsWithPayments()
        {
            var emails = from pair in db.Pairs
                         from person in pair.Persons
                         where pair.Payments
                         select person.Email;

            return emails;
        }

        public IEnumerable<string> GetAllAutocars()
        {
            var emails = from person in db.People
                         where person.Autocar
                         select person.Email;

            return emails;
        }

        public IEnumerable<string> GetAllAutocarsWithoutPayments()
        {
            var emails = from person in db.People
                         where person.Autocar && 
                            !person.AutocarPayments
                         select person.Email;

            return emails;
        }

        public IEnumerable<string> GetAllAutocarsWithPayments()
        {
            var emails = from person in db.People
                         where person.Autocar &&
                            person.AutocarPayments
                         select person.Email;

            return emails;
        }

        public IEnumerable<string> GetEmailsFromPairJoiners()
        {
            var emails = from pj in db.Pair_Joiners
                         select pj.User.Email;

            return emails;
        }

        public IEnumerable<string> GetEmails(Recipients rec)
        { 
            switch(rec)
            { 
                case Recipients.All:
                    return this.GetAllEmails();
                case Recipients.Autocars:
                    return this.GetAllAutocars();
                case Recipients.AutocarsWithoutPayments:
                    return this.GetAllAutocarsWithoutPayments();
                case Recipients.AutocarsWithPayments:
                    return this.GetAllAutocarsWithPayments();
                case Recipients.PairJoiners:
                    return this.GetEmailsFromPairJoiners();
                case Recipients.PairWithoutPayments:
                    return this.GetPairEmailsWithoutPayments();
                case Recipients.PairWithPayments:
                    return this.GetPairEmailsWithPayments();
                default: return null;
            }
        }
    }
}