using BluetoothLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BluetoothLibrary
{
    public partial class MainPage : ContentPage
    {
        private BluetoothController _btController;

        public MainPage()
        {
            InitializeComponent();

            _btController = BluetoothControllerFactory.Factory.CreateBluetoothController(RobotType.EV3);

            listViewBluetooth.ItemsSource = _btController.DiscoveredDevices;

            ViewComponentsInitialization();

            _btController.ErrorOccured += _btController_ErrorOccured;
        }

        private void _btController_ErrorOccured(object sender, ErrorEventArgs e)
        {
            DisplayAlert("Error", e.Message, "Ok");
        }

        private void ViewComponentsInitialization()
        {
            if (!_btController.IsSupported)
            {
                BluetoothButton.IsEnabled = false;
                SendButton.IsEnabled = false;
                LabelBluetooth.IsVisible = true;
                ScanningButton.IsEnabled = false;
            }
            else
            {
                if (_btController.IsEnabled)
                {
                    BluetoothButton.Text = "Disable Bluetooth";
                }
            }
            //_btController.CancelDiscovery();
        }

        private void ButtonBluetooth_Clicked(object sender, EventArgs e)
        {
            if (_btController.IsEnabled) //Bluetooth currently enabled
            {
                BluetoothButton.Text = "Enable Bluetooth";

                _btController.CancelDiscovery();
                _btController.DisableBluetooth();
                ScanningButton.IsEnabled = false;
            }
            else //Bluetooth currently disabled
            {
                BluetoothButton.Text = "Disable Bluetooth";

                _btController.EnableBluetooth();
                ScanningButton.IsEnabled = true;
            }
        }

        private void ScanningButton_Clicked(object sender, EventArgs e)
        {
            if(!_btController.IsScanning)
            {
                _btController.StartDiscovery();
            }
        }

        private void SendButton_Clicked(object sender, EventArgs e)
        {
            string message = EntryBluetooth.Text;
            DisplayAlert("Message", (message == "") ? "None" : message, "OK");
        }

        private async void ListViewBluetooth_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
  
            await _btController.ConnectToDevice(listViewBluetooth.SelectedItem as IDevice);

            
            _stream = new MemoryStream();
            _writer = new BinaryWriter(_stream);


            TestSendInstruction();
            //PlayToneAsync(100, 387, 250);
            //TurnMotorAtSpeedForTime(5, 5000, false);
            
        }

        private void Disconnect_Clicked(object sender, EventArgs e)
        {
            _btController.Disconnect();
        }


        /************************/
        private MemoryStream _stream;
        private BinaryWriter _writer;

        void TestSendInstruction()
        {
            //Play Sound
            _btController.SendInstruction(new byte[] { 15, 0, 4, 0, 128, 0, 0, 148, 1, 129, 100, 130, 75, 2, 130, 250, 0});

            //Motor
            _btController.SendInstruction(new byte[] { 29,0,3,0,128,0,0,173,129,0,129,1,129,100,131,0,0,0,0,131,208,7,0,0,131,0,0,0,0,129,1});
        }

        void PlayToneAsync(int volume, ushort frequency, ushort duration)
        {
            PlayToneAsyncInternal(volume, frequency, duration);
        }

        internal void PlayToneAsyncInternal(int volume, ushort frequency, ushort duration)
        {
            PlayTone(volume, frequency, duration);
           _btController.SendInstruction(ToBytes());
        }

        public void PlayTone(int volume, ushort frequency, ushort duration)
        {
            if (volume < 0 || volume > 100)
                throw new ArgumentException("Volume must be between 0 and 100", "volume");

            AddOpcode(0x9401);
            AddParameter((byte)volume);     // volume
            AddParameter(frequency);    // frequency
            AddParameter(duration); // duration (ms)
        }

        internal void AddOpcode(int opcode)
        {
            // 1 or 2 bytes (opcode + subcmd, if applicable)
            // I combined opcode + sub into ushort where applicable, so we need to pull them back apart here
            if (opcode > 0xff)
                _writer.Write((byte)((ushort)opcode >> 8));
            _writer.Write((byte)opcode);
        }

        internal void AddParameter(byte parameter)
        {
            // 0x81 = long format, 1 byte
            _writer.Write((byte)0x81);
            _writer.Write(parameter);
        }

        internal void AddParameter(ushort parameter)
        {
            // 0x82 = long format, 2 bytes
            _writer.Write((byte)0x82);
            _writer.Write(parameter);
        }

        internal byte[] ToBytes()
        {
            byte[] buff = _stream.ToArray();

            // size of data, not including the 2 size bytes
            ushort size = (ushort)(buff.Length - 2);

            // little-endian
            buff[0] = (byte)size;
            buff[1] = (byte)(size >> 8);

            return buff;
        }


        public void TurnMotorAtSpeedForTime(int speed, uint milliseconds, bool brake)
        {
            TurnMotorAtSpeedForTime(0x01, speed, 0, milliseconds, 0, brake);
        }

        public void TurnMotorAtSpeedForTime(int ports, int speed, uint msRampUp, uint msConstant, uint msRampDown, bool brake)
        {
            if (speed < -100 || speed > 100)
                throw new ArgumentException("Speed must be between -100 and 100 inclusive.", "speed");

            AddOpcode(0xaf);
            AddParameter(0x00);         // layer
            AddParameter((byte)ports);  // ports
            AddParameter((byte)speed);      // power
            AddParameter(msRampUp);     // step1
            AddParameter(msConstant);       // step2
            AddParameter(msRampDown);       // step3
            AddParameter((byte)(brake ? 0x01 : 0x00));      // brake (0 = coast, 1 = brake)
        }

        internal void AddParameter(short parameter)
        {
            // 0x82 = long format, 2 bytes
            _writer.Write((byte)0x82);
            _writer.Write(parameter);
        }

        internal void AddParameter(uint parameter)
        {
            // 0x83 = long format, 4 bytes
            _writer.Write((byte)0x83);
            _writer.Write(parameter);
        }
    }
}
