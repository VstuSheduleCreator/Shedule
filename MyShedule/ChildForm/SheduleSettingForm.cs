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
    public partial class SheduleSettingForm : Form
    {
        public SheduleSettingForm()
        {
            InitializeComponent();
            this.Load += new EventHandler(SheduleSettingForm_Load);
        }

        void SheduleSettingForm_Load(object sender, EventArgs e)
        {
            this.AcceptButton = btnAccept;

            SetValuesControls();

            nudCountDayEducationalWeek.ValueChanged += new EventHandler(UpdateCountDaysShedule);
            nudCountWeeksShedule.ValueChanged += new EventHandler(UpdateCountDaysShedule);
        }

        void UpdateCountDaysShedule(object sender, EventArgs e)
        {
            nudCountDaysShedule.Value = nudCountDayEducationalWeek.Value * nudCountWeeksShedule.Value;
        }

        void SetValuesControls()
        {
            SettingsAplication stg = new SettingsAplication();
            //количество дней в учебной неделе, по умолчанию расписание семи-дневное
            //максимальное значение 7, если необходимо больше следует добавить поле в перечисление Day.. хотя вряд ли есть недели где больше 7 дней :)
            nudCountDayEducationalWeek.Value = stg.CountDayEducationalWeek;
            //количество учебных дней в расписании
            nudCountDaysShedule.Value = stg.CountDaysShedule;
            //количество учебных недель в семестре
            //максимальное значение не ограничено
            nudCountEducationalWeekBySem.Value = stg.CountEducationalWeekBySem;
            //количество занятий в день
            nudCountLessonsOfDay.Value = stg.CountLessonsOfDay;
            //количество недель в расписании, по умолчанию расписание двух-недельное
            //максимальное значение 4, если необходимо больше следует добавить поле в перечисление Week
            nudCountWeeksShedule.Value = stg.CountWeeksShedule;
            //первая пара с которой может начинаться учебный день в будни
            nudFirstLessonsOfWeekDay.Value = stg.FirstLessonsOfWeekDay;
            //первая пара с которой может начинаться учебный день в выходной
            nudFirstLessonsOfWeekEnd.Value = stg.FirstLessonsOfWeekEnd;
            //последня пара которой завершается учебный день в будни
            nudLastLessonsOfWeekDay.Value = stg.LastLessonsOfWeekDay;
            //последня пара которой завершается учебный день в выходной
            nudLastLessonsOfWeekEnd.Value = stg.LastLessonsOfWeekEnd;
            //количество пар в будни
            nudMaxCountLessonsOfWeekDay.Value = stg.MaxCountLessonsOfWeekDay;
            //количество пар в выходные
            nudMaxCountLessonsOfWeekEnd.Value = stg.MaxCountLessonsOfWeekEnd;
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            SettingsAplication stg = new SettingsAplication();
            stg.CountDayEducationalWeek = (int) nudCountDayEducationalWeek.Value;
            stg.CountDaysShedule = (int) nudCountDaysShedule.Value;
            stg.CountEducationalWeekBySem = (int) nudCountEducationalWeekBySem.Value;
            stg.CountLessonsOfDay = (int) nudCountLessonsOfDay.Value;
            stg.CountWeeksShedule = (int) nudCountWeeksShedule.Value;
            stg.FirstLessonsOfWeekDay = (int) nudFirstLessonsOfWeekDay.Value;
            stg.FirstLessonsOfWeekEnd = (int) nudFirstLessonsOfWeekEnd.Value;
            stg.LastLessonsOfWeekDay = (int) nudLastLessonsOfWeekDay.Value;
            stg.LastLessonsOfWeekEnd = (int) nudLastLessonsOfWeekEnd.Value;
            stg.MaxCountLessonsOfWeekDay = (int) nudMaxCountLessonsOfWeekDay.Value;
            stg.MaxCountLessonsOfWeekEnd = (int) nudMaxCountLessonsOfWeekEnd.Value;
            stg.Save();
            this.Close();
        }

        private void btnReject_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
