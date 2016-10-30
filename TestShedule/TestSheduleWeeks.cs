using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyShedule;

namespace TestShedule
{
    [TestClass]
    public class TestSheduleWeeks
    {
        private List<SheduleRoom> getRooms()
        {
            dsShedule sheduleDataSet = new dsShedule();
            string filename = @"../../fixtures/Аудитории.xml";
            sheduleDataSet.Room.ReadXml(filename);
            return DictionaryConverter.RoomsToList(sheduleDataSet);
        }

        private SettingShedule getSetting()
        {
            return new SettingShedule(2, 6, 2, 4, 14, 4, 4, 1, 1, 6, 6);
        }

        private SheduleWeeks getSheduleWeeks()
        {
            List<SheduleRoom> rooms = getRooms();
            SettingShedule setting = getSetting();

            return new SheduleWeeks(rooms, setting, new DateTime(2016, 9, 1));
        }

        private SheduleWeeks shedule;

        [TestMethod]
        public void TestEmptyConstructor()
        {
            shedule = new SheduleWeeks();
            Assert.IsNotNull(shedule);
        }

        [TestMethod]
        public void TestNonEmptyConstructor()
        {
            shedule = getSheduleWeeks();
            Assert.IsNotNull(shedule);
        }

        [TestMethod]
        public void TestGetDaysEmptyShedule()
        {
            shedule = new SheduleWeeks();
            Assert.AreEqual(0, shedule.Days.Count);
        }

        [TestMethod]
        public void TestGetDaysShedule()
        {
            shedule = getSheduleWeeks();
            Assert.AreEqual(12, shedule.Days.Count);
        }

        [TestMethod]
        public void TestGetFirstDaySemEmptyShedule()
        {
            shedule = new SheduleWeeks();
            Assert.AreEqual(new DateTime(), shedule.FirstDaySem);
        }

        [TestMethod]
        public void TestGetFirstDaySem()
        {
            shedule = getSheduleWeeks();
            Assert.AreEqual(new DateTime(2016, 9, 1), shedule.FirstDaySem);
        }

        [TestMethod]
        public void TestGetRoomsInEmptyShedule()
        {
            shedule = new SheduleWeeks();
            Assert.AreEqual(0, shedule.Rooms.Count);
        }

        [TestMethod]
        public void TestGetRooms()
        {
            SettingShedule setting = getSetting();
            List<SheduleRoom> rooms = getRooms();
            shedule = new SheduleWeeks(rooms, setting, new DateTime(2016, 9, 1));

            CollectionAssert.AreEqual(rooms, shedule.Rooms);
        }

        [TestMethod]
        public void TestGetLessonsEmptyShedule()
        {
            shedule = new SheduleWeeks();
            Assert.AreEqual(0, shedule.Lessons.ToList().Count);
        }

        [TestMethod]
        public void TestGetLessons()
        {
            shedule = getSheduleWeeks();
            // shedule.Lessons.Count = количество учебных недель * количество учебных дней в неделю*
            //                         количество учебных часов в неделю * количество аудиторий
            Assert.AreEqual(384, shedule.Lessons.ToList().Count);
        }

        // Todo: написать сюда нормальный тест
        [TestMethod]
        public void TestGetSettingEmptyShedule()
        {
            shedule = new SheduleWeeks();
            Assert.IsNotNull(shedule.Setting);
        }

        [TestMethod]
        public void TestGetSetting()
        {
            SettingShedule setting = getSetting();
            List<SheduleRoom> rooms = getRooms();
            shedule = new SheduleWeeks(rooms, setting, new DateTime(2016, 9, 1));

            Assert.AreEqual(setting, shedule.Setting);
        }

        [TestMethod]
        public void TestGetLessonEmptyShedule()
        {
            SheduleTime time = new SheduleTime();
            shedule = new SheduleWeeks();
            Assert.IsNull(shedule.GetLesson(time, "228"));
        }

        [TestMethod]
        public void TestGetNonExistentLesson()
        {
            SheduleTime time = new SheduleTime();
            shedule = getSheduleWeeks();
            Assert.IsNull(shedule.GetLesson(time, "228"));
        }

