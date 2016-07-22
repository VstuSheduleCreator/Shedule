﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExportToRTF;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;


namespace MyShedule
{
    class ExportShedule
    {
        public ExportShedule()
        {
            NameElements = new List<string>();
        }

        public List<string> NameElements;

        public SheduleWeeks shedule;

        public View view;

        public WordDocument wordDocument = new WordDocument(WordDocumentFormat.A4);

        //Инициализируем необходимые шрифты
        private static Font caption0 = new Font("Times New Roman", 12, FontStyle.Bold);
        private static Font caption1 = new Font("Times New Roman", 12, FontStyle.Regular);
        private static Font caption = new Font("Times New Roman", 14, FontStyle.Bold);
        private static Font normalBold = new Font("Times New Roman", 10, FontStyle.Bold);
        private static Font normal = new Font("Times New Roman", 10, FontStyle.Regular);

        public void Export()
        {
            //http://www.faqdot.net/post/ExportToWord.aspx

            foreach (string name in NameElements)
            {

                //wordDocument.SetPageNumbering(1);

                WriteHeaderReport();

                // Создаем таблицу для поставщика
                WordTable table = wordDocument.NewTable(normal, Color.Black, 13, 9, 10);
                table.SetContentAlignment(ContentAlignment.MiddleCenter);
                table.SetColumnsWidth(new int[] { 3, 5, 5, 5, 5, 5, 5, 5, 35 });
                // table.SetContentAlignment(ContentAlignment.MiddleLeft);
                table.Rows[0].SetFont(caption0);
                ////Записываем в ячейку
                SetTextHeaderTable(table, name);

                table.SetFont(caption);
                SetWeeksHeaderTable(table);

                table.SetFont(caption0);
                SetDaysTable(table, 1, 6, 1, 6);
                SetDaysTable(table, 1, 6, 7, 12);

                table.SetFont(normal);
                FillDaysNumbers(table, name);

                table.SetFont(normal);
                table.SaveToDocument(10000, 0);
            }

            CreateReport(wordDocument);

        }

        private void FillDaysNumbers(WordTable table, string name)
        {
            if (shedule == null)
                return;

            int monthStart = shedule.FirstDaySem.Month;

            List<SheduleLesson> tmp = shedule.GetLessonsByView(view, name).ToList();

            FillLessons(table, monthStart, 1, tmp, Week.FirstWeek, Week.TreeWeek);
            FillLessons(table, monthStart, 7, tmp, Week.SecondWeek, Week.FourWeek);
        }

        

