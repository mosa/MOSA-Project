/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */
using System;
using Mosa.Platform.x86.Intrinsic;

namespace Mosa.Kernel.x86.RealModeEmulator
{
    public static partial class RealEmulator
    {
        /// <summary>
        /// Opcodes enum
        /// </summary>
        public static struct Opcodes
        {
            public static const byte // Define the type for the struct elements
            ADD_MR = 0x00, ADD_MRX = 0x01,
            ADD_RM = 0x02, ADD_RMX = 0x03,
            ADD_AI = 0x04, ADD_AIX = 0x05,

            OR_MR = 0x08, OR_MRX = 0x09,
            OR_RM = 0x0A, OR_RMX = 0x0B,
            OR_AI = 0x0C, OR_AIX = 0x0D,

            AND_MR = 0x20, AND_MRX = 0x21,
            AND_RM = 0x22, AND_RMX = 0x23,
            AND_AI = 0x24, AND_AIX = 0x25,

            SUB_MR = 0x28, SUB_MRX = 0x29,
            SUB_RM = 0x2A, SUB_RMX = 0x2B,
            SUB_AI = 0x2C, SUB_AIX = 0x2D,

            XOR_MR = 0x30, XOR_MRX = 0x31,
            XOR_RM = 0x32, XOR_RMX = 0x33,
            XOR_AI = 0x34, XOR_AIX = 0x35,

            CMP_MR = 0x38, CMP_MRX = 0x39,
            CMP_RM = 0x3A, CMP_RMX = 0x3B,
            CMP_AI = 0x3C, CMP_AIX = 0x3D,

            INC_A = 0x40 | GPRegisters.AL, INC_B = 0x40 | GPRegisters.BL,
            INC_C = 0x40 | GPRegisters.CL, INC_D = 0x40 | GPRegisters.DL,
            INC_Sp = 0x40 | GPRegisters.AH, INC_Bp = 0x40 | GPRegisters.CH,
            INC_Si = 0x40 | GPRegisters.DH, INC_Di = 0x40 | GPRegisters.BH,

            DEC_A = 0x48 | GPRegisters.AL, DEC_B = 0x48 | GPRegisters.BL,
            DEC_C = 0x48 | GPRegisters.CL, DEC_D = 0x48 | GPRegisters.DL,
            DEC_Sp = 0x48 | GPRegisters.AH, DEC_Bp = 0x48 | GPRegisters.CH,
            DEC_Si = 0x48 | GPRegisters.DH, DEC_Di = 0x48 | GPRegisters.BH,

            INT3 = 0xCC, INT_I = 0xCD,
            IRET = 0xCF,

            //DIV_R = 0xFA, DIV_RX = 0xFB,
            //DIV_M = 0xFA, DIV_MX = 0xFB,

            MOV_AMo = 0xA0, MOV_AMoX = 0xA1,
            MOV_MoA = 0xA2, MOV_MoAX = 0xA3,
            MOV_RI_AL = 0xB0 | GPRegisters.AL, MOV_RI_BL = 0xB0 | GPRegisters.BL, MOV_RI_CL = 0xB0 | GPRegisters.CL, MOV_RI_DL = 0xB0 | GPRegisters.DL,
            MOV_RI_AH = 0xB0 | GPRegisters.AH, MOV_RI_BH = 0xB0 | GPRegisters.BH, MOV_RI_CH = 0xB0 | GPRegisters.CH, MOV_RI_DH = 0xB0 | GPRegisters.DH,
            MOV_RI_AX = 0xB0 | GPRegisters.AL | 8, MOV_RI_BX = 0xB0 | GPRegisters.BL | 8, MOV_RI_CX = 0xB0 | GPRegisters.CL | 8, MOV_RI_DX = 0xB0 | GPRegisters.DL | 8,
            MOV_RI_SP = 0xB0 | GPRegisters.AH | 8, MOV_RI_BP = 0xB0 | GPRegisters.CH | 8, MOV_RI_SI = 0xB0 | GPRegisters.DH | 8, MOV_RI_DI = 0xB0 | GPRegisters.BH | 8,
            MOV_MI = 0xC6, MOV_MIX = 0xC7,
            MOV_MR = 0x88, MOV_MRX = 0x89,
            MOV_RM = 0x8A, MOV_RMX = 0x8B,
            MOV_RS = 0x8C, MOV_SR = 0x8E,
            //MOV_MS = 0x8C, MOV_SM = 0x8E,
            
