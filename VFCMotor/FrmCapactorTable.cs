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
    public partial class FrmCapactorTable : Form
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

        public FrmCapactorTable()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            //a = GetAll(this, typeof(NumericUpDown));
            //b = GetAll(this, typeof(Button));
            //c = GetAll(this, typeof(TextBox));
            //RegisterNumericupdown();
            //RegisterControlsEvent();
        }

        private void LoadData(byte[] data)
        {
            if(c==null)
            {
                a = GetAll(this, typeof(NumericUpDown));
                b = GetAll(this, typeof(Button));
                c = GetAll(this, typeof(TextBox));
                RegisterNumericupdown();
                RegisterControlsEvent();
            }
            int j = 1;
            for (int i = 0; i < data.Length; i+=2)
            {
                TextBox textBox = c.FirstOrDefault(t => t.Tag.ToString() == (j).ToString()) as TextBox;
                if (textBox != null)
                    textBox.Text = ((double)(data[i] * 256 + data[i + 1]) / 10).ToString("N1");
              
                j++;
            }
        }

        private void RegisterControlsEvent()
        {
            foreach (var item in b)
            {
                Button btn = (item as Button);
                btn.Click += (s, e) => {
                    NumericUpDown numericUpDown = a.FirstOrDefault(v => v.Tag == btn.Tag) as NumericUpDown;
                    if (numericUpDown != null)
                        ModbusService.SetValue(numericUpDown.TabIndex, Convert.ToInt32(numericUpDown.Value*10));
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
                numericUpDown.TabIndex = int.Parse(numericUpDown.Tag.ToString()) + 124;
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