        private void FillLessons(WordTable table, int monthStart, int row, List<SheduleLesson> tmp, Week week1, Week week2)
        {
            int column = 1;

            for (int counterDay = 1; counterDay <= 6; counterDay++)
            {
                column = 1;
                for (int counterMonth = monthStart; counterMonth < monthStart + 5; counterMonth++)
                {

                    List<int> numbers = (from x in tmp
                                         from p in x.Dates
                                         where x.Day == (Day)counterDay &&
                                             p.Month == counterMonth && (x.Week == week1 || x.Week == week2)
                                         select p.Day).Distinct().OrderBy(e => e).ToList();

                    foreach (int day in numbers)
                    {
                        table.Cell(row, column).WriteLine();
                        table.Cell(row, column).Write(day.ToString());
                    }
                    column++;
                }

                List<SheduleLesson> query1 = (from x in tmp
                                              from p in x.Dates
                                              where x.Day == (Day)counterDay && (x.Week == week1|| x.Week == week2)
                                              select x).ToList();

                List<int> Hours = (from x in query1 select x.Hour).Distinct().OrderBy(e => e).ToList();

                foreach (int hour in Hours)
                {
                    string str = "";
                    str = SheduleTime.GetHourDiscription(hour);
                    table.Cell(row, 7).Write(str);
                    table.Cell(row, 7).WriteLine();
                }

                for (int i = 0; i < Hours.Count; i++)
                {
                    List<SheduleLesson> t1 = query1.Where(x => x.Hour == Hours[i] && x.Week == week1).ToList();
                    List<SheduleLesson> t2 = query1.Where(x => x.Hour == Hours[i] && x.Week == week2).ToList();

                    SheduleLesson lesson = (t1.Count > 0) ? t1.First() : null;
                    SheduleLesson lesson2 = (t2.Count > 0) ? t2.First() : null;

                    if (lesson != null && lesson2 != null && lesson.IsEqual(lesson2))
                    {
                        WriteLesson(table, row, lesson, false);


                        if (i + 1 < Hours.Count)
                        {
                            List<SheduleLesson> Next1 = query1.Where(x => x.Hour == Hours[i + 1] && x.Week == week1).ToList();
                            SheduleLesson next1 = (t1.Count > 0) ? t1.First() : null;

                            List<SheduleLesson> Next2 = query1.Where(x => x.Hour == Hours[i + 1] && x.Week == week2).ToList();
                            SheduleLesson next2 = (t1.Count > 0) ? t1.First() : null;


                            if (next1 != null && next2 != null && next1.IsEqual(next2) && lesson.IsEqual(next1))
                                i++;
                        }
                    }
                    else
                    {
                        if (lesson != null)
                        {

                            WriteLesson(table, row, lesson, true);

                            if (i + 1 < Hours.Count)
                            {
                                List<SheduleLesson> Next = query1.Where(x => x.Hour == Hours[i + 1] && x.Week == week1).ToList();
                                SheduleLesson next = (t1.Count > 0) ? t1.First() : null;

                                if (next != null && lesson.IsEqual(next))
                                    i++;
                            }
                        }

                        if (lesson2 != null)
                        {
                            WriteLesson(table, row, lesson2, true);

                            if (i + 1 < Hours.Count)
                            {
                                List<SheduleLesson> Next = query1.Where(x => x.Hour == Hours[i + 1] && x.Week == week2).ToList();
                                SheduleLesson next = (t1.Count > 0) ? t1.First() : null;

                                if (next != null && lesson.IsEqual(next))
                                    i++;
                            }
                        }
                    }

                }
                row++;
            }
        }

        private void WriteLesson(WordTable table, int row, SheduleLesson lesson, bool outdate)
        {

            string str = "";

            //0-я строчка
            if (view != View.Group)
            {
                str = lesson.GroupsDescription;
                table.Cell(row, 8).Write(str);
                table.Cell(row, 8).WriteLine();
            }

            //1-я строчка
            if (view == View.Discipline)
            {
                str += " (" + SheduleLessonType.Description(lesson.Type) + ")";
                table.Cell(row, 8).Write(str);
                table.Cell(row, 8).WriteLine();
            }
            else
            {
                str = lesson.Discipline + " (" + SheduleLessonType.Description(lesson.Type) + ")";
                table.Cell(row, 8).Write(str);
                table.Cell(row, 8).WriteLine();
            }

            //2-я строчка
            if (outdate)
            {
                str = lesson.DatesDescription;
                table.Cell(row, 8).Write(str);
                table.Cell(row, 8).WriteLine();
            }


            //3-я строчка
            if (view == View.Teacher)
            {
                str = lesson.Room;
                table.Cell(row, 8).Write(str);
                table.Cell(row, 8).WriteLine();
            }

            if (view == View.Room)
            {
                str = lesson.Teacher;
                table.Cell(row, 8).Write(str);
                table.Cell(row, 8).WriteLine();
            }

            if (view != View.Teacher && view != View.Room)
            {
                str = lesson.Teacher + " " + lesson.Room;
                table.Cell(row, 8).Write(str);
                table.Cell(row, 8).WriteLine();
            }
        }

        private static void SetWeeksHeaderTable(WordTable table)
        {
            
            int i = 1;
            string week1 = "I" + Environment.NewLine + "Н Е Д Е Л Я";
            table.Cell(i, 0).Write(week1);

            WordCellRange wsr = table.CellRange(i, 0, i + 5, 0);
            wsr.MergeCells();

            i = i + 6;

            string week2 = "II" + Environment.NewLine + "Н Е Д Е Л Я";

            table.Cell(i, 0).Write(week2);

            wsr = table.CellRange(i, 0, i + 5, 0);
            wsr.MergeCells();
        }

