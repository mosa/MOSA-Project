/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using Mosa.DeviceDrivers;
using Mosa.ClassLib;

namespace Mosa.Emulator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Set Device Driver system to the emulator port method
            DeviceDrivers.Kernel.HAL.SetCreatePortMethod(Mosa.Emulator.EmulatedPorts.RegisterPort);

            // Start the emulated devices
            EmulatedDevices.Setup.Initialize();

            // Start driver system
            DeviceDrivers.Setup.Initialize();

            LinkedList<IDevice> devices = DeviceDrivers.Setup.DeviceManager.GetAllDevices();

            foreach (IDevice device in devices) {
                Console.WriteLine(device.Name);
            }

            return;
        }
    }
}
