using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VFCMotor
{
    public partial class FrmPassword : Form
    {
        public Form SelectedFrom { get; set; }
        public FrmPassword()
        {
            InitializeComponent();
            AcceptButton = button1;
        }

       

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "gmp0712")
            {
                LogService.Password = textBox1.Text;
                this.Hide();
             
                SelectedFrom.Closed += (s, args) => this.Close();
                SelectedFrom.Show();

            }
            else
            {
                MessageBox.Show("password is not correct");
                return;
            }
        }
    }
}
