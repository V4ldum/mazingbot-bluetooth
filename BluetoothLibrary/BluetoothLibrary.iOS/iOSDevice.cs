using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace BluetoothLibrary.iOS
{
    class IOSDevice : IDevice
    {
        public string Name => throw new NotImplementedException();

        public string Address => throw new NotImplementedException();

        public object NativeDevice => throw new NotImplementedException();
    }
}