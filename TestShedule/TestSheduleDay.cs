using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyShedule;

namespace TestShedule
{
    [TestClass]
    public class TestSheduleDay
    {
        private SheduleDay day = new SheduleDay();
        private SheduleDay weekday;

        // TODO: написать тесты для SheduleRoom
        private SheduleDay getWeekDay()
        {
            SettingShedule setting = new SettingShedule(2, 6, 6, 6, 4, 4, 2, 1, 1, 4, 2);
            IEnumerable<SheduleRoom> rooms;

            dsShedule sheduleDataSet = new dsShedule();
            string filename = @"../../fixtures/Аудитории.xml";
            sheduleDataSet.Room.ReadXml(filename);
            rooms = DictionaryConverter.RoomsToList(sheduleDataSet);

            return new SheduleDay(Week.FirstWeek, Day.Monday, rooms, setting, new DateTime(2016, 9, 1));
        }

        [TestMethod]
        public void TestUpdateSettingOfWeekDay()
        {
            day = getWeekDay();
            SettingShedule setting = new SettingShedule(2, 6, 6, 6, 4, 6, 4, 1, 1, 6, 4);
            day.UpdateSetting(setting);

            Assert.AreEqual(day.MaxPossibleCountLessons, 6);
            Assert.AreEqual(day.LastPossibleHour, 6);
            Assert.AreEqual(day.EarlierPossibleHour, 1);
        }

        [TestMethod]
        public void TestFirstLessonHourInEmptyDay()
        {
            Assert.AreEqual(day.FirstLessonHour, 0);
        }

        [TestMethod]
        public void FirstLessonHour()
        {
            day = getWeekDay();
            SheduleTime time = new SheduleTime(Week.FirstWeek, Day.Monday, 1);
            List<DateTime> dates = new List<DateTime> { new DateTime(2016, 9, 6), new DateTime(2016, 9, 20) };
            List<string> groups = new List<string> { "ИВТ-260", "ИВТ-261" };

            List<SheduleLesson> lessons = new List<SheduleLesson>()
            {
               new SheduleLesson(time, "В-404", dates, "Андреев А.Е.", "Основы ЭВМ", groups, LessonType.Lection),
               new SheduleLesson(time, "В-403", dates, "Андреев А.Е.", "Основы ЭВМ", groups, LessonType.Lection),
               new SheduleLesson()
            };

            day.Lessons = lessons;
            
            Assert.AreEqual(day.FirstLessonHour, 1);
        }

        [TestMethod]
        public void TestCountLessonsInEmptyDay()
        {
            Assert.AreEqual(day.CountLessons, 0);
        }

        [TestMethod]
        public void TestCountLessons()
        {
            day = getWeekDay();
            SheduleTime time = new SheduleTime(Week.FirstWeek, Day.Monday, 1);
            List<DateTime> dates = new List<DateTime> { new DateTime(2016, 9, 6), new DateTime(2016, 9, 20) };
            List<string> groups = new List<string> { "ИВТ-260", "ИВТ-261" };

            List<SheduleLesson> lessons = new List<SheduleLesson>()
            {
               new SheduleLesson(time, "В-404", dates, "Андреев А.Е.", "Основы ЭВМ", groups, LessonType.Lection),
               new SheduleLesson(time, "В-403", dates, "Андреев А.Е.", "Основы ЭВМ", groups, LessonType.Lection),
               new SheduleLesson()
            };

            day.Lessons = lessons;

            Assert.AreEqual(day.CountLessons, 2);
        }

        [TestMethod]
        public void NonEmptyLessons()
        {
            day = getWeekDay();
            SheduleTime time = new SheduleTime(Week.FirstWeek, Day.Monday, 1);
            List<DateTime> dates = new List<DateTime> { new DateTime(2016, 9, 6), new DateTime(2016, 9, 20) };
            List<string> groups = new List<string> { "ИВТ-260", "ИВТ-261" };

            List<SheduleLesson> lessons = new List<SheduleLesson>()
            {
               new SheduleLesson(time, "В-404", dates, "Андреев А.Е.", "Основы ЭВМ", groups, LessonType.Lection),
               new SheduleLesson(time, "В-403", dates, "Андреев А.Е.", "Основы ЭВМ", groups, LessonType.Lection),
               new SheduleLesson()
            };

            day.Lessons = lessons;

            Assert.AreEqual(day.NonEmptyLessons.Count(), 2);
        }

