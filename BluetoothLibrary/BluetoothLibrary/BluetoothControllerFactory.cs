using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BluetoothLibrary
{
    public interface IBluetoothControllerFactory
    {
        BluetoothController CreateBluetoothController(RobotType robot);
    }
    /// <summary>
    /// Factory for BluetoothController class.
    /// </summary>
    public abstract class BluetoothControllerFactory : IBluetoothControllerFactory
    {
        /// <summary>
        /// Used to create the correct instance of controller in function of the operating system.
        /// </summary>
        /// <returns>An instance of controller</returns>
        public abstract BluetoothController CreateBluetoothController(RobotType robot);

        public static IBluetoothControllerFactory Factory { get { return DependencyService.Get<IBluetoothControllerFactory>(); } }
    }
}
