using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyShedule;

namespace TestShedule
{
    [TestClass]
    public class TestSheduleLoadItemsAdapter
    {
        private EducationLoadAdapter loadAdapter;

        private EducationLoadAdapter getAdapter()
        {
            dsShedule sheduleDataSet = new dsShedule();
            string filename = @"../../fixtures/Нагрузка.xml";
            sheduleDataSet.Education.ReadXml(filename);
            loadAdapter = new EducationLoadAdapter(DictionaryConverter.EducationToList(sheduleDataSet));

            return loadAdapter;
        }

        [TestMethod]
        public void TestEmptyConstructor()
        {
            loadAdapter = new EducationLoadAdapter();
            Assert.IsNotNull(loadAdapter);
            Assert.AreEqual(0, loadAdapter.Items.Count);
        }

        [TestMethod]
        public void TestConstructor()
        {

            loadAdapter = getAdapter();
            Assert.IsNotNull(loadAdapter);
            Assert.AreEqual(6, loadAdapter.Items.Count);
        }

        [TestMethod]
        public void TestNamesGroupsWithoutItems()
        {
            loadAdapter = new EducationLoadAdapter();
            Assert.AreEqual(0, loadAdapter.NamesGroups.Count);
        }

        [TestMethod]
        public void TestNamesGroups()
        {
            loadAdapter = getAdapter();
            loadAdapter.Items.Add(new LoadItem());
            List<string> groups = new List<string> { "ИВТ-360", "ИВТ-361", "ИВТ-362", "ИВТ-461", "ИВТ-462", "ИВТ-463" };
            CollectionAssert.AreEqual(groups, loadAdapter.NamesGroups);
        }

        [TestMethod]
        public void TestNamesTeachersGroupsWithoutItems()
        {
            loadAdapter = new EducationLoadAdapter();
            Assert.AreEqual(0, loadAdapter.NamesTeachers.Count);
        }

        [TestMethod]
        public void TestNamesTeachers()
        {
            loadAdapter = getAdapter();
            loadAdapter.Items.Add(new LoadItem());
            List<string> teachers = new List<string> { "Коптелова И.П.", "Лукьянов В.С.", "Андреев А.Е.", "Забалуева А.Ф."};
            CollectionAssert.AreEqual(teachers, loadAdapter.NamesTeachers);
        }

        [TestMethod]
        public void TestNamesDisciplinesGroupsWithoutItems()
        {
            loadAdapter = new EducationLoadAdapter();
            Assert.AreEqual(0, loadAdapter.NamesDisciplines.Count);
        }

        [TestMethod]
        public void TestNamesDisciplines()
        {
            loadAdapter = getAdapter();
            loadAdapter.Items.Add(new LoadItem());
            List<string> disciplines = new List<string> { "Теория принятия решений", "Криптография", "Организация ЭВМ" };
            CollectionAssert.AreEqual(disciplines, loadAdapter.NamesDisciplines);
        }

        [TestMethod]
        public void TestDivideLoadOnSubItemsWithoutLoad()
        {
            dsShedule sheduleDataSet = new dsShedule();
            string filename = @"../../fixtures/EmptyLoad.xml";
            sheduleDataSet.Education.ReadXml(filename);
            loadAdapter = new EducationLoadAdapter(DictionaryConverter.EducationToList(sheduleDataSet));

            Assert.AreEqual(0, loadAdapter.DivideLoadOnSubItems().Items.Count);
        }

        [TestMethod]
        public void TestAdd()
        {
            loadAdapter = getAdapter();
            Assert.AreEqual(6, loadAdapter.Items.Count);
            loadAdapter.Add(new LoadItem());
            Assert.AreEqual(7, loadAdapter.Items.Count);
        }

        [TestMethod]
        public void TestLoadItemsDividedWithOneItemThenHoursByMonthEqualStep()
        {
            List<LoadItem> expected = new List<LoadItem>
            {
                new LoadItem("Забалуева А.Ф.", "Организация ЭВМ", new List<string> {"ИВТ-360"}, 32, LessonType.Labwork)
            };

            dsShedule sheduleDataSet = new dsShedule();
            string filename = @"../../fixtures/OnlyOneDisciplineTheHoursByMonthEqualStepLoad.xml";
            sheduleDataSet.Education.ReadXml(filename);
            loadAdapter = new EducationLoadAdapter(DictionaryConverter.EducationToList(sheduleDataSet));

            Assert.AreEqual(expected[0].HoursByMonth, loadAdapter.DivideLoadOnSubItems().Items[0].DivideHours);
            Assert.AreEqual(1, loadAdapter.Items.Count);
        }

