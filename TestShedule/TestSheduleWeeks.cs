using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyShedule;

namespace TestShedule {
    [TestClass]
    public class TestSheduleWeeks {

        private SheduleWeeks getSheduleWeeks()
        {
            List<SheduleRoom> rooms;
            dsShedule sheduleDataSet = new dsShedule();
            string filename = @"../../fixtures/Аудитории.xml";
            sheduleDataSet.Room.ReadXml(filename);
            rooms = DictionaryConverter.RoomsToList(sheduleDataSet);

            SettingShedule setting = new SettingShedule(2, 6, 2, 4, 14, 4, 4, 1, 1, 6, 6);

            SheduleWeeks shedule = new SheduleWeeks(rooms, setting, new DateTime(2016, 9, 1));
       
            return shedule;
        }

        private SheduleWeeks shedule;

        [TestMethod]
        public void TestGetNonExistentLesson() {
            SheduleTime time = new SheduleTime();

            shedule = getSheduleWeeks();
            Assert.IsNull(shedule.GetLesson(time, "228"));
        }

        [TestMethod]
        public void TestGetLesson()
        {
            SheduleTime time = new SheduleTime(Week.FirstWeek, Day.Monday, 1);
            shedule = getSheduleWeeks();

            SheduleLesson lesson = shedule.GetLesson(time, "В-1301");
            Assert.AreEqual(lesson.Day, Day.Monday);
            Assert.AreEqual(lesson.Week, Week.FirstWeek);
            Assert.AreEqual(lesson.Room, "В-1301");
        }

        [TestMethod]
        public void TestFindLessonInList()
        {
            List<DateTime> dates = new List<DateTime> { new DateTime(2016, 9, 1), new DateTime(2016, 9, 15)};

            List<SheduleLesson> lessons = new List<SheduleLesson>
            {
                new SheduleLesson(new SheduleTime(Week.FirstWeek, Day.Monday, 1), "В-1301" , dates),
                new SheduleLesson(new SheduleTime(Week.FirstWeek, Day.Monday, 2), "В-1301", dates)
            };

            shedule = getSheduleWeeks();
            SheduleLesson lesson = shedule.FindLessonInList(lessons, new SheduleTime(Week.FirstWeek, Day.Monday, 1));

            Assert.AreEqual(lesson.Day, Day.Monday);
            Assert.AreEqual(lesson.Week, Week.FirstWeek);
            Assert.AreEqual(lesson.Room, "В-1301");
            Assert.AreEqual(lesson.Hour, 1);
        }

        [TestMethod]
        public void TestFindMonExistentLessonInList() {
            List<DateTime> dates = new List<DateTime> { new DateTime(2016, 9, 1), new DateTime(2016, 9, 15) };

            List<SheduleLesson> lessons = new List<SheduleLesson>
            {
                new SheduleLesson(new SheduleTime(Week.FirstWeek, Day.Monday, 1), "В-1301" , dates),
                new SheduleLesson(new SheduleTime(Week.FirstWeek, Day.Monday, 2), "В-1301", dates)
            };

            shedule = getSheduleWeeks();
            SheduleLesson lesson = shedule.FindLessonInList(lessons, new SheduleTime(Week.FirstWeek, Day.Sunday, 1));

            Assert.IsNull(lesson);
        }

        [TestMethod]
        public void TestGetDay()
        {
            shedule = getSheduleWeeks();

            SheduleDay day = shedule.GetDay(new SheduleTime(Week.FirstWeek, Day.Monday, 1));

            Assert.AreEqual(day.Week, Week.FirstWeek);
            Assert.AreEqual(day.Day, Day.Monday);
        }

        [TestMethod]
        public void TestGetNonExistentDay() {
            shedule = getSheduleWeeks();

            SheduleDay day = shedule.GetDay(new SheduleTime(Week.FourWeek, Day.Sunday, 56));

            Assert.IsNull(day);
        }

        [TestMethod]
        public void TestGetLessonsOfDay() {
            shedule = getSheduleWeeks();

            List<SheduleRoom> rooms;
            dsShedule sheduleDataSet = new dsShedule();
            string filename = @"../../fixtures/Аудитории.xml";
            sheduleDataSet.Room.ReadXml(filename);
            rooms = DictionaryConverter.RoomsToList(sheduleDataSet);

            SettingShedule setting = new SettingShedule(2, 6, 2, 4, 14, 4, 4, 1, 1, 6, 6);

            SheduleDay day = new SheduleDay(Week.FirstWeek, Day.Monday, rooms, setting, new DateTime(2016, 9, 1));
            IEnumerable<SheduleLesson> lessons = shedule.GetLessonsOfDay(day);
            foreach (var lesson in lessons)
            {
                Assert.AreEqual(lesson.Week, Week.FirstWeek);
                Assert.AreEqual(lesson.Day, Day.Monday);
            }
        }

        [TestMethod]
        public void TestGetLessonsOfNonExistentDay() {
            shedule = getSheduleWeeks();

            List<SheduleRoom> rooms;
            dsShedule sheduleDataSet = new dsShedule();
            string filename = @"../../fixtures/Аудитории.xml";
            sheduleDataSet.Room.ReadXml(filename);
            rooms = DictionaryConverter.RoomsToList(sheduleDataSet);

            SettingShedule setting = new SettingShedule(2, 6, 2, 4, 14, 4, 4, 1, 1, 6, 6);

            SheduleDay day = new SheduleDay(Week.FourWeek, Day.Sunday, rooms, setting, new DateTime(2016, 9, 1));
            IEnumerable<SheduleLesson> lessons = shedule.GetLessonsOfDay(day);
            Assert.IsNull(lessons);
        }

        [TestMethod]
        public void TestGetLessonTeacher()
        {
            shedule = getSheduleWeeks();

            IEnumerable<SheduleLesson> lessons = shedule.GetLessonsRoom("В-1301");

            foreach (var lesson in lessons)
            {
                Assert.AreEqual(lesson.Teacher, "В-1301");
            }
            Assert.AreEqual(lessons.Count(), 1);
        }
    }
}
