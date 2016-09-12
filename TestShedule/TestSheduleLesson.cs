using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyShedule;

namespace TestShedule
{
    [TestClass]
    public class TestSheduleLesson
    {
        private SheduleLesson emptyLesson = new SheduleLesson();

        private SheduleLesson getSheduleLesson()
        {
            SheduleLesson lesson;

            SheduleTime time = new SheduleTime(Week.FirstWeek, Day.Monday, 1);

            List<DateTime> dates = new List<DateTime> { new DateTime(2016, 9, 6), new DateTime(2016, 9, 20)};

            lesson = new SheduleLesson(time, "В-404", dates);

            return lesson;
        }

        [TestMethod]
        public void TestIsEmpty()
        {
            Assert.AreEqual(emptyLesson.IsEmpty, true);
        }
    }
}
