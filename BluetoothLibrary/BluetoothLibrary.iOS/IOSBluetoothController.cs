using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using BluetoothLibrary;
using Xamarin.Forms;
using Plugin.BLE;
using System.Collections.ObjectModel;
using Plugin.BLE.Abstractions.Contracts;
using BluetoothLibrary.iOS;
using CoreBluetooth;

[assembly: Dependency(typeof(IOSBluetoothController))]
namespace BluetoothLibrary.iOS
{
    /// <summary>
    /// Represent a BluetoothController for iOS.
    /// </summary>
    internal sealed class IOSBluetoothController : BluetoothController
    {
        public bool IsSupported
        {
            get { return _ble.IsAvailable; }
            private set { }
        }

        public bool IsScanning
        {
            get => false;
            private set { }
        }

        public bool IsEnabled
        {
            get { return _ble.IsOn; }
            private set { }
        }

        public ObservableCollection<IDevice> DiscoveredDevices
        {
            get { return discoveredDevices; }
            private set { discoveredDevices = value; }
        }
        private ObservableCollection<IDevice> discoveredDevices;


        IBluetoothLE _ble;
        IAdapter _adapter;

        /// <summary>
        /// Create an instance of BluetoothController for iOS.
        /// </summary>
        public IOSBluetoothController()
        {
            discoveredDevices = new ObservableCollection<IDevice>();
            _ble = CrossBluetoothLE.Current;
            _adapter = _ble.Adapter;
        }


        public void DisableBluetooth()
        {
            var uri = new NSUrl("App-Prefs:root=Bluetooth");
            UIApplication.SharedApplication.OpenUrl(uri); //To be tested !
        }

        public void EnableBluetooth()
        {
            var uri = new NSUrl("App-Prefs:root=Bluetooth");
            UIApplication.SharedApplication.OpenUrl(uri); //To be tested !
        }

        public async void StartDiscovery()
        {
            if (!_adapter.IsScanning)
            {
                discoveredDevices.Clear();

                _adapter.DeviceDiscovered += (s, a) =>
                {
                    discoveredDevices.Add(a.Device);
                };

                await _adapter.StartScanningForDevicesAsync();
            }
        }

        public async void CancelDiscovery()
        {
            if (_adapter.IsScanning)
            {
                await _adapter.StopScanningForDevicesAsync();
            }
        }
    }
}