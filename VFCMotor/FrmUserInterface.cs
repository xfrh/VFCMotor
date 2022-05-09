using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using VFCMotor.Models;
using VFCMotor.Services;

namespace VFCMotor
{
    public partial class FrmUserInterface : Form
    {
      
        Stopwatch stopWatch = new Stopwatch();
        FrmUserSettings frmUserSettings = new FrmUserSettings();
        FrmSysSettings frmSysSettings = new FrmSysSettings();
        FrmFormula frmFormula = new FrmFormula();
        Dictionary<int, Formula> frum_data = new Dictionary<int, Formula>();
        FrmCalibrate frmCalibrate = new FrmCalibrate();
        FrmCapactorTable frmCapactor = new FrmCapactorTable();
        FrmPulse frmPulse = new FrmPulse();
        public FrmUserInterface()
        {
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            backgroundWorker1.CancelAsync();
            frmUserSettings.Close();
            frmSysSettings.Close();
            frmCalibrate.Close();
            frmCapactor.Close();
            frmPulse.Close();
        }
        protected override void OnLoad(EventArgs e)
        {
            chart1.Series.Clear();
            DbService.Connect();
            Control.CheckForIllegalCrossThreadCalls = false;
            var port = Properties.Settings.Default.ModbusSerialPort;
            if (string.IsNullOrEmpty(port))
            {
                FrmSettings frmSettings = new FrmSettings();
                frmSettings.Show();
            }
            else
                ModbusService.Portname = port;
            RegisterpanelEvent();
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.DoWork += BackgroundWorker1_DoWork;
            backgroundWorker1.ProgressChanged += BackgroundWorker1_ProgressChanged;
            button1.Focus();
            comboBox1.Enabled = false;
            InitForumData();
        
        }

        private void InitForumData()
        {
            frum_data.Clear();
            int c1_start = 0x11;
            int c2_start = 0x1B;
            for (int i = 0; i < 10; i++)
            {
                frum_data.Add(i, new Formula() { C1Addr=c1_start,C1Value=0,C2Addr=c2_start,C2Value=0 });
                c1_start++;
                c2_start++;
            }
            comboBox1.DataSource = new BindingSource(frum_data, null);
            comboBox1.DisplayMember = "Key";
            comboBox1.ValueMember = "Value";
            comboBox1.SelectedIndexChanged += (s, e) =>
            {
                int index = (s as ComboBox).SelectedIndex;
                ModbusService.SetValue(frum_data[index].C1Addr, frum_data[index].C1Value);
                ModbusService.SetValue(frum_data[index].C2Addr, frum_data[index].C2Value);
                ModbusService.SetValue(0x25, index);
                ModbusService.SetValue(0x26, index);
            };
        }

        private void BackgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            byte[] data = (byte[])e.UserState;
            if (data == null) return;
            if (e.ProgressPercentage == 0)
            {
                frmUserSettings.Data = new RowData() { MyVar = data };
                frmFormula.Data = new RowData() { MyVar = data };
                frmSysSettings.Data = new RowData() { MyVar = data };
                LoadData0(data);
            }
            else if (e.ProgressPercentage == 1)
                frmCalibrate.Data = new RowData() { MyVar = data };
            else if (e.ProgressPercentage == 2)
                frmCapactor.Data = new RowData() { MyVar = data };
            else if (e.ProgressPercentage == 3)
                frmPulse.Data = new RowData() { MyVar = data };
            Thread.Sleep(10);
        }

