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

        private SheduleGenerator getValidGenerator(SettingShedule setting = null, string load_path = null)
        {
            SheduleGenerator validGenerator;

            EducationLoadAdapter adapter = new EducationLoadAdapter();

            List<SheduleRoom> rooms = new List<SheduleRoom>();

            if (setting == null) setting = new SettingShedule();

            DateTime firstDaySem = new DateTime();

            Employments employments = new Employments();

            dsShedule sheduleDataSet = new dsShedule();
            
            if(load_path == null ) load_path = @"../../fixtures/Нагрузка.xml";
            sheduleDataSet.Education.ReadXml(load_path);
            adapter = new EducationLoadAdapter(DictionaryConverter.EducationToList(sheduleDataSet));

            string filename = @"../../fixtures/Аудитории.xml";
            sheduleDataSet.Room.ReadXml(filename);
            rooms = DictionaryConverter.RoomsToList(sheduleDataSet);

            validGenerator = new SheduleGenerator(adapter, rooms, setting, firstDaySem, employments);

            return validGenerator;
        }

        private SheduleGenerator generator;

        [TestMethod]
        public void TestLoadItemsDivided()
        {
            generator = getValidGenerator();
            Assert.AreEqual(9, generator.LoadItemsDivided.Count());
        }

        [TestMethod]
        public void TestGenerateWithoutItems()
        {
            generator = getValidGenerator(load_path: @"../../fixtures/EmptyLoad.xml");
            Assert.AreEqual(0, generator.Results.Count);
        }

        [TestMethod]
        public void TestGenerateWithEmptySettings()
        {
            SheduleWeeks shedule;

            generator = getValidGenerator();
            shedule = generator.Generate();

            Assert.AreEqual(0, shedule.Days.Count);
            Assert.AreEqual(9, generator.Results.Count());
        }

        [TestMethod]
        public void TestNumbersOfDaysInSheduleWeek()
        {
            for (int countDay = 1; countDay <= 7; countDay++)
            {
                SettingShedule setting = new SettingShedule(1, countDay, 28, 8, 8, 8, 8, 1, 1, 1, 1);
                generator = getValidGenerator(setting: setting);

                for(int day = 1; day <= 7; day++)
                    if(day <= countDay)
                        Assert.IsNotNull(generator.Generate().GetDay((Week)1, (Day)day));
                    else
                        Assert.IsNull(generator.Generate().GetDay((Week)1, (Day)day));
            }
        }

        // PutTwoLessonOnTwoWeek вызывается когда нагрузка распределяется в первом шагу и нагрузка пропорциональна шагу
        [TestMethod]
        public void TestPutTwoLessonOnTwoWeek()
        {
            SettingShedule setting = new SettingShedule(2, 6, 28, 8, 16, 8, 8, 1, 1, 1, 1);
            generator = getValidGenerator(load_path: @"../../fixtures/LoadProportionallyStep.xml", setting: setting);
            SheduleWeeks shedule = generator.Generate();

            Assert.AreEqual(1, generator.Results.Count);
            Assert.AreEqual(2, shedule.GetLessonsDiscipline("Теория принятия решений").Count());
        }

        //[TestMethod]
        //public void TestGenerateWithoutLoad() {
        //    SheduleWeeks shedule;

        //    generator = getValidGenerator();

        //    dsShedule sheduleDataSet = new dsShedule();
        //    string filename = @"../../fixtures/EmptyLoad.xml";
        //    sheduleDataSet.Education.ReadXml(filename);
        //    generator.LoadItemsAdapter = new EducationLoadAdapter(DictionaryConverter.EducationToList(sheduleDataSet));


        //    shedule = generator.Generate();

        //    Assert.AreEqual(shedule.Rooms.Count, 8);
        //    Assert.AreEqual(generator.Results.Count(), 0);
        //}

        //[TestMethod]
        //public void TestGenerateWithOnlyLab() {
        //    SheduleWeeks shedule;

        //    generator = getValidGenerator();

        //    dsShedule sheduleDataSet = new dsShedule();
        //    string filename = @"../../fixtures/OnlyLabLoad.xml";
        //    sheduleDataSet.Education.ReadXml(filename);
        //    generator.LoadItemsAdapter = new EducationLoadAdapter(DictionaryConverter.EducationToList(sheduleDataSet));


        //    shedule = generator.Generate();

        //    Assert.AreEqual(shedule.Rooms.Count, 8);
        //    Assert.AreEqual(generator.Results.Count(), 3);
        //}

        //[TestMethod]
        //public void TestGenerateWithOnlyPractice() {
        //    SheduleWeeks shedule;

        //    generator = getValidGenerator();

        //    dsShedule sheduleDataSet = new dsShedule();
        //    string filename = @"../../fixtures/OnlyPracticeLoad.xml";
        //    sheduleDataSet.Education.ReadXml(filename);
        //    generator.LoadItemsAdapter = new EducationLoadAdapter(DictionaryConverter.EducationToList(sheduleDataSet));


        //    shedule = generator.Generate();

        //    Assert.AreEqual(shedule.Rooms.Count, 8);
        //    Assert.AreEqual(generator.Results.Count(), 3);
        //}

        //[TestMethod]
        //public void TestGenerateWithOnlyOneLection() {
        //    SheduleWeeks shedule;

        //    generator = getValidGenerator();

        //    dsShedule sheduleDataSet = new dsShedule();
        //    string filename = @"../../fixtures/OnlyOneLectionLoad.xml";
        //    sheduleDataSet.Education.ReadXml(filename);
        //    generator.LoadItemsAdapter = new EducationLoadAdapter(DictionaryConverter.EducationToList(sheduleDataSet));


        //    shedule = generator.Generate();

        //    Assert.AreEqual(shedule.Rooms.Count, 8);
        //    Assert.AreEqual(generator.Results.Count(), 1);
        //}
    }
}
