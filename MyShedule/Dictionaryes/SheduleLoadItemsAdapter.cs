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
    }
}
