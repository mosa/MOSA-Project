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
    public static class IOPort
    {
        public static void RegisterPort(EmulatedIOPort<byte> port) { }
        public static void RegisterPort(EmulatedIOPort<ushort> port) { }
        public static void RegisterPort(EmulatedIOPort<uint> port) { }
        public static void UnregisterPort(EmulatedIOPort<byte> port) { }
        public static void UnregisterPort(EmulatedIOPort<ushort> port) { }
        public static void UnregisterPort(EmulatedIOPort<uint> port) { }
    }
}