        [TestMethod]
        public void TestGetLesson()
        {
            shedule = getSheduleWeeks();

            SheduleTime time;
            SheduleLesson lesson;

            for(int week = 1; week <= 2; week++)
            {
                for(int day = 1; day <= 6; day++)
                {
                    for(int hour = 1; hour <= 4; hour++)
                    {
                        time = new SheduleTime((Week)week, (Day)day, hour);
                        lesson = shedule.GetLesson(time, "В-1301");
                        Assert.AreEqual((Day)day, lesson.Day);
                        Assert.AreEqual((Week)week, lesson.Week);
                        Assert.AreEqual("В-1301", lesson.Room);
                        Assert.AreEqual(hour, lesson.Hour);
                    }
                }
            }
        }

        [TestMethod]
        public void TestFindLessonInEmptyList()
        {
            List<SheduleLesson> lessons = new List<SheduleLesson>();
            shedule = new SheduleWeeks();
            Assert.IsNull(shedule.FindLessonInList(lessons, new SheduleTime(Week.FirstWeek, Day.Monday, 1)));
        }

        [TestMethod]
        public void TestFindLessonInList()
        {
            List<DateTime> dates = new List<DateTime> { new DateTime(2016, 9, 1), new DateTime(2016, 9, 15) };

            List<SheduleLesson> lessons = new List<SheduleLesson>
            {
                new SheduleLesson(new SheduleTime(Week.FirstWeek, Day.Monday, 1), "В-1301" , dates),
                new SheduleLesson(new SheduleTime(Week.FirstWeek, Day.Monday, 2), "В-1301", dates)
            };

            shedule = getSheduleWeeks();
            SheduleLesson lesson = shedule.FindLessonInList(lessons, new SheduleTime(Week.FirstWeek, Day.Monday, 1));

            Assert.AreEqual(Day.Monday, lesson.Day);
            Assert.AreEqual(Week.FirstWeek, lesson.Week);
            Assert.AreEqual("В-1301", lesson.Room);
            Assert.AreEqual(1, lesson.Hour);
        }

        [TestMethod]
        public void TestFindNonExistentLessonInList()
        {
            List<DateTime> dates = new List<DateTime> { new DateTime(2016, 9, 1), new DateTime(2016, 9, 15) };

            List<SheduleLesson> lessons = new List<SheduleLesson>
            {
                new SheduleLesson(new SheduleTime(Week.FirstWeek, Day.Monday, 1), "В-1301" , dates),
                new SheduleLesson(new SheduleTime(Week.FirstWeek, Day.Monday, 2), "В-1301", dates)
            };

            shedule = getSheduleWeeks();
            SheduleLesson lesson = shedule.FindLessonInList(lessons, new SheduleTime(Week.FirstWeek, Day.Sunday, 1));

            Assert.IsNull(lesson);
        }

        [TestMethod]
        public void TestGetDayEmptyList()
        {
            List<SheduleLesson> lessons = new List<SheduleLesson>();
            shedule = new SheduleWeeks();
            Assert.IsNull(shedule.GetDay(Week.FirstWeek, Day.Monday));
        }

        [TestMethod]
        public void TestGetDay()
        {
            shedule = getSheduleWeeks();
            SheduleDay day = shedule.GetDay(new SheduleTime(Week.FirstWeek, Day.Monday, 1));

            Assert.AreEqual(Week.FirstWeek, day.Week);
            Assert.AreEqual(Day.Monday, day.Day);
        }

        [TestMethod]
        public void TestGetNonExistentDay()
        {
            shedule = getSheduleWeeks();
            SheduleDay day = shedule.GetDay(new SheduleTime(Week.FourWeek, Day.Sunday, 7));
            Assert.IsNull(day);
        }

        [TestMethod]
        public void TesGetLessonsOfDayEmptyShedule()
        {
            SettingShedule setting = getSetting();
            List<SheduleRoom> rooms = getRooms();
            shedule = new SheduleWeeks();

            SheduleDay day = new SheduleDay(Week.SecondWeek, Day.Monday, rooms, setting, new DateTime(2016, 9, 1));
            IEnumerable<SheduleLesson> lessons = shedule.GetLessonsOfDay(day);
            Assert.IsNull(lessons);
        }


