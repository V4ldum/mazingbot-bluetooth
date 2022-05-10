using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluetoothLibrary
{
    public class ErrorEventArgs : EventArgs
    {
        private Exception exception;

        public Exception Exception
        {
            get { return exception; }
            set { exception = value; }
        }

        private string message;
        public string Message
        {
            get => message;
            private set { message = value; }
        }

        public ErrorEventArgs(string message, Exception exception = null)
        {
            Exception = exception;
            Message = message ?? "";
        }

    }
}
