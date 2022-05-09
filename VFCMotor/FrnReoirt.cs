using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VFCMotor.Models;
using VFCMotor.Services;

namespace VFCMotor
{
    public partial class FrnReoirt : Form
    {
        string filepath = Environment.CurrentDirectory + "\\report.xls";
        public FrnReoirt()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!Properties.Settings.Default.RealTimeData && !Properties.Settings.Default.SetTimeData && !Properties.Settings.Default.ADC03Data)
            {
                MessageBox.Show("no log set");
                button1.Enabled = false;
            }
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
            progressBar1.Visible = false;
            button1.Enabled = true;
            button2.Enabled = true;

        }

        private void Progress_ProgressChanged(object sender, int e)
        {
            progressBar1.Value = e;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = false;
            Progress<int> progress = new Progress<int>();
            progress.ProgressChanged += Progress_ProgressChanged;
            List<string> coloums = new List<string>();
            if (Properties.Settings.Default.SetTimeData)
            {
                coloums.Add("C1_Set");
                coloums.Add("C2_Set");
            }
             
            if (Properties.Settings.Default.RealTimeData)
            {
                coloums.Add("C1_Read");
                coloums.Add("C2_Read");
            }
            if(Properties.Settings.Default.ADC03Data)
            {
                coloums.Add("ADC0");
                coloums.Add("ADC1");
                coloums.Add("ADC2");
                coloums.Add("ADC3");
            }
            coloums.Add("RegisterDate");
            dateTimePicker1.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            dateTimePicker1.Value = Convert.ToDateTime(System.DateTime.Today.ToShortDateString() + " 00:00:00");
            var lst = DbService.GetLog(coloums, dateTimePicker1.Value, dateTimePicker2.Value);
            if(lst!=null && lst.Count>0)
            {
                progressBar1.Visible = true;
               
                await ExcelExport.GenerateExcel(ConvertToDataTable(lst), filepath, progress);
                button1.Enabled = true;
                button2.Enabled = true;
                MessageBox.Show("Report Completed");
                progressBar1.Visible = false;
                Process.Start(filepath);
              
            }
            else
            {
                MessageBox.Show("no data");
                button1.Enabled = true;
                button2.Enabled = true;
            }
        }

        DataTable ConvertToDataTable<T>(List<T> models)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in models)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (DbService.ClearDb())
                MessageBox.Show("data clear");

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            dateTimePicker1.CustomFormat = "yyyy-MM-dd HH:mm:ss";

            dateTimePicker1.Value = Convert.ToDateTime(System.DateTime.Today.ToShortDateString() + " 00:00:00");
        }
    
    }
}
