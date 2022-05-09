using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFCMotor.Models
{
    class ModData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private byte[] rawData;

        public byte[] RawData
        {
            get { return rawData; }
            set {
                rawData = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RawData"));
            }
        }

        public List<int> PayLoads { 
            get
            {
                List<int> values = new List<int>();
                for (int i = 0; i < RawData.Length;)
                {
                    int value = RawData[i] * 256 + RawData[i + 1];
                    values.Add(value);
                    i += 2;
                    if (i == RawData.Length)
                        break;
                  
                }
                return values;
            }
         }
    }
}
