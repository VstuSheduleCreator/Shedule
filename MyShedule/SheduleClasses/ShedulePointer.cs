﻿using System;

namespace MyShedule
{
    /// <summary> значение ячейки в таблице, указывает на адрес занятия в расписании </summary>
    public class ShedulePointer
    {
        #region Constructors

        public ShedulePointer(SheduleTime time1, SheduleTime time2, string room1, string room2)
        {
            Time1 = time1;
            Time2 = time2;
            Room1 = room1;
            Room2 = room2;
        }

        #endregion

        /// <summary> копировать указатель на ячейку </summary>
        public ShedulePointer Copy() { return new ShedulePointer(Time1, Time2, Room1, Room2); }

        /// <summary> время занятия на 1-2 недели </summary>
        public SheduleTime Time1 { get; set; }

        /// <summary> время занятия на 3-4 недели </summary>
        public SheduleTime Time2 { get; set; }

        /// <summary> аудитория в которой проходит занятие на 1-2 недели </summary>
        public string Room1 { get; set; }

        /// <summary> аудитория в которой проходит занятие на 3-4 недели </summary>
        public string Room2 { get; set; }
    }
}