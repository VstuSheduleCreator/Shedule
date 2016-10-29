using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyShedule;

namespace TestShedule
{
    [TestClass]
    public class TestSheduleRoom
    {
        [TestMethod]
        public void TestPosibleLessonsTypes()
        {
            SheduleRoom room = new SheduleRoom();
            room.Practice = false;

            List<LessonType> expected = new List<LessonType> {LessonType.Lection, LessonType.Labwork};
            CollectionAssert.AreEqual(expected, room.PossibleLessonsTypes);
        }

        [TestMethod]
        public void TestCanHoldLesson()
        {
            SheduleRoom room = new SheduleRoom();
            room.Practice = false;

            Assert.IsTrue(room.CanHoldLesson(LessonType.Labwork));
            Assert.IsTrue(room.CanHoldLesson(LessonType.Lection));
            Assert.IsFalse(room.CanHoldLesson(LessonType.Practice));
        }
    }
}