        [TestMethod]
        public void TestGetLessonsOfDay()
        {
            SettingShedule setting = getSetting();
            List<SheduleRoom> rooms = getRooms();
            shedule = new SheduleWeeks(rooms, setting, new DateTime(2016, 9, 1));

            SheduleDay day = new SheduleDay(Week.SecondWeek, Day.Monday, rooms, setting, new DateTime(2016, 9, 1));
            IEnumerable<SheduleLesson> lessons = shedule.GetLessonsOfDay(day);
            foreach(var lesson in lessons)
            {
                Assert.AreEqual(Week.SecondWeek, lesson.Week);
                Assert.AreEqual(Day.Monday, lesson.Day);
            }
        }

        [TestMethod]
        public void TestGetLessonsOfNonExistentDay()
        {
            SettingShedule setting = getSetting();
            List<SheduleRoom> rooms = getRooms();
            shedule = new SheduleWeeks(rooms, setting, new DateTime(2016, 9, 1));

            SheduleDay day = new SheduleDay(Week.FourWeek, Day.Sunday, rooms, setting, new DateTime(2016, 9, 1));
            IEnumerable<SheduleLesson> lessons = shedule.GetLessonsOfDay(day);
            Assert.IsNull(lessons);
        }

        [TestMethod]
        public void TestGetTeachersNamesEmptyShedule()
        {
            shedule = getSheduleWeeks();
            Assert.AreEqual(0, shedule.TeachersNames.Count());
        }

        [TestMethod]
        public void TestGetTeachersNamesEmptyLessons()
        {
            shedule = getSheduleWeeks();
            Assert.AreEqual(0, shedule.TeachersNames.Count());
        }

        [TestMethod]
        public void TestGetTeachersNames()
        {
            shedule = getSheduleWeeks();
            foreach(SheduleLesson lesson in shedule.Lessons)
            {
                lesson.Teacher = "Котов К.К.";
                lesson.Discipline = "Котоведение";
                lesson.Groups = new List<string> { "К-228" };
                lesson.Room = "1488";
            }
            shedule.Lessons.Last().Teacher= "Псов П.П.";
            Assert.AreEqual(2, shedule.TeachersNames.Count());
        }

        [TestMethod]
        public void TestGetGroupsNamesEmptyShedule()
        {
            shedule = getSheduleWeeks();
            Assert.AreEqual(0, shedule.GroupsNames.Count());
        }

        [TestMethod]
        public void TestGetTeachersGroupsEmptyLessons()
        {
            shedule = getSheduleWeeks();
            Assert.AreEqual(0, shedule.GroupsNames.Count());
        }

        [TestMethod]
        public void TestGetNames()
        {
            shedule = getSheduleWeeks();
            foreach(SheduleLesson lesson in shedule.Lessons)
            {
                lesson.Teacher = "Котов К.К.";
                lesson.Discipline = "Котоведение";
                lesson.Groups = new List<string> { "К-228" };
                lesson.Room = "1488";
            }
            shedule.Lessons.Last().Groups.Add("П-228");
            Assert.AreEqual(2, shedule.GroupsNames.Count());
        }

        [TestMethod]
        public void TestGetDisciplinesNamesEmptyShedule()
        {
            shedule = getSheduleWeeks();
            Assert.AreEqual(0, shedule.DisciplinesNames.Count());
        }

        [TestMethod]
        public void TestGetDisciplinesEmptyLessons()
        {
            shedule = getSheduleWeeks();
            Assert.AreEqual(0, shedule.DisciplinesNames.Count());
        }

        [TestMethod]
        public void TestGetDisciplinesNames()
        {
            shedule = getSheduleWeeks();
            foreach(SheduleLesson lesson in shedule.Lessons)
            {
                lesson.Teacher = "Котов К.К.";
                lesson.Discipline = "Котоведение";
                lesson.Groups = new List<string> { "К-228" };
                lesson.Room = "1488";
            }
            shedule.Lessons.Last().Discipline = "Псоведение";
            Assert.AreEqual(2, shedule.DisciplinesNames.Count());
        }

