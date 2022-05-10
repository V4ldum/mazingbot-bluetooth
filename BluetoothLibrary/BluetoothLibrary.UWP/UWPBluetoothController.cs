using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using BluetoothLibrary;
using System.Collections.ObjectModel;
using BluetoothLibrary.UWP;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using System.Threading;
using Windows.Devices.Enumeration;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.System.Threading;
using Windows.Devices.Bluetooth;
using Windows.System;
using System.IO;

[assembly: Dependency(typeof(UWPBluetoothController))]
namespace BluetoothLibrary.UWP
{
    /// <summary>
    /// Represent a BluetoothController for Windows UWP.
    /// </summary>
    internal class UWPBluetoothController : BluetoothController
    {   //TODO literals
        private const string BLUETOOTH_URI_STRING = @"ms-settings-bluetooth:";

        public event EventHandler<ErrorEventArgs> ErrorOccured;

        private StreamSocket _socket;
        private DataReader _reader;
        private CancellationTokenSource _pollToken;
        private CancellationTokenSource _scanToken;             
        private DeviceWatcher _watcher;

        public UWPBluetoothController()
        {
            _discoveredDevices = new ObservableCollection<IDevice>();

            string[] requestedProperties = new string[] { "System.Devices.Aep.DeviceAddress", "System.Devices.Aep.IsConnected" };
            _watcher = DeviceInformation.CreateWatcher("(System.Devices.Aep.ProtocolId:=\"{e0cbf06c-cd8b-4647-bb8a-263b43f0f974}\")",
                                                       requestedProperties,
                                                       DeviceInformationKind.AssociationEndpoint);
        }

        //TODO check
        public override bool IsSupported
        {
            get => true;//throw new NotImplementedException();
        }

        public override bool IsScanning
        {
            get => _watcher.Status == DeviceWatcherStatus.Started;
        }

        //TODO 
        public override bool IsEnabled
        {
            get => true;// throw new NotImplementedException();
        }

        private ObservableCollection<IDevice> _discoveredDevices;
        public override ObservableCollection<IDevice> DiscoveredDevices
        {
            get => _discoveredDevices;
            protected set => _discoveredDevices = value;
        }

        public override void OnErrorOccured(ErrorEventArgs e) => ErrorOccured?.Invoke(this, e);

        public async override void StartDiscovery()
        {
            string selector = RfcommDeviceService.GetDeviceSelector(RfcommServiceId.SerialPort);
            DeviceInformationCollection devices = await DeviceInformation.FindAllAsync(selector);

            foreach(var d in devices)
            {
                _discoveredDevices.Add(new UWPDevice(d));
            }
        }

        //TODO
        public override void CancelDiscovery()
        {
            return;
            //_scanToken.Cancel();
            throw new NotImplementedException();
        }

        //TODO check
        public override async void DisableBluetooth()
        {
            var uri = new Uri(BLUETOOTH_URI_STRING);
            await Launcher.LaunchUriAsync(uri);
        }

        //TODO check
        public override async void EnableBluetooth()
        {
            var uri = new Uri(BLUETOOTH_URI_STRING);
            await Launcher.LaunchUriAsync(uri);
        }

        //TODO check
        public async override Task ConnectToDevice(IDevice device)
        {
            if (_socket != null)
                OnErrorOccured(new ErrorEventArgs(ERROR_ALREADY_CONNECTED));
            //CancelDiscovery();
            //wtf ?
            if (device == null)
                OnErrorOccured(new ErrorEventArgs(ERROR_UNABLE_CONNECT_DEVICE + device.Name));

            //Quelle possibilité d'erreur ?
            RfcommDeviceService service = await RfcommDeviceService.FromIdAsync(device.Address);
            if (service == null)
                throw new Exception("Unable to connect to LEGO EV3 brick...is the manifest set properly?");

            _socket = new StreamSocket();
            try
            {
                await _socket.ConnectAsync(service.ConnectionHostName, service.ConnectionServiceName,
                                           SocketProtectionLevel.BluetoothEncryptionAllowNullAuthentication);
            }
            catch(IOException io)
            {
                OnErrorOccured(new ErrorEventArgs(ERROR_UNABLE_CONNECT_DEVICE + device.Name, io));
            }
            catch(Exception exception)
            {
                OnErrorOccured(new ErrorEventArgs(ERROR_UNHANDLED_ERROR, exception));
            }

            _reader = new DataReader(_socket.InputStream)
            {
                ByteOrder = ByteOrder.LittleEndian
            };

            //await ThreadPool.RunAsync(PollInput);
        }

        //TODO
        public override void Disconnect()
        {
            //pollinput
            if(_socket != null)
            {
                _socket.Dispose();
                _socket = null;
            }
        }

        public async override void SendInstruction(byte[] instruction)
        {
            if (_socket == null)
                throw new NullReferenceException(ERROR_NO_SOCKET_CREATED);

            await _socket.OutputStream.WriteAsync(instruction.AsBuffer());
        }
    }
}
