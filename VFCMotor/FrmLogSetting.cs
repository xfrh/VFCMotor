using System;
using System.Windows.Forms;


namespace VFCMotor
{
    public partial class FrmLogSetting : Form
    {
        public Action OnLogSetting;
        public FrmLogSetting()
        {
            InitializeComponent();
        }
        protected override void OnLoad(EventArgs e)
        {
            checkBox1.Checked = Properties.Settings.Default.RealTimeData;
            checkBox2.Checked = Properties.Settings.Default.SetTimeData;
            checkBox3.Checked = Properties.Settings.Default.ADC03Data;
            textBox1.Text = Properties.Settings.Default.StartAfterTime.ToString();
                
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.RealTimeData = checkBox1.Checked;
            Properties.Settings.Default.SetTimeData = checkBox2.Checked;
            Properties.Settings.Default.ADC03Data = checkBox3.Checked;
            if(int.TryParse(textBox1.Text,out _))
              Properties.Settings.Default.StartAfterTime = int.Parse(textBox1.Text);
             Properties.Settings.Default.Save();
            OnLogSetting?.Invoke();
           MessageBox.Show("Log Saved!");

        }
    }
}
