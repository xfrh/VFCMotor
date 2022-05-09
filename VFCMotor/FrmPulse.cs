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
    public partial class FrmPulse : Form
    {
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

        public FrmPulse()
        {
            InitializeComponent();
        }
        private void LoadData(byte[] data)
        {
            if (c == null)
               c = GetAll(this, typeof(TextBox));
            int j = 1;
            for (int i = 0; i < data.Length; i += 2)
            {
                TextBox textBox = c.FirstOrDefault(t => t.Tag.ToString() == (j).ToString()) as TextBox;
                if (textBox != null)
                    textBox.Text = ((double)(data[i] * 254 + data[i + 1])).ToString("N1");
                j++;
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
