/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;

namespace Mosa.EmulatedDevices
{
    public static class IOPorts
    {
        public static void RegisterPort(IOPort<byte> port)
        {
            EmulatedIOPorts.SetRead8(port.Port, port.ReadValue);
            EmulatedIOPorts.SetWrite8(port.Port, port.SetValue);
        }

        public static void RegisterPort(IOPort<ushort> port)
        {
            EmulatedIOPorts.SetRead16(port.Port, port.ReadValue);
            EmulatedIOPorts.SetWrite16(port.Port, port.SetValue);
        }

        public static void RegisterPort(IOPort<uint> port)
        {
            EmulatedIOPorts.SetRead32(port.Port, port.ReadValue);
            EmulatedIOPorts.SetWrite32(port.Port, port.SetValue);
        }

        public static void UnregisterPort(IOPort<byte> port)
        {
            EmulatedIOPorts.SetRead8(port.Port, null);
            EmulatedIOPorts.SetWrite8(port.Port, null);
        }

        public static void UnregisterPort(IOPort<ushort> port)
        {
            EmulatedIOPorts.SetRead16(port.Port, null);
            EmulatedIOPorts.SetWrite16(port.Port, null);
        }

        public static void UnregisterPort(IOPort<uint> port)
        {
            EmulatedIOPorts.SetRead32(port.Port, null);
            EmulatedIOPorts.SetWrite32(port.Port, null);
        }
    }
}
