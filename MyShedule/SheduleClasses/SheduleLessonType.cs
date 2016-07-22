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
    public enum LessonType
    {
        Lection,
        Labwork,
        Practice
    }

    public class SheduleLessonType
    {
        public SheduleLessonType(LessonType type)
        {
            Type = type;
        }

        /// <summary>
        /// По какому критерию отображать расписание
        /// </summary>
        public LessonType Type
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
        public string Detail
        {
            get
            {
                return Description(Type);
            }
        }

        public static string Description(LessonType type)
        {
            switch (type)
            {
                case LessonType.Lection: return "Лекция";
                case LessonType.Labwork: return "Лабораторная";
                case LessonType.Practice: return "Практика";
                default :  return String.Empty;
            }
        }

        public static List<SheduleLessonType> GetBaseType()
        {
            List<SheduleLessonType> LessonTypes = new List<SheduleLessonType>();
            LessonTypes.Add(new SheduleLessonType(LessonType.Lection));
            LessonTypes.Add(new SheduleLessonType(LessonType.Labwork));
            LessonTypes.Add(new SheduleLessonType(LessonType.Practice));
            return LessonTypes;
        }
    }

}