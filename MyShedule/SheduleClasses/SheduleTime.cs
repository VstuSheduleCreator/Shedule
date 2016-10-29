using System;

namespace MyShedule
{
    [Serializable]
    public class SheduleTime
    {

        #region Constructors SheduleTime

        public SheduleTime()
        {
            Week = Week.Another;
            Day = Day.Another;
            Hour = 0;
        }

        public SheduleTime(Week week, Day day, int hour)
        {
            Week = week;
            Day = day;
            Hour = hour;
        }

        #endregion

        #region Fields SheduleTime

        public Week Week;

        public Day Day;

        private int _hour;


        public int WeekNumber
        {
            get
            {
                return (int)Week;
            }
        }

        public int DayNumber
        {
            get
            {
                return (int)Day;
            }
        }

        // Номер пары
        public int Hour
        {
            get
            {
                return _hour;
            }
            set
            {
                // В день не может быть больше 8-ми пар
                // hour == 0 обозначает неизвестный час
                if (0 <= value && value < 9)
                    _hour = value;
                else
                    throw new ArgumentOutOfRangeException("_hour", "В день не может быть больше 8-ми пар");
            }
        }

        #endregion

        public SheduleTime Copy()
        {
            return new SheduleTime(Week, Day, Hour);
        }

        public override bool Equals(object obj) 
        {
            if (!(obj is SheduleTime))                
                return false;
            return Equals((SheduleTime)obj); 
        }

        public bool Equals(SheduleTime other) 
        {
            return Week == other.Week && Day == other.Day && Hour == other.Hour;
        }

        #region Compare methods SheduleTime

        public static bool operator ==(SheduleTime time1, SheduleTime time2)
        {
            return time1.Equals(time2);
        }

        public static bool operator !=(SheduleTime time1, SheduleTime time2)
        {
            return !time1.Equals(time2);
        }

        public static bool operator >(SheduleTime time1, SheduleTime time2)
        {
            if (time1.WeekNumber > time2.WeekNumber)
                return true;
            if (time1.WeekNumber == time2.WeekNumber &&
                time1.DayNumber > time2.WeekNumber)
                return true;
            if(time1.WeekNumber == time2.WeekNumber &&
                time1.DayNumber == time2.WeekNumber &&
                time1.Hour > time2.Hour)
                return true;
            return false;
        }

        public static bool operator >=(SheduleTime time1, SheduleTime time2)
        {
            return time1 > time2 || time1 == time2;
        }

        public static bool operator <(SheduleTime time1, SheduleTime time2)
        {
            return !(time1 >= time2);
        }

        public static bool operator <=(SheduleTime time1, SheduleTime time2)
        {
            return !(time1 > time2);
        }

        #endregion

        #region Description fields SheduleTime

        // Вернуть текстовое описания номера занятия
        public string HourDescription
        {
            get
            {
                return GetHourDiscription(_hour);
            }
        }

        public string WeekDescription
        {
            get
            {
                return GetWeekDescription(Week);
            }
        }

        public string DayDescription
        {
            get
            {
                return GetDayDescription(Day);
            }
        }

        public string Description
        {
            get
            {
                return GetDescription(this);
            }
        }

        #endregion

        #region static methods description SheduleTime

        public static string GetDescription(SheduleTime time)
        {
            return GetWeekDescription(time.Week) + ", " + GetDayDescription(time.Day) + ", " + GetHourDiscription(time.Hour);
        }

        public static string GetWeekDescription(Week week)
        {
            string message;
            switch (week)
            {
                case Week.FirstWeek: message = "Неделя I"; break;
                case Week.SecondWeek: message = "Неделя II"; break;
                case Week.TreeWeek: message = "Неделя III"; break;
                case Week.FourWeek: message = "Неделя IV"; break;
                default: message = "Неделя не задана"; break;
            }
            return message;
        }

        // Вернуть текстовое описания номера занятия
        public static string GetHourDiscription(int hour)
        {
            switch(hour)
            {
                case 1: return "1-2";
                case 2: return "3-4";
                case 3: return "5-6";
                case 4: return "7-8";
                case 5: return "9-10";
                case 6: return "11-12";
                case 7: return "13-14";
                case 8: return "15-16";
                default: return "Часы не заданы";
            }
        }

        public static string GetDayDescription(Day day)
        {
            string message;
            switch (day)
            {
                case Day.Monday: message = "Понедельник"; break;
                case Day.Tuesday: message = "Вторник"; break;
                case Day.Wednesday: message = "Среда"; break;
                case Day.Thursday: message = "Четверг"; break;
                case Day.Friday: message = "Пятница"; break;
                case Day.Saturday: message = "Суббота"; break;
                case Day.Sunday: message = "Воскресенье"; break;
                default: message = "День не задан"; break;
            }
            return message;
        }

        #endregion

    }
}
