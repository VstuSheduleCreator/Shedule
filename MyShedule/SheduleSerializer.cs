﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MyShedule
{
    public static class SheduleSerializer
    {
        /// <summary>Сохранить расписание в файл </summary>
        /// <param name="path"> Путь к файлу</param>
        /// <param name="shedule"> Сохраняемое расписание</param>
        public static void SaveData(string path, SheduleWeeks shedule)
        {
            XmlWriter writer = new XmlTextWriter(path, System.Text.Encoding.UTF8);
            XmlSerializer serializer = new XmlSerializer(typeof(SheduleWeeks));
            serializer.Serialize(writer, shedule);
            writer.Close();
        }

        /// <summary>Прочитать расписание из файла</summary>
        /// <param name="path">Путь к файлу</param>
        /// <returns>Полученное расписание</returns>
        public static SheduleWeeks ReadData(string path)
        {
            XmlReader reader = new XmlTextReader(path);
            XmlSerializer serializer = new XmlSerializer(typeof(SheduleWeeks)); 
            SheduleWeeks shedule = (SheduleWeeks)serializer.Deserialize(reader);
            reader.Close();
            return shedule;
        }
    }
}
