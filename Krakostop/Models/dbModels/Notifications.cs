using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace Krakostop.Models.dbModels
{
    public enum NotificationType
    { 
        [Description("Rejestracja")]
        Register = 1,

        [Description("Zatwierdzenie")]
        Confirm = 2,

        [Description("Płatność za parę")]
        PaymentPair = 3,

        [Description("Płatność za autokar")]
        PaymentAutocar = 4
    }

    public class Notifications
    {
        public Notifications()
        {
            Time = DateTime.Now;
        }
        public int ID { get; set; }

        public DateTime Time { get; set; }

        public string Desc { get; set; }

        public NotificationType NotifType { get; set; }

        public string User { get; set; }
    }

    public static class ReflectionHelpers
    {
        public static string GetCustomDescription(object objEnum)
        {
            var fi = objEnum.GetType().GetField(objEnum.ToString());
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : objEnum.ToString();
        }

        public static string Description(this Enum value)
        {
            return GetCustomDescription(value);
        }
    }
}