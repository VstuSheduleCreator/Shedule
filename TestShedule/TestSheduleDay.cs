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
        private SheduleDay day;

        private SheduleDay getDay(SettingShedule setting = null, Week week = Week.FirstWeek, Day dayWeek = Day.Monday)
        {
            if(setting == null)
                setting = new SettingShedule(2, 6, 6, 6, 16, 4, 2, 1, 2, 4, 3);

            IEnumerable<SheduleRoom> rooms;

            dsShedule sheduleDataSet = new dsShedule();
            string filename = @"../../fixtures/Аудитории.xml";
            sheduleDataSet.Room.ReadXml(filename);
            rooms = DictionaryConverter.RoomsToList(sheduleDataSet);

            return new SheduleDay(week, dayWeek, rooms, setting, new DateTime(2016, 9, 1));
        }

        [TestMethod]
        public void TestEmptyConstrucor()
        {
            day = new SheduleDay();

            Assert.AreEqual(Week.Another, day.Week);
            Assert.AreEqual(Day.Another, day.Day);

            Assert.AreEqual(0, day.Dates.Count);
            Assert.AreEqual(0, day.Lessons.Count);

            Assert.AreEqual(0, day.EarlierPossibleHour);
            Assert.AreEqual(0, day.LastPossibleHour);
            Assert.AreEqual(0, day.MaxPossibleCountLessons);
        }

        [TestMethod]
        public void TestConstructorForWeekDay()
        {
            day = getDay(dayWeek: Day.Monday);

            Assert.AreEqual(Week.FirstWeek, day.Week);
            Assert.AreEqual(Day.Monday, day.Day);
            Assert.AreEqual(8, day.Dates.Count);
            Assert.AreEqual(32, day.Lessons.Count);

            Assert.AreEqual(1, day.EarlierPossibleHour);
            Assert.AreEqual(4, day.LastPossibleHour);
            Assert.AreEqual(4, day.MaxPossibleCountLessons);
        }

        [TestMethod]
        public void TestConstructorForWeekEnd()
        {
            day = getDay(dayWeek: Day.Saturday);
            Assert.AreEqual(Week.FirstWeek, day.Week);
            Assert.AreEqual(Day.Saturday, day.Day);
            Assert.AreEqual(8, day.Dates.Count);
            Assert.AreEqual(8, day.Lessons.Count);

            Assert.AreEqual(2, day.EarlierPossibleHour);
            Assert.AreEqual(3, day.LastPossibleHour);
            Assert.AreEqual(2, day.MaxPossibleCountLessons);
        }

        [TestMethod]
        public void TestConstructorWitCountEducationWeekBySemProportionallyCountWeeksShedule()
        {
            SettingShedule setting = new SettingShedule(2, 6, 6, 6, 16, 4, 2, 1, 2, 4, 3);
            day = getDay(setting = setting);
            Assert.AreEqual(8, day.Dates.Count);
        }

        [TestMethod]
        public void TestConstructorWitCountEducationWeekBySemDisproportionallyCountWeeksShedule()
        {
            SettingShedule setting = new SettingShedule(2, 6, 6, 6, 17, 4, 2, 1, 2, 4, 3);
            day = getDay(setting = setting);
            Assert.AreEqual(9, day.Dates.Count);
        }

        [TestMethod]
        public void TestFirstLessonHourInEmptyDay()
        {
            day = new SheduleDay();
            Assert.AreEqual(0, day.FirstLessonHour);
        }

        [TestMethod]
        public void FirstLessonHour()
        {
            day = getDay();
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
            
            Assert.AreEqual(1, day.FirstLessonHour);
        }

        [TestMethod]
        public void TestCountLessonsInEmptyDay()
        {
            day = new SheduleDay();
            Assert.AreEqual(0, day.CountLessons);
        }

        [TestMethod]
        public void TestCountLessons()
        {
            day = getDay();
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

            Assert.AreEqual(2, day.CountLessons);
        }

        [TestMethod]
        public void NonEmptyLessons()
        {
            day = getDay();
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

            Assert.AreEqual(2, day.NonEmptyLessons.Count());
        }

        [TestMethod]
        public void TestCountLessonsGroupInEmptyDay() {
            day = new SheduleDay();
            Assert.AreEqual(0, day.CountLessonsGroup("ИВТ-260", Week.FirstWeek, Day.Monday));
        }

        [TestMethod]
        public void TestCountLessonsGroup() {
            day = getDay();
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

            Assert.AreEqual(2, day.CountLessonsGroup("ИВТ-260", Week.FirstWeek, Day.Monday));
            Assert.AreEqual(0, day.CountLessonsGroup("ИВТ-262", Week.FirstWeek, Day.Monday));
        }

        [TestMethod]
        public void TestLimitLessonsNotExceededInEmptyDay() {
            day = new SheduleDay();
            Assert.IsFalse(day.LimitLessonsNotExceeded(new List<string> { "ИВТ-260" }, Week.FirstWeek, Day.Monday));
        }

        [TestMethod]
        public void TestLimitLessonsNotExceeded()
        {
            day = getDay();
            SheduleTime time = new SheduleTime(Week.FirstWeek, Day.Monday, 1);
            List<DateTime> dates = new List<DateTime> { new DateTime(2016, 9, 6), new DateTime(2016, 9, 20) };
            List<string> groups = new List<string> { "ИВТ-260", "ИВТ-261" };

            List<SheduleLesson> lessons = new List<SheduleLesson>()
            {
               new SheduleLesson(time, "В-404", dates, "Андреев А.Е.", "Основы ЭВМ", groups, LessonType.Lection),
               new SheduleLesson()
            };
            day.Lessons = lessons;

            Assert.IsTrue(day.LimitLessonsNotExceeded(new List<string> { "ИВТ-260" }, Week.FirstWeek, Day.Monday));
        }


        [TestMethod]
        public void TestLimitLessons()
        {
            day = getDay();
            SheduleTime time = new SheduleTime(Week.FirstWeek, Day.Monday, 1);
            List<DateTime> dates = new List<DateTime> { new DateTime(2016, 9, 6), new DateTime(2016, 9, 20) };
            List<string> groups = new List<string> { "ИВТ-260", "ИВТ-261" };

            List<SheduleLesson> lessons = new List<SheduleLesson>()
            {
               new SheduleLesson(time, "В-404", dates, "Андреев А.Е.", "Основы ЭВМ", groups, LessonType.Lection),
               new SheduleLesson()
            };
            day.Lessons = lessons;
            Assert.IsTrue(day.LimitLessonsNotExceeded(new List<string> { "ИВТ-260" }, Week.FirstWeek, Day.Monday, 1));
            Assert.IsFalse(day.LimitLessonsNotExceeded(new List<string> { "ИВТ-260" }, Week.FirstWeek, Day.Monday, 28));

        }

        [TestMethod]
        public void TestDatesDescriptionInEmptyDay()
        {
            day = new SheduleDay();
            Assert.AreEqual("", day.DatesDescription);
        }

        [TestMethod]
        public void TestDatesDescription()
        {
            day = getDay();
            Assert.AreEqual("01.09, 15.09, 29.09, 13.10, 27.10, 10.11, 24.11, 08.12", day.DatesDescription);
        }

    }
}
