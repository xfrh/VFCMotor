using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VFCMotor.Models;

namespace VFCMotor
{
    public partial class FrmCalibrate : Form
    {
        IEnumerable<Control> a;
        IEnumerable<Control> b;
        IEnumerable<Control> c;
        private RowData data;

        public RowData Data
        {
            get { return data; }
            set { 
                data = value;
                LoadData(data.MyVar);
            }
        }

        public FrmCalibrate()
        {
            InitializeComponent();
        }

        protected async override void OnLoad(EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            a = GetAll(this, typeof(NumericUpDown));
            b = GetAll(this, typeof(Button));
            c = GetAll(this, typeof(TextBox));
            RegisterNumericupdown();
            RegisterControlsEvents();
           
            toggleSwitch1.CheckedChanged += ToggleSwitch1_CheckedChanged;
            toggleSwitch2.CheckedChanged += ToggleSwitch2_CheckedChanged;
           

        }


      

        private void LoadData(byte[] data)
        {
          
                    if (data != null)
                    {
                        toggleSwitch1.Checked = (data[0] * 256 + data[1]) == 1;
                        toggleSwitch2.Checked = (data[2] * 256 + data[3]) == 1;
                        textBox13.Text = (data[4] * 256 + data[5]).ToString();
                        textBox19.Text = (data[6] * 256 + data[7]).ToString();
                        textBox14.Text = (data[8] * 256 + data[9]).ToString();
                        textBox20.Text = (data[10] * 256 + data[11]).ToString();
                        textBox15.Text = (data[12] * 256 + data[13]).ToString();
                        textBox21.Text = (data[14] * 256 + data[15]).ToString();
                        textBox16.Text = (data[16] * 256 + data[17]).ToString();
                        textBox22.Text = (data[18] * 256 + data[19]).ToString();
                        textBox17.Text = (data[20] * 256 + data[21]).ToString();
                        textBox23.Text = (data[22] * 256 + data[23]).ToString();
                        textBox18.Text = (data[24] * 256 + data[25]).ToString();
                        textBox24.Text = (data[26] * 256 + data[27]).ToString();
                    }
        
          
        }

        public void RegisterNumericupdown()
        {
            foreach (var item in a)
            {
                NumericUpDown numericUpDown = (item as NumericUpDown);
                numericUpDown.Maximum = 40000;
                numericUpDown.Minimum = -40000;
                numericUpDown.Accelerations.Add(new NumericUpDownAcceleration(2, 100));
                numericUpDown.Accelerations.Add(new NumericUpDownAcceleration(5, 1000));
                numericUpDown.Accelerations.Add(new NumericUpDownAcceleration(8, 5000));

            }
        }

        private void RegisterControlsEvents()
        {
            foreach (var item in b)
            {
                Button btn = (item as Button);
                btn.Click += (s, e) => {
                    NumericUpDown numericUpDown = a.FirstOrDefault(v => v.Tag == btn.Tag) as NumericUpDown;
                    if (numericUpDown != null)
                    {
                        ModbusService.SetValue(numericUpDown.TabIndex, Convert.ToInt32(numericUpDown.Value));
                    
                    }
                        
                };
            }
        }

        private void ToggleSwitch2_CheckedChanged(object sender, EventArgs e)
        {
            if (toggleSwitch2.Checked)
                ModbusService.SetValue(0x70, 1);
            else
                ModbusService.SetValue(0x70, 0);
           
        }

        private void ToggleSwitch1_CheckedChanged(object sender, EventArgs e)
        {
            if (toggleSwitch1.Checked)
                ModbusService.SetValue(0x6F, 1);
            else
                ModbusService.SetValue(0x6F, 0);
           
        }

  

        public IEnumerable<Control> GetAll(Control control, Type type)
        {
            var controls = control.Controls.Cast<Control>();

            return controls.SelectMany(ctrl => GetAll(ctrl, type))
                                      .Concat(controls)
                                      .Where(c => c.GetType() == type);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            FrmCapactorTable frmCapactorTable = new FrmCapactorTable();
            frmCapactorTable.Show();
        }

   
    }
}
