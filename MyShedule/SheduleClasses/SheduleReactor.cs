using System;
using System.Collections.Generic;
using System.Linq;

namespace MyShedule
{
    public interface ISheduleGenerator
    {
        SheduleWeeks Generate();
    }

    public class SheduleGenerator: ISheduleGenerator
    {
        #region Generator Fields

        /// <summary> генерируемое расписание</summary>
        public SheduleWeeks Shedule;

        /// <summary> адаптер к учебной нагрузке</summary>
        public EducationLoadAdapter LoadItemsAdapter;

        /// <summary> результаты распределения нагрузки </summary>
        public List<DistributeResult> Results = new List<DistributeResult>();

        // разделить на под элементы с кол-вом пар не больше двух
        public readonly decimal Step = 8;

        #endregion

        #region Generator Construcrors

        public SheduleGenerator(EducationLoadAdapter adapter, List<SheduleRoom> rooms,
            SettingShedule setting, DateTime firstDaySem, Employments employments)
        {
            Shedule = new SheduleWeeks(rooms, setting, firstDaySem);
            Shedule.Employments = employments;
            LoadItemsAdapter = adapter;
            Shedule.Employments.ClearGeneratedLessons();
        }

        #endregion

        // элементы нагрузки для составления расписания
        public virtual IEnumerable<LoadItem> LoadItemsDivided
        {
            get
            {
                return LoadItemsAdapter.DivideLoadOnSubItems(Step).SortLoadItemsOnRegularIntervals();
            }
        }

        protected virtual IEnumerable<int> GetSortedDays()
        {
            List<int> days = new List<int>();

            int Monday = 1;
            int Wednesday = 3;
            int Saturday = 6;
            //в первую очередь проставляем занятия в субботу
            if(Shedule.Setting.CountDaysEducationWeek >= 6)
                days.Add(Saturday);
            //во вторую очередь проставляем в понедельник
            days.Add(Monday);

            for(int day = 1; day <= Shedule.Setting.CountDaysEducationWeek; day++)
                if(day != Wednesday && day != Saturday && day != Monday)
                    days.Add(day);

            //в последнюю очередь проставляем в среду. пусть студенты отдохнут посреди недели
            if(Shedule.Setting.CountDaysEducationWeek >= 3)
                days.Add(Wednesday);

            return days;
        }

        private IEnumerable<SheduleDay> SortedDays
        {
            get
            {
                //отсортировать недели по количеству занятий
                WeekInfo[] sortedWeeks = Shedule.GetSortedWeeksByCountLessons().ToArray();
                int[] sortedDays = GetSortedDays().ToArray();
                for(int counterWeek = 0; counterWeek < sortedWeeks.Length; counterWeek++)
                {
                    //цикл по всем дням расписания, дни = кол-во_учебных_дней * кол-во_недель_в_расписании 
                    for(int counterDay = 0; counterDay < sortedDays.Length; counterDay++)
                    {
                        yield return Shedule.GetDay(sortedWeeks[counterWeek].Week, (Day)sortedDays[counterDay]);
                    }
                }
            }
        }

        /// <summary> Сгенерировать расписание </summary>
        /// <returns> Созданное расписание</returns>
        public SheduleWeeks Generate()
        {
            //убрать занятости проставленные генератором расписания
            Shedule.Employments.ClearGeneratedLessons();

            //распределить все элементы нагрузки
            foreach(LoadItem item in LoadItemsDivided)
            {
                decimal load = item.DivideHours;
                //расписание строим с двух попыток, на первой попытке ставим сдвоенные занятия везде где возможно
                foreach(SheduleDay day in SortedDays)
                {
                    List<Position> freePositions = Shedule.FreePositions(day, item);
                    //обработать день в расписании
                    //можно ли еще добалять пары в этот день или это день для группы заполнен
                    if(freePositions.Count > 0 && load > 0 && day.LimitLessonsNotExceeded(item.Groups, day.Week, day.Day))
                        //первый проход генератора
                        load = load / Step == 1 ? PutTwoLessonOnTwoWeek(freePositions.ToList(), load, item)
                            : PutTwoLessonOnWeek(freePositions.ToList(), load, item);
                }
                //на второй попытке расставляем занятия в свободные ячейки как получится
                foreach(SheduleDay day in SortedDays)
                {
                    //обработать день в расписании
                    //можно ли еще добалять пары в этот день или это день для группы заполнен
                    List<Position> freePositions = Shedule.FreePositions(day, item);
                    if(freePositions.Count > 0 && load > 0 && day.LimitLessonsNotExceeded(item.Groups, day.Week, day.Day))
                        //второй проход дорасставить как получится и куда получится
                        load = PutOneLessonOnOneOrTwoWeek(freePositions.ToList(), load, item);
                }
                //сохранить результат распределения элемента нагрузки
                SaveResultDistribute(load, item);
            }

            Results = Results.OrderBy(x => x.Complete).ToList();
            return Shedule;
        }

