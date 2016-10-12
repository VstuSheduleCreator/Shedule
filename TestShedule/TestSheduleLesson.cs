using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyShedule;

namespace TestShedule
{
    [TestClass]
    public class TestSheduleLesson
    {
        private SheduleLesson lesson = new SheduleLesson();

        private SheduleLesson getSheduleLesson()
        {
            SheduleLesson lesson;

            SheduleTime time = new SheduleTime(Week.FirstWeek, Day.Monday, 1);

            List<DateTime> dates = new List<DateTime> { new DateTime(2016, 9, 6), new DateTime(2016, 9, 20)};
            List<string> groups = new List<string> { "ИВТ-260", "ИВТ-261" };

            lesson = new SheduleLesson(time, "В-404", dates, "Андреев А.Е.", "Основы ЭВМ", groups, LessonType.Lection);

            return lesson;
        }

        [TestMethod]
        public void TestCopy()
        {
            lesson = getSheduleLesson();
            SheduleLesson lesson2 = lesson.Copy();

            Assert.AreEqual(lesson.Time, lesson2.Time);
            Assert.AreEqual(lesson.Teacher, lesson2.Teacher);
            Assert.AreEqual(lesson.Discipline, lesson2.Discipline);
            Assert.AreEqual(lesson.Type, lesson2.Type);
            Assert.AreEqual(lesson.Room, lesson2.Room);
            CollectionAssert.AreEqual(lesson.Groups, lesson2.Groups);
            CollectionAssert.AreEqual(lesson.Dates, lesson2.Dates);
        }

        [TestMethod]
        public void TestIsEmpty() {
            Assert.AreEqual(lesson.IsEmpty, true);
        }

        [TestMethod]
        public void TestClear()
        {
            lesson = getSheduleLesson();
            lesson.Clear();

            Assert.AreEqual(lesson.Teacher, String.Empty);
            Assert.AreEqual(lesson.Discipline, String.Empty);
            Assert.AreEqual(lesson.Groups.Count, 0);
            Assert.AreEqual(lesson.Type, LessonType.Lection);
        }

        [TestMethod]
        public void TestUpdateFields()
        {
            lesson = getSheduleLesson();
            List<DateTime> dates = new List<DateTime>() {new DateTime(2016, 9, 17)};
            lesson.UpdateFields("Андреев А.Е", "Основы ЭВМ", new List<string> {"ИВТ-260"}, LessonType.Labwork, dates);

            Assert.AreEqual(lesson.Teacher, "Андреев А.Е");
            Assert.AreEqual(lesson.Discipline, "Основы ЭВМ");
            Assert.AreEqual(lesson.Groups.Count, 1);
            Assert.AreEqual(lesson.Type, LessonType.Labwork);
            CollectionAssert.AreEqual(lesson.Dates, dates);
        }

        [TestMethod]
        public void TestIsEqual()
        {
            lesson = getSheduleLesson();
            SheduleLesson lesson2 = getSheduleLesson();

            Assert.IsTrue(lesson.IsEqual(lesson2));
        }

        [TestMethod]
        public void TestEmptyGroupsDescription()
        {
            lesson = getSheduleLesson();
            lesson.Groups = new List<string>();
            Assert.AreEqual(lesson.GroupsDescription, String.Empty);
        }

        [TestMethod]
        public void TestOneGroupsDescription() {
            lesson = getSheduleLesson();
            lesson.Groups = new List<string> {"ИВТ-260"};

            Assert.AreEqual(lesson.GroupsDescription, "ИВТ-260");
        }

        [TestMethod]
        public void TestFewGroupsDescription() {
            lesson = getSheduleLesson();

            Assert.AreEqual(lesson.GroupsDescription, "ИВТ-260, ИВТ-261");
        }

        //[TestMethod]
        //public void TestFewSimilarGroupsDescription() {
        //    lesson = getSheduleLesson();
        //    lesson.Groups = new List<string> { "ИВТ-260", "ИВТ-260" };

        //    Assert.AreEqual(lesson.GroupsDescription, "ИВТ-260, ИВТ-260");
        //}

        [TestMethod]
        public void TestDatesDiscription()
        {
            lesson = getSheduleLesson();
            Assert.AreEqual(lesson.DatesDescription, "06.09, 20.09");
        }

        [TestMethod]
        public void TestOneDataDescription()
        {
            lesson = getSheduleLesson();
            lesson.Dates = new List<DateTime> {new DateTime(2016, 9, 6)};
            Assert.AreEqual(lesson.DatesDescription, "06.09");
        }

        [TestMethod]
        public void TestWrapSheduleGoup()
        {
            lesson = getSheduleLesson();
            Assert.AreEqual(lesson.WrapSheduleGroup, "Основы ЭВМ\nЛекция\nАндреев А.Е.\nВ-404");
        }

        [TestMethod]
        public void TestWrapSheduleTeacher() {
            lesson = getSheduleLesson();
            Assert.AreEqual(lesson.WrapSheduleTeacher, "Основы ЭВМ\nИВТ-260, ИВТ-261\nЛекция\nВ-404");
        }

        [TestMethod]
        public void TestWrapSheduleDiscipline() {
            lesson = getSheduleLesson();
            Assert.AreEqual(lesson.WrapSheduleDiscipline, "Андреев А.Е.\nИВТ-260, ИВТ-261\nЛекция\nВ-404");
        }

        [TestMethod]
        public void TestWrapSheduleRoom() {
            lesson = getSheduleLesson();
            Assert.AreEqual(lesson.WrapSheduleRoom, "Андреев А.Е.\nИВТ-260, ИВТ-261\nЛекция\nОсновы ЭВМ");
        }
    }
}
