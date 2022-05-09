using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFCMotor.Models
{
    public class Capacitance
    {
        public double C1_Set { get; set; }
        public double C2_Set { get; set; }
        public double C1_Read { get; set; }
        public double C2_Read { get; set; }
        public double ADC0 { get; set; }
        public double ADC1 { get; set; }
        public double ADC2 { get; set; }
        public double ADC3 { get; set; }
        public string RegisterDate { get; set; }
    }
}
