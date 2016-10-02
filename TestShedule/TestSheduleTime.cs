using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyShedule;

namespace TestShedule
{
    [TestClass]
    public class TestSheduleTime
    {

        private SheduleTime time;

        [TestMethod]
        public void TestEmptyConstructor()
        {
            time = new SheduleTime();

            Assert.AreEqual(Week.Another, time.Week);
            Assert.AreEqual(Day.Another, time.Day);
            Assert.AreEqual(0, time.Hour);
        }

        [TestMethod]
        public void TestNonEmptyConstructor()
        {
            time = new SheduleTime(Week.FirstWeek, Day.Monday, 3);

            Assert.AreEqual(Week.FirstWeek, time.Week);
            Assert.AreEqual(Day.Monday, time.Day);
            Assert.AreEqual(3, time.Hour);
        }

        [TestMethod]
        public void TestWeekNumber()
        {
            time = new SheduleTime(Week.FirstWeek, Day.Monday, 3);

            Assert.AreEqual(1, time.WeekNumber);
        }

        [TestMethod]
        public void TestDayNumber()
        {
            time = new SheduleTime(Week.FirstWeek, Day.Monday, 3);

            Assert.AreEqual(1, time.DayNumber);
        }

        [TestMethod]
        public void TestHourLessAllowed()
        {
            try
            {
                time = new SheduleTime(Week.FirstWeek, Day.Monday, -1);
                Assert.Fail();
            }
            catch(ArgumentOutOfRangeException)
            {
            }
        }

        [TestMethod]
        public void TestHourAllowed()
        {
            time = new SheduleTime(Week.FirstWeek, Day.Monday, 5);
            Assert.AreEqual(5, time.Hour);
        }

        [TestMethod]
        public void TestHourLargerAllowed()
        {
            try
            {
                time = new SheduleTime(Week.FirstWeek, Day.Monday, 16);
                Assert.Fail();
            }
            catch(ArgumentOutOfRangeException)
            {
            }
        }

        [TestMethod]
        public void TestCopy()
        {
            time = new SheduleTime(Week.FirstWeek, Day.Monday, 5);
            Assert.AreEqual(time, time.Copy());
        }

        [TestMethod]
        public void TestEquals()
        {
            SheduleTime time1 = new SheduleTime(Week.FirstWeek, Day.Monday, 5);
            SheduleTime time2 = new SheduleTime(Week.FirstWeek, Day.Monday, 5);
            Assert.IsTrue(time1.Equals(time2));
        }

        [TestMethod]
        public void TestNotEquals()
        {
            SheduleTime time1 = new SheduleTime(Week.FirstWeek, Day.Monday, 5);
            SheduleTime time2 = new SheduleTime(Week.FirstWeek, Day.Monday, 4);
            Assert.IsFalse(time1.Equals(time2));
        }

        [TestMethod]
        public void TestNotEqualsAnotherObject()
        {
            SheduleTime time1 = new SheduleTime(Week.FirstWeek, Day.Monday, 5);
            object time2 = new object();
            Assert.IsFalse(time1.Equals(time2));
        }

        // opertor ==
        [TestMethod]
        public void TestOperatorEqualIsTrue()
        {
            SheduleTime time1 = new SheduleTime(Week.FirstWeek, Day.Monday, 5);
            SheduleTime time2 = new SheduleTime(Week.FirstWeek, Day.Monday, 5);
            Assert.IsTrue(time1 == time2);

            time1 = new SheduleTime();
            time2 = new SheduleTime();
            Assert.IsTrue(time1 == time2);
        }

        // opertor ==
        [TestMethod]
        public void TestOperatorEqualIsFalse()
        {
            SheduleTime time1 = new SheduleTime(Week.FirstWeek, Day.Monday, 5);
            SheduleTime time2 = new SheduleTime(Week.FirstWeek, Day.Monday, 4);
            Assert.IsFalse(time1 == time2);
        }

        // opertor !=
        [TestMethod]
        public void TestOperatorNotEqualIsTrue()
        {
            SheduleTime time1 = new SheduleTime(Week.FirstWeek, Day.Monday, 5);
            SheduleTime time2 = new SheduleTime(Week.FirstWeek, Day.Monday, 1);
            Assert.IsTrue(time1 != time2);
        }

        // opertor !=
        [TestMethod]
        public void TestOperatorNotEqualIsFalse()
        {
            SheduleTime time1 = new SheduleTime(Week.FirstWeek, Day.Monday, 5);
            SheduleTime time2 = new SheduleTime(Week.FirstWeek, Day.Monday, 5);
            Assert.IsFalse(time1 != time2);
        }

        // opertator >
        [TestMethod]
        public void TestOpertatorGreatest()
        {
            List<SheduleTime> times = new List<SheduleTime>
            {
                new SheduleTime(Week.SecondWeek, Day.Tuesday, 2),
                new SheduleTime(Week.FirstWeek, Day.Tuesday, 2),
                new SheduleTime(Week.FirstWeek, Day.Monday, 2),
            };

            time = new SheduleTime(Week.FirstWeek, Day.Monday, 1);

            foreach(SheduleTime greatestTime in times)
            {
                Assert.IsTrue(greatestTime > time);
                Assert.IsFalse(time > greatestTime);
            }
        }

