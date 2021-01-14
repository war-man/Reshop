using System;
using System.Globalization;

namespace Reshop.Application.Services.DateConvertor
{
    public static class DateConvertor
    {
        public static string ToShamsi(this DateTime value)
        {
            PersianCalendar pc = new PersianCalendar();
            return pc.GetYear(value) + "/" + pc.GetMonth(value).ToString("00") + "/" +
                   pc.GetDayOfMonth(value).ToString("00") + " - " + pc.GetHour(value).ToString("00") + ":" + pc.GetMinute(value).ToString("00");

        }
    }
}