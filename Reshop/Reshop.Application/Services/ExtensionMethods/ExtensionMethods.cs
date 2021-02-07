using System;
using System.Globalization;

namespace Reshop.Application.Services.ExtensionMethods
{
    public static class ExtensionMethods
    {
        public static string ToShamsi(this DateTime value)
        {
            PersianCalendar pc = new PersianCalendar();
            return pc.GetYear(value) + "/" + pc.GetMonth(value).ToString("00") + "/" +
                   pc.GetDayOfMonth(value).ToString("00") + " - " + pc.GetHour(value).ToString("00") + ":" + pc.GetMinute(value).ToString("00");

        }

        public static string ToToman(this decimal value)
        {
            return value.ToString("#,0 تومان");
        }
    }
}