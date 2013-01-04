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
        public static uint Op_Flag_CLC(ref State state, uint param)
        {
            ushort flag = 0x001;
            state.Flags &= (ushort)~flag;
            return ErrorCodes.ERR_OK;
        }
        public static uint Op_Flag_STC(ref State state, uint param)
        {
            state.Flags |= 0x001;
            return ErrorCodes.ERR_OK;
        }
        public static uint Op_Flag_CLI(ref State state, uint param)
        {
            ushort flag = 0x200;
            state.Flags &= (ushort)~flag;
            return ErrorCodes.ERR_OK;
        }
        public static uint Op_Flag_STI(ref State state, uint param)
        {
            state.Flags |= 0x200;
            return ErrorCodes.ERR_OK;
        }
        public static uint Op_Flag_CLD(ref State state, uint param)
        {
            ushort flag = 0x400;
            state.Flags &= (ushort)~flag;
            return ErrorCodes.ERR_OK;
        }
        public static uint Op_Flag_STD(ref State state, uint param)
        {
            state.Flags |= 0x400;
            return ErrorCodes.ERR_OK;
        }
        public static uint Op_Flag_CMC(ref State state, uint param)
        {
            state.Flags ^= 0x001;
            return ErrorCodes.ERR_OK;
        }
        public static uint Op_Flag_SAHF(ref State state, uint param)
        {
            byte mask = 0xD5;
            state.Flags &= (ushort)~mask;
            state.Flags |= (ushort)(state.AX.H & mask);
            return ErrorCodes.ERR_OK;
        }
        public static uint Op_Flag_LAHF(ref State state, uint param)
        {
            state.AX.H = (byte)(state.Flags & 0xFF);
            return ErrorCodes.ERR_OK;
        }
    }
}