        [TestMethod]
        public void TestCountLessonsGroupInEmptyDay() {
            Assert.AreEqual(day.CountLessonsGroup("ИВТ-260", Week.FirstWeek, Day.Monday), 0);
        }

        [TestMethod]
        public void TestCountLessonsGroup() {
            day = getWeekDay();
            SheduleTime time = new SheduleTime(Week.FirstWeek, Day.Monday, 1);
            List<DateTime> dates = new List<DateTime> { new DateTime(2016, 9, 6), new DateTime(2016, 9, 20) };
            List<string> groups = new List<string> { "ИВТ-260", "ИВТ-261" };

            List<SheduleLesson> lessons = new List<SheduleLesson>()
            {
               new SheduleLesson(time, "В-404", dates, "Андреев А.Е.", "Основы ЭВМ", groups, LessonType.Lection),
               new SheduleLesson(time, "В-403", dates, "Андреев А.Е.", "Основы ЭВМ", groups, LessonType.Lection),
               new SheduleLesson()
            };
            day.Lessons = lessons;

            Assert.AreEqual(day.CountLessonsGroup("ИВТ-260", Week.FirstWeek, Day.Monday), 2);
            Assert.AreEqual(day.CountLessonsGroup("ИВТ-262", Week.FirstWeek, Day.Monday), 0);
        }

        [TestMethod]
        public void TestLimitLessonsNotExceededInEmptyDay() {
            Assert.AreEqual(day.LimitLessonsNotExceeded(new List<string> { "ИВТ-260" }, Week.FirstWeek, Day.Monday), false);
        }

        [TestMethod]
        public void TestLimitLessonsNotExceeded()
        {
            day = getWeekDay();
            SheduleTime time = new SheduleTime(Week.FirstWeek, Day.Monday, 1);
            List<DateTime> dates = new List<DateTime> { new DateTime(2016, 9, 6), new DateTime(2016, 9, 20) };
            List<string> groups = new List<string> { "ИВТ-260", "ИВТ-261" };

            List<SheduleLesson> lessons = new List<SheduleLesson>()
            {
               new SheduleLesson(time, "В-404", dates, "Андреев А.Е.", "Основы ЭВМ", groups, LessonType.Lection),
               new SheduleLesson()
            };
            day.Lessons = lessons;

            Assert.AreEqual(day.LimitLessonsNotExceeded(new List<string> { "ИВТ-260" }, Week.FirstWeek, Day.Monday), true);
        }


        [TestMethod]
        public void TestLimitLessons()
        {
            day = getWeekDay();
            SheduleTime time = new SheduleTime(Week.FirstWeek, Day.Monday, 1);
            List<DateTime> dates = new List<DateTime> { new DateTime(2016, 9, 6), new DateTime(2016, 9, 20) };
            List<string> groups = new List<string> { "ИВТ-260", "ИВТ-261" };

            List<SheduleLesson> lessons = new List<SheduleLesson>()
            {
               new SheduleLesson(time, "В-404", dates, "Андреев А.Е.", "Основы ЭВМ", groups, LessonType.Lection),
               new SheduleLesson()
            };
            day.Lessons = lessons;
            Assert.AreEqual(day.LimitLessonsNotExceeded(new List<string> { "ИВТ-260" }, Week.FirstWeek, Day.Monday, 1), true);
            Assert.AreEqual(day.LimitLessonsNotExceeded(new List<string> { "ИВТ-260" }, Week.FirstWeek, Day.Monday, 28), false);

        }

        [TestMethod]
        public void TestDatesDescriptionInEmptyDay()
        {
            Assert.AreEqual(day.DatesDescription, "");
        }

        [TestMethod]
        public void TestDatesDescription()
        {
            day = getWeekDay();
            Assert.AreEqual(day.DatesDescription, "01.09, 15.09, 29.09");
        }

    }
}
