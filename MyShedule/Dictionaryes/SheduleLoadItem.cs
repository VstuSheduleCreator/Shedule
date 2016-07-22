﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace MyShedule
{
    /// <summary>
    /// Кузьмин Д.С. 16/04/2011
    /// Класс нагрузка для одной группы по одной дисциплине
    /// </summary>
    [Serializable]
    public class LoadItem : ICloneable
    {
        public object Clone()
        { 
            return CloneObject(this); 
        }

        public static object CloneObject(object obj) 
        {
            using (System.IO.MemoryStream memStream = new System.IO.MemoryStream()) 
            { 
                BinaryFormatter binaryFormatter = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.Clone));
                binaryFormatter.Serialize(memStream, obj);
                memStream.Seek(0, System.IO.SeekOrigin.Begin); 
                return binaryFormatter.Deserialize(memStream); 
            } 
        }
        /// <summary>
        /// Конструктор
        /// </summary>
        public LoadItem()
        {
            Groups = new List<string>();
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="teacher">Преподаватель</param>
        /// <param name="discimpline">Дисциплина</param>
        /// <param name="groups">Группы</param>
        /// <param name="hours">Часы за семестр</param>
        /// <param name="type">Тип занятия</param>
        public LoadItem(string teacher, 
                        string discimpline, 
                        List<string> groups, 
                        decimal hours, 
                        LessonType type)
        {
            Teacher = teacher;
            Discipline = discimpline;
            Groups = groups;
            HoursSem = hours;
            LessonType = type;
        }

        /// <summary>
        /// Конструктор с идентификаторами
        /// </summary>
        public LoadItem(string teacher, 
                        string discipline, 
                        List<string> groups, 
                        decimal hours,
                        LessonType type, 
                        int teacherId, 
                        int disciplId, 
                        int groupId)
        {
            Teacher = teacher;
            Discipline = discipline;
            Groups = groups;
            HoursSem = hours;
            LessonType = type;
            TeacherId = teacherId;
            DisciplineId = disciplId;
            GroupId = groupId;
        }

        public LoadItem Copy()
        {
            return new LoadItem(Teacher, Discipline, Groups, HoursSem, LessonType);
        }

        /// <summary>
        /// Идентификатор
        /// На 07.05.11 не используется
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Тип занятия
        /// </summary>
        public LessonType LessonType { get; set; }

        /// <summary>
        /// Преподаватель дисциплины
        /// </summary>
        public string Teacher { get; set; }

        /// <summary>
        /// Дисциплина
        /// </summary>
        public string Discipline { get; set; }

        /// <summary>
        /// Группы, занятие может вестись сразу у нескольких групп
        /// </summary>
        public List<string> Groups { get; set; }

        /// <summary>
        /// Количество часов дисциплины за семестр
        /// </summary>
        public decimal HoursSem
        {
            get
            {
                return _hours;
            }
            set
            {
                _hours = (value >= 0) ? value : 0;
            }
        }

        /// <summary>
        /// Количество часов в расписании
        /// </summary>
        public int HoursByMonth
        {
            get
            {
                return ConvertHoursSemToMouth(_hours);
            }
        }

        /// <summary>
        /// Перевести часы в семестре в часы расписания 
        /// </summary>
        /// <param name="HoursSem">Часы за семестр</param>
        /// <returns>Часы в расписании</returns>
        private int ConvertHoursSemToMouth(decimal HoursSem)
        {
            SettingsAplication stg = new SettingsAplication();
            return (int)HoursSem / 4;
        }


        public decimal DivideHours
        {
            get;
            set;
        }

        /// <summary>
        /// Часы за семестр
        /// </summary>
        private decimal _hours;

        /// <summary>
        /// Индентификатор преподователя
        /// На 07.05.11 не используется
        /// </summary>
        public int TeacherId { get; set; }

        /// <summary>
        /// Идентификатор дисциплины
        /// На 07.05.11 не используется
        /// </summary>
        public int DisciplineId { get; set; }

        /// <summary>
        /// Идентификатор группы
        /// На 07.05.11 не используется
        /// </summary>
        public int GroupId { get; set; }
    }
}