        [TestMethod]
        public void TestLoadItemsDividedWithOneItemThenHoursByMonthLargerStep()
        {
            List<LoadItem> expected = new List<LoadItem>
            {
                new LoadItem("Забалуева А.Ф.", "Организация ЭВМ", new List<string> {"ИВТ-360"}, 40, LessonType.Labwork)
            };

            dsShedule sheduleDataSet = new dsShedule();
            string filename = @"../../fixtures/OnlyOneDisciplineTheHoursByMonthLargerStepLoad.xml";
            sheduleDataSet.Education.ReadXml(filename);
            loadAdapter = new EducationLoadAdapter(DictionaryConverter.EducationToList(sheduleDataSet));

            Assert.AreEqual(8, loadAdapter.DivideLoadOnSubItems(8).Items[0].DivideHours);
            Assert.AreEqual(expected[0].HoursByMonth - 8, loadAdapter.DivideLoadOnSubItems(8).Items[1].DivideHours);
        }

        [TestMethod]
        public void TestLoadItemsDividedWithOneItemThenHoursByMonthLessStep()
        {
            List<LoadItem> expected = new List<LoadItem>
            {
                new LoadItem("Забалуева А.Ф.", "Организация ЭВМ", new List<string> {"ИВТ-360"}, 24, LessonType.Labwork)
            };

            dsShedule sheduleDataSet = new dsShedule();
            string filename = @"../../fixtures/OnlyOneDisciplineTheHoursByMonthLessStepLoad.xml";
            sheduleDataSet.Education.ReadXml(filename);
            loadAdapter = new EducationLoadAdapter(DictionaryConverter.EducationToList(sheduleDataSet));

            Assert.AreEqual(expected[0].HoursByMonth, loadAdapter.DivideLoadOnSubItems().Items[0].DivideHours);
        }

        [TestMethod]
        public void TestLoadItemsDividedWithSevearlItems()
        {
            List<LoadItem> expected = new List<LoadItem>
            {
                new LoadItem("Забалуева А.Ф.", "Организация ЭВМ", new List<string> {"ИВТ-360"}, 40, LessonType.Labwork),
                new LoadItem("Лукьянов В.С.", "Криптография", new List<string> {"ИВТ-360", "ИВТ-361", "ИВТ-362"}, 40, LessonType.Labwork)
            };

            dsShedule sheduleDataSet = new dsShedule();
            string filename = @"../../fixtures/SeveralDisciplinesLoad.xml";
            sheduleDataSet.Education.ReadXml(filename);
            loadAdapter = new EducationLoadAdapter(DictionaryConverter.EducationToList(sheduleDataSet));

            Assert.AreEqual(4, loadAdapter.DivideLoadOnSubItems().Items.Count);

            Assert.AreEqual(8, loadAdapter.DivideLoadOnSubItems().Items[0].DivideHours);
            Assert.AreEqual(expected[0].Discipline, loadAdapter.DivideLoadOnSubItems().Items[0].Discipline);

            Assert.AreEqual(expected[0].HoursByMonth - 8, loadAdapter.DivideLoadOnSubItems().Items[1].DivideHours);
            Assert.AreEqual(expected[0].Discipline, loadAdapter.DivideLoadOnSubItems().Items[1].Discipline);

            Assert.AreEqual(8, loadAdapter.DivideLoadOnSubItems().Items[2].DivideHours);
            Assert.AreEqual(expected[1].Discipline, loadAdapter.DivideLoadOnSubItems().Items[2].Discipline);

            Assert.AreEqual(expected[1].HoursByMonth - 8, loadAdapter.DivideLoadOnSubItems().Items[3].DivideHours);
            Assert.AreEqual(expected[1].Discipline, loadAdapter.DivideLoadOnSubItems().Items[3].Discipline);
        }

        [TestMethod]
        public void TestSorteLoadItemsOnRegularIntervals()
        {
            List<LoadItem> expected = new List<LoadItem>
            {
                new LoadItem("Забалуева А.Ф.", "Организация ЭВМ", new List<string> {"ИВТ-360"}, 40, LessonType.Labwork),
                new LoadItem("Лукьянов В.С.", "Криптография", new List<string> {"ИВТ-360", "ИВТ-361", "ИВТ-362"}, 40, LessonType.Labwork)
            };

            dsShedule sheduleDataSet = new dsShedule();
            string filename = @"../../fixtures/SeveralDisciplinesLoad.xml";
            sheduleDataSet.Education.ReadXml(filename);
            loadAdapter = new EducationLoadAdapter(DictionaryConverter.EducationToList(sheduleDataSet));

            List<LoadItem> items = loadAdapter.DivideLoadOnSubItems().SortLoadItemsOnRegularIntervals().ToList();

            Assert.AreEqual(4, items.Count());

            Assert.AreEqual(8, items[0].DivideHours);
            Assert.AreEqual(expected[0].Discipline, items[0].Discipline);

            Assert.AreEqual(8, items[1].DivideHours);
            Assert.AreEqual(expected[1].Discipline, items[1].Discipline);

            Assert.AreEqual(expected[0].HoursByMonth - 8, items[2].DivideHours);
            Assert.AreEqual(expected[0].Discipline, items[2].Discipline);

            Assert.AreEqual(expected[1].HoursByMonth - 8, items[3].DivideHours);
            Assert.AreEqual(expected[1].Discipline, items[3].Discipline);
        }
    }
}
