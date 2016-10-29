using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyShedule
{
    public class SheduleRoom
    {
        public SheduleRoom()
        {
            Name = String.Empty;
            DisciplinesLection = new List<string>();
            DisciplinesLabWork = new List<string>();
            DisciplinesPractice = new List<string>();
            Lection = true;
            LabWork = true;
            Practice = true;
        }

        public SheduleRoom(string name,
                            List<string> disciplineLection,
                            List<string> disciplineLabWork,
                            List<string> disciplinePractice,
                            bool lection,
                            bool labWork,
                            bool practice)
        {
            Name = name;
            DisciplinesLection = disciplineLection;
            DisciplinesLabWork = disciplineLabWork;
            DisciplinesPractice = disciplinePractice;
            Lection = lection;
            LabWork = labWork;
            Practice = practice;
        }

        public string Name { get; set; }
        public bool Lection { get; set; }
        public List<string> DisciplinesLection { get; set; }
        public bool LabWork { get; set; }
        public List<string> DisciplinesLabWork { get; set; }
        public bool Practice { get; set; }
        public List<string> DisciplinesPractice { get; set; }

        public List<LessonType> PossibleLessonsTypes
        {
            get
            {
                List<LessonType> possibleLessonsTypes = new List<LessonType>();

                if (Lection) possibleLessonsTypes.Add(LessonType.Lection);
                if (Practice)possibleLessonsTypes.Add(LessonType.Practice);
                if (LabWork)possibleLessonsTypes.Add(LessonType.Labwork);

                return possibleLessonsTypes;
            }
        }

        public bool CanHoldLesson(LessonType type)
        {
            return PossibleLessonsTypes.Contains(type);
        }
    }
}
