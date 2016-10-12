﻿using System;
﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyShedule;

namespace TestShedule {
    [TestClass]
    public class TestSettingShedule {

        private SettingShedule get_valid_setting()
        {
           return new SettingShedule(4, 7, 28, 8, 8, 8, 8, 1, 1, 1, 1);
        }

        private SettingShedule setting;

        [TestMethod]
        public void CountWeeksLargerAllowed()
        {
            setting = get_valid_setting();
            try
            {
                setting.CountWeeksShedule = 5;
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        [TestMethod]
        public void CountWeeksLessAllowed()
        {
            setting = get_valid_setting();
            try
            {
                setting.CountWeeksShedule = 0;
                Assert.Fail();
            } catch(ArgumentOutOfRangeException)
            {
            }
        }

        [TestMethod]
        public void CountWeeksAllowed()
        {
            setting = get_valid_setting();
            Assert.AreEqual(4, setting.CountWeeksShedule);
        }

        [TestMethod]
        public void CountDaysEducationWeekLargerAllowed()
        {
            setting = get_valid_setting();
            try
            {
                setting.CountDaysEducationWeek = 8;
                Assert.Fail();
            } catch(ArgumentOutOfRangeException)
            {
            }
        }

        [TestMethod]
        public void CountDaysEducationWeekLessAllowed()
        {
            setting = get_valid_setting();
            try
            {
                setting.CountDaysEducationWeek = 0;
                Assert.Fail();
            } catch(ArgumentOutOfRangeException)
            {
            }
        }

        [TestMethod]
        public void CountDaysEducationWeekAllowed()
        {
            setting = get_valid_setting();
            Assert.AreEqual(4, setting.CountWeeksShedule);
        }

        [TestMethod]
        public void InvalidCountDaysShedule()
        {
            setting = get_valid_setting();
            setting.CountDaysShedule= 228;
            Assert.AreEqual(setting.CountWeeksShedule * setting.CountDaysEducationWeek, setting.CountDaysShedule);
        }

        [TestMethod]
        public void CountLessonsOfDayLargerAllowed()
        {
            setting = get_valid_setting();
            try
            {
                setting.CountLessonsOfDay = 9;
                Assert.Fail();
            } catch(ArgumentOutOfRangeException)
            {
            }
        }

        [TestMethod]
        public void CountLessonsOfDayLessAllowed()
        {
            setting = get_valid_setting();
            try
            {
                setting.CountLessonsOfDay = -1;
                Assert.Fail();
            } catch(ArgumentOutOfRangeException)
            {
            }
        }

        [TestMethod]
        public void CountLessonsOfDayAllowed()
        {
            setting = get_valid_setting();
            setting.CountLessonsOfDay = 0;
            Assert.AreEqual(0, setting.CountLessonsOfDay);
        }

        [TestMethod]
        public void CountEducationalWeekBySemLargerAllowed()
        {
            setting = get_valid_setting();
            try
            {
                setting.CountEducationalWeekBySem = 53;
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        [TestMethod]
        public void CountEducationalWeekBySemLessAllowed()
        {
            setting = get_valid_setting();
            try
            {
                setting.CountEducationalWeekBySem = 0;
                Assert.Fail();
            } catch(ArgumentOutOfRangeException)
            {
            }
        }

        [TestMethod]
        public void CountEducationalWeekBySemAllowed()
        {
            setting = get_valid_setting();
            Assert.AreEqual(8, setting.CountEducationalWeekBySem);
        }

        [TestMethod]
        public void MaxCountLessonsOfWeekDayAllowed()
        {
            setting = get_valid_setting();
            setting.CountLessonsOfDay = 5;
            setting.MaxCountLessonsOfWeekDay = 0;
            Assert.AreEqual(0, setting.MaxCountLessonsOfWeekDay);
        }

        [TestMethod]
        public void MaxCountLessonsOfWeekDayLargerAllowed()
        {
            setting = get_valid_setting();
            setting.CountLessonsOfDay = 5;
            try
            {
                setting.MaxCountLessonsOfWeekDay = 6;
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        [TestMethod]
        public void MaxCountLessonsOfWeekDayLessAllowed()
        {
            setting = get_valid_setting();
            try
            {
                setting.MaxCountLessonsOfWeekDay = -1;
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        [TestMethod]
        public void MaxCountLessonsOfWeekEndAllowed()
        {
            setting = get_valid_setting();
            setting.CountLessonsOfDay = 5;
            setting.MaxCountLessonsOfWeekEnd = 0;
            Assert.AreEqual(0, setting.MaxCountLessonsOfWeekEnd);
        }

        [TestMethod]
        public void MaxCountLessonsOfWeekEndLargerAllowed()
        {
            setting = get_valid_setting();
            setting.CountLessonsOfDay = 5;
            try
            {
                setting.MaxCountLessonsOfWeekEnd = 6;
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        [TestMethod]
        public void MaxCountLessonsOfWeekEndLessAllowed()
        {
            setting = get_valid_setting();
            try
            {
                setting.MaxCountLessonsOfWeekEnd = -1;
                Assert.Fail();
            } catch(ArgumentOutOfRangeException)
            {
            }
        }

    }
}
