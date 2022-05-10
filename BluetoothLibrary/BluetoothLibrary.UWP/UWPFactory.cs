using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BluetoothLibrary;
using BluetoothLibrary.UWP;
using Xamarin.Forms;

[assembly: Dependency(typeof(UWPFactory))]
namespace BluetoothLibrary.UWP
{
    public class UWPFactory : BluetoothControllerFactory
    {
        private const string ERROR_UNKNOWN_ROBOT = "Unknown robot type.";

        private Dictionary<RobotType, Func<BluetoothController>> dictionary = new Dictionary<RobotType, Func<BluetoothController>>
        {
            {RobotType.EV3, () => new UWPEV3BluetoothController() }
        };

        public override BluetoothController CreateBluetoothController(RobotType robot)
        {
            dictionary.TryGetValue(robot, out Func<BluetoothController> value);

            if (value == null) throw new InvalidOperationException(ERROR_UNKNOWN_ROBOT);

            return value();
        }
    }
}