using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyShedule;

namespace TestShedule
{
    [TestClass]
    public class TestSheduleLoadItem
    {
        private LoadItem loadItem;

        private LoadItem getLoadItem()
        {
            return new LoadItem(
                "Коптелова И.П.",
                "Теория принятия решений",
                new List<string> { "ИВТ - 461", "ИВТ - 462", "ИВТ - 463" },
                40,
                0
                );
        }

        [TestMethod]
        public void TestCopy()
        {
            loadItem = getLoadItem();
            LoadItem loadItem2 = loadItem.Copy();
            Assert.AreEqual(loadItem.Teacher, loadItem2.Teacher);
            Assert.AreEqual(loadItem.Discipline, loadItem2.Discipline);
            Assert.AreEqual(loadItem.Groups, loadItem2.Groups);
            Assert.AreEqual(loadItem.HoursSem, loadItem2.HoursSem);
            Assert.AreEqual(loadItem.LessonType, loadItem2.LessonType);
        }

        [TestMethod]
        public void TestEmptyConstructor()
        {
            loadItem = new LoadItem();
            Assert.IsNull(loadItem.Teacher);
            Assert.IsNull(loadItem.Discipline);
            Assert.AreEqual(0, loadItem.Groups.Count);
            Assert.AreEqual(0, loadItem.HoursSem);
            Assert.AreEqual(LessonType.Lection, loadItem.LessonType);
        }

        [TestMethod]
        public void TestConstructor()
        {
            loadItem = getLoadItem();

            Assert.AreEqual("Коптелова И.П.", loadItem.Teacher);
            Assert.AreEqual("Теория принятия решений", loadItem.Discipline);
            CollectionAssert.AreEqual(new List<string> { "ИВТ - 461", "ИВТ - 462", "ИВТ - 463" }, loadItem.Groups);
            Assert.AreEqual(40, loadItem.HoursSem);
            Assert.AreEqual(LessonType.Lection, loadItem.LessonType);
        }

        [TestMethod]
        public void TestGetHoursSemEmptyItem()
        {
            loadItem = new LoadItem();
            Assert.AreEqual(0, loadItem.HoursSem);
        }

        [TestMethod]
        public void TestGetHours()
        {
            loadItem = getLoadItem();
            Assert.AreEqual(40, loadItem.HoursSem);
        }

        [TestMethod]
        public void TestSetHours()
        {
        }

        [TestMethod]
        public void TestHourSmallAllowed()
        {
            loadItem = new LoadItem();
            try
            {
                loadItem.HoursSem = 0;
                Assert.Fail();
            } catch(ArgumentOutOfRangeException)
            {
            }
        }

        [TestMethod]
        public void TestHourAllowed()
        {
            loadItem = new LoadItem();
            loadItem.HoursSem = 1;
            Assert.AreEqual(1, loadItem.HoursSem);
        }

        [TestMethod]
        public void TestHoursByMonth()
        {
            loadItem = getLoadItem();
            loadItem.HoursSem = 43;
            Assert.AreEqual(11, loadItem.HoursByMonth);
        }
    }
}
