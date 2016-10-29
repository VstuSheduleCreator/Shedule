using System;
using System.Collections.Generic;
using System.Linq;

namespace MyShedule
{
    public class EducationLoadAdapter
    {
        public EducationLoadAdapter()
        {
            Items = new List<LoadItem>();
        }

        public EducationLoadAdapter(IEnumerable<LoadItem> items)
        {
            Items = items.ToList();
        }

        public List<LoadItem> Items;

        public List<LoadItem> NonEmptyItems
        {
            get { return (from i in Items where i.NonEmpty() select i).ToList(); }
        }

        public List<string> NamesGroups
        {
            get
            {
                return (from p in NonEmptyItems from g in p.Groups orderby g select g).Distinct().ToList();
            }
        }

        public List<string> NamesTeachers
        {
            get
            {
                return (from p in NonEmptyItems select p.Teacher).Distinct().ToList();
            }
        }

        public List<string> NamesDisciplines
        {
            get
            {
                return (from p in NonEmptyItems select p.Discipline).Distinct().ToList();
            }
        }

        public void Add(LoadItem item)
        {
            Items.Add(item);
        }

        //разделить элементы нагрузки на подэлементы с количеством часов не больше 8
        // 8 часов = 2 недели (1-я и 3-я или 2-я и 4-я) * 2 пары сдвоенные * 2 часа одна пара
        public EducationLoadAdapter DivideLoadOnSubItems(decimal step=8)
        {
            EducationLoadAdapter result = new EducationLoadAdapter();

            foreach(LoadItem item in Items.OrderByDescending(x => x.HoursByMonth).ToList())
            {
                decimal loadCounter = item.HoursByMonth;

                while(loadCounter > 0)
                {
                    LoadItem subitem = item.Copy();
                    subitem.DivideHours = loadCounter - step > 0 ? step : loadCounter;
                    loadCounter -= step;
                    result.Add(subitem);
                }
            }

            return result;
        }

        //упорядочить элементы нагрузки таким образом чтобы дисциплины распределялись равномерно по неделям
        public IEnumerable<LoadItem> SortLoadItemsOnRegularIntervals()
        {
            List<LoadItem> items = Items.ToList();
            Array disciplines = items.OrderByDescending(x => x.HoursByMonth).Select(x => x.Discipline).Distinct().ToArray();

            while(items.Count > 0)
            {
                foreach(string discipline in disciplines)
                {
                    List<LoadItem> query = items.Where(x => x.Discipline == discipline).ToList();
                    if(query.Count > 0)
                    {
                        items.Remove(query.First());
                        yield return query.First();
                    }
                }
            }
        }


    }
}
