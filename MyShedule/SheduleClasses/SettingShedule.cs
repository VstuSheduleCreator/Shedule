using System;

namespace MyShedule
{
    public class SettingShedule
    {
        public SettingShedule()
        {
        }

        public SettingShedule(int countWeeksShedule, int countDayEducationalWeek, int countDaysShedule, int countLessonsOfDay,
            int countEducationalWeekBySem, int maxCountLessonsOfWeekDay, int maxCountLessonsOfWeekEnd, int firstLessonsOfWeekDay,
            int firstLessonsOfWeekEnd, int lastLessonsOfWeekDay, int lastLessonsOfWeekEnd)
        {
            CountWeeksShedule = countWeeksShedule;
            CountDaysEducationWeek = countDayEducationalWeek;
            CountDaysShedule = countDaysShedule;
            CountLessonsOfDay = countLessonsOfDay;
            CountEducationalWeekBySem = countEducationalWeekBySem;
            MaxCountLessonsOfWeekDay = maxCountLessonsOfWeekDay;
            MaxCountLessonsOfWeekEnd = maxCountLessonsOfWeekEnd;
            FirstLessonsOfWeekDay = firstLessonsOfWeekDay;
            FirstLessonsOfWeekEnd = firstLessonsOfWeekEnd;
            LastLessonsOfWeekDay = lastLessonsOfWeekDay;
            LastLessonsOfWeekEnd = lastLessonsOfWeekEnd;
        }

        private int _countWeeksShedule;
        private int _countDaysEducationWeek;
        //int countDaysShedule;
        private int _countLessonsOfDay;
        private int _countEducationalWeekBySem;
        //-------------------------------//
        private int _maxCountLessonsOfWeekDay;
        private int _maxCountLessonsOfWeekEnd;
        //int firstLessonsOfWeekDay;
        //int firstLessonsOfWeekEnd;
        //int lastLessonsOfWeekDay;
        //int lastLessonsOfWeekEnd;

        //количество недель в расписании, по умолчанию расписание двух-недельное
        //максимальное значение 4, если необходимо больше следует добавить поле в перечисление Week
        public int CountWeeksShedule
        {
            get
            {
                return _countWeeksShedule;
            }
            set
            {
                if(0 < value && value <= 4)
                {
                    _countWeeksShedule = value;
                }
                else 
                {
                    throw new ArgumentOutOfRangeException("_countWeeksShedule", "Количество недель может быть от 1 до 4");
                }
            }
        }
        //количество дней в учебной неделе, по умолчанию расписание семи-дневное
        //максимальное значение 7, если необходимо больше следует добавить поле в перечисление Day.. хотя вряд ли есть недели где больше 7 дней :)
        public int CountDaysEducationWeek
        {
            get
            {
                return _countDaysEducationWeek;
            }
            set
            {
                if(0 < value && value <= 7)
                {
                    _countDaysEducationWeek = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("_countDaysEducationWeek", "Количество дней в недели должно быть от 1 до 7");
                }
            }
        }
        //количество учебных дней в расписании
        public int CountDaysShedule
        {
            get
            {
                return _countWeeksShedule * _countDaysEducationWeek;
            }
            set {
            }
        }
        //в день не больше восьми пар
        //максимальное значение не ограничено 
        public int CountLessonsOfDay
        {
            get
            {
                return _countLessonsOfDay;
            }
            set
            {
                if(0 <= value && value <= 8)
                {
                    _countLessonsOfDay = value;
                } 
                else
                {
                    throw new ArgumentOutOfRangeException("_countLessonsOfDay", "Количество пар в день должно быть от 1 до 8");
                }
            }
        }
        //количество учебных недель в семестре
        //максимальное значение не ограничено
        public int CountEducationalWeekBySem
        {
            get
            {
                return _countEducationalWeekBySem;
            }
            set
            {
                if(0 < value && value <= 52)
                {
                    _countEducationalWeekBySem = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("_countEducationalWeekBySem", "Количество учебных недель в семестре должно быть от 1 до 52");
                }
            }
        }

        //----------------------------------------------------------
        //количество пар в будни
        public int MaxCountLessonsOfWeekDay
        {
            get
            {
                return _maxCountLessonsOfWeekDay;
            }
            set
            {
                if(0 <= value && value <= _countLessonsOfDay)
                {
                    _maxCountLessonsOfWeekDay = value;
                }
                else 
                {
                    throw new ArgumentOutOfRangeException("_maxCountLessonsOfWeekDay", "максимальное количество пар в будни должно быть от 1 до _countLessonsOfDay");
                }
            }
        }

        //количество пар в выходные
        public int MaxCountLessonsOfWeekEnd
        {
            get
            {
                return _maxCountLessonsOfWeekEnd;
            }
            set
            {
                if(0 <= value && value <= _countLessonsOfDay)
                {
                    _maxCountLessonsOfWeekEnd = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("_maxCountLessonsOfWeekEnd", "максимальное количество пар в выходные должно быть от 1 до _countLessonsOfDay");
                }
            }
        }
        // Todo: добавить гетеры и сетеры для следующих полей
        //первая пара с которой может начинаться учебный день в будни
        public int FirstLessonsOfWeekDay;
        //первая пара с которой может начинаться учебный день в выходной
        public int FirstLessonsOfWeekEnd;
        //последня пара которой завершается учебный день в будни
        public int LastLessonsOfWeekDay;
        //последня пара которой завершается учебный день в выходной
        public int LastLessonsOfWeekEnd;
    }
}