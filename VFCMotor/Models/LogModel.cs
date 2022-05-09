using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFCMotor.Models
{
   public class LogModel
    {
        public int TimeSpan { get; set; }
        public string[] TestedData { get; set; }
        public string[] ADC1 { get; set; }
        public string[] ADC2 { get; set; }
        public string  RootDir { get; set; }
    }
}
