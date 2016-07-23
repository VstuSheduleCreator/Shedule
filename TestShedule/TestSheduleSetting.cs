﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyShedule;

namespace TestShedule {
    [TestClass]
    public class TestSettingShedule {

        private SettingShedule get_valid_setting() {
           return new SettingShedule(4, 7, 28, 8, 8, 8, 8, 1, 1, 1, 1);
        }

        private SettingShedule setting;

        [TestMethod]
        public void CountWeeksLargerAllowed() {
            setting = get_valid_setting();
            setting.CountWeeksShedule = 5;
            Assert.AreEqual(setting.CountWeeksShedule, 4);
        }

        [TestMethod]
        public void CountWeeksLessAllowed() {
            setting = get_valid_setting();
            setting.CountWeeksShedule = 0;
            Assert.AreEqual(setting.CountWeeksShedule, 1);
        }

        [TestMethod]
        public void CountDaysEducationWeekLargerAllowed() {
            setting = get_valid_setting();
            setting.CountDaysEducationWeek = 8;
            Assert.AreEqual(setting.CountDaysEducationWeek, 7);
        }

        [TestMethod]
        public void CountDaysEducationWeekLessAllowed() {
            setting = get_valid_setting();
            setting.CountDaysEducationWeek = 0;
            Assert.AreEqual(setting.CountDaysEducationWeek, 1);
        }

        [TestMethod]
        public void InvalidCountDaysShedule() {
            setting = get_valid_setting();
            setting.CountDaysShedule= 228;
            Assert.AreEqual(setting.CountDaysShedule, setting.CountWeeksShedule * setting.CountDaysEducationWeek);
        }

        [TestMethod]
        public void CountLessonsOfDayLargerAllowed() {
            setting = get_valid_setting();
            setting.CountLessonsOfDay = 9;
            Assert.AreEqual(setting.CountLessonsOfDay, 8);
        }

        [TestMethod]
        public void CountLessonsOfDayLessAllowed() {
            setting = get_valid_setting();
            setting.CountLessonsOfDay = 0;
            Assert.AreEqual(setting.CountLessonsOfDay, 1);
        }

        [TestMethod]
        public void CountEducationalWeekBySemLargerAllowed() {
            setting = get_valid_setting();
            setting.CountEducationalWeekBySem = 53;
            Assert.AreEqual(setting.CountEducationalWeekBySem, 52);
        }

        [TestMethod]
        public void CountEducationalWeekBySemLessAllowed() {
            setting = get_valid_setting();
            setting.CountEducationalWeekBySem = 0;
            Assert.AreEqual(setting.CountEducationalWeekBySem, 1);
        }

        [TestMethod]
        public void MaxCountLessonsOfWeekDayLargerAllowed() {
            setting = get_valid_setting();
            setting.CountLessonsOfDay = 5;
            setting.MaxCountLessonsOfWeekDay = 6;
            Assert.AreEqual(setting.MaxCountLessonsOfWeekDay, setting.CountLessonsOfDay);
        }

        [TestMethod]
        public void MaxCountLessonsOfWeekDayLessAllowed() {
            setting = get_valid_setting();
            setting.MaxCountLessonsOfWeekDay = 0;
            Assert.AreEqual(setting.MaxCountLessonsOfWeekDay, 1);
        }

        [TestMethod]
        public void MaxCountLessonsOfWeekEndLargerAllowed() {
            setting = get_valid_setting();
            setting.CountLessonsOfDay = 5;
            setting.MaxCountLessonsOfWeekDay = 6;
            Assert.AreEqual(setting.MaxCountLessonsOfWeekDay, setting.CountLessonsOfDay);
        }

        [TestMethod]
        public void MaxCountLessonsOfWeekEndLessAllowed() {
            setting = get_valid_setting();
            setting.MaxCountLessonsOfWeekDay = 0;
            Assert.AreEqual(setting.MaxCountLessonsOfWeekDay, 1);
        }

    }
}