            //MUL_R = 0xF6, MUL_RX = 0xF7,
            //MUL_M = 0xF6, MUL_MX = 0xF7,

            NOP = 0x90,
            XCHG_AA = 0x90, XCHG_AB = 0x90 | GPRegisters.BL,
            XCHG_AC = 0x90 | GPRegisters.CL, XCHG_AD = 0x90 | GPRegisters.DL,
            XCHG_ASp = 0x90 | GPRegisters.AH, XCHG_ABp = 0x90 | GPRegisters.CH,
            XCHG_ASi = 0x90 | GPRegisters.DH, XCHG_ADi = 0x90 | GPRegisters.BH,
            XCHG_RM = 0x86, XCHG_RMX = 0x87,

            NOT_R = 0xF6, NOT_RX = 0xF7,
            NOT_M = 0xF6, NOT_MX = 0xF7,


            IN_AI = 0xE4, IN_AIX = 0xE5,
            OUT_IA = 0xE6, OUT_IAX = 0xE7,
            IN_ADx = 0xEC, IN_ADxX = 0xED,
            OUT_DxA = 0xEE, OUT_DxAX = 0xEF,

            PUSH_AX = 0x50 | GPRegisters.AL, PUSH_BX = 0x50 | GPRegisters.BL,
            PUSH_CX = 0x50 | GPRegisters.CL, PUSH_DX = 0x50 | GPRegisters.DL,
            PUSH_SP = 0x50 | GPRegisters.AH, PUSH_BP = 0x50 | GPRegisters.CH,
            PUSH_SI = 0x50 | GPRegisters.DH, PUSH_DI = 0x50 | GPRegisters.BH,
            // PUSH_MX = 0xFF,	// - TODO: Check (maybe 0x87)
            PUSH_ES = 0x06 | (SRegisters.SREG_ES << 3), PUSH_CS = 0x06 | (SRegisters.SREG_CS << 3),
            PUSH_SS = 0x06 | (SRegisters.SREG_SS << 3), PUSH_DS = 0x06 | (SRegisters.SREG_DS << 3),
            PUSH_I8 = 0x6A, PUSH_I = 0x68,
            PUSHA = 0x60, PUSHF = 0x9C,

            POP_AX = 0x58 | GPRegisters.AL, POP_BX = 0x58 | GPRegisters.BL,
            POP_CX = 0x58 | GPRegisters.CL, POP_DX = 0x58 | GPRegisters.DL,
            POP_SP = 0x58 | GPRegisters.AH, POP_BP = 0x58 | GPRegisters.CH,
            POP_SI = 0x58 | GPRegisters.DH, POP_DI = 0x58 | GPRegisters.BH,
            POP_ES = 7 | (SRegisters.SREG_ES << 3),
            POP_SS = 7 | (SRegisters.SREG_SS << 3), POP_DS = 7 | (SRegisters.SREG_DS << 3),
            POP_MX = 0x8F,
            POPA = 0x61, POPF = 0x9D,

            RET_N = 0xC3, RET_iN = 0xC2,
            RET_F = 0xCB, RET_iF = 0xCA,

            CALL_MF = 0xFF, CALL_MN = 0xFF,
            CALL_N = 0xE8, CALL_F = 0x9A,
            CALL_R = 0xFF,

            JMP_MF = 0xFF, JMP_N = 0xE9,
            JMP_S = 0xEB, JMP_F = 0xEA,

            LES = 0xC4,
            LDS = 0xC5,
            LEA = 0x8D,

            CBW = 0x98,

            CLC = 0xF8, STC = 0xF9,
            CLI = 0xFA, STI = 0xFB,
            CLD = 0xFC, STD = 0xFD,

            TEST_RM = 0x84, TEST_RMX = 0x85,
            TEST_AI = 0xA8, TEST_AIX = 0xA9,

            MOVSB = 0xA4, MOVSW = 0xA5,
            CMPSB = 0xA6, CMPSW = 0xA7,
            STOSB = 0xAA, STOSW = 0xAB,
            LODSB = 0xAC, LODSW = 0xAD,
            SCASB = 0xAE, SCASW = 0xAF,
            INSB = 0x6C, INSW = 0x6D,
            OUTSB = 0x6E, OUTSW = 0x6F,

            // Unimplemented
            FPU_ARITH = 0xDC,

