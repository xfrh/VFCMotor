
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Windows.Forms;


namespace VFCMotor
{
    public partial class FrmSettings : Form
    {
       
        public FrmSettings()
        {
            InitializeComponent();
        }

        private void BindPortList(ComboBox comboBox,List<string> portList)
        {
            comboBox.DataSource = portList;
            switch (comboBox.Name)
            {
                case "comboBox1":
                    var port1 = Properties.Settings.Default.ModbusSerialPort;
                    if (string.IsNullOrEmpty(port1))
                        comboBox.SelectedIndex = 0;
                    else
                        comboBox.SelectedIndex = comboBox.FindStringExact(port1);
                    break;
                case "comboBox2":
                    var port2 = Properties.Settings.Default.ModbusSerialPort2;
                    if (string.IsNullOrEmpty(port2))
                        comboBox.SelectedIndex = 0;
                    else
                        comboBox.SelectedIndex = comboBox.FindStringExact(port2);
                    break;
                 default:
                    comboBox.SelectedIndex = 0;
                    break;

            }
        }

        protected override void OnLoad(EventArgs e)
        {
            BindPortList(comboBox1, Getserialport());
            BindPortList(comboBox2, Getserialport());
            if (Properties.Settings.Default.MultiablePort)
            {
                checkBox1.Checked = true;
                comboBox2.Enabled = true;
            }
            else
            {
                checkBox1.Checked = false;
                comboBox2.Enabled = false;
            }
        }

        private List<string> Getserialport()
        {
            return SerialPort.GetPortNames().ToList();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ModbusSerialPort = comboBox1.SelectedItem.ToString();
            if (checkBox1.Checked)
                Properties.Settings.Default.ModbusSerialPort2 = comboBox2.SelectedItem.ToString();
            else
                Properties.Settings.Default.ModbusSerialPort2 = string.Empty;
            Properties.Settings.Default.Save();
            ModbusService.Portname = comboBox1.SelectedItem.ToString();

            MessageBox.Show("Ports Saved!");
           
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            comboBox2.Enabled = checkBox1.Checked;
        }
    }
}
