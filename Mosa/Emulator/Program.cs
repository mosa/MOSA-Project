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
using Mosa.DeviceDrivers.PCI;
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
            Console.WriteLine("Device: ");
            foreach (IDevice device in devices) {

                Console.Write(device.Name);
                Console.Write(" [");

                switch (device.Status) {
                    case DeviceStatus.Error: Console.Write("Error"); break;
                    case DeviceStatus.Offline: Console.Write("Offline"); break;
                    case DeviceStatus.Online: Console.Write("Online"); break;
                    case DeviceStatus.Initializing: Console.Write("Initializing"); break;
                    case DeviceStatus.NotFound: Console.Write("Not Found"); break;
                }
                Console.Write("]");

                if (device.Parent != null) {
                    Console.Write(" - Parent: ");
                    Console.Write(device.Parent.Name);
                }
                Console.WriteLine();

                if (device is PCIDevice) {
                    PCIDevice pciDevice = (device as PCIDevice);

                    Console.Write("  Vendor:0x");
                    Console.Write(pciDevice.VendorID.ToString("X"));
                    Console.Write(" Device:0x");
                    Console.Write(pciDevice.DeviceID.ToString("X"));
                    Console.Write(" Class:0x");
                    Console.Write(pciDevice.ClassCode.ToString("X"));
                    Console.Write(" Rev:0x");
                    Console.Write(pciDevice.RevisionID.ToString("X"));
                    Console.WriteLine();

                    foreach (PCIBaseAddress address in pciDevice.BaseAddresses) {
                        if (address.Address != 0) {
                            Console.Write("    ");
                            //Console.WriteLine (address.ToString ());

                            if (address.Region == AddressRegion.IO)
                                Console.Write("I/O Port at 0x");
                            else
                                Console.Write("Memory at 0x");

                            Console.Write(address.Address.ToString("X"));

                            Console.Write(" [size=");

                            if ((address.Size & 0xFFFFF) == 0) {
                                Console.Write((address.Size >> 20).ToString());
                                Console.Write("M");
                            }
                            else if ((address.Size & 0x3FF) == 0) {
                                Console.Write((address.Size >> 10).ToString());
                                Console.Write("K");
                            }
                            else
                                Console.Write(address.Size.ToString());

                            Console.Write("]");

                            if (address.Prefetchable)
                                Console.Write("(prefetchable)");

                            Console.WriteLine();
                        }
                    }

                    if (pciDevice.IRQ != 0) {
                        Console.Write("    ");
                        Console.Write("IRQ at ");
                        Console.Write(pciDevice.IRQ.ToString());
                        Console.WriteLine();
                    }
                }
            }

            return;
        }
    }
}
