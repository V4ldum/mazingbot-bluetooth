using System;
using System.Text;

using Android.Bluetooth;
using Xamarin.Forms;
using Plugin.BLE;
using System.Collections.ObjectModel;
using BluetoothLibrary.Droid;
using Java.Util;
using Plugin.BLE.Abstractions.Contracts;
using System.Threading.Tasks;

[assembly: Dependency(typeof(AndroidBluetoothController))]
namespace BluetoothLibrary.Droid
{
    /// <summary>
    /// Represent a BluetoothController for Android.
    /// </summary>
    internal class AndroidBluetoothController : BluetoothController
    {
        protected const string BLUETOOTH_UUID_STRING = @"00001101-0000-1000-8000-00805f9b34fb";

        public override bool IsSupported
        {
            get => _btAdapter != null;
        }

        public override bool IsScanning
        {
            get => _adapter.IsScanning; 
        }

        public override bool IsEnabled
        {
            get
            {
                if (IsSupported)
                {
                    return _btAdapter.IsEnabled;
                }
                return false;
            }
        }

        public override ObservableCollection<IDevice> DiscoveredDevices
        {
            get => _discoveredDevices;
            protected set => _discoveredDevices = value;
        }
        protected ObservableCollection<IDevice> _discoveredDevices;

        protected IAdapter _adapter;
        protected BluetoothAdapter _btAdapter;
        protected BluetoothSocket _socket;

        /// <summary>
        /// Create an instance of BluetoothController for Android.
        /// </summary>
        public AndroidBluetoothController()
        {
            _discoveredDevices = new ObservableCollection<IDevice>();
            _adapter = CrossBluetoothLE.Current.Adapter;
            _btAdapter = BluetoothAdapter.DefaultAdapter;
        }

        public override void DisableBluetooth()
        {
            if (IsEnabled)
            {
                Disconnect();
                _btAdapter.Disable();
            }
        }

        public override void EnableBluetooth()
        {
            if (!IsEnabled)
            {
                _btAdapter.Enable();
            }
        }

        public override void StartDiscovery()
        {
            foreach (var d in _btAdapter.BondedDevices)
            {
                _discoveredDevices.Add(new AndroidDevice(d)); //filtre connecté ??
            }
        }

        public override async void CancelDiscovery()
        {
            if (_adapter.IsScanning)
            {
                await _adapter.StopScanningForDevicesAsync();
            }
        }

        public override async Task ConnectToDevice(IDevice device)
        {
            if (_socket != null)
            {
                OnErrorOccured(new ErrorEventArgs(ERROR_ALREADY_CONNECTED));
            }
            CancelDiscovery();
            BluetoothDevice d = _btAdapter.GetRemoteDevice(device.Address);
            _socket = d.CreateRfcommSocketToServiceRecord(UUID.FromString(BLUETOOTH_UUID_STRING));

            try
            {
                await _socket.ConnectAsync();
            }
            catch (Java.IO.IOException io)
            {
                OnErrorOccured(new ErrorEventArgs(ERROR_UNABLE_CONNECT_DEVICE + device.Name, io));
            }
            catch (Exception exception)
            { 
                 OnErrorOccured(new ErrorEventArgs(ERROR_UNHANDLED_ERROR, exception));
            }

            PollInput();
        }

        protected new void PollInput()
        {
            throw new NotImplementedException();
        }

        public override void Disconnect()
        {
            if (_socket != null)
            {
                _socket.Dispose();
                _socket = null;
            }
        }

        protected virtual void Cancel

        public override async void SendInstruction(byte[] data)
        {
            if (_socket == null)
            {
                throw new NullReferenceException(ERROR_NO_SOCKET_CREATED);
            }
            await _socket.OutputStream.WriteAsync(data, 0, data.Length);
        }
    }
}