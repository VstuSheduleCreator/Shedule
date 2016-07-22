﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyShedule
{
    /// <summary>
    /// Как отображать расписание, представляет собой один из вариантов проекции расписания
    /// Класс используется в привязке к выпадающему списку
    /// </summary>
    public class SheduleView
    {
        public SheduleView(View type)
        {
            Type = type;
        }

        /// <summary>
        /// По какому критерию отображать расписание
        /// </summary>
        public View Type
        {
            get;
            set;
        }
        /// <summary>
        /// Числовое значение перечисления, используется в привязке к выпадающему списку
        /// </summary>
        public int TypeCode
        {
            get
            {
                return (int)Type;
            }
        }

        /// <summary>
        /// Описание проекции
        /// </summary>
        public string Description
        {
            get
            {
                return GetDescription(Type);
            }
        }

        public static string GetDescription(View type)
        {
            switch (type)
            {
                case View.Teacher: return "Расписание преподавателей";
                case View.Discipline: return "Расписание дисциплин";
                case View.Group: return "Расписание групп";
                case View.Room: return "Расписание аудиторий";
                default: return String.Empty;
            }
        }

        public string Name
        {
            get
            {
                return GetName(Type);
            }
        }

        public static string GetName(View type)
        {
            switch (type)
            {
                case View.Teacher: return "преподаватель";
                case View.Discipline: return "дисциплина";
                case View.Group: return "группа";
                case View.Room: return "аудитория";
                default: return String.Empty;
            }
        }

        public static List<SheduleView> BasicViews
        {
            get
            {
                List<SheduleView> views = new List<SheduleView>();
                views.Add(new SheduleView(View.Group));
                views.Add(new SheduleView(View.Teacher));
                views.Add(new SheduleView(View.Room));
                views.Add(new SheduleView(View.Discipline));
                return views;
            }
        }
    }

    /// <summary>
    /// По какому критерию отображать расписание
    /// </summary>
    public enum View
    {
        // Отобразить расписание по преподам 
        Teacher = 1,
        // Отобразить расписание по группам
        Group,
        // Отобразить расписание по предметам 
        Discipline,
        // Отобразить расписание по аудиториям
        Room
    }
}