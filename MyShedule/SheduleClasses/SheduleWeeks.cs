using System;
using System.Collections.Generic;
using System.Linq;

namespace MyShedule
{
    
    /* класс расписание содержит дни и уроки, управляет днями и уроками */
    /// <summary> Класс расписание </summary>
    [Serializable]
    public class SheduleWeeks
    {
        #region Constructors

        //пустой конcтруктор необходим для сериализации расписания
        public SheduleWeeks()
        {
            _days = new List<SheduleDay>();
            _firstDaySem = new DateTime();
            _rooms = new List<SheduleRoom>();
            _setting = new SettingShedule();
            Employments = new Employments();
        }

        public SheduleWeeks(List<SheduleRoom> rooms, SettingShedule setting, DateTime firstDaySem)
        {
            _rooms = rooms;
            _setting = setting;
            _firstDaySem = firstDaySem;
            _employments = new Employments();
            _employments.Clear();
            InitializeDays();
        }

        #endregion

        #region Fields

        private List<SheduleDay> _days;
        private DateTime _firstDaySem;
        private List<SheduleRoom> _rooms;
        private SettingShedule _setting;
        private Employments _employments;

        public List<SheduleDay> Days
        {
            get { return _days; }
        }

        public DateTime FirstDaySem
        {
            get { return _firstDaySem; }
        }

        public List<SheduleRoom> Rooms
        {
            get { return _rooms; }
        }

        public SettingShedule Setting
        {
            get { return _setting; }
        }

        public Employments Employments
        {
            get { return _employments; }
            set { _employments = value; }
        }

        #endregion

        #region Initialize Days

        //инициализация расписания
        private void InitializeDays()
        {
            _days = new List<SheduleDay>();

            // Понедельник первой недели семестра
            DateTime dateCounter = FirstDaySem.AddDays(-(int)FirstDaySem.DayOfWeek+1);

            for (int week = 1; week <= Setting.CountWeeksShedule; week++)
            {
                for (int day = 1; day <= Setting.CountDaysEducationWeek; day++) 
                {
                    _days.Add(new SheduleDay((Week)week, (Day)day, Rooms, Setting, dateCounter));
                    dateCounter = dateCounter.AddDays(1);
                }

                // Прибавляет незаполненные дни, чтобы счетчик начался со следующей недели
                dateCounter += TimeSpan.FromDays(7 - Setting.CountDaysEducationWeek);
            }

            //if (FirstDaySem.Month == 2)
            //{
            //    for (int i = 0; i < Setting.CountWeeksShedule; i++)
            //    {
            //        for (int day = i * Setting.CountDaysEducationWeek * 2; 
            //            day < Setting.CountDaysEducationWeek + (i * Setting.CountDaysEducationWeek * 2); day++)
            //        {
            //            SheduleDay bufferDay = Days[day];
            //            Week bufferWeek = Days[day + Setting.CountDaysEducationWeek].Week;
            //            Days[day] = Days[day + Setting.CountDaysEducationWeek];
            //            Days[day].Week = bufferDay.Week;
            //            Days[day + Setting.CountDaysEducationWeek] = bufferDay;
            //            Days[day + Setting.CountDaysEducationWeek].Week = bufferWeek;
            //        }
            //    }
            //}
        }

        #endregion

        /// <summary> Cписок всех занятий из расписания </summary>
        public IEnumerable<SheduleLesson> Lessons { get { return from day in Days from lesson in day.Lessons select lesson; } }

        /// <summary> Получить расписание занятия </summary>
        public SheduleLesson GetLesson(SheduleTime time, string room) {
            IEnumerable<SheduleLesson> query = Lessons.Where(x => x.Time == time && x.Room == room);
            return query.Count() > 0 ? query.First() : null;
        }

        // Todo: этот метод скорее должен быть статическим, а может его вообще быть не должно..
        /// <summary> Найти расписание занятия из списка занятий</summary>
        public SheduleLesson FindLessonInList(IEnumerable<SheduleLesson> items, SheduleTime time) {
            IEnumerable<SheduleLesson> query = items.Where(x => x.Time == time);
            return query.Count() > 0 ? query.First() : null;
        }

        /// <summary> Получить расписание определенного дня </summary>
        public SheduleDay GetDay(Week week, Day day)
        {
            IEnumerable<SheduleDay> query = Days.Where(x => x.Week == week && x.Day == day);
            return query.Count() > 0 ? query.First() : null;
        }
        public SheduleDay GetDay(SheduleTime time)   { return GetDay(time.Week, time.Day); }

        /// <summary> Получить все занятия определенного дня </summary>
        public IEnumerable<SheduleLesson> GetLessonsOfDay(Week week, Day day) 
        {
            IEnumerable<SheduleDay> query = Days.Where(x => x.Week == week && x.Day == day);
            return query.Count() > 0 ? query.First().Lessons : null;
        }

        public IEnumerable<SheduleLesson> GetLessonsOfDay(SheduleDay day)     { return GetLessonsOfDay(day.Week, day.Day); }
        

        #region GET NAMES ELEMENTS BY VIEW

        /// <summary> Cписок ФИО преподавателей из расписания </summary>
        public IEnumerable<string> TeachersNames    { get { return (from x in Lessons where !x.IsEmpty select x.Teacher).Distinct(); } }

        /// <summary> Cписок названий групп из расписания </summary>
        public IEnumerable<string> GroupsNames      { get { return (from x in Lessons where !x.IsEmpty from g in x.Groups select g).Distinct(); } }

        /// <summary> Cписок названий дисциплин из расписания </summary>
        public IEnumerable<string> DisciplinesNames { get { return (from x in Lessons where !x.IsEmpty select x.Discipline).Distinct(); } }

        /// <summary> Cписок названий аудиторий из расписания </summary>
        public IEnumerable<string> RoomsNames       { get { return (from x in Lessons where !x.IsEmpty select x.Room).Distinct(); } }

        #endregion

        #region GET LESSONS BY VIEW

        /// <summary> Получить список список занятий определенного преподавателя </summary>
        public IEnumerable<SheduleLesson> GetLessonsTeacher(string teacher) { return from x in Lessons where x.Teacher == teacher select x; }

        /// <summary> Получить список список занятий определенной группы</summary>
        public IEnumerable<SheduleLesson> GetLessonsGroup(string _group) { return from x in Lessons from g in x.Groups where g == _group select x; }

        /// <summary> Получить список список занятий определенной дисциплины</summary>
        public IEnumerable<SheduleLesson> GetLessonsDiscipline(string discipline) { return from x in Lessons where x.Discipline == discipline select x; }

        /// <summary> Получить список список занятий определенной аудитоии</summary>
        public IEnumerable<SheduleLesson> GetLessonsRoom(string room) { return from x in Lessons where x.Room == room && !x.IsEmpty select x; }

        /// <summary> Получить все занятия по определенному элементу проекции </summary>
        public IEnumerable<SheduleLesson> GetLessonsByView(View view, string name) {
            return view == View.Discipline ? GetLessonsDiscipline(name) : view == View.Group ? GetLessonsGroup(name) : 
                   view == View.Room ? GetLessonsRoom(name) : view == View.Teacher ? GetLessonsTeacher(name) : new List<SheduleLesson>();
        }

        #endregion
    }
}
