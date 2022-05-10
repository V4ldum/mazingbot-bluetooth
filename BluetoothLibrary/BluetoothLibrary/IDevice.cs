using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluetoothLibrary
{
    /// <summary>
    /// Represents a device to connect with Bluetooth.
    /// </summary>
    public interface IDevice
    {
        /// <summary>
        /// The name of the device. If the name is not set, this value is the Bluetooth address of the device.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The Bluetooth address of the device.
        /// </summary>
        string Address { get; }

        /// <summary>
        /// The instance of device used by the current operating system. It should be cast in order to be used properly.
        /// </summary>
        object NativeDevice { get; }
    }
}