            // Overrides
            OVR_ES = 0x26,
            OVR_CS = 0x2E,
            OVR_SS = 0x36,
            OVR_DS = 0x3E,

            REPNZ = 0xF2, REP = 0xF3,
            LOOPNZ = 0xE0, LOOPZ = 0xE1,
            LOOP = 0xE2;
        };

        public static uint PARITY8(uint value)
        {
            return (uint)(((value >> 7) ^ (value >> 6) ^ (value >> 5) ^ (value >> 4) ^ (value >> 3) ^ (value >> 2) ^ (value >> 1) ^ (value)) & 1);
        }

        public static uint SET_COMM_FLAGS(ref State state, uint value, ushort width)
        {
            ushort flags = (Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_PF);
            state.Flags &= (ushort)~flags;
            state.Flags |= (ushort)((value == 0) ? Flags.FLAG_ZF : 0);
            state.Flags |= (ushort)(((value >> (width - 1)) != 0) ? Flags.FLAG_SF : 0);
            state.Flags |= (ushort)((PARITY8(value) == 0) ? Flags.FLAG_PF : 0);
            return ErrorCodes.ERR_OK;
        }

        /// <summary>
        /// Reads 1 byte as an unsigned byte from CS:IP and increases IP by 1.
        /// </summary>
        /// <param name="state">The State struct instance</param>
        /// <param name="destination">The destination variable</param>
        /// <returns>Operation Success</returns>
        public static unsafe uint READ_INSTR8(ref State state, ref byte destination)
        {
            byte value = 0;
            uint err = Int_Read8(ref state, state.CS, (ushort)(state.IP + state.Decoder.IPOffset), &value);
            if (err != ErrorCodes.ERR_OK) return err;
            state.Decoder.IPOffset++;
            destination = value;
            return ErrorCodes.ERR_OK;
        }

        /// <summary>
        /// Reads 1 byte as an signed byte from CS:IP and increases IP by 1. (NOT FULLY IMPLEMENTED)
        /// </summary>
        /// <param name="state">The State struct instance</param>
        /// <param name="destination">The destination variable</param>
        /// <returns>Operation Success</returns>
        public static unsafe uint READ_INSTR8S(ref State state, ref byte destination)
        {
            byte value = 0;
            uint err = Int_Read8(ref state, state.CS, (ushort)(state.IP + state.Decoder.IPOffset), &value);
            if (err != ErrorCodes.ERR_OK) return err;
            state.Decoder.IPOffset++;
            destination = value;
            return ErrorCodes.ERR_OK;
        }

        /// <summary>
        /// Reads 2 bytes as an unsigned short from CS:IP and increases IP by 2.
        /// </summary>
        /// <param name="state">The State struct instance</param>
        /// <param name="destination">The destination variable</param>
        /// <returns>Operation Success</returns>
        public static unsafe uint READ_INSTR16(ref State state, ref ushort destination)
        {
            ushort value = 0;
            uint err = Int_Read16(ref state, state.CS, (ushort)(state.IP + state.Decoder.IPOffset), &value);
            if (err != ErrorCodes.ERR_OK) return err;
            state.Decoder.IPOffset += 2;
            destination = value;
            return ErrorCodes.ERR_OK;
        }

        /// <summary>
        /// Reads 4 bytes as an unsigned integer from CS:IP and increases IP by 4.
        /// </summary>
        /// <param name="state">The State struct instance</param>
        /// <param name="destination">The destination variable</param>
        /// <returns>Operation Success</returns>
        public static unsafe uint READ_INSTR32(ref State state, ref uint destination)
        {
            uint value = 0;
            uint err = Int_Read32(ref state, state.CS, (ushort)(state.IP + state.Decoder.IPOffset), &value);
            if (err != ErrorCodes.ERR_OK) return err;
            state.Decoder.IPOffset += 4;
            destination = value;
            return ErrorCodes.ERR_OK;
        }

        public static unsafe uint Int_GetPtr(ref State state, ushort segment, ushort offset, void** ptr)
        {
            // Calculate the address and block
            uint addr = (uint)(segment * 16 + offset);

            // Check for null
            //NOT NEEDED! (I think)
            //if (block != 0 && state.Memory[block] == null) return ErrorCodes.ERR_BADMEM;

            // Fetch the pointer
            *ptr = (uint*)(state.Memory + addr);

            // Return success
            return ErrorCodes.ERR_OK;
        }

