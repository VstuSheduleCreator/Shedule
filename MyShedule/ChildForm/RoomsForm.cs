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
    public partial class RoomsForm : Form
    {
        public RoomsForm()
        {
            InitializeComponent();

            ds = new dsShedule();

            this.Load += new EventHandler(RoomsForm_Load);

        }

        void RoomsForm_Load(object sender, EventArgs e)
        {
            InitGrid();

            this.FormClosing += new FormClosingEventHandler(RoomsForm_FormClosing);
        }

        void RoomsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("Сохранить перед закрытием? ", "внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == System.Windows.Forms.DialogResult.OK)
                Save();

        }

        public List<SheduleLessonType> LessonTypes;
        public dsShedule ds;


        private void InitGrid()
        {
            dgvRooms.AutoGenerateColumns = false;

            DataGridViewTextBoxColumn clmn = new DataGridViewTextBoxColumn();
            DataGridViewCheckBoxColumn chkmn = new DataGridViewCheckBoxColumn();

            clmn.DataPropertyName = "Id";
            clmn.Name = "id";
            clmn.Visible = false;
            dgvRooms.Columns.Add(clmn);

            clmn = new DataGridViewTextBoxColumn();
            clmn.DataPropertyName = "Name";
            clmn.Name = "name";
            clmn.HeaderText = "Аудитория";
            clmn.Width = 80;
            dgvRooms.Columns.Add(clmn);

            //--------------------------------------
            chkmn = new DataGridViewCheckBoxColumn();
            chkmn.DataPropertyName = "Lection";
            chkmn.Name = "lection";
            chkmn.HeaderText = "Лекции";
            chkmn.Width = 60;
            dgvRooms.Columns.Add(chkmn);

            clmn = new DataGridViewTextBoxColumn();
            clmn.DataPropertyName = "DisciplineLection";
            clmn.Name = "disciplineLection";
            clmn.HeaderText = "Дисциплина";
            clmn.Width = 150;
            dgvRooms.Columns.Add(clmn);

            //-----------------------------------
            chkmn = new DataGridViewCheckBoxColumn();
            chkmn.DataPropertyName = "LabWork";
            chkmn.Name = "labwork";
            chkmn.HeaderText = "Лаборат.";
            chkmn.Width = 60;
            dgvRooms.Columns.Add(chkmn);
            
            clmn = new DataGridViewTextBoxColumn();
            clmn.DataPropertyName = "DisciplineLabWork";
            clmn.Name = "disciplineLabWork";
            clmn.HeaderText = "Дисциплина";
            clmn.Width = 150;
            dgvRooms.Columns.Add(clmn);

            //-----------------------------------------

            chkmn = new DataGridViewCheckBoxColumn();
            chkmn.DataPropertyName = "Practice";
            chkmn.Name = "practice";
            chkmn.HeaderText = "Практики";
            chkmn.Width = 60;
            dgvRooms.Columns.Add(chkmn);

            clmn = new DataGridViewTextBoxColumn();
            clmn.DataPropertyName = "DisciplinePractice";
            clmn.Name = "disciplinePractice";
            clmn.HeaderText = "Дисциплина";
            clmn.Width = 150;
            dgvRooms.Columns.Add(clmn);

            BindingSource source = new BindingSource();
            source.DataSource = ds.Room;
            dgvRooms.DataSource = source;

            bindingNavigator1.BindingSource = source;
            dgvRooms.RowPrePaint += new DataGridViewRowPrePaintEventHandler(dgvEducationLoad_RowPrePaint);
            dgvRooms.RowHeadersWidth = 25;
        }

        private void dgvEducationLoad_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            // получаем текущий элемент DataGridView
            DataGridView grid = sender as DataGridView;
            if (grid != null)
            {
                if (e.RowIndex % 2 == 1)
                    // нечетная строка
                    grid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGray;
                else
                    // четная строка
                    grid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            }
        }

        private void tsbSaveFile_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void Save()
        {
            SaveFileDialog SaveDlg = new SaveFileDialog();
            SaveDlg.DefaultExt = "xml";
            SaveDlg.FileName = "Аудитории.xml";
            SaveDlg.Filter = "(*.xml)|*.xml";

            DialogResult RDlg = SaveDlg.ShowDialog();
            string filename = SaveDlg.FileName;
            if (RDlg == DialogResult.OK && filename != "")
                WriteXmlFile(filename);
        }

        private void WriteXmlFile(string filename)
        {
            try
            {
                ds.Room.WriteXml(filename);
            }
            catch (Exception)
            {
                MessageBox.Show("Не могу сохранить в файл");
            }
        }

        private void tsbReadFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog opendlg = new OpenFileDialog();
            opendlg.DefaultExt = "xml";
            opendlg.Filter = "(*.xml)|*.xml";
            DialogResult res = opendlg.ShowDialog();
            string filename = opendlg.FileName;
            if (res == DialogResult.OK && filename != "")
            {
                ReadXmlFile(filename);
            }
        }

        private void ReadXmlFile(string filename)
        {
            try
            {
                this.ds.Room.Clear();
                this.ds.Room.ReadXml(filename);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не могу открыть табель" + ex.Message);
            }
        }

    }
}
