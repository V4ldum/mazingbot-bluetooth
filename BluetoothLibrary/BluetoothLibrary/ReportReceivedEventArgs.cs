using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluetoothLibrary
{
    public class ReportReceivedEventArgs : EventArgs
    {
        private byte[] _report;
        public byte[] Report
        {
            get => _report;
            set => _report = value; 
        }
    }
}
