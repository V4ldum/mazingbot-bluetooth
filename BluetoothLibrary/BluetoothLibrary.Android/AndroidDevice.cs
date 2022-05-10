using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BluetoothLibrary;
using Android.Bluetooth;

namespace BluetoothLibrary.Droid
{
    internal sealed class AndroidDevice : IDevice
    {
        public AndroidDevice(BluetoothDevice device)
        {
            NativeDevice = device;
        }

        public string Name
        {
            get
            {
                if(_nativeDevice.Name != null)
                {
                    return _nativeDevice.Name;
                }
                return Address;
            }
        }

        public string Address
        {
            get => _nativeDevice.Address.ToUpper();
        }

        private BluetoothDevice _nativeDevice;
        public object NativeDevice
        {
            get => _nativeDevice;
            private set => _nativeDevice = value as BluetoothDevice;
        }
    }
}