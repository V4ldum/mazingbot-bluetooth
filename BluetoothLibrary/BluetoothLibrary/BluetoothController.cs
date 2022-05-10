using BluetoothLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluetoothLibrary
{
    /// <summary>
    /// Used to get a platform specific controller.
    /// </summary>
    public abstract class BluetoothController
    {
        protected const string ERROR_ALREADY_CONNECTED = "Already connected, disconnect first.";
        protected const string ERROR_UNHANDLED_ERROR = "Unhandled error, check with the nearest tech!";
        protected const string ERROR_NO_SOCKET_CREATED = "No socket created.";
        protected const string ERROR_UNABLE_CONNECT_DEVICE = "Unable to connect to this device : ";

        //comment
        public event EventHandler<ReportReceivedEventArgs> ReportReceived;
        //comment
        public virtual void OnReportReceived(byte[] buffer) => ReportReceived?.Invoke(this, new ReportReceivedEventArgs { Report = buffer });
        //comment
        public event EventHandler<ErrorEventArgs> ErrorOccured;
        //comment
        public virtual void OnErrorOccured(ErrorEventArgs e) => ErrorOccured?.Invoke(this, e);

        /// <summary>
        /// Check if the device can communicate with Bluetooth.
        /// </summary>
        public abstract bool IsSupported
        {
            get;
        }

        /// <summary>
        /// Check if the device is currently scanning.
        /// </summary>
        public abstract bool IsScanning
        {
            get;
        }

        /// <summary>
        /// Check if the Bluetooth is currently enabled.
        /// </summary>
        public abstract bool IsEnabled
        {
            get;
        }

        /// <summary>
        /// Collection of devices discovered through scanning.
        /// </summary>
        public abstract ObservableCollection<IDevice> DiscoveredDevices
        {
            get;
            protected set;
        }

        /// <summary>
        /// Start scanning for devices.
        /// </summary>
        public abstract void StartDiscovery();

        /// <summary>
        /// Cancel the scanning.
        /// </summary>
        public abstract void CancelDiscovery();

        /// <summary>
        /// Disable the Bluetooth if possible, or open the bluetooth settings for the user to desactivate it manually.
        /// </summary>
        public abstract void DisableBluetooth();

        /// <summary>
        /// Enable the Bluetooth if possible, or open the Bluetooth settings for the user to activate it manually.
        /// </summary>
        public abstract void EnableBluetooth();

        /// <summary>
        /// Try to connect to the device in parameter.
        /// </summary>
        /// <param name="device">The device to connect with.</param>
        public abstract Task ConnectToDevice(IDevice device);

        /// <summary>
        /// Stop the current Bluetooth connexion.
        /// </summary>
        public abstract void Disconnect();

        /// <summary>
        /// Send an instruction to the devices connected with.
        /// </summary>
        /// <param name="instruction"></param>
        public abstract void SendInstruction(byte[] instruction);

        //comment
        protected abstract void PollInput();

        
    }
}
 