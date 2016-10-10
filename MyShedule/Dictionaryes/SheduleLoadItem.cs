using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace MyShedule
{
    /// <summary>
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

        public LoadItem Copy()
        {
            return new LoadItem(Teacher, Discipline, Groups, HoursSem, LessonType);
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
        /// Часы за семестр
        /// </summary>
        private decimal _hours;

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

        public decimal DivideHours
        {
            get;
            set;
        }

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
                if(value > 0)
                    _hours = value;
                else
                    throw new ArgumentOutOfRangeException("_hours", "Количество часов должно быть больше нуля");
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

        public bool NonEmpty()
        {
            return Teacher != "" && Discipline != "" && Groups.Count != 0;
        }

        /// <summary>
        /// Перевести часы в семестре в часы расписания 
        /// </summary>
        /// <param name="HoursSem">Часы за семестр</param>
        /// <returns>Часы в расписании</returns>
        private int ConvertHoursSemToMouth(decimal HoursSem)
        {
            SettingsAplication stg = new SettingsAplication();
            return (int)Math.Ceiling(HoursSem / 4);
        }

    }
}