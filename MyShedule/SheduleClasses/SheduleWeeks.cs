﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyShedule
{
    
    /* класс расписание содержит дни и уроки, управляет днями и уроками */
    /// <summary> Класс расписание </summary>
    [Serializable]
    public class SheduleWeeks
    {
        #region Constructors, Fields and Initialize Days

        //пустой конcтруктор необходим для сериализации расписания
        public SheduleWeeks() { 
            FirstDaySem = DateTime.Now; 
        }

        public SheduleWeeks(List<SheduleRoom> rooms, SettingShedule setting, DateTime firstDaySem)
        {
            Rooms = rooms;
            Setting = setting;
            FirstDaySem = firstDaySem;
            InitializeDays();
            Employments = new Employments();
            Employments.Clear();
        }

        public List<SheduleDay> Days;

        public DateTime FirstDaySem;

        public List<SheduleRoom> Rooms;

        public SettingShedule Setting;

        public Employments Employments;

        //инициализация расписания
        private void InitializeDays()
        {
            Days = new List<SheduleDay>();

            DateTime TempDate = FirstDaySem;
            DateTime DateCounter = FirstDaySem;

            for (int week = 1; week <= (Setting.CountWeeksShedule + 2); week++)
            {
                TempDate = DateCounter;

                for (int day = 1; day <= Setting.CountDaysEducationWeek; day++) 
                {
                    Days.Add(new SheduleDay((Week)week, (Day)day, Rooms, Setting, DateCounter));
                    DateCounter = DateCounter.AddDays(1);
                }

                DateCounter = TempDate + TimeSpan.FromDays(7);
            }

            if (FirstDaySem.Month == 2)
            {
                for (int i = 0; i < Setting.CountWeeksShedule; i++)
                {
                    for (int day = i * Setting.CountDaysEducationWeek * 2; 
                        day < Setting.CountDaysEducationWeek + (i * Setting.CountDaysEducationWeek * 2); day++)
                    {
                        SheduleDay bufferDay = Days[day];
                        Week bufferWeek = Days[day + Setting.CountDaysEducationWeek].Week;
                        Days[day] = Days[day + Setting.CountDaysEducationWeek];
                        Days[day].Week = bufferDay.Week;
                        Days[day + Setting.CountDaysEducationWeek] = bufferDay;
                        Days[day + Setting.CountDaysEducationWeek].Week = bufferWeek;
                    }
                }
            }
        }

        #endregion

        /// <summary> Cписок всех занятий из расписания </summary>
        public IEnumerable<SheduleLesson> Lessons { get { return from day in Days from lesson in day.Lessons select lesson; } }

        /// <summary> Получить расписание занятия </summary>
        public SheduleLesson GetLesson(SheduleTime Time, string Room) {
            IEnumerable<SheduleLesson> query = Lessons.Where(x => x.Time == Time && x.Room == Room);
            return query.Count() > 0 ? query.First() : null;
        }

        /// <summary> Найти расписание занятия из списка занятий</summary>
        public SheduleLesson FindLessonInList(IEnumerable<SheduleLesson> items, SheduleTime time) {
            IEnumerable<SheduleLesson> query = items.Where(x => x.Time == time);
            return query.Count() > 0 ? query.First() : null;
        }

        /// <summary> Получить расписание определенного дня </summary>
        public SheduleDay GetDay(Week week, Day day) { return Days.Single(e => e.Week == week && e.Day == day); }
        public SheduleDay GetDay(SheduleTime time)   { return GetDay(time.Week, time.Day); }

        /// <summary> Получить все занятия определенного дня </summary>
        public IEnumerable<SheduleLesson> GetLessonsOfDay(Week week, Day day) { return Days.Single(e => e.Week == week && e.Day == day).Lessons; } 
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
        public IEnumerable<SheduleLesson> GetLessonsTeacher(string Teacher) { return from x in Lessons where x.Teacher == Teacher select x; }

        /// <summary> Получить список список занятий определенной группы</summary>
        public IEnumerable<SheduleLesson> GetLessonsGroup(string Group) { return from x in Lessons from g in x.Groups where g == Group select x; }

        /// <summary> Получить список список занятий определенной дисциплины</summary>
        public IEnumerable<SheduleLesson> GetLessonsDiscipline(string Discipline) { return from x in Lessons where x.Discipline == Discipline select x; }

        /// <summary> Получить список список занятий определенной аудитоии</summary>
        public IEnumerable<SheduleLesson> GetLessonsRoom(string Room) { return from x in Lessons where x.Room == Room && !x.IsEmpty select x; }

        /// <summary> Получить все занятия по определенному элементу проекции </summary>
        public IEnumerable<SheduleLesson> GetLessonsByView(View view, string name) {
            return view == View.Discipline ? GetLessonsDiscipline(name) : view == View.Group ? GetLessonsGroup(name) : 
                   view == View.Room ? GetLessonsRoom(name) : view == View.Teacher ? GetLessonsTeacher(name) : new List<SheduleLesson>();
        }

        #endregion
    }
}
