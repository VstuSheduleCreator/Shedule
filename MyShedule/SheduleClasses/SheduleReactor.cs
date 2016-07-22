﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace MyShedule
{
    public interface ISheduleGenerator
    {
       SheduleWeeks Generate();
    }

    public class SheduleGenerator : ISheduleGenerator
    {
        #region Generator Fields

        /// <summary> генерируемое расписание</summary>
        public SheduleWeeks Shedule;

        /// <summary> адаптер к учебной нагрузке</summary>
        public EducationLoadAdapter LoadItemsAdapter;

        /// <summary> занятости групп, аудиторий и преподавателей </summary>
        public Employments Employments = new Employments();
        
        /// <summary> список аудиторий</summary>
        public List<SheduleRoom> Rooms = new List<SheduleRoom>();

        /// <summary> результаты распределения нагрузки </summary>
        public List<DistributeResult> Results = new List<DistributeResult>();

        // разделить на под элементы с кол-вом пар не больше двух
        public readonly decimal Step = 8;

        #endregion

        #region Generator Construcrors

        public SheduleGenerator(EducationLoadAdapter Adapter, List<SheduleRoom> rooms, 
            SettingShedule Setting, DateTime firstDaySem, Employments employments)
        {
            Rooms = rooms;
            Shedule = new SheduleWeeks(Rooms, Setting, firstDaySem);
            Employments = employments;
            Shedule.Employments = Employments;
            LoadItemsAdapter = Adapter;
            Shedule.Employments.ClearGeneratedLessons();
        }

        #endregion

        //получить элменты нагрузки для составления расписания
        public virtual IEnumerable<LoadItem> LoadItemsDivided { get { return GetLoadItemsDivided(); } }

        protected virtual IEnumerable<LoadItem> GetLoadItemsDivided()
        {
            IEnumerable<LoadItem> divided = DivideLoadOnSubItems(LoadItemsAdapter.Items);
            return SortLoadItemsOnRegularIntervals(divided.ToList());
        }

        //разделить элементы нагрузки на подэлементы с количеством часов не больше 8
        // 8 часов = 2 недели (1-я и 3-я или 2-я и 4-я) * 2 пары сдвоенные * 2 часа одна пара
        protected virtual IEnumerable<LoadItem> DivideLoadOnSubItems(IEnumerable<LoadItem> LoadItems)
        {
            foreach (LoadItem item in LoadItems.OrderByDescending(x => x.HoursByMonth).ToList())
            {
                decimal LoadCounter = item.HoursByMonth;

                while (LoadCounter > 0)
                {
                    LoadItem subitem = item.Copy();
                    subitem.DivideHours = LoadCounter - Step > 0 ? Step : LoadCounter;
                    LoadCounter = LoadCounter - Step > 0 ? LoadCounter - Step : 0;

                    yield return subitem;
                }
            }
        }

        //упорядочить элементы нагрузки таким образом чтобы дисциплины распределялись равномерно по неделям
        protected virtual IEnumerable<LoadItem> SortLoadItemsOnRegularIntervals(IEnumerable<LoadItem> LoadItems)
        {
            List<LoadItem> items = LoadItems.ToList();
            Array Disciplines = items.OrderByDescending(x => x.HoursByMonth).Select(x => x.Discipline).Distinct().ToArray();

            while (items.Count > 0)
            {
                foreach (string Discipline in Disciplines)
                {
                    List<LoadItem> query = items.Where(x => x.Discipline == Discipline).ToList();
                    if (query.Count > 0)
                    {
                        items.Remove(query.First());
                        yield return query.First();
                    }
                }
            }
        }

        //отсортировать недели по наименьшей загруженности
        private IEnumerable<WeekInfo> GetSortedWeeksByCountLessons(SheduleWeeks shedule)
        {
            return (from day in shedule.Days
                    group day by day.Week into grp
                    select new WeekInfo(grp.Key, grp.Sum(d => d.CountLessons))).OrderBy(day => day.CountLessons);
        }

        protected virtual IEnumerable<int> GetSortedDays()
        {
            List<int> days = new List<int>();
           
            int Monday = 1;
            int Wednesday = 3;
            int Saturday = 6;
            //в первую очередь проставляем занятия в субботу
            days.Add(Saturday);
            //во вторую очередь проставляем в понедельник
            days.Add(Monday);

            for (int day = 1; day <= this.Shedule.Setting.CountDaysEducationWeek; day++)
                if (day != Wednesday && day != Saturday && day != Monday)
                    days.Add(day);

            //в последнюю очередь проставляем в среду. пусть студенты отдохнут посреди недели
            days.Add(Wednesday);

            return days;
        }

        /// <summary> Сгенерировать расписание </summary>
        /// <returns> Созданное расписание</returns>
        public SheduleWeeks Generate()
        {
            //убрать занятости проставленные генератором расписания
            Employments.ClearGeneratedLessons();
            //распределить все элементы нагрузки
            foreach (LoadItem item in LoadItemsDivided)
            {
                decimal Load = item.DivideHours;
                //расписание строим с двух попыток, на первой попытке ставим сдвоенные занятия везде где возможно
                //на второй попытке расставляем занятия в свободные ячейки как получится
                for (int Attempt = 1; Attempt <= 2 && Load > 0; Attempt++)
                {
                    //отсортировать недели по количеству занятий
                    WeekInfo[] sortedWeeks = GetSortedWeeksByCountLessons(Shedule).ToArray();
                    for (int counterWeek = 0; counterWeek < sortedWeeks.Length && Load > 0; counterWeek++)
                    {
                        int[] sortedDays = GetSortedDays().ToArray();
                        //цикл по всем дням расписания, дни = кол-во_учебных_дней * кол-во_недель_в_расписании 
                        for (int counterDay = 0; counterDay < sortedDays.Length && Load > 0; counterDay++)
                        {
                            SheduleDay Day = Shedule.GetDay(sortedWeeks[counterWeek].Week, (Day)sortedDays[counterDay]);
                            //обработать день в расписании
                            Load = GoToHoursShedule(item, Load, Day, Attempt);
                        }
                    }
                }
                //сохранить результат распределения элемента нагрузки
                SaveResultDistribute(Load, item);
            }

            Results = Results.OrderBy(x => x.Complete).ToList();
            return Shedule;
        }

        protected virtual void SaveResultDistribute(decimal Load, LoadItem item)
        {
            DistributeResult result = (Load > 0) ? new DistributeResult(false, GetItemInfo(item), "Не известно") :
                    new DistributeResult(true, GetItemInfo(item), String.Empty);
            this.Results.Add(result);
        }

        protected virtual string GetItemInfo(LoadItem item)
        {
            string groups = String.Empty;
            foreach(string group in item.Groups)
                groups += item.Groups.Last() == group ? group : group + "/";
            return String.Format("{0}, {1}, {2}", item.Teacher, item.Discipline, groups);
        }

        protected decimal GoToHoursShedule(LoadItem item, decimal Load, SheduleDay dayShedule, int Attempt)
        {
            //можно ли еще добалять пары в этот день или это день для группы заполнен
            if (dayShedule.LimitLessonsNotExceeded(item.Groups, dayShedule.Week, dayShedule.Day))
            {
                List<Position> freePositions = new List<Position>();
                //цикл по доступным часам для этого дня (в будни и выходные часы для проставления пар разные)
                for (int Hour = dayShedule.EarlierPossibleHour; Hour <= dayShedule.LastPossibleHour; Hour++)
                {
                    SheduleTime Time = new SheduleTime(dayShedule.Week, dayShedule.Day, Hour);
                    //выбираем аудиторию
                    SheduleRoom Room = FindRoom(item, Time);
                    if (Room != null)
                        freePositions.Add(new Position(Room, Time));
                }
                // добавляем занятия
                if(freePositions.Count > 0)
                   Load = PutLessonsOnFreePositions(freePositions, Load, Attempt, item);
            }
            return Load;
        }

        private decimal PutTwoLessonOnTwoWeek(List<Position> freePositions, decimal Load, LoadItem item)
        {
            for (int counter = 0; counter < freePositions.Count - 1 && Load > 0; counter++)
            {
                Position curr = freePositions[counter];
                Position next = freePositions[counter + 1];

                if (curr.Time.WeekNumber > 2 || next.Time.WeekNumber > 2)
                    continue;

                SheduleTime CurrAfterTwoWeek = GetTimeAfterTwoWeek(curr.Time);
                SheduleTime NextAfterTwoWeek = GetTimeAfterTwoWeek(next.Time);

                SheduleDay currDay = Shedule.GetDay(curr.Time);
                SheduleDay nextDay = Shedule.GetDay(CurrAfterTwoWeek);

                if (!IsHoursNear(curr.Time, next.Time) || !currDay.LimitLessonsNotExceeded(item.Groups, currDay.Week, currDay.Day, 2) ||
                    !nextDay.LimitLessonsNotExceeded(item.Groups, CurrAfterTwoWeek.Week, CurrAfterTwoWeek.Day, 2))
                    continue;

                if (CanPutToPosition(curr.Time, curr.Room, item) && CanPutToPosition(next.Time, next.Room, item) &&
                    CanPutToPosition(CurrAfterTwoWeek, curr.Room, item) && CanPutToPosition(NextAfterTwoWeek, next.Room, item))
                {
                    Load = PutLesson(item, curr.Time, curr.Room, Load);
                    Load = PutLesson(item, next.Time, next.Room, Load);
                    Load = PutLesson(item, CurrAfterTwoWeek, curr.Room, Load);
                    Load = PutLesson(item, NextAfterTwoWeek, next.Room, Load);
                    counter++;
                    return Load;
                }
            }

            return Load;
        }

        private bool IsHoursNear(SheduleTime time1, SheduleTime time2) {
            return IsHoursNear(time1.Hour, time2.Hour);
        }
        private bool IsHoursNear(int hour1, int hour2) {
            return hour1 + 1 == hour2;
        }

        private decimal PutTwoLessonOnWeek(List<Position> Adding, decimal Load, LoadItem item)
        {
            for (int counter = 0; counter < Adding.Count - 1 && Load > 0; counter++)
            {
                Position curr = Adding[counter];
                Position next = Adding[counter + 1];

                SheduleDay day = Shedule.GetDay(curr.Time);
                if(!IsHoursNear(curr.Time, next.Time) || !day.LimitLessonsNotExceeded(item.Groups, day.Week, day.Day, 2))
                  continue;

                if (CanPutToPosition(curr.Time, curr.Room, item) && CanPutToPosition(next.Time, next.Room, item))
                {
                    Load = PutLesson(item, curr.Time, curr.Room, Load);
                    Load = PutLesson(item, next.Time, next.Room, Load);
                }
            }

            return Load;
        }

        protected virtual decimal PutLessonsOnFreePositions(IEnumerable<Position> freePositions,
            decimal Load, int Attempt, LoadItem item)
        {
            switch (Attempt)
            {
                //первый проход генератора 
                case 1:
                    return Load / Step == 1 ? PutTwoLessonOnTwoWeek(freePositions.ToList(), Load, item) :
                        PutTwoLessonOnWeek(freePositions.ToList(), Load, item);
                //второй проход дорасставить как получится и куда получится
                case 2:
                    return PutOneLessonOnOneOrTwoWeek(freePositions.ToList(), Load, item);
                default: return Load;
            }
        }

        protected virtual bool CanPutToPosition(SheduleTime time, SheduleRoom room, LoadItem item)
        {
            SheduleDay day = Shedule.GetDay(time.Week, time.Day);

            return  time.WeekNumber <= 4 && time.WeekNumber >= 1 && day != null &&
                    day.LimitLessonsNotExceeded(item.Groups, time.Week, time.Day, 1) &&
                    Employments.IsHourFree(item.Teacher, item.Groups, room.Name, time);
        }

        protected virtual decimal PutOneLessonOnOneOrTwoWeek(List<Position> freePositions, decimal Load, LoadItem item)
        {
            foreach(Position position in freePositions)
            {
                SheduleDay day = Shedule.GetDay(position.Time);
                if (day.LimitLessonsNotExceeded(item.Groups, day.Week, day.Day, 1))
                {
                    Load = PutLesson(item, position.Time, position.Room, Load);
                    if (Load > 0 && CanPutToPosition(GetTimeAfterTwoWeek(position.Time), position.Room, item))
                        Load = PutLesson(item, GetTimeAfterTwoWeek(position.Time), position.Room, Load);
                }
                if (Load == 0)
                    break;
            }
            return Load;
        }

        private SheduleTime GetTimeAfterTwoWeek(SheduleTime now)
        {
            SheduleTime AfterTwoWeek = now.Copy();
            AfterTwoWeek.Week = (Week)(AfterTwoWeek.WeekNumber + 2);
            return AfterTwoWeek;
        }

        protected virtual decimal PutLesson(LoadItem item, SheduleTime time, SheduleRoom room, decimal Load)
        {
            //получить занятие из расписания
            SheduleLesson Lesson = Shedule.GetLesson(time, room.Name);
            //задать параметры занятию
            Lesson.UpdateFields(item.Teacher, item.Discipline, item.Groups, item.LessonType);
            //проставить занятости
            Employments.Add(item.Teacher, item.Groups, room.Name, time, ReasonEmployment.GeneratorPutLesson);
            //уменьшить нагрузку на два академичаских часа или на одну пару
            return Load - 2;
        }

        
        protected virtual SheduleRoom FindRoom(LoadItem item, SheduleTime time)
        {
            //проверяем не привязан ли этот предмет к определенной аудитории
            var query = GetRoomsBindingDiscipline(item.LessonType, item.Discipline).ToList();
            //если предмет не привязан то ищем по всем аудиториям, если же привязан то только по отобранным
            List<SheduleRoom> rooms = (query.Count > 0) ? query : Rooms; 
            //выбираем аудиторию, ищем пока не найдется подходящая не занятая
            foreach (SheduleRoom room in rooms) {
                //можно ли в этой аудитории проводить данный вид занятия
                if (!PossibleAppointLessonRoom(room, item.LessonType))
                    continue; // если нельзя то переходим к следующей аудитории
                //проверяем накладки по аудитоиям, группам и преподам
                if (Employments.IsHourFree(item.Teacher, item.Groups, room.Name, time))
                    return room; //больше не ищем аудиторию
            }
            return null;
        }

        //Получить аудитории привязанные к определенному предмету по определенному типу занятия
        private IEnumerable<SheduleRoom> GetRoomsBindingDiscipline(LessonType type, string discipline)
        {
            return Rooms.Select(room => room).Where(room =>
            (type == LessonType.Lection && room.DisciplinesLection.Where(disc => disc == discipline).Count() > 0) ||
            (type == LessonType.Labwork && room.DisciplinesLabWork.Where(disc => disc == discipline).Count() > 0) ||
            (type == LessonType.Practice && room.DisciplinesPractice.Where(disc => disc == discipline).Count() > 0));
        }

        //Можно ли проводить данный вид занятия в этой аудитории
        private bool PossibleAppointLessonRoom(SheduleRoom room, LessonType type){
            return type == LessonType.Lection ? room.Lection : type == LessonType.Labwork ? 
                room.LabWork : type == LessonType.Practice ? room.Practice : true;
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

    internal struct WeekInfo
    {
        public WeekInfo(Week week, int countLessons) {
            Week = week; CountLessons = countLessons;
        }
        public int WeekNumber { get { return (int)Week; } }
        public Week Week;
        public int CountLessons;
    }

    public struct Position
    {
        public Position(SheduleRoom room, SheduleTime time)
        {
            Room = room;
            Time = time;
        }

        public SheduleRoom Room;
        public SheduleTime Time;
    }
}