        public static unsafe uint Int_Read8(ref State state, ushort segment, ushort offset, byte* destination)
        {
            // Create pointer
            byte* ptr;

            // Fetch pointer and error
            uint err = Int_GetPtr(ref state, segment, offset, (void**)&ptr);

            // If error is not ERR_OK, return error
            if (err != ErrorCodes.ERR_OK) return err;

            // Update destination with pointer
            *destination = *ptr;

            // Return success
            return ErrorCodes.ERR_OK;
        }

        public static unsafe uint Int_Read16(ref State state, ushort segment, ushort offset, ushort* destination)
        {
            ushort* ptr;
            uint err = Int_GetPtr(ref state, segment, offset, (void**)&ptr);
            if (err != ErrorCodes.ERR_OK) return err;
            *destination = *ptr;
            return ErrorCodes.ERR_OK;
        }

        public static unsafe uint Int_Read32(ref State state, ushort segment, ushort offset, uint* destination)
        {
            uint* ptr;
            uint err = Int_GetPtr(ref state, segment, offset, (void**)&ptr);
            if (err != ErrorCodes.ERR_OK) return err;
            *destination = *ptr;
            return ErrorCodes.ERR_OK;
        }

        public static unsafe uint Int_Write8(ref State state, ushort segment, ushort offset, byte value)
        {
            // Create pointer
            byte* ptr;

            // Fetch pointer and error
            uint err = Int_GetPtr(ref state, segment, offset, (void**)&ptr);

            // If error is not ERR_OK, return error
            if (err != ErrorCodes.ERR_OK) return err;

            // Update pointer with value
            *ptr = value;

            // Return success
            return ErrorCodes.ERR_OK;
        }

        public static unsafe uint Int_Write16(ref State state, ushort segment, ushort offset, ushort value)
        {
            ushort* ptr;
            uint err = Int_GetPtr(ref state, segment, offset, (void**)&ptr);
            if (err != ErrorCodes.ERR_OK) return err;
            *ptr = value;
            return ErrorCodes.ERR_OK;
        }

        public static unsafe uint Int_Write32(ref State state, ushort segment, ushort offset, uint value)
        {
            uint* ptr;
            uint err = Int_GetPtr(ref state, segment, offset, (void**)&ptr);
            if (err != ErrorCodes.ERR_OK) return err;
            *ptr = value;
            return ErrorCodes.ERR_OK;
        }

        public static unsafe uint Int_GetModRM(ref State state, uint* mod, uint* rrr, uint* mmm)
        {
            byte vByte = 0;
            if (READ_INSTR8(ref state, ref vByte) != 0) return vByte;
            if (mod != null) mod = (uint*)(vByte >> 6);
            if (rrr != null) rrr = (uint*)((vByte >> 3) & 7);
            if (mmm != null) mmm = (uint*)(vByte & 7);
            return ErrorCodes.ERR_OK;
        }

        public static unsafe uint PUSH(ref State state, ushort value)
        {
            state.SP.W -= 2;
            return Int_Write16(ref state, state.SS, state.SP.W, value);
        }

        public static unsafe uint POP(ref State state, ref ushort destination)
        {
            ushort value = 0;
            uint err = Int_Read16(ref state, state.SS, state.SP.W, &value);
            if (err != ErrorCodes.ERR_OK) return err;
            state.SP.W += 2;
            destination = value;
            return ErrorCodes.ERR_OK;
        }

        public static unsafe ushort* Segment(ref State state, uint code)
        {
            switch(code)
            {
                case SRegisters.SREG_ES: return (ushort*)state.AddressES;
                case SRegisters.SREG_CS: return (ushort*)state.AddressCS;
                case SRegisters.SREG_SS: return (ushort*)state.AddressSS;
                case SRegisters.SREG_DS: return (ushort*)state.AddressDS;
                case SRegisters.SREG_FS: return (ushort*)state.AddressFS;
                case SRegisters.SREG_GS: return (ushort*)state.AddressGS;
                default: return (ushort*)0;
            }
        }

        public static unsafe byte* RegB(ref State state, uint number)
        {
            if (number >= 4)
                return (byte*)state.Registers[number - 4].AddressH;
            else
                return (byte*)state.Registers[number].Address;
        }

        public static unsafe ushort* RegW(ref State state, uint number)
        {
            return (ushort*)state.Registers[number].Address;
        }
    }
}