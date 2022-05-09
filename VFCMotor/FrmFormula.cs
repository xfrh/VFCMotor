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
    public partial class FrmFormula : Form
    {
        IEnumerable<Control> a;
        IEnumerable<Control> b;
        IEnumerable<Control> c;

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
        public FrmFormula()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            a = GetAll(this, typeof(NumericUpDown));
            b = GetAll(this, typeof(Button));
            c = GetAll(this, typeof(TextBox));
            RegisterNumericupdown();
            RegisterControlEvents();
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

        private void LoadData(byte[] data)
        {


            textBox1.Text = ((double)(data[34] * 256 + data[35]) / 10).ToString("N1");
            textBox4.Text = ((double)(data[36] * 256 + data[37]) / 10).ToString("N1");
            textBox6.Text = ((double)(data[38] * 256 + data[39]) / 10).ToString("N1");
            textBox8.Text = ((double)(data[40] * 256 + data[41]) / 10).ToString("N1");
            textBox10.Text = ((double)(data[42] * 256 + data[43]) / 10).ToString("N1");
            textBox12.Text = ((double)(data[44] * 256 + data[45]) / 10).ToString("N1");
            textBox14.Text = ((double)(data[46] * 256 + data[47]) / 10).ToString("N1");
            textBox16.Text = ((double)(data[48] * 256 + data[49]) / 10).ToString("N1");

            textBox18.Text = ((double)(data[50] * 256 + data[51]) / 10).ToString("N1");
            textBox20.Text = ((double)(data[52] * 256 + data[53]) / 10).ToString("N1");
            textBox2.Text = ((double)(data[54] * 256 + data[55]) / 10).ToString("N1");
            textBox3.Text = ((double)(data[56] * 256 + data[57]) / 10).ToString("N1");
            textBox5.Text = ((double)(data[58] * 256 + data[59]) / 10).ToString("N1");
            textBox7.Text = ((double)(data[60] * 256 + data[61]) / 10).ToString("N1");
            textBox9.Text = ((double)(data[62] * 256 + data[63]) / 10).ToString("N1");
            textBox11.Text = ((double)(data[64] * 256 + data[65]) / 10).ToString("N1");
            textBox13.Text = ((double)(data[66] * 256 + data[67]) / 10).ToString("N1");
            textBox15.Text = ((double)(data[68] * 256 + data[69]) / 10).ToString("N1");
            textBox17.Text = ((double)(data[70] * 256 + data[71]) / 10).ToString("N1");
            textBox19.Text = ((double)(data[72] * 256 + data[73]) / 10).ToString("N1");

        }


        private void RegisterControlEvents()
        {
            foreach (var item in b)
            {
                Button btn = item as Button;
                btn.Click += (s, e) =>
                {
                    var controls= a.Where((t) => t.Tag == btn.Tag).ToList();
                    foreach (var nub in controls)
                    {
                        NumericUpDown numericUpDown = nub as NumericUpDown;
                        if(numericUpDown.Value>0)
                         ModbusService.SetValue(numericUpDown.TabIndex, Convert.ToInt32(numericUpDown.Value*10));
                    }
                };

            }
        }

        public IEnumerable<Control> GetAll(Control control, Type type)
        {
            var controls = control.Controls.Cast<Control>();

            return controls.SelectMany(ctrl => GetAll(ctrl, type))
                                      .Concat(controls)
                                      .Where(c => c.GetType() == type);
        }


     
    }
}
