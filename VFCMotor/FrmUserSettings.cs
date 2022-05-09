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
    public partial class FrmUserSettings : Form
    {
        IEnumerable<Control> a;
        IEnumerable<Control> b;
        IEnumerable<Control> c;
       
        public FrmUserSettings()
        {
            InitializeComponent();
        }

        private RowData data;

        public RowData Data
        {
            get { return data; }
            set { 
                data = value;
                LoadData(data.MyVar);
            }
        }


        protected override void OnLoad(EventArgs e)
        {
            a = GetAll(this, typeof(NumericUpDown));
            b = GetAll(this, typeof(Button));
            c = GetAll(this, typeof(TextBox));
            RegisterNumericupdown();
            RegisterControlsEvent();
            
        }
      

        private void RegisterControlsEvent()
        {
            foreach (var item in b)
            {
                Button btn = (item as Button);
                btn.Click += (s, e) => {
                    NumericUpDown numericUpDown = a.FirstOrDefault(v => v.Tag == btn.Tag) as NumericUpDown;
                    if (numericUpDown != null)
                    {
                        if(btn.Tag.ToString()=="2" || btn.Tag.ToString()=="7")
                            ModbusService.SetValue(numericUpDown.TabIndex, Convert.ToInt32(numericUpDown.Value*10));
                       else
                        ModbusService.SetValue(numericUpDown.TabIndex, Convert.ToInt32(numericUpDown.Value));
                    }
                      
                };
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



        public IEnumerable<Control> GetAll(Control control, Type type)
        {
            var controls = control.Controls.Cast<Control>();

            return controls.SelectMany(ctrl => GetAll(ctrl, type))
                                      .Concat(controls)
                                      .Where(c => c.GetType() == type);
        }




        private void  LoadData(byte[] data)
        {
        
             if (data != null)
             {
                        textBox2.Text = ((double)(data[10] * 256 + data[11])).ToString("N1");
                        textBox46.Text = ((double)(data[12] * 256 + data[13])).ToString("N1");
                        textBox4.Text = ((double)(data[14] * 256 + data[15])/10).ToString("N1");
                        textBox47.Text = ((double)(data[16] * 256 + data[17]) / 10).ToString("N1");
                        textBox6.Text = (data[18] * 256 + data[19]).ToString("N1");
                        textBox48.Text = (data[20] * 256 + data[21]).ToString("N1");
                        textBox8.Text = (data[22] * 256 + data[23]).ToString("N1");
                        textBox49.Text = (data[24] * 256 + data[25]).ToString("N1");
                        textBox10.Text = (data[26] * 256 + data[27]).ToString("N1");
                        textBox50.Text = (data[28] * 256 + data[29]).ToString("N1");
             }
             
            }
         
        

        private void button5_Click(object sender, EventArgs e)
        {
           
                ModbusService.SetValue(0x0F,11);
        
           
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ModbusService.SetValue(0x0F, 12);
        }

        private void button8_Click(object sender, EventArgs e)
        {
                ModbusService.SetValue(0x10, 11);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            ModbusService.SetValue(0x10, 12);
        }
    }
}
