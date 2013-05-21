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
        /// <summary>
        /// Size of memory block (4KB)
        /// </summary>
        public static const uint BLOCK_SIZE = 0x1000;

        /// <summary>
        /// Magic return Instruction Pointer
        /// </summary>
        private static ushort MAGIC_IP = 0xFFFF;

        /// <summary>
        /// Magic return Code Segment
        /// </summary>
        private static ushort MAGIC_CS = 0xFFFF;

        private static ushort HLE_CS = 0xB800;

        public delegate uint OpcodeFunction(ref State state, uint param);

        /// <summary>
        /// Emulator error code enum
        /// </summary>
        public static class ErrorCodes
        {
            public const ushort // Define the type for the struct elements
            ERR_OK = 0, // No error
            ERR_CONTINUE = 1, // Internal non-error, continue decoding
            ERR_INVAL = 2, // Bad parameter passed to emulator
            ERR_BADMEM = 3, // Emulator access invalid memory
            ERR_UNDEFOPCODE = 4, // Undefined Opcode
            ERR_DIVIDE = 5, // Divide error
            ERR_BUG = 6, // Bug in the emulator
            ERR_BREAK = 7, // Breakpoint hit
            ERR_HALT = 8, // CPU halted
            ERR_FCNRET = 9, // Magic CS/IP Reached, function return

            ERR_LAST = 10; // Size of the error enum
        }

        /// <summary>
        /// Flags enum
        /// </summary>
        public static class Flags
        {
            public const ushort // Define the type for the struct elements
            FLAG_CF = 0x001, // Carry flag
            FLAG_PF = 0x004, // Parity flag
            FLAG_AF = 0x010, // Adjust flag
            FLAG_ZF = 0x040, // Zero flag
            FLAG_SF = 0x080, // Sign flag
            FLAG_TF = 0x100, // Trap flag (for single stepping)
            FLAG_IF = 0x200, // Interrupt flag
            FLAG_DF = 0x400, // Direction flag
            FLAG_OF = 0x800, // Overflow flag

            FLAG_DEFAULT = 0x2; // Default flag
        }

        /// <summary>
        /// General Purpose Registers enum
        /// </summary>
        public static class GPRegisters
        {
            public const byte // Define the type for the struct elements
            AL = 0x0,
            CL = 0x1,
            DL = 0x2,
            BL = 0x3,
            AH = 0x4,
            CH = 0x5,
            DH = 0x6,
            BH = 0x7;
        }

        /// <summary>
        /// General Purpose X Registers enum
        /// </summary>
        public static class GPRegistersX
        {
            public const byte // Define the type for the struct elements
            AX = 0x0,
            CX = 0x1,
            DX = 0x2,
            BX = 0x3,
            SP = 0x4,
            BP = 0x5,
            SI = 0x6,
            DI = 0x7;
        }

        /// <summary>
        /// Special Registers enum
        /// </summary>
        public static class SRegisters
        {
            public const byte // Define the type for the struct elements
            SREG_ES = 0x0,
            SREG_CS = 0x1,
            SREG_SS = 0x2,
            SREG_DS = 0x3,
            SREG_FS = 0x4, // added 386
            SREG_GS = 0x5;
        }

        /// <summary>
        /// Create a new Real Emulator State
        /// </summary>
        /// <returns>The State instance</returns>
        public static unsafe void CreateState(out State state)
        {
            // Create a new Real Emulator State instance
            state = new State();

            // Setup memory address and registers
            uint regmem = KernelMemory.AllocateMemory(0x3C);
            state.Memory = KernelMemory.AllocateMemory(0x100000);
            for (int i = 0; i < 8; i++)
                state.Registers[i] = new Register((uint)(regmem + (i * 4)));
            state.SegmentAddress = regmem + 32;

            // Initial stack
            state.Flags = Flags.FLAG_DEFAULT;

            // Stub CS/IP
            state.CS = 0xF000;
            state.IP = 0xFFF0;

            // Return the new state
            //return state;
        }

        public static unsafe uint Int_CallInterrupt(ref State state, uint number)
        {
            uint err;

            if (number < 0 || number > 0xFF)
            {
                //printf("WARNING: %i is not a valid interrupt number", Num);
                return ErrorCodes.ERR_INVAL;
            }

            state.SP.W -= 2;
            err = Int_Write16(ref state, state.SS, state.SP.W, (state.Flags));
            if (err != ErrorCodes.ERR_OK) return err;

            state.SP.W -= 2;
            err = Int_Write16(ref state, state.SS, state.SP.W, (state.CS));
            if (err != ErrorCodes.ERR_OK) return err;

            state.SP.W -= 2;
            err = Int_Write16(ref state, state.SS, state.SP.W, (state.IP));
            if (err != ErrorCodes.ERR_OK) return err;

            err = Int_Read16(ref state, 0, (ushort)(number * 4), (ushort*)state.AddressIP);
            if (err != ErrorCodes.ERR_OK) return err;
            err = Int_Read16(ref state, 0, (ushort)(number * 4 + 2), (ushort*)state.AddressCS);
            if (err != ErrorCodes.ERR_OK) return err;

            return ErrorCodes.ERR_OK;
        }

        public static uint CallInterrupt(ref State state, uint number)
        {
            state.CS = MAGIC_CS;
            state.IP = MAGIC_IP;

            uint err = Int_CallInterrupt(ref state, number);
            if (err != ErrorCodes.ERR_OK) return err;

            return Call(ref state);
        }

        public static uint Call(ref State state)
        {
            uint err;
            for (; ; )
            {
                err = RunOne(ref state);
                if (err == ErrorCodes.ERR_FCNRET)
                    return ErrorCodes.ERR_OK;
                if (err != ErrorCodes.ERR_OK)
                    return err;
            }
        }

        public static uint RunOne(ref State state)
        {
            if (state.IP == 0xFFFF && state.CS == 0xFFFF)
                return ErrorCodes.ERR_FCNRET;

            if (state.CS == 0xB800 && state.IP < 0x100)
            {

                if (state.HLECallbacks[state.IP] != null)
                    state.HLECallbacks[state.IP](ref state, state.IP);

                caOperations[0xCF].Function(ref state, 0);
                return 0;
            }

            uint err = Int_DoOpcode(ref state);
            switch (err)
            {
                case ErrorCodes.ERR_OK:
                    break;
                case ErrorCodes.ERR_DIVIDE:
                    err = Int_CallInterrupt(ref state, 0);
                    if (err != 0) return err;
                    break;

                default:
                    return err;
            }

            return ErrorCodes.ERR_OK;
        }

        public static unsafe uint Int_DoOpcode(ref State state)
        {
            byte opcode = 0;
            uint err;
            ushort startIP, startCS;

            startIP = state.IP;
            startCS = state.CS;

            state.Decoder.OverrideSegment = -1;
            state.Decoder.RepeatType = 0;
            state.Decoder.bOverrideOperand = false;
            state.Decoder.bOverrideAddress = false;
            state.Decoder.bDontChangeIP = false;
            state.Decoder.IPOffset = 0;
            state.InstrNum++;

            do
            {
                err = Int_Read8(ref state, state.CS, (ushort)((ushort)((ushort)(state.IP + state.Decoder.IPOffset))), &opcode);
                if (err != ErrorCodes.ERR_OK) return err;
                state.Decoder.IPOffset++;

                if (opcode == 0xF1)
                {
                    //printf(" Executing unset memory (opcode 0xF1) %04x:%04x", state.CS, state.IP);
                    return ErrorCodes.ERR_BREAK; // BREAKPOINT matches error code 7 //return 7;
                }
                if (caOperations[opcode].Function == null)
                {
                    //printf(" Unkown Opcode 0x%02x", opcode);
                    return ErrorCodes.ERR_UNDEFOPCODE;
                }
                if (caOperations[opcode].ModRMNames != null)
                {
                    uint rrr = 0;
                    Int_GetModRM(ref state, null, &rrr, null);
                    state.Decoder.IPOffset--;
                }
                else
                {
                }
                err = caOperations[opcode].Function(ref state, caOperations[opcode].Argument);
            } while (err == ErrorCodes.ERR_CONTINUE);

            if (err != ErrorCodes.ERR_OK) return err;

            //if (state.Decoder.RepeatType)
            //{
            //}

            if (!state.Decoder.bDontChangeIP)
                state.IP += state.Decoder.IPOffset;

            if (state.Decoder.IPOffset == 2)
            {
                byte byte1 = 0xFF, byte2 = 0xFF;
                err = Int_Read8(ref state, startCS, (ushort)(startIP + 0), &byte1);
                if (err != ErrorCodes.ERR_OK) return err;
                err = Int_Read8(ref state, startCS, (ushort)(startIP + 1), &byte2);
                if (err != ErrorCodes.ERR_OK) return err;

                if (byte1 == 0 && byte2 == 0)
                {
                    if (state.bWasLastOperationNull)
                        return ErrorCodes.ERR_BREAK;
                    state.bWasLastOperationNull = true;
                }
                else
                    state.bWasLastOperationNull = false;
            }
            else
                state.bWasLastOperationNull = false;
            return ErrorCodes.ERR_OK;
        }


        public static unsafe uint Op_Ext_0F(ref State state, uint param)
        {
            byte extra;
            uint err = Int_Read8(ref state, state.CS, (ushort)(state.IP + state.Decoder.IPOffset), &extra);
            if (err != ErrorCodes.ERR_OK) return err;
            state.Decoder.IPOffset++;

            if (caOperations0F[extra].Function == null)
            {
                //printf(" Unkown Opcode 0x0F 0x%02x", extra);
                return ErrorCodes.ERR_UNDEFOPCODE;
            }

            return caOperations0F[extra].Function(ref state, caOperations0F[extra].Argument);
        }

        public static unsafe uint Op_Unary_M(ref State state, uint param)
        {
            const byte width = 8;
            uint err, op_num;
            byte* dest;

            err = Int_GetModRM(ref state, null, &op_num, null);
            if (err != ErrorCodes.ERR_OK) return err;
            state.Decoder.IPOffset--;

            switch (op_num)
            {
                case 0:
                    err = Int_ParseModRM(ref state, null, &dest, 0);
                    if (err != ErrorCodes.ERR_OK) return err;
                    (*dest)++;
                    ushort flag = (0x800 | 0x040 | 0x080 | 0x004 | 0x010);
                    state.Flags &= (ushort)~flag;
                    if (*dest == (1 << (width - 1)))
                        state.Flags |= 0x800;
                    if ((*dest & 15) == 0)
                        state.Flags |= 0x010;

                    SET_COMM_FLAGS(ref state, *dest, width);
                    break;
                case 1:
                    err = Int_ParseModRM(ref state, null, &dest, 0);
                    if (err != ErrorCodes.ERR_OK) return err;
                    (*dest)--;
                    flag = (0x800 | 0x010);
                    state.Flags &= (ushort)~flag;
                    if (*dest + 1 == (1 << (width - 1)))
                        state.Flags |= 0x800;
                    if ((*dest & 15) + 1 == 16)
                        state.Flags |= 0x010;

                    SET_COMM_FLAGS(ref state, *dest, width);
                    break;
                default:
                    //printf(" - Unary M /%i unimplemented\n", op_num);
                    return ErrorCodes.ERR_UNDEFOPCODE;
            }

            return ErrorCodes.ERR_OK;
        }

        public static unsafe uint Op_Unary_MX(ref State state, uint param)
        {
            uint err, op_num, mod, mmm;

            err = Int_GetModRM(ref state, &mod, &op_num, &mmm);
            if (err != ErrorCodes.ERR_OK) return err;
            state.Decoder.IPOffset--;

            if (state.Decoder.bOverrideOperand)
            {
                uint* dest;
                // Cannot have this variable constant otherwise IDE throws error
                //const byte width = 32;
                byte width = 32;
                switch (op_num)
                {
                    case 0:
                        err = Int_ParseModRMX(ref state, null, (ushort**)&dest, 0);
                        if (err != ErrorCodes.ERR_OK) return err;
                        (*dest)++;
                        ushort flag = (0x800 | 0x040 | 0x080 | 0x004 | 0x010);
                        state.Flags &= (ushort)~flag;
                        if (*dest == (1 << (width - 1)))
                            state.Flags |= 0x800;
                        if ((*dest & 15) == 0)
                            state.Flags |= 0x010;

                        SET_COMM_FLAGS(ref state, *dest, width);

                        SET_COMM_FLAGS(ref state, *dest, width);
                        break;
                    case 1:
                        err = Int_ParseModRMX(ref state, null, (ushort**)&dest, 0);
                        if (err != ErrorCodes.ERR_OK) return err;
                        (*dest)--;
                        flag = (0x800 | 0x010);
                        state.Flags &= (ushort)~flag;
                        if (*dest + 1 == (1 << (width - 1)))
                            state.Flags |= 0x800;
                        if ((*dest & 15) + 1 == 16)
                            state.Flags |= 0x010;

                        SET_COMM_FLAGS(ref state, *dest, width);

                        SET_COMM_FLAGS(ref state, *dest, width);
                        break;
                    case 6:
                        err = Int_ParseModRMX(ref state, null, (ushort**)&dest, 0);
                        if (err != ErrorCodes.ERR_OK) return err;
                        state.SP.W -= 2;
                        err = Int_Write16(ref state, state.SS, state.SP.W, (ushort)(*dest));
                        if (err != ErrorCodes.ERR_OK) return err;
                        break;
                    default:
                        //printf(" - Unary MX (32) /%i unimplemented\n", op_num);
                        return ErrorCodes.ERR_UNDEFOPCODE;
                }
            }
            else
            {
                ushort* dest;
                const byte width = 16;
                switch (op_num)
                {
                    case 0:
                        err = Int_ParseModRMX(ref state, null, &dest, 0);
                        if (err != ErrorCodes.ERR_OK) return err;
                        (*dest)++;
                        ushort flag = (0x800 | 0x040 | 0x080 | 0x004 | 0x010);
                        state.Flags &= (ushort)~flag;
                        if (*dest == (1 << (width - 1)))
                            state.Flags |= 0x800;
                        if ((*dest & 15) == 0)
                            state.Flags |= 0x010;

                        SET_COMM_FLAGS(ref state, *dest, width);

                        SET_COMM_FLAGS(ref state, *dest, width);
                        break;
                    case 1:
                        err = Int_ParseModRMX(ref state, null, &dest, 0);
                        if (err != ErrorCodes.ERR_OK) return err;
                        (*dest)--;

                        flag = (0x800 | 0x010);
                        state.Flags &= (ushort)~flag;
                        if (*dest + 1 == (1 << (width - 1)))
                            state.Flags |= 0x800;
                        if ((*dest & 15) + 1 == 16)
                            state.Flags |= 0x010;

                        SET_COMM_FLAGS(ref state, *dest, width);

                        SET_COMM_FLAGS(ref state, *dest, width);
                        break;
                    case 2:
                        err = Int_ParseModRMX(ref state, null, &dest, 0);
                        if (err != ErrorCodes.ERR_OK) return err;
                        state.SP.W -= 2;
                        err = Int_Write16(ref state, state.SS, state.SP.W, ((ushort)(state.IP + state.Decoder.IPOffset)));
                        if (err != ErrorCodes.ERR_OK) return err;
                        state.IP = *dest;
                        state.Decoder.bDontChangeIP = true;
                        break;
                    case 3:
                        if (mod == 3) return ErrorCodes.ERR_UNDEFOPCODE;
                        err = Int_ParseModRMX(ref state, null, &dest, 0);
                        if (err != ErrorCodes.ERR_OK) return err;
                        state.SP.W -= 2;
                        err = Int_Write16(ref state, state.SS, state.SP.W, (state.CS));
                        if (err != ErrorCodes.ERR_OK) return err;
                        state.SP.W -= 2;
                        err = Int_Write16(ref state, state.SS, state.SP.W, ((ushort)(state.IP + state.Decoder.IPOffset)));
                        if (err != ErrorCodes.ERR_OK) return err;
                        state.IP = dest[0];
                        state.CS = dest[1];
                        state.Decoder.bDontChangeIP = true;
                        break;
                    case 4:
                        err = Int_ParseModRMX(ref state, null, &dest, 0);
                        if (err != 0) return err;
                        state.IP = *dest;
                        state.Decoder.bDontChangeIP = true;
                        break;
                    case 5:
                        if (mod == 3) return ErrorCodes.ERR_UNDEFOPCODE;
                        err = Int_ParseModRMX(ref state, null, &dest, 0);
                        if (err != 0) return err;
                        state.IP = dest[0];
                        state.CS = dest[1];
                        state.Decoder.bDontChangeIP = true;
                        break;
                    case 6:
                        err = Int_ParseModRMX(ref state, null, &dest, 0);
                        if (err != 0) return err;
                        state.SP.W -= 2;
                        err = Int_Write16(ref state, state.SS, state.SP.W, (*dest));
                        if (err != 0)return err;
                        break;

                    default:
                        //printf(" - Unary MX (16) /%i unimplemented\n", op_num);
                        return ErrorCodes.ERR_UNDEFOPCODE;
                }
            }

            return ErrorCodes.ERR_OK;
        }

        public static unsafe uint DoFunc(ref State state, uint mmm, ushort disp, ushort* segment, uint* offset)
        {
            uint addr;
            ushort seg;

            switch (mmm)
            {
                case 2:
                case 3:
                case 6:
                    seg = SRegisters.SREG_SS;
                    break;
                default:
                    seg = SRegisters.SREG_DS;
                    break;
            }

            if (state.Decoder.OverrideSegment != -1)
                seg = (ushort)state.Decoder.OverrideSegment;

            seg = *Segment(ref state, seg);

            switch (mmm & 7)
            {
                case 0:
                    addr = (uint)(state.BX.W + state.SI.W);
                    break;
                case 1:
                    addr = (uint)(state.BX.W + state.DI.W);
                    break;
                case 2:
                    addr = (uint)(state.BP.W + state.SI.W);
                    break;
                case 3:
                    addr = (uint)(state.BP.W + state.DI.W);
                    break;
                case 4:
                    addr = state.SI.W;
                    break;
                case 5:
                    addr = state.DI.W;
                    break;
                case 6:
                    if ((mmm & 8) == 8)
                    {
                        uint err = Int_Read16(ref state, state.CS, (ushort)((ushort)(state.IP + state.Decoder.IPOffset)), &disp);
                        if (err != ErrorCodes.ERR_OK) return err;
                        state.Decoder.IPOffset += 2;
                        addr = disp;
                    }
                    else
                    {
                        addr = state.BP.W;
                    }
                    break;
                case 7:
                    addr = state.BX.W;
                    break;
                default:
                    //printf("Unknown mmm value passed to DoFunc (%i)", mmm);
                    return ErrorCodes.ERR_BUG;
            }
            if ((mmm & 8) != 0)
            {
                addr += disp;
            }
            *segment = seg;
            *offset = addr;

            return ErrorCodes.ERR_OK;
        }

        public static unsafe uint DoFunc32(ref State state, uint mmm, uint disp, ushort* segment, uint* offset)
        {
            uint addr;
            ushort seg;
            byte sib;

            switch (mmm)
            {
                case 2:
                case 3:
                case 6:
                    seg = SRegisters.SREG_SS;
                    break;
                default:
                    seg = SRegisters.SREG_DS;
                    break;
            }

            if (state.Decoder.OverrideSegment != -1)
                seg = (ushort)state.Decoder.OverrideSegment;

            seg = *Segment(ref state, seg);

            switch (mmm & 7)
            {
                case 0:
                    addr = state.AX.D;
                    break;
                case 1:
                    addr = state.CX.D;
                    break;
                case 2:
                    addr = state.DX.D;
                    break;
                case 3:
                    addr = state.BX.D;
                    break;
                case 4:
                    uint err = Int_Read8(ref state, state.CS, (ushort)((ushort)((ushort)(state.IP + state.Decoder.IPOffset))), &sib);
                    if (err != ErrorCodes.ERR_OK) return err;
                    state.Decoder.IPOffset++;
                    addr = 0;

                    switch ((sib >> 3) & 7)
                    {
                        case 0: addr = state.AX.D; break;
                        case 1: addr = state.CX.D; break;
                        case 2: addr = state.DX.D; break;
                        case 3: addr = state.BX.D; break;
                        case 4: addr = 0; break;
                        case 5: addr = state.BP.D; break;
                        case 6: addr = state.SI.D; break;
                        case 7: addr = state.DI.D; break;
                    }

                    addr <<= (sib >> 6);

                    switch (sib & 7)
                    {
                        case 0: addr += state.AX.D; break;
                        case 1: addr += state.CX.D; break;
                        case 2: addr += state.DX.D; break;
                        case 3: addr += state.BX.D; break;
                        case 4: addr += state.SP.D; break;
                        case 5:
                            if ((mmm & 8) == 8)
                            {
                                err = Int_Read32(ref state, state.CS, (ushort)((ushort)((ushort)(state.IP + state.Decoder.IPOffset))), &disp);
                                if (err != ErrorCodes.ERR_OK) return err;
                                state.Decoder.IPOffset += 4;
                            }
                            else
                            {
                                addr += state.BP.D;
                            }
                            break;
                        case 6: addr += state.SI.D; break;
                        case 7: addr += state.DI.D; break;
                    }
                    break;
                case 5:
                    if ((mmm & 8) == 8)
                    {
                        err = Int_Read32(ref state, state.CS, (ushort)((ushort)((ushort)(state.IP + state.Decoder.IPOffset))), &addr);
                        if (err != ErrorCodes.ERR_OK) return err;
                        state.Decoder.IPOffset += 4;
                    }
                    else
                    {
                        addr = state.BP.D;
                    }
                    break;
                case 6:
                    addr = state.SI.D;
                    break;
                case 7:
                    addr = state.DI.D;
                    break;
                default:
                    //printf("Unknown mmm value passed to DoFunc32 (%i)", mmm);
                    return ErrorCodes.ERR_BUG;
            }
            if ((mmm & 8) != 8)
            {
                addr += disp;
            }
            *segment = seg;
            *offset = addr;
            return ErrorCodes.ERR_OK;
        }

        public static unsafe uint Int_GetMMM(ref State state, uint mod, uint mmm, ushort* segment, uint* offset)
        {
            ushort ofs;
            uint err;

            if (state.Decoder.bOverrideAddress)
            {
                switch (mod)
                {
                    case 0:
                        err = DoFunc32(ref state, mmm | 8, 0, segment, offset);
                        if (err != ErrorCodes.ERR_OK) return err;
                        break;
                    case 1:
                        err = Int_Read8(ref state, state.CS, (ushort)((ushort)((ushort)(state.IP + state.Decoder.IPOffset))), (byte*)&ofs);
                        if (err != ErrorCodes.ERR_OK) return err;
                        state.Decoder.IPOffset++;
                        err = DoFunc32(ref state, mmm, ofs, segment, offset);
                        if (err != ErrorCodes.ERR_OK) return err;
                        break;
                    case 2:
                        err = Int_Read32(ref state, state.CS, (ushort)((ushort)((ushort)(state.IP + state.Decoder.IPOffset))), (uint*)&ofs);
                        if (err != ErrorCodes.ERR_OK) return err;
                        state.Decoder.IPOffset += 4;
                        err = DoFunc32(ref state, mmm, ofs, segment, offset);
                        if (err != ErrorCodes.ERR_OK) return err;
                        break;
                    case 3:
                        //printf("mod=3 passed to Int_GetMMM");
                        return ErrorCodes.ERR_BUG;
                    default:
                        //printf("Unknown mod value passed to Int_GetMMM (%i)", mod);
                        return ErrorCodes.ERR_BUG;
                }
            }
            else
            {
                switch (mod)
                {
                    case 0:
                        err = DoFunc(ref state, mmm | 8, 0, segment, offset);
                        if (err != ErrorCodes.ERR_OK) return err;
                        break;
                    case 1:
                        err = Int_Read8(ref state, state.CS, (ushort)((ushort)((ushort)(state.IP + state.Decoder.IPOffset))), (byte*)&ofs);
                        if (err != ErrorCodes.ERR_OK) return err;
                        state.Decoder.IPOffset++;
                        err = DoFunc(ref state, mmm, ofs, segment, offset);
                        if (err != ErrorCodes.ERR_OK) return err;
                        break;
                    case 2:
                        err = Int_Read16(ref state, state.CS, (ushort)((ushort)((ushort)(state.IP + state.Decoder.IPOffset))), &ofs);
                        if (err != ErrorCodes.ERR_OK) return err;
                        state.Decoder.IPOffset += 2;
                        err = DoFunc(ref state, mmm, ofs, segment, offset);
                        if (err != ErrorCodes.ERR_OK) return err;
                        break;
                    case 3:
                        //printf("mod=3 passed to Int_GetMMM");
                        return ErrorCodes.ERR_BUG;
                    default:
                        //printf("Unknown mod value passed to Int_GetMMM (%i)", mod);
                        return ErrorCodes.ERR_BUG;
                }
            }

            return ErrorCodes.ERR_OK;
        }

        public static unsafe uint Int_ParseModRM(ref State state, byte** to, byte** from, int bReverse)
        {
            uint err;
            uint mod = 0, rrr = 0, mmm = 0;

            Int_GetModRM(ref state, &mod, &rrr, &mmm);

            if (to != (byte**)0) *to = (byte*)RegB(ref state, rrr);

            if (from != (byte**)0)
            {
                if (mod == 3)
                    *from = (byte*)RegB(ref state, mmm);
                else
                {
                    ushort segment;
                    uint offset;
                    err = Int_GetMMM(ref state, mod, mmm, &segment, &offset);
                    if (err != ErrorCodes.ERR_OK) return err;
                    err = Int_GetPtr(ref state, segment, (ushort)offset, (void**)from);
                    if (err != ErrorCodes.ERR_OK) return err;
                }
            }
            return ErrorCodes.ERR_OK;
        }

        public static unsafe uint Int_ParseModRMX(ref State state, ushort** reg, ushort** mem, int bReverse)
        {
            uint err;
            uint mod = 0, rrr = 0, mmm = 0;

            Int_GetModRM(ref state, &mod, &rrr, &mmm);

            if (reg != (ushort**)0) *reg = (ushort*)RegW(ref state, rrr);

            if (mem != (ushort**)0)
            {
                if (mod == 3)
                    *mem = (ushort*)RegW(ref state, mmm);
                else
                {
                    ushort segment;
                    uint offset;
                    err = Int_GetMMM(ref state, mod, mmm, &segment, &offset);
                    if (err != ErrorCodes.ERR_OK) return err;
                    err = Int_GetPtr(ref state, segment, (ushort)offset, (void**)mem);
                    if (err != ErrorCodes.ERR_OK) return err;
                }
            }

            return ErrorCodes.ERR_OK;
        }

    }
}