        protected virtual void SaveResultDistribute(decimal load, LoadItem item)
        {
            DistributeResult result = (load > 0) ? new DistributeResult(false, item.Info, "Не известно") :
                    new DistributeResult(true, item.Info, String.Empty);
            Results.Add(result);
        }

        private decimal PutTwoLessonOnTwoWeek(List<Position> freePositions, decimal load, LoadItem item)
        {
            for(int counter = 0; counter < freePositions.Count - 1 && load > 0; counter++)
            {
                Position curr = freePositions[counter];
                Position next = freePositions[counter + 1];

                if(curr.Time.WeekNumber > 2 || next.Time.WeekNumber > 2)
                    continue;

                SheduleTime currAfterTwoWeek = GetTimeAfterTwoWeek(curr.Time);
                SheduleTime nextAfterTwoWeek = GetTimeAfterTwoWeek(next.Time);

                SheduleDay currDay = Shedule.GetDay(curr.Time);
                SheduleDay nextDay = Shedule.GetDay(currAfterTwoWeek);

                //if(nextDay == null)
                //    continue;
                if(!IsHoursNear(curr.Time, next.Time) || !currDay.LimitLessonsNotExceeded(item.Groups, currDay.Week, currDay.Day, 2) ||
                    !nextDay.LimitLessonsNotExceeded(item.Groups, currAfterTwoWeek.Week, currAfterTwoWeek.Day, 2))
                    continue;

                if(Shedule.CanPutToPosition(curr.Time, curr.Room, item) && Shedule.CanPutToPosition(next.Time, next.Room, item) &&
                    Shedule.CanPutToPosition(currAfterTwoWeek, curr.Room, item) && Shedule.CanPutToPosition(nextAfterTwoWeek, next.Room, item))
                {
                    load = PutLesson(item, curr.Time, curr.Room, load);
                    load = PutLesson(item, currAfterTwoWeek, curr.Room, load);
                    load = PutLesson(item, next.Time, next.Room, load);
                    load = PutLesson(item, nextAfterTwoWeek, next.Room, load);
                    return load;
                }
            }

            return load;
        }

        private bool IsHoursNear(SheduleTime time1, SheduleTime time2)
        {
            return IsHoursNear(time1.Hour, time2.Hour);
        }
        private bool IsHoursNear(int hour1, int hour2)
        {
            return hour1 + 1 == hour2;
        }

        private decimal PutTwoLessonOnWeek(List<Position> adding, decimal load, LoadItem item)
        {
            for(int counter = 0; counter < adding.Count - 1 && load > 0; counter++)
            {
                Position curr = adding[counter];
                Position next = adding[counter + 1];

                SheduleDay day = Shedule.GetDay(curr.Time);
                if(!IsHoursNear(curr.Time, next.Time) || !day.LimitLessonsNotExceeded(item.Groups, day.Week, day.Day, 2))
                    continue;

                if(Shedule.CanPutToPosition(curr.Time, curr.Room, item) && Shedule.CanPutToPosition(next.Time, next.Room, item))
                {
                    load = PutLesson(item, curr.Time, curr.Room, load);
                    load = PutLesson(item, next.Time, next.Room, load);
                }
            }

            return load;
        }


        protected virtual decimal PutOneLessonOnOneOrTwoWeek(List<Position> freePositions, decimal load, LoadItem item)
        {
            foreach(Position position in freePositions)
            {
                SheduleDay day = Shedule.GetDay(position.Time);
                if(day.LimitLessonsNotExceeded(item.Groups, day.Week, day.Day, 1) && load > 0 && Shedule.CanPutToPosition(position.Time, position.Room, item))
                {
                    load = PutLesson(item, position.Time, position.Room, load);
                    if(load > 0 && Shedule.CanPutToPosition(position.Time, position.Room, item))
                        load = PutLesson(item, position.Time, position.Room, load);
                }
                if(load == 0)
                    break;
            }
            return load;
        }

        private SheduleTime GetTimeAfterTwoWeek(SheduleTime now)
        {
            SheduleTime afterTwoWeek = now.Copy();
            afterTwoWeek.Week = (Week)(afterTwoWeek.WeekNumber + 2);
            return afterTwoWeek;
        }

        protected virtual decimal PutLesson(LoadItem item, SheduleTime time, SheduleRoom room, decimal load)
        {
            //получить занятие из расписания
            SheduleLesson lesson = Shedule.GetLesson(time, room.Name);

            //задать параметры занятию
            lesson.UpdateFields(item.Teacher, item.Discipline, item.Groups, item.LessonType);
            //проставить занятости
            Shedule.Employments.Add(item.Teacher, item.Groups, room.Name, time, ReasonEmployment.GeneratorPutLesson);
            //уменьшить нагрузку на два академичаских часа или на одну пару
            return load - 2;
        }
    }

    public class DistributeResult
    {
        public DistributeResult(bool complete, string iteminfo, string reason)
        {
            Complete = complete;
            ItemInfo = iteminfo;
            Reason = reason;
        }

        public bool Complete;
        public string ItemInfo;
        public string Reason;
    }
}
