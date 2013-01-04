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
        public static unsafe uint Op_PUSH_Segment(ref State state, uint param)
        {
            ushort* ptr = Segment(ref state, param);
            state.SP.W -= 2;
            uint err = Int_Write16(ref state, state.SS, state.SP.W, (*ptr));
            if (err != ErrorCodes.ERR_OK) return err;
            return ErrorCodes.ERR_OK;
        }

        public static unsafe uint Op_POP_Segment(ref State state, uint param)
        {
            ushort* ptr = Segment(ref state, param);
            uint err = Int_Read16(ref state, state.SS, state.SP.W, ptr);
            if (err != ErrorCodes.ERR_OK) return err;
            state.SP.W += 2;
            return ErrorCodes.ERR_OK;
        }

        public static unsafe uint Op_PUSH_Reg(ref State state, uint param)
        {
            ushort* ptr = RegW(ref state, param);
            if (state.Decoder.bOverrideOperand) return ErrorCodes.ERR_UNDEFOPCODE;
            state.SP.W -= 2;
            uint err = Int_Write16(ref state, state.SS, state.SP.W, (*ptr));
            if (err != ErrorCodes.ERR_OK) return err;
            return ErrorCodes.ERR_OK;
        }

        public static unsafe uint Op_POP_Reg(ref State state, uint param)
        {
            ushort* ptr = RegW(ref state, param);
            if (state.Decoder.bOverrideOperand) return ErrorCodes.ERR_UNDEFOPCODE;
            uint err = Int_Read16(ref state, state.SS, state.SP.W, ptr);
            if (err != ErrorCodes.ERR_OK) return err;
            state.SP.W += 2;
            return ErrorCodes.ERR_OK;
        }

        public static unsafe uint Op_PUSH_A(ref State state, uint param)
        {
            uint err;
            ushort pt2;
            if (state.Decoder.bOverrideOperand) return ErrorCodes.ERR_UNDEFOPCODE;
            pt2 = state.SP.W;
            state.SP.W -= 2;
            err = Int_Write16(ref state, state.SS, state.SP.W, (state.AX.W));
            if (err != ErrorCodes.ERR_OK) return err;

            state.SP.W -= 2;
            err = Int_Write16(ref state, state.SS, state.SP.W, (state.CX.W));
            if (err != ErrorCodes.ERR_OK) return err;
            state.SP.W -= 2;
            err = Int_Write16(ref state, state.SS, state.SP.W, (state.DX.W));
            if (err != ErrorCodes.ERR_OK) return err; 
            state.SP.W -= 2;
            err = Int_Write16(ref state, state.SS, state.SP.W, (state.BX.W));
            if (err != ErrorCodes.ERR_OK) return err;
            state.SP.W -= 2;
            err = Int_Write16(ref state, state.SS, state.SP.W, (pt2));
            if (err != ErrorCodes.ERR_OK) return err;
            state.SP.W -= 2;
            err = Int_Write16(ref state, state.SS, state.SP.W, (state.BP.W));
            if (err != ErrorCodes.ERR_OK) return err;
            state.SP.W -= 2;
            err = Int_Write16(ref state, state.SS, state.SP.W, (state.SI.W));
            if (err != ErrorCodes.ERR_OK) return err;
            state.SP.W -= 2;
            err = Int_Write16(ref state, state.SS, state.SP.W, (state.DI.W));
            if (err != ErrorCodes.ERR_OK) return err;

            return ErrorCodes.ERR_OK;
        }

        public static unsafe uint Op_POP_A(ref State state, uint param)
        {
            uint err;
            if (state.Decoder.bOverrideOperand) return ErrorCodes.ERR_UNDEFOPCODE;
            err = Int_Read16(ref state, state.SS, state.SP.W, (ushort*)state.DI.Address);
            if (err != ErrorCodes.ERR_OK) return err;
            state.SP.W += 2;
            err = Int_Read16(ref state, state.SS, state.SP.W, (ushort*)state.SI.Address);
            if (err != ErrorCodes.ERR_OK) return err;
            state.SP.W += 2;
            err = Int_Read16(ref state, state.SS, state.SP.W, (ushort*)state.BP.Address);
            if (err != ErrorCodes.ERR_OK) return err;
            state.SP.W += 2;

            state.SP.W += 2;
            err = Int_Read16(ref state, state.SS, state.SP.W, (ushort*)state.BX.Address);
            if (err != ErrorCodes.ERR_OK) return err;
            state.SP.W += 2;
            err = Int_Read16(ref state, state.SS, state.SP.W, (ushort*)state.DX.Address);
            if (err != ErrorCodes.ERR_OK) return err;
            state.SP.W += 2;
            err = Int_Read16(ref state, state.SS, state.SP.W, (ushort*)state.CX.Address);
            if (err != ErrorCodes.ERR_OK) return err;
            state.SP.W += 2;
            err = Int_Read16(ref state, state.SS, state.SP.W, (ushort*)state.AX.Address);
            if (err != ErrorCodes.ERR_OK) return err;
            state.SP.W += 2;

            return ErrorCodes.ERR_OK;
        }

        public static unsafe uint Op_PUSH_F(ref State state, uint param)
        {
            state.SP.W -= 2;
            uint err = Int_Write16(ref state, state.SS, state.SP.W, (state.Flags));
            if (err != ErrorCodes.ERR_OK) return err;
            return ErrorCodes.ERR_OK;
        }

        public static unsafe uint Op_POP_F(ref State state, uint param)
        {
            ushort tmp;
            const ushort keep_mask = 0x7002;
            const ushort set_mask = 0x0FD5;
            uint err = Int_Read16(ref state, state.SS, state.SP.W, &tmp);
            if (err != ErrorCodes.ERR_OK) return err;
            state.SP.W += 2;

            state.Flags &= keep_mask;
            tmp &= set_mask;
            state.Flags |= (ushort)(tmp | 2);
            return ErrorCodes.ERR_OK;
        }

        public static unsafe uint Op_PUSH_MX(ref State state, uint param)
        {
            if (state.Decoder.bOverrideOperand) return ErrorCodes.ERR_UNDEFOPCODE;
            return ErrorCodes.ERR_UNDEFOPCODE;
        }
        public static unsafe uint Op_POP_MX(ref State state, uint param)
        {
            uint err;
            ushort* ptr;
            if (state.Decoder.bOverrideOperand) return ErrorCodes.ERR_UNDEFOPCODE;

            err = Int_ParseModRMX(ref state, null, &ptr, 0);
            if (err != ErrorCodes.ERR_OK) return err;
            err = Int_Read16(ref state, state.SS, state.SP.W, ptr);
            if (err != ErrorCodes.ERR_OK) return err;
            state.SP.W += 2;

            return ErrorCodes.ERR_OK;
        }

        public static unsafe uint Op_PUSH_I(ref State state, uint param)
        {
            ushort val;
            uint err = Int_Read16(ref state, state.CS, (ushort)(state.IP + state.Decoder.IPOffset), &val);
            if (err != ErrorCodes.ERR_OK) return err;
            state.Decoder.IPOffset += 2;
            
            state.SP.W -= 2;
            err = Int_Write16(ref state, state.SS, state.SP.W, (val));
            if (err != ErrorCodes.ERR_OK) return err;

            return ErrorCodes.ERR_OK;
        }

        public static unsafe uint Op_PUSH_I8(ref State state, uint param)
        {
            byte val;
            uint err = Int_Read16(ref state, state.CS, (ushort)(state.IP + state.Decoder.IPOffset), (ushort*)&val);
            if (err != ErrorCodes.ERR_OK) return err;
            state.Decoder.IPOffset += 2;
            
            state.SP.W -= 2;
            err = Int_Write16(ref state, state.SS, state.SP.W, (val));
            if (err != ErrorCodes.ERR_OK) return err;

            return ErrorCodes.ERR_OK;
        }
    }
}