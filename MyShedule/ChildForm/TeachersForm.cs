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
    public partial class TeachersForm : Form
    {
        public TeachersForm()
        {
            InitializeComponent();

            ds = new dsShedule();

            this.Load += new EventHandler(TeachersForm_Load);

        }

        void TeachersForm_Load(object sender, EventArgs e)
        {
            InitGrid();

            this.FormClosing += new FormClosingEventHandler(TeachersForm_FormClosing);
        }

        void TeachersForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("Сохранить перед закрытием? ", "внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == System.Windows.Forms.DialogResult.OK)
                Save();

        }

        public List<SheduleLessonType> LessonTypes;
        public dsShedule ds;


        private void InitGrid()
        {
            dgvTeachers.AutoGenerateColumns = false;

            DataGridViewTextBoxColumn clmn = new DataGridViewTextBoxColumn();
            DataGridViewCheckBoxColumn chkmn = new DataGridViewCheckBoxColumn();

            clmn.DataPropertyName = "Id";
            clmn.Name = "id";
            clmn.Visible = false;
            dgvTeachers.Columns.Add(clmn);

            clmn = new DataGridViewTextBoxColumn();
            clmn.DataPropertyName = "Name";
            clmn.Name = "name";
            clmn.HeaderText = "Преподаватель";
            clmn.Width = clmn.HeaderText.Length*8;
            dgvTeachers.Columns.Add(clmn);

            BindingSource source = new BindingSource();
            source.DataSource = ds.Teacher;
            dgvTeachers.DataSource = source;

            bindingNavigator1.BindingSource = source;
            dgvTeachers.RowPrePaint += new DataGridViewRowPrePaintEventHandler(dgvEducationLoad_RowPrePaint);
            dgvTeachers.RowHeadersWidth = 25;
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
            SaveDlg.FileName = "Преподаватели.xml";
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
                ds.Teacher.WriteXml(filename);
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
                this.ds.Teacher.Clear();
                this.ds.Teacher.ReadXml(filename);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не могу открыть файл" + ex.Message);
            }
        }

    }
}
