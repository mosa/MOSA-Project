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
        public unsafe struct State
        {
            /// <summary>
            /// General Registers - AX
            /// </summary>
            public Register AX
            {
                get { return Registers[GPRegistersX.AX]; }
            }

            /// <summary>
            /// General Registers - CX
            /// </summary>
            public Register CX
            {
                get { return Registers[GPRegistersX.CX]; }
            }

            /// <summary>
            /// General Registers - DX
            /// </summary>
            public Register DX
            {
                get { return Registers[GPRegistersX.CX]; }
            }

            /// <summary>
            /// General Registers - BX
            /// </summary>
            public Register BX
            {
                get { return Registers[GPRegistersX.BX]; }
            }

            /// <summary>
            /// General Registers - SP
            /// </summary>
            public Register SP
            {
                get { return Registers[GPRegistersX.SP]; }
            }

            /// <summary>
            /// General Registers - BP
            /// </summary>
            public Register BP
            {
                get { return Registers[GPRegistersX.BP]; }
            }

            /// <summary>
            /// General Registers - SI
            /// </summary>
            public Register SI
            {
                get { return Registers[GPRegistersX.SI]; }
            }

            /// <summary>
            /// General Registers - DI
            /// </summary>
            public Register DI
            {
                get { return Registers[GPRegistersX.DI]; }
            }

            /// <summary>
            /// General Registers
            /// </summary>
            public Register[] Registers;

            /// <summary>
            /// Memory address for segments
            /// </summary>
            public uint SegmentAddress = 0x0u;
            public uint AddressSS { get { return SegmentAddress; } }
            public uint AddressDS { get { return SegmentAddress + 4; } }
            public uint AddressES { get { return SegmentAddress + 8; } }
            public uint AddressCS { get { return SegmentAddress + 12; } }
            public uint AddressIP { get { return SegmentAddress + 16; } }

            /// <summary>
            /// Stack Segment
            /// </summary>
            public ushort SS
            {
                get { return Native.Get16(SegmentAddress); }
                set { Native.Set16(SegmentAddress, value); }
            }

            /// <summary>
            /// Data Segment
            /// </summary>
            public ushort DS
            {
                get { return Native.Get16(SegmentAddress + 4); }
                set { Native.Set16(SegmentAddress + 4, value); }
            }

            /// <summary>
            /// Extra Segment
            /// </summary>
            public ushort ES
            {
                get { return Native.Get16(SegmentAddress + 8); }
                set { Native.Set16(SegmentAddress + 8, value); }
            }

            /// <summary>
            /// Code Segment
            /// </summary>
            public ushort CS
            {
                get { return Native.Get16(SegmentAddress + 12); }
                set { Native.Set16(SegmentAddress + 12, value); }
            }

            /// <summary>
            /// Instruction Pointer
            /// </summary>
            public ushort IP
            {
                get { return Native.Get16(SegmentAddress + 16); }
                set { Native.Set16(SegmentAddress + 16, value); }
            }

            /// <summary>
            /// State Flags
            /// </summary>
            public ushort Flags;

            /// <summary>
            /// 1MB of mode address space for the emulator
            /// </summary>
            public unsafe uint* Memory;

            /// <summary>
            /// Total Executed Instructions
            /// </summary>
            public uint InstrNum;

            /// <summary>
            /// High Level Emulation Callback
            /// </summary>
            /// <param name="state">The state instance</param>
            /// <param name="intNum">The interrupt number</param>
            /// <returns>1 if the call was handled, 0 if it should be emulated</returns>
            public static delegate uint HLECallBack(ref State state, uint number);
            public HLECallBack[] HLECallbacks = new HLECallBack[256];

            public bool bWasLastOperationNull;

            /// <summary>
            /// Decoder State
            /// </summary>
            public struct DecoderStruct
            {
                public short OverrideSegment;
                public ushort RepeatType;
                public bool bOverrideOperand;
                public bool bOverrideAddress;
                public bool bDontChangeIP;
                public ushort IPOffset;
            }

            /// <summary>
            /// Decoder State instance
            /// </summary>
            public DecoderStruct Decoder = new DecoderStruct();
        }
    }
}