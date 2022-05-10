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
using BluetoothLibrary.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidFactory))]
namespace BluetoothLibrary.Droid
{
    public class AndroidFactory : BluetoothControllerFactory
    {
        private const string ERROR_UNKNOWN_ROBOT = "Unknown robot type.";

        private Dictionary<RobotType, Func<BluetoothController>> dictionary = new Dictionary<RobotType, Func<BluetoothController>>
        {
            {RobotType.EV3, () => new AndroidEV3BluetoothController() }
        };

        public override BluetoothController CreateBluetoothController(RobotType robot)
        {
            dictionary.TryGetValue(robot, out Func<BluetoothController> value);

            if (value == null) throw new InvalidOperationException(ERROR_UNKNOWN_ROBOT);

            return value();
        }
    }
}