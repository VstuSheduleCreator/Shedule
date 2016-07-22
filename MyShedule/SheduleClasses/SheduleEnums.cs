﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyShedule
{
    /// <summary>
    /// Перечесление неделя в расписании 
    /// </summary>
    public enum Week
    {
        Another = 0,
        FirstWeek = 1,
        SecondWeek,
        TreeWeek,
        FourWeek
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

    public enum TypeShedule
    {
        //Вечерники
        Evening,
        //Заочники
        Correspondence
    }

}