﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace MyShedule
{
    /// <summary>Класс расписание дня. </summary>
    /* Cодержит коллекцию уроков. Определяет возможность назначения уроков определенной группе 
     * или нескольким группам, определяет с каких пар начинаются и заканчиваются уроки в этот день */
    [Serializable]
    public class SheduleDay
    {
        #region Constructors SheduleDay

        //пустой конcтруктор необходим для сериализации расписания
        public SheduleDay()
        {
            //times
            Week = Week.Another;
            Day = Day.Another; 
            //main
            Dates = new List<DateTime>();
            Lessons = new List<SheduleLesson>();
            //settings
            _earlierPossibleHour = 0;
            _lastPossibleHour = 0;
            _maxPossibleCountLessons = 0;
        }

        public SheduleDay(Week week, Day day, IEnumerable<SheduleRoom> rooms, SettingShedule setting, DateTime firstDate)
        {
            //times
            Week = week;
            Day = day;
            //settings
            InitializeSettingOfDay(setting, day);
            //main
            InitializeDatesOfDay(firstDate, setting.CountWeeksShedule, setting.CountEducationalWeekBySem);
            Lessons = InitializeLessonsOfDay(rooms.ToList(), setting.CountLessonsOfDay).ToList();
        }

        #endregion

        #region Fields SheduleDay

        /// <summary> перечисление, номер недели в расписании </summary>
        public Week Week;

        /// <summary> перечисление, номер дня в расписании </summary>
        public Day Day;

        /// <summary> список занятий в определенный день, двумерная сетка занятий, на одной оси сетки
        /// находятся аудитории всего N,  на другой номера пар всего M, т.е. M на N занятий </summary>
        public List<SheduleLesson> Lessons;

        /// <summary> календарные даты дня расписания </summary>
        public List<DateTime> Dates;

        /* настройки
         * earlierPossibleHour - пары в этот день начиниются не раньше это пары
         * lastPossibleHour - пары заканчиваются в этот день не позже этой пары
         * maxPossibleCountLessons - пар в этот день не может быть больше этого количества
         */
        private int _earlierPossibleHour;
        private int _lastPossibleHour;
        private int _maxPossibleCountLessons;

        private static readonly int countDaysByWeek = 7;

        #endregion

        //создать занятия для определенного дня, метод вызывается в конструкторе
        private IEnumerable<SheduleLesson> InitializeLessonsOfDay(IEnumerable<SheduleRoom> rooms, int countHoursCreating)
        {
            //у каждого дня в расписании имеется двумерная сетка занятий, на одной оси сетки
            //находятся аудитории всего N,  на другой номера пар всего M, т.е. M на N занятий
            foreach (SheduleRoom room in rooms)
                for (int counterHour = 1; counterHour <= countHoursCreating; counterHour++)
                    yield return new SheduleLesson(new SheduleTime(Week, Day, counterHour), room.Name, GetDatesLesson());
        }
    
        //инициализировать настройки
        private void InitializeSettingOfDay(SettingShedule setting, Day day)
        {
            //с понедельника по пятницу и в выходные часы для проставления пар отличаются
            _earlierPossibleHour = (int) day <= (int) Day.Friday ? setting.FirstLessonsOfWeekDay : setting.FirstLessonsOfWeekEnd;
            _lastPossibleHour = (int) day <= (int) Day.Friday ? setting.LastLessonsOfWeekDay : setting.LastLessonsOfWeekEnd;
            _maxPossibleCountLessons = (int) day <= (int) Day.Friday ? setting.MaxCountLessonsOfWeekDay : setting.MaxCountLessonsOfWeekEnd;
        }

        /// <summary> обновить настройки у дня </summary>
        public void UpdateSetting(SettingShedule setting){
            InitializeSettingOfDay(setting, Day);
        }

        //расставить календарные даты дня
        private void InitializeDatesOfDay(DateTime firstDate, int countWeekShedule, int countWeeksSem)
        {
            //инициализ. список календарных дат, добавляем первую дату, остальные вычисляем на основе первой даты
            Dates = new List<DateTime>() { firstDate };
            for (int date = 1; date <= countWeeksSem / countWeekShedule; date++)
                Dates.Add(Dates.Last() + TimeSpan.FromDays(countDaysByWeek * countWeekShedule));
        }

        //получить даты занятия в этот день
        private IEnumerable<DateTime> GetDatesLesson() {
            for (int i = 0; i < Dates.Count; i += 2)
              yield return Dates[i];
        }

        /// <summary> Первая проставленная пара в дне </summary>
        public int FirstLessonHour { get { return CountLessons > 0 ? NonEmptyLessons.First().Hour : 0; } }

        /// <summary> Количество проставленных занятий в дне </summary>
        public int CountLessons { get { return NonEmptyLessons.Count(); } }

        /// <summary> Получить все непустые занятия </summary>
        public IEnumerable<SheduleLesson> NonEmptyLessons { get { return Lessons.Where(x => !x.IsEmpty); } }


        /// <summary> получить количество занятий у определенной группы в этот день </summary>
        public int CountLessonsGroup(string nameGroup, Week week, Day day) {
            return (from x in Lessons from grp in x.Groups where !x.IsEmpty && grp == nameGroup && x.Week == week && x.Day == day select x).Count();
        }

        /// <summary> проверить не будет переполнен день если добавить одно занятие </summary>
        public bool LimitLessonsNotExceeded(IEnumerable<string> groups, Week week, Day day){
            return LimitLessonsNotExceeded(groups, week, day, 1);
        }

        /// <summary> проверить не будет переполнен день если добавить количество занятий равное CountPut </summary>
        public bool LimitLessonsNotExceeded(IEnumerable<string> groups, Week week, Day day, int countPut)
        {
            foreach (string group in groups)
                if (CountLessonsGroup(group, week, day) + countPut > MaxPossibleCountLessons)
                    return false;

            return true;
        }

        /// <summary> пары в этот день начиниются не раньше этой пары </summary>
        public int EarlierPossibleHour { get { return _earlierPossibleHour; } }

        /// <summary> пары заканчиваются в этот день не позже этой пары </summary>
        public int LastPossibleHour { get { return _lastPossibleHour; } }

        /// <summary> количетсво пар в этот день не может превышать этого количества </summary>
        public int MaxPossibleCountLessons { get { return _maxPossibleCountLessons; } }

        /// <summary> строка со списком календарных дат дня в формате {0:dd}.{0:MM} </summary>
        public string DatesDescription
        {
            get {
                string datesLine = String.Empty;
                foreach (DateTime dt in Dates)
                    datesLine += String.Format("{0:dd}.{0:MM}., ", dt);

                return datesLine;
            }
        }
    }
}
