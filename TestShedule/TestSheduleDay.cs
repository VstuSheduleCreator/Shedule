using System;
using System.Collections.Generic;
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
            Week week;
            Day day;
            SettingShedule setting = new SettingShedule(2, 6, 6, 4, 32, 4, 2, 1, 1, 4, 2);
            DateTime firstDate;
            IEnumerable<SheduleRoom> rooms;

            return new SheduleDay();
        }

        [TestMethod]
        public void TestFirstLessonHourInEmptyDay()
        {
            Assert.AreEqual(day.FirstLessonHour, 0);
        }

        [TestMethod]
        public void TestCountLessonsInEmptyDay()
        {
            Assert.AreEqual(day.CountLessons, 0);
        }

        //[TestMethod]
        //public void NonEmptyLessons()
        //{
        //    Assert.AreEqual(day.NonEmptyLessons, 0);
        //}

        //[TestMethod]
        //public void TestCountLessonsGroupInEmptyDay()
        //{
        //    Assert.AreEqual(day.CountLessons, 0);
        //}

        //[TestMethod]
        //public void TestLimitLessonsNotExceededInEmptyDay()
        //{
        //    Assert.AreEqual(day.CountLessons, 0);
        //}

        [TestMethod]
        public void TestDatesDescriptionInEmptyDay()
        {
            Assert.AreEqual(day.DatesDescription, "");
        }

    }
}
