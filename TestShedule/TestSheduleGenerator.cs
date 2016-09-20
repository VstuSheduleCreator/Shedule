using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyShedule;

namespace TestShedule
{
    [TestClass]
    public class TestSheduleGeerator
    {

        private SheduleGenerator getValidGenerator()
        {
            SheduleGenerator validGenerator;

            EducationLoadAdapter adapter = new EducationLoadAdapter();

            List<SheduleRoom> rooms = new List<SheduleRoom>();

            SettingShedule setting = new SettingShedule();

            DateTime firstDaySem = new DateTime();

            Employments employments = new Employments();

            dsShedule sheduleDataSet = new dsShedule();
            string filename = @"../../fixtures/Нагрузка.xml";
            sheduleDataSet.Education.ReadXml(filename);
            adapter = new EducationLoadAdapter(DictionaryConverter.EducationToList(sheduleDataSet));

            filename = @"../../fixtures/Аудитории.xml";
            sheduleDataSet.Room.ReadXml(filename);
            rooms = DictionaryConverter.RoomsToList(sheduleDataSet);

            validGenerator = new SheduleGenerator(adapter, rooms, setting, firstDaySem, employments);

            return validGenerator;
        }

        private SheduleGenerator generator;

        [TestMethod]
        public void TestGenerate()
        {
            SheduleWeeks shedule;

            generator = getValidGenerator();
            shedule = generator.Generate();

            Assert.AreEqual(shedule.Rooms.Count,8);
            Assert.AreEqual(generator.Results.Count(), 9);
        }

        [TestMethod]
        public void TestGenerateWithoutLoad() {
            SheduleWeeks shedule;

            generator = getValidGenerator();

            dsShedule sheduleDataSet = new dsShedule();
            string filename = @"../../fixtures/EmptyLoad.xml";
            sheduleDataSet.Education.ReadXml(filename);
            generator.LoadItemsAdapter = new EducationLoadAdapter(DictionaryConverter.EducationToList(sheduleDataSet));


            shedule = generator.Generate();

            Assert.AreEqual(shedule.Rooms.Count, 8);
            Assert.AreEqual(generator.Results.Count(), 0);
        }

        [TestMethod]
        public void TestGenerateWithOnlyLab() {
            SheduleWeeks shedule;

            generator = getValidGenerator();

            dsShedule sheduleDataSet = new dsShedule();
            string filename = @"../../fixtures/OnlyLabLoad.xml";
            sheduleDataSet.Education.ReadXml(filename);
            generator.LoadItemsAdapter = new EducationLoadAdapter(DictionaryConverter.EducationToList(sheduleDataSet));


            shedule = generator.Generate();

            Assert.AreEqual(shedule.Rooms.Count, 8);
            Assert.AreEqual(generator.Results.Count(), 3);
        }

        [TestMethod]
        public void TestGenerateWithOnlyPractice() {
            SheduleWeeks shedule;

            generator = getValidGenerator();

            dsShedule sheduleDataSet = new dsShedule();
            string filename = @"../../fixtures/OnlyPracticeLoad.xml";
            sheduleDataSet.Education.ReadXml(filename);
            generator.LoadItemsAdapter = new EducationLoadAdapter(DictionaryConverter.EducationToList(sheduleDataSet));


            shedule = generator.Generate();

            Assert.AreEqual(shedule.Rooms.Count, 8);
            Assert.AreEqual(generator.Results.Count(), 3);
        }

        [TestMethod]
        public void TestGenerateWithOnlyOneLection() {
            SheduleWeeks shedule;

            generator = getValidGenerator();

            dsShedule sheduleDataSet = new dsShedule();
            string filename = @"../../fixtures/OnlyOneLectionLoad.xml";
            sheduleDataSet.Education.ReadXml(filename);
            generator.LoadItemsAdapter = new EducationLoadAdapter(DictionaryConverter.EducationToList(sheduleDataSet));


            shedule = generator.Generate();

            Assert.AreEqual(shedule.Rooms.Count, 8);
            Assert.AreEqual(generator.Results.Count(), 1);
        }
    }
}
