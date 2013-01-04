/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

using Mosa.Platform.x86.Intrinsic;

namespace Mosa.Kernel.x86.RealModeEmulator
{
    public static partial class RealEmulator
    {
        public class Register
        {
            /// <summary>
            /// Memory address of the Register
            /// </summary>
            private uint address = 0u;

            /// <summary>
            /// Public unsigned integer register value
            /// </summary>
            public uint D
            {
                get { return Native.Get32(address); }
                set { Native.Set32(address, value); }
            }

            /// <summary>
            /// Public unsigned short register value
            /// </summary>
            public ushort W
            {
                get { return Native.Get16(address); }
                set { Native.Set16(address, value); }
            }

            /// <summary>
            /// Public unsigned short low byte register value
            /// </summary>
            public byte L
            {
                get { return Native.Get8(address); }
                set { Native.Set8(address, value); }
            }

            /// <summary>
            /// Public unsigned short high byte register value
            /// </summary>
            public byte H
            {
                get { return Native.Get8(address + 1); }
                set { Native.Set8(address + 1, value); }
            }

            /// <summary>
            /// Memory address of the register
            /// </summary>
            public uint Address
            {
                get { return (address); }
            }

            /// <summary>
            /// Memory address of the register's low 16bit high byte
            /// </summary>
            public uint AddressH
            {
                get { return (address + 1); }
            }

            public Register(uint address)
            {
                this.address = address;
            }
        }
    }
}