        private void LoadData0(byte[] data)
        {
          

            textBox1.Text = ((double)(data[0] * 256 + data[1]) / 10).ToString("N1");
            textBox2.Text = ((double)(data[2] * 256 + data[3]) / 10).ToString("N1");
            pictureBox7.Image = (data[6] * 256 + data[7]) == 0 ? Image.FromFile("images\\down.png") : Image.FromFile("images\\up.png");
            pictureBox13.Image = (data[8] * 256 + data[9]) == 0 ? Image.FromFile("images\\down.png") : Image.FromFile("images\\up.png");
            textBox5.Text = ((double)(data[10] * 256 + data[11])).ToString();
            textBox6.Text = ((double)(data[12] * 256 + data[13])).ToString();

            textBox7.Text = ((double)(data[94] * 256 + data[95])).ToString();
            textBox8.Text = ((double)(data[96] * 256 + data[97])).ToString();
            textBox9.Text = ((double)(data[98] * 256 + data[99])).ToString();
            textBox10.Text = ((double)(data[100] * 256 + data[101])).ToString();
            pictureBox3.Image = IsBitSet(data[4], 0) ? Image.FromFile("images\\yes.png") : Image.FromFile("images\\no.png");
            pictureBox4.Image = IsBitSet(data[4], 1) ? Image.FromFile("images\\yes.png") : Image.FromFile("images\\no.png");
            pictureBox5.Image = IsBitSet(data[4], 2) ? Image.FromFile("images\\yes.png") : Image.FromFile("images\\no.png");


            pictureBox1.Image = IsBitSet(data[4], 3) ? Image.FromFile("images\\green.png") : Image.FromFile("images\\red.png");
            pictureBox2.Image = IsBitSet(data[4], 4) ? Image.FromFile("images\\green.png") : Image.FromFile("images\\red.png");
            pictureBox6.Image = IsBitSet(data[4], 5) ? Image.FromFile("images\\green.png") : Image.FromFile("images\\red.png");

            pictureBox12.Image = IsBitSet(data[5], 0) ? Image.FromFile("images\\yes.png") : Image.FromFile("images\\no.png");
            pictureBox11.Image = IsBitSet(data[5], 1) ? Image.FromFile("images\\yes.png") : Image.FromFile("images\\no.png");
            pictureBox10.Image = IsBitSet(data[5], 2) ? Image.FromFile("images\\yes.png") : Image.FromFile("images\\no.png");
            pictureBox9.Image = IsBitSet(data[5], 3) ? Image.FromFile("images\\yes.png") : Image.FromFile("images\\no.png");
            pictureBox8.Image = IsBitSet(data[5], 4) ? Image.FromFile("images\\yes.png") : Image.FromFile("images\\no.png");



            TimeSpan ts = stopWatch.Elapsed;
            long tick = DateTime.Now.Ticks;

            if (Properties.Settings.Default.RealTimeData | Properties.Settings.Default.SetTimeData | Properties.Settings.Default.ADC03Data)
            {
                if (ts.Seconds > Properties.Settings.Default.StartAfterTime)
                {
                    label29.Text = "LOG ON";
                    pictureBox14.Image = Image.FromFile("images\\green.png");
                    Capacitance capacitance = new Capacitance()
                    {
                        C1_Set = (double)(data[14] * 256 + data[15])/10,
                        C2_Set = (double)(data[16] * 256 + data[17])/10,
                        C1_Read = (double)(data[0] * 256 + data[1])/10,
                        C2_Read = (double)(data[2] * 256 + data[3])/10,
                        ADC0 = (double)(data[94] * 256 + data[95]),
                        ADC1 = (double)(data[96] * 256 + data[97]),
                        ADC2 = (double)(data[98] * 256 + data[99]),
                        ADC3 = (double)(data[100] * 256 + data[101]),
                        RegisterDate = DateTime.Now.ToString("yyyy-MM-dd HH:hh:mm:ss")

                    };
                    DbService.AddLog(capacitance);
                }
                else
                {
                    label29.Text = "LOG OFF";
                    pictureBox14.Image = Image.FromFile("images\\red.png");
                }
            }

            if (checkBox2.Checked)
            {
                double c1_data = ((double)(data[14] * 256 + data[15]))/10;
                double c2_data = ((double)(data[16] * 256 + data[17]))/10;

                chart1.Series[0].Points.AddXY(tick, c1_data);
                chart1.Series[1].Points.AddXY(tick, c2_data);
            }
            if (checkBox3.Checked)
            {
                double c1_data = ((double)(data[0] * 256 + data[1]))/10;
                double c2_data = ((double)(data[2] * 256 + data[3]))/10;
                chart1.Series[0].Points.AddXY(tick, c1_data);
                chart1.Series[1].Points.AddXY(tick, c2_data);

            }
            if (checkBox4.Checked)
            {
                double c1_data = (double)(data[94] * 256 + data[95]);
                double c2_data = (double)(data[96] * 256 + data[97]);
                double c3_data = (double)(data[98] * 256 + data[99]);

                chart1.Series[0].Points.AddXY(tick, c1_data);
                chart1.Series[1].Points.AddXY(tick, c2_data);
                chart1.Series[2].Points.AddXY(tick, c3_data);
            }

            frum_data[0].C1Value = data[34] * 256 + data[35];
            frum_data[1].C1Value = data[36] * 256 + data[37];
            frum_data[2].C1Value = data[38] * 256 + data[39];
            frum_data[3].C1Value = data[40] * 256 + data[41];
            frum_data[4].C1Value = data[42] * 256 + data[43];
            frum_data[5].C1Value = data[44] * 256 + data[45];
            frum_data[6].C1Value = data[46] * 256 + data[47];
            frum_data[7].C1Value = data[48] * 256 + data[49];
            frum_data[8].C1Value = data[50] * 256 + data[51];
            frum_data[9].C1Value = data[52] * 256 + data[53];
           
            frum_data[0].C2Value = data[54] * 256 + data[55];
            frum_data[1].C2Value = data[56] * 256 + data[57];
            frum_data[2].C2Value = data[58] * 256 + data[59];
            frum_data[3].C2Value = data[60] * 256 + data[61];
            frum_data[4].C2Value = data[62] * 256 + data[63];
            frum_data[5].C2Value = data[64] * 256 + data[65];
            frum_data[6].C2Value = data[66] * 256 + data[67];
            frum_data[7].C2Value = data[68] * 256 + data[69];
            frum_data[8].C2Value = data[70] * 256 + data[71];
            frum_data[9].C2Value = data[72] * 256 + data[73];

        }

        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            while (!backgroundWorker1.CancellationPending)
            {
                byte[] data = ModbusService.ReadMultiData(0, 51);
                worker.ReportProgress(0, data);
               byte[] data2 = ModbusService.ReadMultiData(0x6F, 14);
                worker.ReportProgress(1, data2);
                byte[] data3 = ModbusService.ReadMultiData(0x7D, 40);
                worker.ReportProgress(2, data3);
                byte[] data4 = ModbusService.ReadMultiData(0xA5, 40);
                worker.ReportProgress(3, data4);
                Thread.Sleep(1000);
            }
        }

        private void InitLog()
        {
            if (Properties.Settings.Default.RealTimeData | Properties.Settings.Default.SetTimeData | Properties.Settings.Default.ADC03Data)
                stopWatch.Start();
            else
                stopWatch.Stop();
            label29.Text = "LOG OFF";
            pictureBox14.Image = Image.FromFile("images\\red.png");
            label29.Focus();

        }

        private void RegisterpanelEvent()
        {
            foreach (var item in panel9.Controls)
            {
                if(item is CheckBox)
                {
                    (item as CheckBox).CheckedChanged += (s, e) => chk_Click(s, e);
                }
            }
        }

      
        CheckBox lastChecked;
        private void chk_Click(object sender, EventArgs e)
        {
            CheckBox activeCheckBox = sender as CheckBox;
            if (activeCheckBox != lastChecked && lastChecked != null) lastChecked.Checked = false;
            lastChecked = activeCheckBox.Checked ? activeCheckBox : null;
            if (lastChecked != null && lastChecked.Checked)
            {
                chart1.Series.Clear();
                if (lastChecked.Text == "C1 C2 设定值")
                {
                    chart1.Series.Add("C1 设定值");
                    chart1.Series.Add("C2 设定值");
                    chart1.Series["C1 设定值"].ChartType = SeriesChartType.Line;
                    chart1.Series["C2 设定值"].ChartType = SeriesChartType.Line;

                


                }
                else if (lastChecked.Text == "C1 C2  实时值")
                {
                    chart1.Series.Add("C1 实时值");
                    chart1.Series.Add("C2 实时值");
                    chart1.Series["C1 实时值"].ChartType = SeriesChartType.Line;
                    chart1.Series["C2 实时值"].ChartType = SeriesChartType.Line;

                }
                else if (lastChecked.Text == "ADC0-3 读数")
                {
                    chart1.Series.Add("ADC0");
                    chart1.Series.Add("ADC1");
                    chart1.Series.Add("ADC2");
                    chart1.Series["ADC0"].ChartType = SeriesChartType.Line;
                    chart1.Series["ADC1"].ChartType = SeriesChartType.Line;
                    chart1.Series["ADC2"].ChartType = SeriesChartType.Line;



                }

                chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                chart1.ChartAreas[0].AxisX.MinorGrid.Enabled = false;
                chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
                chart1.ChartAreas[0].AxisY.MinorGrid.Enabled = false;
                chart1.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
                chart1.ChartAreas[0].AxisY.LabelStyle.Enabled = false;
                chart1.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
                chart1.ChartAreas[0].AxisY.LabelStyle.Enabled = false;

             

            }
        }

 
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = true;
            comboBox1.Enabled = true;
            InitLog();
           if (backgroundWorker1.IsBusy != true)
            {
              backgroundWorker1.RunWorkerAsync();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
        
            button2.Enabled = false;
            button1.Enabled = true;
            comboBox1.Enabled = false;
            button1.Focus();
            stopWatch.Stop();
            label29.Text = "LOG OFF";
            pictureBox14.Image = Image.FromFile("images\\red.png");
            backgroundWorker1.CancelAsync();
        }

      

        bool IsBitSet(byte b, int pos)
        {
            return (b & (1 << pos)) != 0;
        }

        private int GetbitValue(byte input, int index)
        {
            if (index > sizeof(byte))
            {
                return -1;
            }
             int value = input << (sizeof(byte) - 1 - index);
              value = value >> (sizeof(byte) - 1);
            return value;
        }

      

        private void 串口设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmSettings frmSettings = new FrmSettings();
            frmSettings.Show();
        }

        private void lOG设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmLogSetting frmLogSetting = new FrmLogSetting();
            frmLogSetting.OnLogSetting += () => InitLog();
            frmLogSetting.ShowDialog();
        }

        private void 退出XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
            this.Close();
        }

        private void 基础配置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUserSettings = new FrmUserSettings();
            frmUserSettings.Show();
        }

        private void 配方配置ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            frmFormula = new FrmFormula();
            
            if (string.IsNullOrEmpty(LogService.Password))
            {
                FrmPassword frmPassword = new FrmPassword();
                frmPassword.SelectedFrom = frmFormula;
                frmPassword.ShowDialog();
                return;
            }
            else
                frmFormula.Show();
         
        }

        private void 二级调试ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCalibrate = new FrmCalibrate();
            if (string.IsNullOrEmpty(LogService.Password))
            {

                FrmPassword frmPassword = new FrmPassword();
                frmPassword.SelectedFrom = frmCalibrate;
                frmPassword.ShowDialog();
                return;
            }
            else
            frmCalibrate.Show();
        }

        private void 容值表TToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCapactor = new FrmCapactorTable();
            if (string.IsNullOrEmpty(LogService.Password))
            {

                FrmPassword frmPassword = new FrmPassword();
                frmPassword.SelectedFrom = frmCapactor;
                frmPassword.ShowDialog();
                return;
            }
            else

                frmCapactor.Show();
        }

        private void 打开OToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrnReoirt frnReoirt = new FrnReoirt();
            frnReoirt.ShowDialog();
        }


        private void 系统参数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSysSettings = new FrmSysSettings();
            frmSysSettings.Show();
        }

        private void 脉冲表参数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPulse = new FrmPulse();
            frmPulse.Show();
        }
    }
}
