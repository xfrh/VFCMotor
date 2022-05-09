using System.ComponentModel;

namespace VFCMotor.Models
{
    public class RowData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private byte[] myVar;

        public byte[] MyVar
        {
            get { return myVar; }
            set { 
                myVar = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MyVar"));
            }
        }

    }
}
