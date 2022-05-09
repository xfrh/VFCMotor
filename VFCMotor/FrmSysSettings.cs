using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VFCMotor.Models;

namespace VFCMotor
{
    public partial class FrmSysSettings : Form
    {
      
        public FrmSysSettings()
        {
            InitializeComponent();
        }
        private RowData data;

        public RowData Data
        {
            get { return data; }
            set
            {
                data = value;
                LoadData(data.MyVar);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
         
            InitToggle();
          
        }


        private void LoadData(byte[] data)
        {

            textBox63.Text = ((double)(data[82] * 256 + data[83])).ToString("N1");
            textBox6.Text = ((double)(data[84] * 256 + data[85])).ToString("N1");
            textBox64.Text = ((double)(data[86] * 256 + data[87])).ToString("N1");
            textBox4.Text = ((double)(data[88] * 256 + data[89])).ToString("N1");
            textBox65.Text = ((double)(data[90] * 256 + data[91])).ToString("N1");
            textBox2.Text = ((double)(data[92] * 256 + data[93])).ToString("N1");

        }

        private void InitToggle()
        {
            int intValue = 0xFFF; //65535; //8191;
            foreach (var item in groupBox1.Controls)
            {
                if (item is JCS.ToggleSwitch)
                {
                    (item as JCS.ToggleSwitch).CheckedChanged += (s, e) =>
                    {
                        JCS.ToggleSwitch sender = s as JCS.ToggleSwitch;
                        if (sender.Checked)
                            intValue |= 1 << int.Parse(sender.Tag.ToString());
                        else
                            intValue &= ~(1 << int.Parse(sender.Tag.ToString()));
                        ModbusService.SetValue(2, intValue);
                    };

                }
            }
        }

        private void button58_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox69.Text, out _))
                ModbusService.SetValue(0x29, int.Parse(textBox69.Text));
        }

        private void button60_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox70.Text, out _))
                ModbusService.SetValue(0x2B, int.Parse(textBox70.Text));
        }

        private void button62_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox71.Text, out _))
                ModbusService.SetValue(0x2D, int.Parse(textBox71.Text));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox5.Text, out _))
                ModbusService.SetValue(0x2A, int.Parse(textBox5.Text));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox3.Text, out _))
                ModbusService.SetValue(0x2C, int.Parse(textBox3.Text));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox1.Text, out _))
                ModbusService.SetValue(0x2E, int.Parse(textBox1.Text));
        }

        private void button55_Click(object sender, EventArgs e)
        {
            ModbusService.SetValue(0x28, 1);
        }

        private void button56_Click(object sender, EventArgs e)
        {
            ModbusService.SetValue(0x28, 2);
        }
    }
}