        // opertator <
        [TestMethod]
        public void TestOpertatorLess()
        {
            List<SheduleTime> times = new List<SheduleTime>
            {
                new SheduleTime(Week.SecondWeek, Day.Tuesday, 2),
                new SheduleTime(Week.FirstWeek, Day.Tuesday, 2),
                new SheduleTime(Week.FirstWeek, Day.Monday, 2),
            };

            time = new SheduleTime(Week.FirstWeek, Day.Monday, 1);

            foreach(SheduleTime greatestTime in times)
            {
                Assert.IsTrue(time < greatestTime);
                Assert.IsFalse(greatestTime < time);
            }
        }

        // opertator >=
        [TestMethod]
        public void TestOpertatorGreatestOrEqualIsTrue()
        {
            List<SheduleTime> times = new List<SheduleTime>
            {
                new SheduleTime(Week.SecondWeek, Day.Tuesday, 2),
                new SheduleTime(Week.FirstWeek, Day.Tuesday, 2),
                new SheduleTime(Week.FirstWeek, Day.Monday, 2),
                new SheduleTime(Week.FirstWeek, Day.Monday, 1)
        };

            time = new SheduleTime(Week.FirstWeek, Day.Monday, 1);

            foreach(SheduleTime greatestTime in times)
                Assert.IsTrue(greatestTime >= time);
        }

        // opertator >=
        [TestMethod]
        public void TestOpertatorGreatestOrEqualIsFalse()
        {
            List<SheduleTime> times = new List<SheduleTime>
            {
                new SheduleTime(Week.SecondWeek, Day.Tuesday, 2),
                new SheduleTime(Week.FirstWeek, Day.Tuesday, 2),
                new SheduleTime(Week.FirstWeek, Day.Monday, 2),
            };

            time = new SheduleTime(Week.FirstWeek, Day.Monday, 1);

            foreach(SheduleTime greatestTime in times)
                Assert.IsFalse(time >= greatestTime);
        }

        // opertator <=
        [TestMethod]
        public void TestOpertatorLessOrEqual()
        {
            List<SheduleTime> times = new List<SheduleTime>
            {
                new SheduleTime(Week.SecondWeek, Day.Tuesday, 2),
                new SheduleTime(Week.FirstWeek, Day.Tuesday, 2),
                new SheduleTime(Week.FirstWeek, Day.Monday, 2),
                new SheduleTime(Week.FirstWeek, Day.Monday, 1)
        };

            time = new SheduleTime(Week.FirstWeek, Day.Monday, 1);

            foreach(SheduleTime greatestTime in times)
                Assert.IsTrue(time <= greatestTime);
        }

        // opertator >=
        [TestMethod]
        public void TestOpertatorLessOrEqualIsFalse()
        {
            List<SheduleTime> times = new List<SheduleTime>
            {
                new SheduleTime(Week.SecondWeek, Day.Tuesday, 2),
                new SheduleTime(Week.FirstWeek, Day.Tuesday, 2),
                new SheduleTime(Week.FirstWeek, Day.Monday, 2),
            };

            time = new SheduleTime(Week.FirstWeek, Day.Monday, 1);

            foreach(SheduleTime greatestTime in times)
                Assert.IsFalse(greatestTime <= time);
        }

        [TestMethod]
        public void TestHourDescriptionInEmptyTime()
        {
            time = new SheduleTime();
            Assert.AreEqual("Часы не заданы", time.HourDescription);
        }

        [TestMethod]
        public void TestHourDescriptionInNonEmptyTime()
        {
            time = new SheduleTime(Week.FirstWeek, Day.Monday, 2);
            Assert.AreEqual("3-4", time.HourDescription);
        }

        [TestMethod]
        public void TestWeekDescriptionInEmptyTime()
        {
            time = new SheduleTime();
            Assert.AreEqual("Неделя не задана", time.WeekDescription);
        }

        [TestMethod]
        public void TestWeekDescriptionInNonEmptyTime()
        {
            time = new SheduleTime(Week.FirstWeek, Day.Monday, 2);
            Assert.AreEqual("Неделя I", time.WeekDescription);
        }

        [TestMethod]
        public void TestDayDescriptionInEmptyTime()
        {
            time = new SheduleTime();
            Assert.AreEqual("День не задан", time.DayDescription);
        }

        [TestMethod]
        public void TestDayDescriptionInNonEmptyTime()
        {
            time = new SheduleTime(Week.FirstWeek, Day.Monday, 2);
            Assert.AreEqual("Понедельник", time.DayDescription);
        }

        [TestMethod]
        public void TestDescriptionInEmptyTime()
        {
            time = new SheduleTime();
            Assert.AreEqual("Неделя не задана, день не задан, часы не заданы", time.Description);
        }

        [TestMethod]
        public void TestDescriptionInNonEmptyTime()
        {
            time = new SheduleTime(Week.FirstWeek, Day.Monday, 2);
            Assert.AreEqual("Неделя I, понедельник, 3-4", time.Description);
        }
    }
}
