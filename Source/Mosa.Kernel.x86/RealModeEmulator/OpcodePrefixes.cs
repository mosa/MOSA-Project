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
        public static unsafe uint Op_Ovr_Seg(ref State state, uint param)
        {
            if (state.Decoder.OverrideSegment != -1)
                return ErrorCodes.ERR_UNDEFOPCODE;
            Segment(ref state, param);
            state.Decoder.OverrideSegment = (short)param;
            return ErrorCodes.ERR_CONTINUE;
        }

        public static uint Op_Ovr_OpSize(ref State state, uint param)
        {
            if (state.Decoder.bOverrideOperand)
                return ErrorCodes.ERR_UNDEFOPCODE;
            ;
            state.Decoder.bOverrideOperand = true;
            return ErrorCodes.ERR_CONTINUE;
        }
        public static uint Op_Ovr_AddrSize(ref State state, uint param)
        {
            if (state.Decoder.bOverrideAddress)
                return ErrorCodes.ERR_UNDEFOPCODE;
            ;
            state.Decoder.bOverrideAddress = true;
            return ErrorCodes.ERR_CONTINUE;
        }

        public static uint Op_Prefix_REP(ref State state, uint param)
        {
            if (state.Decoder.RepeatType != 0)
                return ErrorCodes.ERR_UNDEFOPCODE;
            state.Decoder.RepeatType = Opcodes.REP;
            return ErrorCodes.ERR_CONTINUE;
        }
        public static uint Op_Prefix_REPNZ(ref State state, uint param)
        {
            if (state.Decoder.RepeatType != 0)
                return ErrorCodes.ERR_UNDEFOPCODE;
            state.Decoder.RepeatType = Opcodes.REPNZ;
            return ErrorCodes.ERR_CONTINUE;
        }
    }
}