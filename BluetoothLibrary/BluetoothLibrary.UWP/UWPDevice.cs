using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Plugin.BLE.Abstractions;
using System.Threading;

namespace BluetoothLibrary.UWP
{
    internal class UWPDevice : IDevice
    {
        private DeviceInformation device;
        /// <summary>
        /// Create an instance of UWPDevice.
        /// </summary>
        /// <param name="device"></param>
        internal UWPDevice(DeviceInformation device)
        {
            this.device = device;
        }

        public string Name => (device.Name == "") ? Address : device.Name;

        public string Address => device.Id;

        public object NativeDevice => device;
    }
}
