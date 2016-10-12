using System.Collections.Generic;
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
    }
}
