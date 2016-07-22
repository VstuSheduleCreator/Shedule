﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MyShedule
{
    [Serializable]
    public class Employments
    {
        public Employments()
        {
            Teachers = new EmploymentList();
            Groups = new EmploymentList();
            Rooms = new EmploymentList();
        }

        //занятость аудиторий
        public EmploymentList Rooms;
        //занятость преподавателей
        public EmploymentList Teachers;
        //занятость групп
        public EmploymentList Groups;

        //проставить занятость
        public void Add(string Teacher, List<string> groups, string Room, SheduleTime Time, ReasonEmployment Reason)
        {
            //добавить занятость по аудитории
            Rooms.Add(new Employment(Room, Time, Reason));
            //добавить занятость по преподователю
            Teachers.Add(new Employment(Teacher, Time, Reason));
            //добавить занятость по группе
            foreach (string group in groups)
                Groups.Add(new Employment(group, Time, Reason));          
        }

        public void Remove(string Teacher, List<string> groups, string Room, SheduleTime Time)
        {
            //удалить занятость по аудитории
            Rooms.Remove(new Employment(Room, Time));
            //удалить занятость по преподователю
            Teachers.Remove(new Employment(Teacher, Time));
            //удалить занятость по группе
            //если занятие у нескольких групп
            foreach (string group in groups)
              Groups.Remove(new Employment(group, Time));
        }

        public void RemoveInView(View view, Employment item)
        {
            switch (view)
            {
                case View.Teacher: Teachers.Remove(item); break;

                case View.Discipline: break; //по дисциплинам ограничений нет

                case View.Group: Groups.Remove(item); break;

                case View.Room: Rooms.Remove(item); break;
            }
        }

        public void AddInView(View view, Employment item)
        {
            switch (view)
            {
                case View.Teacher: Teachers.Add(item); break;

                case View.Discipline: break; //по дисциплинам ограничений нет

                case View.Group: Groups.Add(item); break;

                case View.Room: Rooms.Add(item); break;
            }
        }

        public void Clear()
        {
            ClearGeneratedLessons();
            ClearUserBlocked();
        }

        public void ClearGeneratedLessons()
        {
            Rooms.RemoveAll(ReasonEmployment.GeneratorPutLesson);
            Rooms.RemoveAll(ReasonEmployment.UnionLesson);
            Teachers.RemoveAll(ReasonEmployment.GeneratorPutLesson);
            Teachers.RemoveAll(ReasonEmployment.UnionLesson);
            Groups.RemoveAll(ReasonEmployment.GeneratorPutLesson);
            Groups.RemoveAll(ReasonEmployment.UnionLesson);
        }

        public void ClearUserBlocked()
        {
            Rooms.RemoveAll(ReasonEmployment.UserBlocked);
            Teachers.RemoveAll(ReasonEmployment.UserBlocked);
            Groups.RemoveAll(ReasonEmployment.UserBlocked);
        }

        //можно ли назначать занятие в этот день, в этой аудитории, для этого препода и для этой группы
        public bool IsHourFree(string Teacher, List<string> groups, string Room, SheduleTime Time)
        {
            return Teachers.IsFree(Teacher, Time) && Rooms.IsFree(Room, Time) && GroupsFree(groups, Time);
        }

        //свободна группа для проведения пары или у нее уже есть занятия, например в другом корпусе
        public bool GroupsFree(List<string> groups, SheduleTime Time)
        {
            foreach (string Group in groups)
            {
                if (Groups.ItemExist(Group, Time))
                    return false;
            }
            return true;
        }
    }

    [Serializable]
    public class Employment
    {
        public Employment()
        {
            Name = String.Empty;
            Time = null;
            Reason = ReasonEmployment.Another;
        }

        public Employment(string name, SheduleTime time)
        {
            Name = name;
            Time = time;
            Reason = ReasonEmployment.Another;
        }

        public Employment(string name, SheduleTime time, ReasonEmployment reason)
        {
            Name = name;
            Time = time;
            Reason = reason;
        }

        public string Name;


        public SheduleTime Time;


        public ReasonEmployment Reason;

    }

    [Serializable]
    [XmlInclude(typeof(Employment))]
    public class EmploymentList : IList
    {
        public Employment[] _contents = new Employment[1000];
        private int _count;

        public EmploymentList()
        {
            InitItems();
            _count = 0;
        }

        private void InitItems()
        {
            for (int i = 0; i < 1000; i++)
                _contents[i] = new Employment();
        }

        // IList Members
        public int Add(object value)
        {
            return Add(value as Employment);
        }

        public int Add(Employment e)
        {
            return Add(e.Name, e.Time, e.Reason);
        }

        public int Add(string Name, SheduleTime Time, ReasonEmployment Reason)
        {
            if (_count < _contents.Length)
            {
                Employment item = new Employment(Name, Time, Reason);
                // добавляем если этот элемент еще не добавлен
                if (!ItemExist(item))
                {
                    _contents[_count] = item;
                    _count++;

                    return (_count - 1);
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                return -1;
            }
        }

        public bool IsFree(Employment item)
        {
            return !ItemExist(item);
        }

        public bool IsFree(string Name, SheduleTime Time)
        {
            return !ItemExist(Name, Time);
        }

        public bool ItemExist(Employment item)
        {
            return ItemExist(item.Name, item.Time);
        }

        public bool ItemExist(string Name, SheduleTime Time)
        {
            return (from e in _contents where e.Name == Name && e.Time == Time select e).Count() > 0;
        }

        public Employment GetItem(Employment item)
        {
            return GetItem(item.Name, item.Time);
        }

        public Employment GetItem(string Name, SheduleTime Time)
        {
            List<Employment> items = (from e in _contents where e.Name == Name && e.Time == Time select e).ToList();
            return (items.Count > 0) ? items.First() : null;
        }

        public void Clear()
        {
            _count = 0;
        }

        public bool Contains(object value)
        {
            bool inList = false;
            for (int i = 0; i < Count; i++)
            {
                if (_contents[i] == value)
                {
                    inList = true;
                    break;
                }
            }
            return inList;
        }

        public int IndexOf(object value)
        {
            return IndexOf(value as Employment);
        }

        public int IndexOf(Employment value)
        {
            int itemIndex = -1;
            for (int i = 0; i < Count; i++)
            {
                if (_contents[i].Time == value.Time && _contents[i].Name == value.Name)
                {
                    itemIndex = i;
                    break;
                }
            }
            return itemIndex;
        }

        public void Insert(int index, object value)
        {
            Insert(index, value as object);
        }

        public void Insert(int index, Employment value)
        {
            if ((_count + 1 <= _contents.Length) && (index < Count) && (index >= 0))
            {
                // добавляем если этот элемент еще не добавлен
                if (!ItemExist(value))
                {
                    _count++;

                    for (int i = Count - 1; i > index; i--)
                    {
                        _contents[i] = _contents[i - 1];
                    }
                    _contents[index] = value;
                }
            }
        }

        public bool IsFixedSize
        {
            get
            {
                return true;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public void RemoveAll(ReasonEmployment reason)
        {
            List<Employment> removing = (from e in _contents where e.Reason == reason select e).ToList();
            foreach (Employment e in removing)
            {
                Remove(e);
            }
        }

        public void Remove(object value)
        {
            Remove(value as Employment);
        }

        public void Remove(Employment value)
        {
            RemoveAt(IndexOf(value));
        }

        public void RemoveAt(int index)
        {
            if ((index >= 0) && (index < Count))
            {
                for (int i = index; i < Count - 1; i++)
                {
                    _contents[i] = _contents[i + 1];
                }
                _count--;
                // очищаем запоследний элемент
                _contents[_count] = new Employment();
            }
        }

        public object this[int index]
        {
            get
            {
                return _contents[index];
            }
            set
            {
                _contents[index] = value as Employment;
            }
        }

        // ICollection Members

        public void CopyTo(Array array, int index)
        {
            int j = index;
            for (int i = 0; i < Count; i++)
            {
                array.SetValue(_contents[i], j);
                j++;
            }
        }

        public int Count
        {
            get
            {
                return _count;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        // Return the current instance since the underlying store is not
        // publicly available.
        public object SyncRoot
        {
            get
            {
                return this;
            }
        }

        // IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            // Refer to the IEnumerator documentation for an example of
            // implementing an enumerator.
            throw new Exception("The method or operation is not implemented.");
        }
    }



    public enum ReasonEmployment
    {
        UserBlocked = 1,
        UserPutLesson,
        GeneratorPutLesson,
        UnionLesson,
        Another
    }
}