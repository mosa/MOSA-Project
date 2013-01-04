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
        public static uint Op_CALL_N(ref State state, uint param)
        {
            uint err;
            ushort dist = 0;

            err = Int_Read16(state, state.CS, (ushort)(state.IP + state.Decoder.IPOffset), ref dist);
            if (err != ErrorCodes.ERR_OK) return err;

            state.Decoder.IPOffset += 2;
            state.SP.W -= 2;

            err = Int_Write16(state, state.SS, state.SP.W, (ushort)(state.IP + state.Decoder.IPOffset));
            if (err != ErrorCodes.ERR_OK) return err;

            state.IP += dist;
            return ErrorCodes.ERR_OK;
        }

        public static uint Op_CALL_F(ref State state, uint param)
        {
            uint err;
            ushort ofs = 0, seg = 0;
            err = Int_Read16(state, state.CS, (ushort)(state.IP + state.Decoder.IPOffset), ref ofs);
            if (err != ErrorCodes.ERR_OK) return err;
            state.Decoder.IPOffset += 2;

            err = Int_Read16(state, state.CS, (ushort)(state.IP + state.Decoder.IPOffset), ref seg);
            if (err != ErrorCodes.ERR_OK) return err;
            state.Decoder.IPOffset += 2;

            state.SP.W -= 2;
            err = Int_Write16(state, state.SS, state.SP.W, (state.CS));
            if (err != ErrorCodes.ERR_OK) return err;

            state.SP.W -= 2;
            err = Int_Write16(state, state.SS, state.SP.W, (ushort)(state.IP + state.Decoder.IPOffset));
            if (err != ErrorCodes.ERR_OK) return err;
            state.CS = seg;
            state.IP = ofs;
            state.Decoder.bDontChangeIP = true;
            return ErrorCodes.ERR_OK;
        }

        public static uint Op_RET_N(ref State state, uint param)
        {
            ushort ofs = 0;
            uint err = Int_Read16(state, state.SS, state.SP.W, ref ofs);
            if (err != ErrorCodes.ERR_OK) return err;
            state.SP.W += 2;

            state.IP = ofs;
            state.Decoder.bDontChangeIP = true;
            return ErrorCodes.ERR_OK;
        }

        public static uint Op_RET_F(ref State state, uint param)
        {
            uint err = 0;
            ushort ofs = 0, seg = 0;
            err = Int_Read16(state, state.SS, state.SP.W, ref ofs);
            if (err != ErrorCodes.ERR_OK) return err;
            state.SP.W += 2;

            err = Int_Read16(state, state.SS, state.SP.W, ref seg);
            if (err != ErrorCodes.ERR_OK) return err;
            state.SP.W += 2;

            state.CS = seg;
            state.IP = ofs;
            state.Decoder.bDontChangeIP = true;
            return ErrorCodes.ERR_OK;
        }

        public static uint Op_RET_iN(ref State state, uint param)
        {
            uint err;
            ushort ofs = 0;
            ushort popSize = 0;
            err = Int_Read16(state, state.CS, (ushort)(state.IP + state.Decoder.IPOffset), ref popSize);
            if (err != ErrorCodes.ERR_OK) return err;
            state.Decoder.IPOffset += 2;

            err = Int_Read16(state, state.SS, state.SP.W, ref ofs);
            if (err != ErrorCodes.ERR_OK) return err;
            state.SP.W += 2;

            state.IP = ofs;
            state.SP.W += popSize;
            state.Decoder.bDontChangeIP = true;
            return ErrorCodes.ERR_OK;
        }

        public static uint Op_RET_iF(ref State state, uint param)
        {
            uint err;
            ushort ofs = 0, seg = 0;
            ushort popSize = 0;
            err = Int_Read16(state, state.CS, (ushort)(state.IP + state.Decoder.IPOffset), ref popSize);
            if (err != ErrorCodes.ERR_OK) return err;
            state.Decoder.IPOffset += 2;

            err = Int_Read16(state, state.SS, state.SP.W, ref ofs);
            if (err != ErrorCodes.ERR_OK) return err;
            state.SP.W += 2;

            err = Int_Read16(state, state.SS, state.SP.W, ref seg);
            if (err != ErrorCodes.ERR_OK) return err;
            state.SP.W += 2;

            state.CS = seg;
            state.IP = ofs;
            state.SP.W += popSize;
            state.Decoder.bDontChangeIP = true;
            return ErrorCodes.ERR_OK;
        }

        public static unsafe uint _CallInterrupt(ref State state, uint number)
        {
            uint err;
            ushort seg = 0, ofs = 0;

            err = Int_Read16(state, 0, (ushort)(number * 4 + 0), ref ofs);
            if (err != ErrorCodes.ERR_OK) return err;
            err = Int_Read16(state, 0, (ushort)(number * 4 + 2), ref seg);
            if (err != ErrorCodes.ERR_OK) return err;

            if (ofs == 0 && seg == 0)
            {
                //printf(" Caught attempt to execute IVT pointing to 0000:0000");
                return ErrorCodes.ERR_BADMEM;
            }

            if (seg == 0xB800 && ofs == number)
            {
                //if( state.HLECallbacks[Num] )
                    state.HLECallbacks[number](state, number);
                return ErrorCodes.ERR_OK;
            }

            state.SP.W -= 2;
            err = Int_Write16(state, state.SS, state.SP.W, (state.Flags));
            if (err != ErrorCodes.ERR_OK) return err;

            state.SP.W -= 2;
            err = Int_Write16(state, state.SS, state.SP.W, (state.CS));
            if (err != ErrorCodes.ERR_OK) return err;

            state.SP.W -= 2;
            err = Int_Write16(state, state.SS, state.SP.W, (ushort)(state.IP + state.Decoder.IPOffset));
            if (err != ErrorCodes.ERR_OK) return err;

            state.IP = ofs;
            state.CS = seg;
            ushort flag = 0x200;
            flag |= 0x100;
            state.Flags &= (ushort)~flag;

            state.Decoder.bDontChangeIP = true;

            return ErrorCodes.ERR_OK;
        }


        public static uint Op_INT_3(ref State state, uint param)
        {
            return _CallInterrupt(ref state, 3);
        }

        public static uint Op_INT_I(ref State state, uint param)
        {
            byte num = 0;
            uint err = Int_Read8(state, state.CS, (ushort)(state.IP + state.Decoder.IPOffset), ref num);
            if (err != ErrorCodes.ERR_OK) return err;
            state.Decoder.IPOffset++;
            return _CallInterrupt(ref state, num);
        }

        public static uint Op_INTO_z(ref State state, uint param)
        {
            if ((state.Flags & 0x800) == 0x800)
                return _CallInterrupt(ref state, 4);
            else
                return ErrorCodes.ERR_OK;
        }

        public static uint Op_IRET_z(ref State state, uint param)
        {
            uint err = 0;
            ushort v = 0;
            err = Int_Read16(state, state.SS, state.SP.W, ref v);
            if (err != ErrorCodes.ERR_OK) return err;
            state.SP.W += 2;
            state.IP = v;

            err = Int_Read16(state, state.SS, state.SP.W, ref v);
            if (err != ErrorCodes.ERR_OK) return err;
            state.SP.W += 2;
            state.CS = v;

            err = Int_Read16(state, state.SS, state.SP.W, ref v);
            if (err != ErrorCodes.ERR_OK) return err;
            state.SP.W += 2;
            state.Flags = v;

            state.Decoder.bDontChangeIP = true;
            return ErrorCodes.ERR_OK;
        }
    }
}