        private void WriteHeaderReport()
        {
            //Выводим заголовок
            wordDocument.SetFont(caption);
            wordDocument.SetTextAlign(WordTextAlign.Center);
            wordDocument.WriteLine("ФАКУЛЬТЕТ ПОСЛЕВУЗОВСКОГО ОБРАЗОВАНИЯ");
            wordDocument.WriteLine("");

            wordDocument.SetFont(caption1);
            wordDocument.SetTextAlign(WordTextAlign.Right);
            wordDocument.WriteLine('"' + "УТВЕРЖДАЮ" + '"');
            wordDocument.WriteLine("Проректор по учебной работе");
            wordDocument.WriteLine("__________________Гоник И.Л.");
            wordDocument.WriteLine('"' + "______" + '"' + "______________2011 г.");
            wordDocument.WriteLine("");

            wordDocument.SetFont(caption0);
            wordDocument.SetTextAlign(WordTextAlign.Center);
            wordDocument.WriteLine("Расписание занятий по второму высшему образованию 3 курса ФПО");
            wordDocument.WriteLine("(специальность 230101 " + '"' + "Вычислительные машины, комплексы, системы и сети" + '"' + ",");
            wordDocument.WriteLine("очно-заочная форма) на II семестр 2010-2011 уч. года");
            wordDocument.WriteLine("");
        }

        private void SetTextHeaderTable(WordTable table, string group)
        {
            table.Cell(0, 0).Write("");

            int mountStart = shedule != null ? shedule.FirstDaySem.Month : 9;//сентябрь по умолчанию
            SetMouthsHeader(table, mountStart, 0, 1, 5);

            table.Cell(0, 6).Write("День");
            table.Cell(0, 7).Write("Часы");
            table.Cell(0, 8).Write(group);
        }

        private void SetDaysTable(WordTable table, int dayStart, int columnIndex, int rowIndexStart, int rowIndexEnd)
        {
            for (int counterRow = rowIndexStart, counterDay = dayStart; counterRow <= rowIndexEnd; counterRow++, counterDay++)
            {
                table.Cell(counterRow, columnIndex).Write(GetDayName(counterDay));
            }
        }

        private void SetMouthsHeader(WordTable table, int mouthStart, int rowIndex, int columnIndexStart, int columnIndexEnd)
        {
            for (int counterClmn = columnIndexStart, counterMouth = mouthStart; counterClmn <= columnIndexEnd; counterClmn++)
            {
                table.Cell(rowIndex, counterClmn).Write(GetMouthName(counterMouth));
                counterMouth = counterMouth == 12 ? 1 : counterMouth + 1;
            }
        }

        private static string GetDayName(int dayNumber)
        {
            switch (dayNumber)
            {
                case 1: return "пн";
                case 2: return "вт";
                case 3: return "ср";
                case 4: return "чт";
                case 5: return "пт";
                case 6: return "сб";
                case 7: return "вс";
                default: return @"DAY N\A";
            }
        }

        private static string GetMouthName(int monthNumber)
        {
            //Янв, Фев, Мар, Апр, Май, Июн, Июл, Авг, Сен, Окт, Ноя, Дек
            switch (monthNumber)
            {
                case 1: return "Янв";         case 2: return "Фев";         case 3: return "Мар";
                case 4: return "Апр";         case 5: return "Май";         case 6: return "Июн";
                case 7: return "Июл";         case 8: return "Авг";         case 9: return "Сен";
                case 10: return "Окт";        case 11: return "Ноя";        case 12: return "Дек";
                default: return @"MOUTH N\A";
            }
        }

        private static void CreateReport(WordDocument wordDocument)
        {
            SaveFileDialog frmSave = new SaveFileDialog();
            frmSave.Filter = "(*.doc)|*.doc";
            frmSave.FileName = "report1.doc";
            if (frmSave.ShowDialog() == DialogResult.OK && frmSave.FileName != "")
            {
                // Проверяем наличие файла
                if (File.Exists(frmSave.FileName) == true)
                {
                    try
                    {
                        File.Delete(frmSave.FileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

                // Сохраняем и открываем файл
                wordDocument.SaveToFile(frmSave.FileName);

                // Запускаем связанную с этим расширением программу
                Process.Start(frmSave.FileName);

            }
        }
    }
}