        [TestMethod]
        public void TestGetRoomsNamesEmptyShedule()
        {
            shedule = getSheduleWeeks();
            Assert.AreEqual(0, shedule.RoomsNames.Count());
        }

        [TestMethod]
        public void TestGetRoomsEmptyLessons()
        {
            shedule = getSheduleWeeks();
            Assert.AreEqual(0, shedule.RoomsNames.Count());
        }

        [TestMethod]
        public void TestGetRoomsNames()
        {
            shedule = getSheduleWeeks();
            foreach (SheduleLesson lesson in shedule.Lessons)
            {
                lesson.Teacher = "Котов К.К.";
                lesson.Discipline = "Котоведение";
                lesson.Groups = new List<string> {"К-228"};
                lesson.Room = "1488";
            }
            shedule.Lessons.Last().Room = "1489";
            Assert.AreEqual(2, shedule.RoomsNames.Count());
        }

        [TestMethod]
        public void TestGetLessonsTeacherEmptyShedule()
        {
            shedule = new SheduleWeeks();
            Assert.AreEqual(0, shedule.GetLessonsTeacher("Котов К.К.").Count());
        }

        [TestMethod]
        public void TestGetEmptyLessonsTeacher()
        {
            shedule = getSheduleWeeks();
            Assert.AreEqual(0, shedule.GetLessonsTeacher("Котов К.К.").Count());
        }

        [TestMethod]
        public void TestGetLessonsNotExistTeacher()
        {
            shedule = getSheduleWeeks();
            foreach(SheduleLesson lesson in shedule.Lessons)
            {
                lesson.Teacher = "Котов К.К.";
                lesson.Discipline = "Котоведение";
                lesson.Groups = new List<string> { "К-228" };
                lesson.Room = "1488";
            }
            Assert.AreEqual(0, shedule.GetLessonsTeacher("Бобров Б.Б.").Count());
        }

        [TestMethod]
        public void TestGetLessonsTeacher()
        {
            shedule = getSheduleWeeks();
            foreach(SheduleLesson lesson in shedule.Lessons)
            {
                lesson.Teacher = "Котов К.К.";
                lesson.Discipline = "Котоведение";
                lesson.Groups = new List<string> { "К-228" };
                lesson.Room = "1488";
            }
            shedule.Lessons.Last().Teacher = "Псов П.П.";

            foreach(SheduleLesson lesson in shedule.GetLessonsTeacher("Котов К.К."))
            {
                Assert.AreEqual(lesson.Teacher, "Котов К.К.");
            }
            Assert.AreEqual(383, shedule.GetLessonsTeacher("Котов К.К.").Count());
        }

        [TestMethod]
        public void TestGetLessonsGroupEmptyShedule()
        {
            shedule = new SheduleWeeks();
            Assert.AreEqual(0, shedule.GetLessonsTeacher("Котов К.К.").Count());
        }

        [TestMethod]
        public void TestGetEmptyLessonsGroup()
        {
            shedule = getSheduleWeeks();
            Assert.AreEqual(0, shedule.GetLessonsGroup("К-228").Count());
        }

        [TestMethod]
        public void TestGetLessonsNotExistGroup()
        {
            shedule = getSheduleWeeks();
            foreach(SheduleLesson lesson in shedule.Lessons)
            {
                lesson.Teacher = "Котов К.К.";
                lesson.Discipline = "Котоведение";
                lesson.Groups = new List<string> { "К-228" };
                lesson.Room = "1488";
            }
            Assert.AreEqual(0, shedule.GetLessonsGroup("П-228.").Count());
        }

        [TestMethod]
        public void TestGetLessonsGroup()
        {
            shedule = getSheduleWeeks();
            foreach(SheduleLesson lesson in shedule.Lessons)
            {
                lesson.Teacher = "Котов К.К.";
                lesson.Discipline = "Котоведение";
                lesson.Groups = new List<string> { "К-228" };
                lesson.Room = "1488";
            }
            shedule.Lessons.Last().Groups = new List<string> { "П-228" };

            foreach(SheduleLesson lesson in shedule.GetLessonsGroup("К-228"))
            {
                CollectionAssert.Contains(lesson.Groups, "К-228");
            }
            Assert.AreEqual(383, shedule.GetLessonsGroup("К-228").Count());
        }

