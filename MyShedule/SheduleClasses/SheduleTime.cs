﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyShedule
{
    [Serializable]
    public class SheduleTime
    {
        public SheduleTime()
        {
            Week = MyShedule.Week.Another;
            Day = MyShedule.Day.Another;
            Hour = 0;
        }

        public SheduleTime(Week week, Day day, int hour)
        {
            Week = week;
            Day = day;
            Hour = hour;
        }

        public Week Week
        {
            get
            {
                return _Week;
            }
            set
            {
                _Week = value;
            }

        }

        public int WeekNumber
        {
            get
            {
                return (int)_Week;
            }
        }

        private Week _Week = new Week();

        public Day Day
        {
            get
            {
                return _Day;
            }
            set
            {
                _Day = value;
            }
        }

        public int DayNumber
        {
            get
            {
                return (int)_Day;
            }
        }

        private Day _Day = new Day();

        // Номер пары
        public int Hour
        {
            get
            {
                return _Hour;
            }
            set
            {
                // В день не может быть больше 8-ми пар
                // надо будет Exception поставить в будущем
                //SheduleSettings stg = new SheduleSettings();
                //_Hour = (value >= 1 && value <= stg.CountLessonsOfDay) ? value : 0;
                _Hour = value;
            }
        }

        private int _Hour;

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

        public override int GetHashCode()
        {
            return WeekNumber*DayNumber*Hour;
        }

        public bool Equals(SheduleTime other) 
        {
            return Week == other.Week && Day == other.Day && Hour == other.Hour;
        }

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
            else if (time1.WeekNumber < time2.WeekNumber)
                return false;
            else
            {
                if (time1.DayNumber > time2.DayNumber)
                    return true;
                else if (time1.DayNumber < time2.DayNumber)
                    return false;
                {
                    if (time1.Hour > time2.Hour)
                        return true;
                    else if (time1.Hour < time2.Hour)
                        return false;
                    else return false;
                }
            }
        }

        public static bool operator >=(SheduleTime time1, SheduleTime time2)
        {
            if (time1.WeekNumber > time2.WeekNumber)
                return true;
            else if (time1.WeekNumber < time2.WeekNumber)
                return false;
            else
            {
                if (time1.DayNumber > time2.DayNumber)
                    return true;
                else if (time1.DayNumber < time2.DayNumber)
                    return false;
                {
                    if (time1.Hour > time2.Hour)
                        return true;
                    else if (time1.Hour < time2.Hour)
                        return false;
                    else return true;
                }
            }
        }

        public static bool operator <(SheduleTime time1, SheduleTime time2)
        {
            return !(time1 >= time2);
        }

        public static bool operator <=(SheduleTime time1, SheduleTime time2)
        {
            return !(time1 > time2);
        }

        // Вернуть текстовое описания номера занятия
        public string HourDescription
        {
            get
            {
                return GetHourDiscription(_Hour);
            }
        }

        public string WeekDescription
        {
            get
            {
                return GetWeekDescription(_Week);
            }
        }

        public string DayDescription
        {
            get
            {
                return GetDayDescription(_Day);
            }
        }

        public string Description
        {
            get
            {
                return WeekDescription + ' ' + DayDescription + ' ' + HourDescription;
            }
        }

        public static string GetDescription(SheduleTime time)
        {
            return GetWeekDescription(time.Week) + ", " + GetDayDescription(time.Day) + ", " + GetHourDiscription(time.Hour);
        }

        public static string GetWeekDescription(Week week)
        {
            string message = string.Empty;
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
        public static string GetHourDiscription(int Hour)
        {
            switch (Hour)
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
            string message = string.Empty;
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

    }
}
