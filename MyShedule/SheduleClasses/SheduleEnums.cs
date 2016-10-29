namespace MyShedule
{
    /// <summary>
    /// Перечесление неделя в расписании 
    /// </summary>
    public enum Week
    {
        Another = 0,
        FirstWeek = 1,
        SecondWeek = 2,
        TreeWeek = 3,
        FourWeek = 4
    }

    /// <summary>
    /// Перечесление день недели в расписании
    /// </summary>
    public enum Day
    {
        Another = 0,
        Monday = 1,
        Tuesday = 2,
        Wednesday = 3,
        Thursday = 4,
        Friday = 5,
        Saturday = 6,
        Sunday = 7
    }

    /// <summary>
    /// Перечесление Форма обучения
    /// </summary>
    public enum TypeShedule
    {
        //Вечерники
        Evening,
        //Заочники
        Correspondence
    }
}