        [TestMethod]
        public void TestGetLessonsDisciplineEmptyShedule()
        {
            shedule = new SheduleWeeks();
            Assert.AreEqual(0, shedule.GetLessonsDiscipline("Котоведение").Count());
        }

        [TestMethod]
        public void TestGetEmptyLessonsDiscipline()
        {
            shedule = getSheduleWeeks();
            Assert.AreEqual(0, shedule.GetLessonsDiscipline("Котоведение").Count());
        }

        [TestMethod]
        public void TestGetLessonsNotExistDiscipline()
        {
            shedule = getSheduleWeeks();
            foreach(SheduleLesson lesson in shedule.Lessons)
            {
                lesson.Teacher = "Котов К.К.";
                lesson.Discipline = "Котоведение";
                lesson.Groups = new List<string> { "К-228" };
                lesson.Room = "1488";
            }
            Assert.AreEqual(0, shedule.GetLessonsDiscipline("Псоведение").Count());
        }

        [TestMethod]
        public void TestGetLessonDiscipline()
        {
            shedule = getSheduleWeeks();
            foreach(SheduleLesson lesson in shedule.Lessons)
            {
                lesson.Teacher = "Котов К.К.";
                lesson.Discipline = "Котоведение";
                lesson.Groups = new List<string> { "К-228" };
                lesson.Room = "1488";
            }
            shedule.Lessons.Last().Discipline = "Псоведение";

            foreach(SheduleLesson lesson in shedule.GetLessonsDiscipline("Котоведение"))
            {
               Assert.AreEqual("Котоведение", lesson.Discipline);
            }
            Assert.AreEqual(383, shedule.GetLessonsDiscipline("Котоведение").Count());
        }

        [TestMethod]
        public void TestGetLessonsRoomEmptyShedule()
        {
            shedule = new SheduleWeeks();
            Assert.AreEqual(0, shedule.GetLessonsRoom("1488").Count());
        }

        [TestMethod]
        public void TestGetEmptyLessonsRoom()
        {
            shedule = getSheduleWeeks();
            Assert.AreEqual(0, shedule.GetLessonsRoom("1488").Count());
        }

        [TestMethod]
        public void TestGetLessonsNotExistRoom()
        {
            shedule = getSheduleWeeks();
            foreach(SheduleLesson lesson in shedule.Lessons)
            {
                lesson.Teacher = "Котов К.К.";
                lesson.Discipline = "Котоведение";
                lesson.Groups = new List<string> { "К-228" };
                lesson.Room = "1488";
            }
            Assert.AreEqual(0, shedule.GetLessonsRoom("1489").Count());
        }

        [TestMethod]
        public void TestGetLessonRoom()
        {
            shedule = getSheduleWeeks();
            foreach(SheduleLesson lesson in shedule.Lessons)
            {
                lesson.Teacher = "Котов К.К.";
                lesson.Discipline = "Котоведение";
                lesson.Groups = new List<string> { "К-228" };
                lesson.Room = "1488";
            }
            shedule.Lessons.Last().Room = "1489";

            foreach(SheduleLesson lesson in shedule.GetLessonsRoom("1488"))
            {
                Assert.AreEqual("1488", lesson.Room);
            }
            Assert.AreEqual(383, shedule.GetLessonsRoom("1488").Count());
        }

        [TestMethod]
        public void TestGetSortedWeeksByCountLessonsEmtyShedule()
        {
            shedule = new SheduleWeeks();

            Assert.AreEqual(0, shedule.GetSortedWeeksByCountLessons().Count());
        }

        [TestMethod]
        public void TestGetSortedWeeksByCountLessons()
        {
            shedule = getSheduleWeeks();

            Assert.AreEqual(shedule.Setting.CountWeeksShedule, shedule.GetSortedWeeksByCountLessons().Count());
        }
    }
}
