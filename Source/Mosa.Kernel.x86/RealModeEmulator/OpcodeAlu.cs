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
        #region ALU Part 1

        public static unsafe uint Op_ADD_RM(ref State state, uint param)
        {
            uint ret;
            const byte width = 8;
            byte v;
            byte* dest = (byte*)0, src = (byte*)0;
            ret = Int_ParseModRM(ref state, &dest, &src, 0);
            if (ret != 0)
                return ret;
            v = (byte)(*dest + *src);
            ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF | Flags.FLAG_AF);
            state.Flags &= (ushort)~flag;
            state.Flags |= (ushort)((*dest & (1 << (width - 1))) == (*src & (1 << (width - 1))) && (v & (1 << (width - 1))) != (*dest & (1 << (width - 1))) ? Flags.FLAG_OF : 0);
            if (*dest != 0)
                state.Flags |= (ushort)((v <= *src) ? Flags.FLAG_CF : 0);
            else
                state.Flags |= (ushort)((v < *src) ? Flags.FLAG_CF : 0);
            if ((*dest & 15) != 0)
                state.Flags |= (ushort)((v & 15) <= (*src & 15) ? Flags.FLAG_AF : 0);
            else
                state.Flags |= (ushort)((v & 15) < (*src & 15) ? Flags.FLAG_AF : 0);
            SET_COMM_FLAGS(ref state, v, width);
            if (dest != (byte*)0)
                *dest = v;
            return 0;
        }

        public static unsafe uint Op_ADD_RMX(ref State state, uint param)
        {
            uint ret;
            void* destPtr = (void*)0, srcPtr = (void*)0;
            ret = Int_ParseModRMX(ref state, (ushort**)&destPtr, (ushort**)&srcPtr, 0);
            if (ret != 0)
                return ret;
            if (state.Decoder.bOverrideOperand)
            {
                uint v;
                uint* dest = (uint*)destPtr, src = (uint*)srcPtr;
                byte width = 32;
                v = (uint)(*dest + *src);
                ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF | Flags.FLAG_AF);
                state.Flags &= (ushort)~flag;
                state.Flags |= (ushort)((*dest & (1 << (width - 1))) == (*src & (1 << (width - 1))) && (v & (1 << (width - 1))) != (*dest & (1 << (width - 1))) ? Flags.FLAG_OF : 0);
                if (*dest != 0)
                    state.Flags |= (ushort)((v <= *src) ? Flags.FLAG_CF : 0);
                else
                    state.Flags |= (ushort)((v < *src) ? Flags.FLAG_CF : 0);
                if ((*dest & 15) != 0)
                    state.Flags |= (ushort)((v & 15) <= (*src & 15) ? Flags.FLAG_AF : 0);
                else
                    state.Flags |= (ushort)((v & 15) < (*src & 15) ? Flags.FLAG_AF : 0);
                SET_COMM_FLAGS(ref state, v, width);
                if (dest != (uint*)0)
                    *dest = v;
            }
            else
            {
                ushort v;
                ushort* dest = (ushort*)destPtr, src = (ushort*)srcPtr;
                byte width = 16;
                v = (ushort)(*dest + *src);
                ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF | Flags.FLAG_AF);
                state.Flags &= (ushort)~flag;
                state.Flags |= (ushort)((*dest & (1 << (width - 1))) == (*src & (1 << (width - 1))) && (v & (1 << (width - 1))) != (*dest & (1 << (width - 1))) ? Flags.FLAG_OF : 0);
                if (*dest != 0)
                    state.Flags |= (ushort)((v <= *src) ? Flags.FLAG_CF : 0);
                else
                    state.Flags |= (ushort)((v < *src) ? Flags.FLAG_CF : 0);
                if ((*dest & 15) != 0)
                    state.Flags |= (ushort)((v & 15) <= (*src & 15) ? Flags.FLAG_AF : 0);
                else
                    state.Flags |= (ushort)((v & 15) < (*src & 15) ? Flags.FLAG_AF : 0);
                SET_COMM_FLAGS(ref state, v, width);
                if (dest != (ushort*)0)
                    *dest = v;
            }
            return 0;
        }

        public static unsafe uint Op_ADD_MR(ref State state, uint param)
        {
            uint ret;
            const byte width = 8;
            byte v;
            byte* dest = (byte*)0, src = (byte*)0;
            ret = Int_ParseModRM(ref state, &src, &dest, 1);
            if (ret != 0)
                return ret;
            v = (byte)(*dest + *src);
            ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF | Flags.FLAG_AF);
            state.Flags &= (ushort)~flag;
            state.Flags |= (ushort)((*dest & (1 << (width - 1))) == (*src & (1 << (width - 1))) && (v & (1 << (width - 1))) != (*dest & (1 << (width - 1))) ? Flags.FLAG_OF : 0);
            if (*dest != 0)
                state.Flags |= (ushort)((v <= *src) ? Flags.FLAG_CF : 0);
            else
                state.Flags |= (ushort)((v < *src) ? Flags.FLAG_CF : 0);
            if ((*dest & 15) != 0)
                state.Flags |= (ushort)((v & 15) <= (*src & 15) ? Flags.FLAG_AF : 0);
            else
                state.Flags |= (ushort)((v & 15) < (*src & 15) ? Flags.FLAG_AF : 0);
            SET_COMM_FLAGS(ref state, v, width);
            if (dest != (byte*)0)
                *dest = v;
            return 0;
        }

        public static unsafe uint Op_ADD_MRX(ref State state, uint param)
        {
            uint ret;
            void* destPtr = (void*)0, srcPtr = (void*)0;
            ret = Int_ParseModRMX(ref state, (ushort**)&srcPtr, (ushort**)&destPtr, 1);
            if (ret != 0)
                return ret;
            if (state.Decoder.bOverrideOperand)
            {
                uint v;
                uint* dest = (uint*)destPtr, src = (uint*)srcPtr;
                byte width = 32;
                v = (uint)(*dest + *src);
                ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF | Flags.FLAG_AF);
                state.Flags &= (ushort)~flag;
                state.Flags |= (ushort)((*dest & (1 << (width - 1))) == (*src & (1 << (width - 1))) && (v & (1 << (width - 1))) != (*dest & (1 << (width - 1))) ? Flags.FLAG_OF : 0);
                if (*dest != 0)
                    state.Flags |= (ushort)((v <= *src) ? Flags.FLAG_CF : 0);
                else
                    state.Flags |= (ushort)((v < *src) ? Flags.FLAG_CF : 0);
                if ((*dest & 15) != 0)
                    state.Flags |= (ushort)((v & 15) <= (*src & 15) ? Flags.FLAG_AF : 0);
                else
                    state.Flags |= (ushort)((v & 15) < (*src & 15) ? Flags.FLAG_AF : 0);
                SET_COMM_FLAGS(ref state, v, width);
                if (dest != (uint*)0)
                    *dest = v;
            }
            else
            {
                ushort v;
                ushort* dest = (ushort*)destPtr, src = (ushort*)srcPtr;
                byte width = 16;
                v = (ushort)(*dest + *src);
                ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF | Flags.FLAG_AF);
                state.Flags &= (ushort)~flag;
                state.Flags |= (ushort)((*dest & (1 << (width - 1))) == (*src & (1 << (width - 1))) && (v & (1 << (width - 1))) != (*dest & (1 << (width - 1))) ? Flags.FLAG_OF : 0);
                if (*dest != 0)
                    state.Flags |= (ushort)((v <= *src) ? Flags.FLAG_CF : 0);
                else
                    state.Flags |= (ushort)((v < *src) ? Flags.FLAG_CF : 0);
                if ((*dest & 15) != 0)
                    state.Flags |= (ushort)((v & 15) <= (*src & 15) ? Flags.FLAG_AF : 0);
                else
                    state.Flags |= (ushort)((v & 15) < (*src & 15) ? Flags.FLAG_AF : 0);
                SET_COMM_FLAGS(ref state, v, width);
                if (dest != (ushort*)0)
                    *dest = v;
            }
            return 0;
        }

        public static unsafe uint Op_ADD_AI(ref State state, uint param)
        {
            const byte width = 8;
            byte srcData, v;
            byte* dest = (byte*)state.AX.Address, src = &srcData;
            uint ret = READ_INSTR8(ref state, ref srcData);
            if (ret != 0)
                return ret;
            v = (byte)(*dest + *src);
            ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF | Flags.FLAG_AF);
            state.Flags &= (ushort)~flag;
            state.Flags |= (ushort)((*dest & (1 << (width - 1))) == (*src & (1 << (width - 1))) && (v & (1 << (width - 1))) != (*dest & (1 << (width - 1))) ? Flags.FLAG_OF : 0);
            if (*dest != 0)
                state.Flags |= (ushort)((v <= *src) ? Flags.FLAG_CF : 0);
            else
                state.Flags |= (ushort)((v < *src) ? Flags.FLAG_CF : 0);
            if ((*dest & 15) != 0)
                state.Flags |= (ushort)((v & 15) <= (*src & 15) ? Flags.FLAG_AF : 0);
            else
                state.Flags |= (ushort)((v & 15) < (*src & 15) ? Flags.FLAG_AF : 0);
            SET_COMM_FLAGS(ref state, v, width);
            if (dest != (byte*)0)
                *dest = v;
            return 0;
        }

        public static unsafe uint Op_ADD_AIX(ref State state, uint param)
        {
            if (state.Decoder.bOverrideOperand)
            {
                uint srcData, v;
                uint* dest = (uint*)state.AX.Address, src = &srcData;
                byte width = 32;
                uint ret = READ_INSTR32(ref state, ref srcData);
                if (ret != 0)
                    return ret;
                v = (uint)(*dest + *src);
                ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF | Flags.FLAG_AF);
                state.Flags &= (ushort)~flag;
                state.Flags |= (ushort)((*dest & (1 << (width - 1))) == (*src & (1 << (width - 1))) && (v & (1 << (width - 1))) != (*dest & (1 << (width - 1))) ? Flags.FLAG_OF : 0);
                if (*dest != 0)
                    state.Flags |= (ushort)((v <= *src) ? Flags.FLAG_CF : 0);
                else
                    state.Flags |= (ushort)((v < *src) ? Flags.FLAG_CF : 0);
                if ((*dest & 15) != 0)
                    state.Flags |= (ushort)((v & 15) <= (*src & 15) ? Flags.FLAG_AF : 0);
                else
                    state.Flags |= (ushort)((v & 15) < (*src & 15) ? Flags.FLAG_AF : 0);
                SET_COMM_FLAGS(ref state, v, width);
                if (dest != (uint*)0)
                    *dest = v;
            }
            else
            {
                ushort srcData, v;
                ushort* dest = (ushort*)state.AX.Address, src = &srcData;
                byte width = 16;
                uint ret = READ_INSTR16(ref state, ref srcData);
                if (ret != 0)
                    return ret;
                v = (ushort)(*dest + *src);
                ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF | Flags.FLAG_AF);
                state.Flags &= (ushort)~flag;
                state.Flags |= (ushort)((*dest & (1 << (width - 1))) == (*src & (1 << (width - 1))) && (v & (1 << (width - 1))) != (*dest & (1 << (width - 1))) ? Flags.FLAG_OF : 0);
                if (*dest != 0)
                    state.Flags |= (ushort)((v <= *src) ? Flags.FLAG_CF : 0);
                else
                    state.Flags |= (ushort)((v < *src) ? Flags.FLAG_CF : 0);
                if ((*dest & 15) != 0)
                    state.Flags |= (ushort)((v & 15) <= (*src & 15) ? Flags.FLAG_AF : 0);
                else
                    state.Flags |= (ushort)((v & 15) < (*src & 15) ? Flags.FLAG_AF : 0);
                SET_COMM_FLAGS(ref state, v, width);
                if (dest != (ushort*)0)
                    *dest = v;
            }
            return 0;
        }

        public static unsafe uint Op_OR_RM(ref State state, uint param)
        {
            uint ret;
            const byte width = 8;
            byte v;
            byte* dest = (byte*)0, src = (byte*)0;
            ret = Int_ParseModRM(ref state, &dest, &src, 0);
            if (ret != 0)
                return ret;
            v = (byte)(*dest | *src);
            ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF);
            state.Flags &= (ushort)~flag;
            SET_COMM_FLAGS(ref state, v, width);
            if (dest != (byte*)0)
                *dest = v;
            return 0;
        }

        public static unsafe uint Op_OR_RMX(ref State state, uint param)
        {
            uint ret;
            void* destPtr = (void*)0, srcPtr = (void*)0;
            ret = Int_ParseModRMX(ref state, (ushort**)&destPtr, (ushort**)&srcPtr, 0);
            if (ret != 0)
                return ret;
            if (state.Decoder.bOverrideOperand)
            {
                uint v;
                uint* dest = (uint*)destPtr, src = (uint*)srcPtr;
                byte width = 32;
                v = (uint)(*dest | *src);
                ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF);
                state.Flags &= (ushort)~flag;
                SET_COMM_FLAGS(ref state, v, width);
                if (dest != (uint*)0)
                    *dest = v;
            }
            else
            {
                ushort v;
                ushort* dest = (ushort*)destPtr, src = (ushort*)srcPtr;
                byte width = 16;
                v = (ushort)(*dest | *src);
                ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF);
                state.Flags &= (ushort)~flag;
                SET_COMM_FLAGS(ref state, v, width);
                if (dest != (ushort*)0)
                    *dest = v;
            }
            return 0;
        }

        public static unsafe uint Op_OR_MR(ref State state, uint param)
        {
            uint ret;
            const byte width = 8;
            byte v;
            byte* dest = (byte*)0, src = (byte*)0;
            ret = Int_ParseModRM(ref state, &src, &dest, 1);
            if (ret != 0)
                return ret;
            v = (byte)(*dest | *src);
            ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF);
            state.Flags &= (ushort)~flag;
            SET_COMM_FLAGS(ref state, v, width);
            if (dest != (byte*)0)
                *dest = v;
            return 0;
        }

        public static unsafe uint Op_OR_MRX(ref State state, uint param)
        {
            uint ret;
            void* destPtr = (void*)0, srcPtr = (void*)0;
            ret = Int_ParseModRMX(ref state, (ushort**)&srcPtr, (ushort**)&destPtr, 1);
            if (ret != 0)
                return ret;
            if (state.Decoder.bOverrideOperand)
            {
                uint v;
                uint* dest = (uint*)destPtr, src = (uint*)srcPtr;
                byte width = 32;
                v = (uint)(*dest | *src);
                ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF);
                state.Flags &= (ushort)~flag;
                SET_COMM_FLAGS(ref state, v, width);
                if (dest != (uint*)0)
                    *dest = v;
            }
            else
            {
                ushort v;
                ushort* dest = (ushort*)destPtr, src = (ushort*)srcPtr;
                byte width = 16;
                v = (ushort)(*dest | *src);
                ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF);
                state.Flags &= (ushort)~flag;
                SET_COMM_FLAGS(ref state, v, width);
                if (dest != (ushort*)0)
                    *dest = v;
            }
            return 0;
        }

        public static unsafe uint Op_OR_AI(ref State state, uint param)
        {
            const byte width = 8;
            byte srcData, v;
            byte* dest = (byte*)state.AX.Address, src = &srcData;
            uint ret = READ_INSTR8(ref state, ref srcData);
            if (ret != 0)
                return ret;
            v = (byte)(*dest | *src);
            ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF);
            state.Flags &= (ushort)~flag;
            SET_COMM_FLAGS(ref state, v, width);
            if (dest != (byte*)0)
                *dest = v;
            return 0;
        }

        public static unsafe uint Op_OR_AIX(ref State state, uint param)
        {
            if (state.Decoder.bOverrideOperand)
            {
                uint srcData, v;
                uint* dest = (uint*)state.AX.Address, src = &srcData;
                byte width = 32;
                uint ret = READ_INSTR32(ref state, ref srcData);
                if (ret != 0)
                    return ret;
                v = (uint)(*dest | *src);
                ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF);
                state.Flags &= (ushort)~flag;
                SET_COMM_FLAGS(ref state, v, width);
                if (dest != (uint*)0)
                    *dest = v;
            }
            else
            {
                ushort srcData, v;
                ushort* dest = (ushort*)state.AX.Address, src = &srcData;
                byte width = 16;
                uint ret = READ_INSTR16(ref state, ref srcData);
                if (ret != 0)
                    return ret;
                v = (ushort)(*dest | *src);
                ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF);
                state.Flags &= (ushort)~flag;
                SET_COMM_FLAGS(ref state, v, width);
                if (dest != (ushort*)0)
                    *dest = v;
            }
            return 0;
        }

        public static unsafe uint Op_ADC_RM(ref State state, uint param)
        {
            uint ret;
            const byte width = 8;
            byte v;
            byte* dest = (byte*)0, src = (byte*)0;
            ret = Int_ParseModRM(ref state, &dest, &src, 0);
            if (ret != 0)
                return ret;
            v = (byte)(*dest + *src + ((state.Flags & Flags.FLAG_CF) == Flags.FLAG_CF ? 1 : 0));
            ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF | Flags.FLAG_AF);
            state.Flags &= (ushort)~flag;
            state.Flags |= (ushort)((*dest & (1 << (width - 1))) == (*src & (1 << (width - 1))) && (v & (1 << (width - 1))) != (*dest & (1 << (width - 1))) ? Flags.FLAG_OF : 0);
            if (*dest != 0)
                state.Flags |= (ushort)((v <= *src) ? Flags.FLAG_CF : 0);
            else
                state.Flags |= (ushort)((v < *src) ? Flags.FLAG_CF : 0);
            if ((*dest & 15) != 0)
                state.Flags |= (ushort)((v & 15) <= (*src & 15) ? Flags.FLAG_AF : 0);
            else
                state.Flags |= (ushort)((v & 15) < (*src & 15) ? Flags.FLAG_AF : 0);
            if (*dest != 0 && v == *src)
                state.Flags |= Flags.FLAG_CF;
            SET_COMM_FLAGS(ref state, v, width);
            if (dest != (byte*)0)
                *dest = v;
            return 0;
        }

        public static unsafe uint Op_ADC_RMX(ref State state, uint param)
        {
            uint ret;
            void* destPtr = (void*)0, srcPtr = (void*)0;
            ret = Int_ParseModRMX(ref state, (ushort**)&destPtr, (ushort**)&srcPtr, 0);
            if (ret != 0)
                return ret;
            if (state.Decoder.bOverrideOperand)
            {
                uint v;
                uint* dest = (uint*)destPtr, src = (uint*)srcPtr;
                byte width = 32;
                v = (uint)(*dest + *src + ((state.Flags & Flags.FLAG_CF) == Flags.FLAG_CF ? 1 : 0));
                ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF | Flags.FLAG_AF);
                state.Flags &= (ushort)~flag;
                state.Flags |= (ushort)((*dest & (1 << (width - 1))) == (*src & (1 << (width - 1))) && (v & (1 << (width - 1))) != (*dest & (1 << (width - 1))) ? Flags.FLAG_OF : 0);
                if (*dest != 0)
                    state.Flags |= (ushort)((v <= *src) ? Flags.FLAG_CF : 0);
                else
                    state.Flags |= (ushort)((v < *src) ? Flags.FLAG_CF : 0);
                if ((*dest & 15) != 0)
                    state.Flags |= (ushort)((v & 15) <= (*src & 15) ? Flags.FLAG_AF : 0);
                else
                    state.Flags |= (ushort)((v & 15) < (*src & 15) ? Flags.FLAG_AF : 0);
                if (*dest != 0 && v == *src)
                    state.Flags |= Flags.FLAG_CF;
                SET_COMM_FLAGS(ref state, v, width);
                if (dest != (uint*)0)
                    *dest = v;
            }
            else
            {
                ushort v;
                ushort* dest = (ushort*)destPtr, src = (ushort*)srcPtr;
                byte width = 16;
                v = (ushort)(*dest + *src + ((state.Flags & Flags.FLAG_CF) == Flags.FLAG_CF ? 1 : 0));
                ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF | Flags.FLAG_AF);
                state.Flags &= (ushort)~flag;
                state.Flags |= (ushort)((*dest & (1 << (width - 1))) == (*src & (1 << (width - 1))) && (v & (1 << (width - 1))) != (*dest & (1 << (width - 1))) ? Flags.FLAG_OF : 0);
                if (*dest != 0)
                    state.Flags |= (ushort)((v <= *src) ? Flags.FLAG_CF : 0);
                else
                    state.Flags |= (ushort)((v < *src) ? Flags.FLAG_CF : 0);
                if ((*dest & 15) != 0)
                    state.Flags |= (ushort)((v & 15) <= (*src & 15) ? Flags.FLAG_AF : 0);
                else
                    state.Flags |= (ushort)((v & 15) < (*src & 15) ? Flags.FLAG_AF : 0);
                if (*dest != 0 && v == *src)
                    state.Flags |= Flags.FLAG_CF;
                SET_COMM_FLAGS(ref state, v, width);
                if (dest != (ushort*)0)
                    *dest = v;
            }
            return 0;
        }

        public static unsafe uint Op_ADC_MR(ref State state, uint param)
        {
            uint ret;
            const byte width = 8;
            byte v;
            byte* dest = (byte*)0, src = (byte*)0;
            ret = Int_ParseModRM(ref state, &src, &dest, 1);
            if (ret != 0)
                return ret;
            v = (byte)(*dest + *src + ((state.Flags & Flags.FLAG_CF) == Flags.FLAG_CF ? 1 : 0));
            ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF | Flags.FLAG_AF);
            state.Flags &= (ushort)~flag;
            state.Flags |= (ushort)((*dest & (1 << (width - 1))) == (*src & (1 << (width - 1))) && (v & (1 << (width - 1))) != (*dest & (1 << (width - 1))) ? Flags.FLAG_OF : 0);
            if (*dest != 0)
                state.Flags |= (ushort)((v <= *src) ? Flags.FLAG_CF : 0);
            else
                state.Flags |= (ushort)((v < *src) ? Flags.FLAG_CF : 0);
            if ((*dest & 15) != 0)
                state.Flags |= (ushort)((v & 15) <= (*src & 15) ? Flags.FLAG_AF : 0);
            else
                state.Flags |= (ushort)((v & 15) < (*src & 15) ? Flags.FLAG_AF : 0);
            if (*dest != 0 && v == *src)
                state.Flags |= Flags.FLAG_CF;
            SET_COMM_FLAGS(ref state, v, width);
            if (dest != (byte*)0)
                *dest = v;
            return 0;
        }

        public static unsafe uint Op_ADC_MRX(ref State state, uint param)
        {
            uint ret;
            void* destPtr = (void*)0, srcPtr = (void*)0;
            ret = Int_ParseModRMX(ref state, (ushort**)&srcPtr, (ushort**)&destPtr, 1);
            if (ret != 0)
                return ret;
            if (state.Decoder.bOverrideOperand)
            {
                uint v;
                uint* dest = (uint*)destPtr, src = (uint*)srcPtr;
                byte width = 32;
                v = (uint)(*dest + *src + ((state.Flags & Flags.FLAG_CF) == Flags.FLAG_CF ? 1 : 0));
                ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF | Flags.FLAG_AF);
                state.Flags &= (ushort)~flag;
                state.Flags |= (ushort)((*dest & (1 << (width - 1))) == (*src & (1 << (width - 1))) && (v & (1 << (width - 1))) != (*dest & (1 << (width - 1))) ? Flags.FLAG_OF : 0);
                if (*dest != 0)
                    state.Flags |= (ushort)((v <= *src) ? Flags.FLAG_CF : 0);
                else
                    state.Flags |= (ushort)((v < *src) ? Flags.FLAG_CF : 0);
                if ((*dest & 15) != 0)
                    state.Flags |= (ushort)((v & 15) <= (*src & 15) ? Flags.FLAG_AF : 0);
                else
                    state.Flags |= (ushort)((v & 15) < (*src & 15) ? Flags.FLAG_AF : 0);
                if (*dest != 0 && v == *src)
                    state.Flags |= Flags.FLAG_CF;
                SET_COMM_FLAGS(ref state, v, width);
                if (dest != (uint*)0)
                    *dest = v;
            }
            else
            {
                ushort v;
                ushort* dest = (ushort*)destPtr, src = (ushort*)srcPtr;
                byte width = 16;
                v = (ushort)(*dest + *src + ((state.Flags & Flags.FLAG_CF) == Flags.FLAG_CF ? 1 : 0));
                ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF | Flags.FLAG_AF);
                state.Flags &= (ushort)~flag;
                state.Flags |= (ushort)((*dest & (1 << (width - 1))) == (*src & (1 << (width - 1))) && (v & (1 << (width - 1))) != (*dest & (1 << (width - 1))) ? Flags.FLAG_OF : 0);
                if (*dest != 0)
                    state.Flags |= (ushort)((v <= *src) ? Flags.FLAG_CF : 0);
                else
                    state.Flags |= (ushort)((v < *src) ? Flags.FLAG_CF : 0);
                if ((*dest & 15) != 0)
                    state.Flags |= (ushort)((v & 15) <= (*src & 15) ? Flags.FLAG_AF : 0);
                else
                    state.Flags |= (ushort)((v & 15) < (*src & 15) ? Flags.FLAG_AF : 0);
                if (*dest != 0 && v == *src)
                    state.Flags |= Flags.FLAG_CF;
                SET_COMM_FLAGS(ref state, v, width);
                if (dest != (ushort*)0)
                    *dest = v;
            }
            return 0;
        }

        public static unsafe uint Op_ADC_AI(ref State state, uint param)
        {
            const byte width = 8;
            byte srcData, v;
            byte* dest = (byte*)state.AX.Address, src = &srcData;
            uint ret = READ_INSTR8(ref state, ref srcData);
            if (ret != 0)
                return ret;
            v = (byte)(*dest + *src + ((state.Flags & Flags.FLAG_CF) == Flags.FLAG_CF ? 1 : 0));
            ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF | Flags.FLAG_AF);
            state.Flags &= (ushort)~flag;
            state.Flags |= (ushort)((*dest & (1 << (width - 1))) == (*src & (1 << (width - 1))) && (v & (1 << (width - 1))) != (*dest & (1 << (width - 1))) ? Flags.FLAG_OF : 0);
            if (*dest != 0)
                state.Flags |= (ushort)((v <= *src) ? Flags.FLAG_CF : 0);
            else
                state.Flags |= (ushort)((v < *src) ? Flags.FLAG_CF : 0);
            if ((*dest & 15) != 0)
                state.Flags |= (ushort)((v & 15) <= (*src & 15) ? Flags.FLAG_AF : 0);
            else
                state.Flags |= (ushort)((v & 15) < (*src & 15) ? Flags.FLAG_AF : 0);
            if (*dest != 0 && v == *src)
                state.Flags |= Flags.FLAG_CF;
            SET_COMM_FLAGS(ref state, v, width);
            if (dest != (byte*)0)
                *dest = v;
            return 0;
        }

        public static unsafe uint Op_ADC_AIX(ref State state, uint param)
        {
            if (state.Decoder.bOverrideOperand)
            {
                uint srcData, v;
                uint* dest = (uint*)state.AX.Address, src = &srcData;
                byte width = 32;
                uint ret = READ_INSTR32(ref state, ref srcData);
                if (ret != 0)
                    return ret;
                v = (uint)(*dest + *src + ((state.Flags & Flags.FLAG_CF) == Flags.FLAG_CF ? 1 : 0));
                ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF | Flags.FLAG_AF);
                state.Flags &= (ushort)~flag;
                state.Flags |= (ushort)((*dest & (1 << (width - 1))) == (*src & (1 << (width - 1))) && (v & (1 << (width - 1))) != (*dest & (1 << (width - 1))) ? Flags.FLAG_OF : 0);
                if (*dest != 0)
                    state.Flags |= (ushort)((v <= *src) ? Flags.FLAG_CF : 0);
                else
                    state.Flags |= (ushort)((v < *src) ? Flags.FLAG_CF : 0);
                if ((*dest & 15) != 0)
                    state.Flags |= (ushort)((v & 15) <= (*src & 15) ? Flags.FLAG_AF : 0);
                else
                    state.Flags |= (ushort)((v & 15) < (*src & 15) ? Flags.FLAG_AF : 0);
                if (*dest != 0 && v == *src)
                    state.Flags |= Flags.FLAG_CF;
                SET_COMM_FLAGS(ref state, v, width);
                if (dest != (uint*)0)
                    *dest = v;
            }
            else
            {
                ushort srcData, v;
                ushort* dest = (ushort*)state.AX.Address, src = &srcData;
                byte width = 16;
                uint ret = READ_INSTR16(ref state, ref srcData);
                if (ret != 0)
                    return ret;
                v = (ushort)(*dest + *src + ((state.Flags & Flags.FLAG_CF) == Flags.FLAG_CF ? 1 : 0));
                ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF | Flags.FLAG_AF);
                state.Flags &= (ushort)~flag;
                state.Flags |= (ushort)((*dest & (1 << (width - 1))) == (*src & (1 << (width - 1))) && (v & (1 << (width - 1))) != (*dest & (1 << (width - 1))) ? Flags.FLAG_OF : 0);
                if (*dest != 0)
                    state.Flags |= (ushort)((v <= *src) ? Flags.FLAG_CF : 0);
                else
                    state.Flags |= (ushort)((v < *src) ? Flags.FLAG_CF : 0);
                if ((*dest & 15) != 0)
                    state.Flags |= (ushort)((v & 15) <= (*src & 15) ? Flags.FLAG_AF : 0);
                else
                    state.Flags |= (ushort)((v & 15) < (*src & 15) ? Flags.FLAG_AF : 0);
                if (*dest != 0 && v == *src)
                    state.Flags |= Flags.FLAG_CF;
                SET_COMM_FLAGS(ref state, v, width);
                if (dest != (ushort*)0)
                    *dest = v;
            }
            return 0;
        }

        public static unsafe uint Op_SBB_RM(ref State state, uint param)
        {
            uint ret;
            const byte width = 8;
            byte v;
            byte* dest = (byte*)0, src = (byte*)0;
            ret = Int_ParseModRM(ref state, &dest, &src, 0);
            if (ret != 0)
                return ret;
            uint c = (uint)((state.Flags & Flags.FLAG_CF) == Flags.FLAG_CF ? 1 : 0);
            ulong r = (*src + c);
            v = (byte)(*dest - r);
            ushort flag = (Flags.FLAG_OF | Flags.FLAG_CF | Flags.FLAG_AF);
            state.Flags &= (ushort)~flag;
            state.Flags |= (ushort)((*dest < r) ? Flags.FLAG_CF : 0);
            state.Flags |= (ushort)(((ulong)(*dest & 15) < (r & 15) - c) ? Flags.FLAG_AF : 0);
            uint _sub_tmp = (uint)(((*dest ^ r) & (ulong)(*dest ^ v)) & (1 << (width - 1)));
            if (_sub_tmp != 0)
                state.Flags |= Flags.FLAG_OF;
            SET_COMM_FLAGS(ref state, v, width);
            if (dest != (byte*)0)
                *dest = v;
            return 0;
        }

        public static unsafe uint Op_SBB_RMX(ref State state, uint param)
        {
            uint ret;
            void* destPtr = (void*)0, srcPtr = (void*)0;
            ret = Int_ParseModRMX(ref state, (ushort**)&destPtr, (ushort**)&srcPtr, 0);
            if (ret != 0)
                return ret;
            if (state.Decoder.bOverrideOperand)
            {
                uint v;
                uint* dest = (uint*)destPtr, src = (uint*)srcPtr;
                byte width = 32;
                uint c = (uint)((state.Flags & Flags.FLAG_CF) == Flags.FLAG_CF ? 1 : 0);
                ulong r = (*src + c);
                v = (uint)(*dest - r);
                ushort flag = (Flags.FLAG_OF | Flags.FLAG_CF | Flags.FLAG_AF);
                state.Flags &= (ushort)~flag;
                state.Flags |= (ushort)((*dest < r) ? Flags.FLAG_CF : 0);
                state.Flags |= (ushort)(((*dest & 15) < (r & 15) - c) ? Flags.FLAG_AF : 0);
                uint _sub_tmp = (uint)(((*dest ^ r) & (ulong)(*dest ^ v)) & (ulong)(1 << (width - 1)));
                if (_sub_tmp != 0)
                    state.Flags |= Flags.FLAG_OF;
                SET_COMM_FLAGS(ref state, v, width);
                if (dest != (uint*)0)
                    *dest = v;
            }
            else
            {
                ushort v;
                ushort* dest = (ushort*)destPtr, src = (ushort*)srcPtr;
                byte width = 16;
                uint c = (uint)((state.Flags & Flags.FLAG_CF) == Flags.FLAG_CF ? 1 : 0);
                ulong r = (*src + c);
                v = (ushort)(*dest - r);
                ushort flag = (Flags.FLAG_OF | Flags.FLAG_CF | Flags.FLAG_AF);
                state.Flags &= (ushort)~flag;
                state.Flags |= (ushort)((*dest < r) ? Flags.FLAG_CF : 0);
                state.Flags |= (ushort)(((ulong)(*dest & 15) < (r & 15) - c) ? Flags.FLAG_AF : 0);
                uint _sub_tmp = (uint)(((*dest ^ r) & (ulong)(*dest ^ v)) & (ulong)(1 << (width - 1)));
                if (_sub_tmp != 0)
                    state.Flags |= Flags.FLAG_OF;
                SET_COMM_FLAGS(ref state, v, width);
                if (dest != (ushort*)0)
                    *dest = v;
            }
            return 0;
        }

        public static unsafe uint Op_SBB_MR(ref State state, uint param)
        {
            uint ret;
            const byte width = 8;
            byte v;
            byte* dest = (byte*)0, src = (byte*)0;
            ret = Int_ParseModRM(ref state, &src, &dest, 1);
            if (ret != 0)
                return ret;
            uint c = (uint)((state.Flags & Flags.FLAG_CF) == Flags.FLAG_CF ? 1 : 0);
            ulong r = (*src + c);
            v = (byte)(*dest - r);
            ushort flag = (Flags.FLAG_OF | Flags.FLAG_CF | Flags.FLAG_AF);
            state.Flags &= (ushort)~flag;
            state.Flags |= (ushort)((*dest < r) ? Flags.FLAG_CF : 0);
            state.Flags |= (ushort)(((ulong)(*dest & 15) < (r & 15) - c) ? Flags.FLAG_AF : 0);
            uint _sub_tmp = (uint)(((*dest ^ r) & (ulong)(*dest ^ v)) & (1 << (width - 1)));
            if (_sub_tmp != 0)
                state.Flags |= Flags.FLAG_OF;
            SET_COMM_FLAGS(ref state, v, width);
            if (dest != (byte*)0)
                *dest = v;
            return 0;
        }

        public static unsafe uint Op_SBB_MRX(ref State state, uint param)
        {
            uint ret;
            void* destPtr = (void*)0, srcPtr = (void*)0;
            ret = Int_ParseModRMX(ref state, (ushort**)&srcPtr, (ushort**)&destPtr, 1);
            if (ret != 0)
                return ret;
            if (state.Decoder.bOverrideOperand)
            {
                uint v;
                uint* dest = (uint*)destPtr, src = (uint*)srcPtr;
                byte width = 32;
                uint c = (uint)((state.Flags & Flags.FLAG_CF) == Flags.FLAG_CF ? 1 : 0);
                ulong r = (*src + c);
                v = (uint)(*dest - r);
                ushort flag = (Flags.FLAG_OF | Flags.FLAG_CF | Flags.FLAG_AF);
                state.Flags &= (ushort)~flag;
                state.Flags |= (ushort)((*dest < r) ? Flags.FLAG_CF : 0);
                state.Flags |= (ushort)(((*dest & 15) < (r & 15) - c) ? Flags.FLAG_AF : 0);
                uint _sub_tmp = (uint)(((*dest ^ r) & (*dest ^ v)) & (ulong)(1 << (width - 1)));
                if (_sub_tmp != 0)
                    state.Flags |= Flags.FLAG_OF;
                SET_COMM_FLAGS(ref state, v, width);
                if (dest != (uint*)0)
                    *dest = v;
            }
            else
            {
                ushort v;
                ushort* dest = (ushort*)destPtr, src = (ushort*)srcPtr;
                byte width = 16;
                uint c = (uint)((state.Flags & Flags.FLAG_CF) == Flags.FLAG_CF ? 1 : 0);
                ulong r = (*src + c);
                v = (ushort)(*dest - r);
                ushort flag = (Flags.FLAG_OF | Flags.FLAG_CF | Flags.FLAG_AF);
                state.Flags &= (ushort)~flag;
                state.Flags |= (ushort)((*dest < r) ? Flags.FLAG_CF : 0);
                state.Flags |= (ushort)(((ulong)(*dest & 15) < (r & 15) - c) ? Flags.FLAG_AF : 0);
                uint _sub_tmp = (uint)(((*dest ^ r) & (ulong)(*dest ^ v)) & (ulong)(1 << (width - 1)));
                if (_sub_tmp != 0)
                    state.Flags |= Flags.FLAG_OF;
                SET_COMM_FLAGS(ref state, v, width);
                if (dest != (ushort*)0)
                    *dest = v;
            }
            return 0;
        }

        public static unsafe uint Op_SBB_AI(ref State state, uint param)
        {
            const byte width = 8;
            byte srcData, v;
            byte* dest = (byte*)state.AX.Address, src = &srcData;
            uint ret = READ_INSTR8(ref state, ref srcData);
            if (ret != 0)
                return ret;
            uint c = (uint)((state.Flags & Flags.FLAG_CF) == Flags.FLAG_CF ? 1 : 0);
            ulong r = (*src + c);
            v = (byte)(*dest - r);
            ushort flag = (Flags.FLAG_OF | Flags.FLAG_CF | Flags.FLAG_AF);
            state.Flags &= (ushort)~flag;
            state.Flags |= (ushort)((*dest < r) ? Flags.FLAG_CF : 0);
            state.Flags |= (ushort)(((ulong)(*dest & 15) < (r & 15) - c) ? Flags.FLAG_AF : 0);
            uint _sub_tmp = (uint)(((*dest ^ r) & (ulong)(*dest ^ v)) & (1 << (width - 1)));
            if (_sub_tmp != 0)
                state.Flags |= Flags.FLAG_OF;
            SET_COMM_FLAGS(ref state, v, width);
            if (dest != (byte*)0)
                *dest = v;
            return 0;
        }

        public static unsafe uint Op_SBB_AIX(ref State state, uint param)
        {
            if (state.Decoder.bOverrideOperand)
            {
                uint srcData, v;
                uint* dest = (uint*)state.AX.Address, src = &srcData;
                byte width = 32;
                uint ret = READ_INSTR32(ref state, ref srcData);
                if (ret != 0)
                    return ret;
                uint c = (uint)((state.Flags & Flags.FLAG_CF) == Flags.FLAG_CF ? 1 : 0);
                ulong r = (*src + c);
                v = (uint)(*dest - r);
                ushort flag = (Flags.FLAG_OF | Flags.FLAG_CF | Flags.FLAG_AF);
                state.Flags &= (ushort)~flag;
                state.Flags |= (ushort)((*dest < r) ? Flags.FLAG_CF : 0);
                state.Flags |= (ushort)(((*dest & 15) < (r & 15) - c) ? Flags.FLAG_AF : 0);
                uint _sub_tmp = (uint)(((*dest ^ r) & (*dest ^ v)) & (ulong)(1 << (width - 1)));
                if (_sub_tmp != 0)
                    state.Flags |= Flags.FLAG_OF;
                SET_COMM_FLAGS(ref state, v, width);
                if (dest != (uint*)0)
                    *dest = v;
            }
            else
            {
                ushort srcData, v;
                ushort* dest = (ushort*)state.AX.Address, src = &srcData;
                byte width = 16;
                uint ret = READ_INSTR16(ref state, ref srcData);
                if (ret != 0)
                    return ret;
                uint c = (uint)((state.Flags & Flags.FLAG_CF) == Flags.FLAG_CF ? 1 : 0);
                ulong r = (*src + c);
                v = (ushort)(*dest - r);
                ushort flag = (Flags.FLAG_OF | Flags.FLAG_CF | Flags.FLAG_AF);
                state.Flags &= (ushort)~flag;
                state.Flags |= (ushort)((*dest < r) ? Flags.FLAG_CF : 0);
                state.Flags |= (ushort)(((ulong)(*dest & 15) < (r & 15) - c) ? Flags.FLAG_AF : 0);
                uint _sub_tmp = (uint)(((*dest ^ r) & (ulong)(*dest ^ v)) & (ulong)(1 << (width - 1)));
                if (_sub_tmp != 0)
                    state.Flags |= Flags.FLAG_OF;
                SET_COMM_FLAGS(ref state, v, width);
                if (dest != (ushort*)0)
                    *dest = v;
            }
            return 0;
        }

        public static unsafe uint Op_AND_RM(ref State state, uint param)
        {
            uint ret;
            const byte width = 8;
            byte v;
            byte* dest = (byte*)0, src = (byte*)0;
            ret = Int_ParseModRM(ref state, &dest, &src, 0);
            if (ret != 0)
                return ret;
            v = (byte)(*dest & *src);
            ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF);
            state.Flags &= (ushort)~flag;
            SET_COMM_FLAGS(ref state, v, width);
            if (dest != (byte*)0)
                *dest = v;
            return 0;
        }

        public static unsafe uint Op_AND_RMX(ref State state, uint param)
        {
            uint ret;
            void* destPtr = (void*)0, srcPtr = (void*)0;
            ret = Int_ParseModRMX(ref state, (ushort**)&destPtr, (ushort**)&srcPtr, 0);
            if (ret != 0)
                return ret;
            if (state.Decoder.bOverrideOperand)
            {
                uint v;
                uint* dest = (uint*)destPtr, src = (uint*)srcPtr;
                byte width = 32;
                v = (uint)(*dest & *src);
                ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF);
                state.Flags &= (ushort)~flag;
                SET_COMM_FLAGS(ref state, v, width);
                if (dest != (uint*)0)
                    *dest = v;
            }
            else
            {
                ushort v;
                ushort* dest = (ushort*)destPtr, src = (ushort*)srcPtr;
                byte width = 16;
                v = (ushort)(*dest & *src);
                ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF);
                state.Flags &= (ushort)~flag;
                SET_COMM_FLAGS(ref state, v, width);
                if (dest != (ushort*)0)
                    *dest = v;
            }
            return 0;
        }

        public static unsafe uint Op_AND_MR(ref State state, uint param)
        {
            uint ret;
            const byte width = 8;
            byte v;
            byte* dest = (byte*)0, src = (byte*)0;
            ret = Int_ParseModRM(ref state, &src, &dest, 1);
            if (ret != 0)
                return ret;
            v = (byte)(*dest & *src);
            ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF);
            state.Flags &= (ushort)~flag;
            SET_COMM_FLAGS(ref state, v, width);
            if (dest != (byte*)0)
                *dest = v;
            return 0;
        }

        public static unsafe uint Op_AND_MRX(ref State state, uint param)
        {
            uint ret;
            void* destPtr = (void*)0, srcPtr = (void*)0;
            ret = Int_ParseModRMX(ref state, (ushort**)&srcPtr, (ushort**)&destPtr, 1);
            if (ret != 0)
                return ret;
            if (state.Decoder.bOverrideOperand)
            {
                uint v;
                uint* dest = (uint*)destPtr, src = (uint*)srcPtr;
                byte width = 32;
                v = (uint)(*dest & *src);
                ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF);
                state.Flags &= (ushort)~flag;
                SET_COMM_FLAGS(ref state, v, width);
                if (dest != (uint*)0)
                    *dest = v;
            }
            else
            {
                ushort v;
                ushort* dest = (ushort*)destPtr, src = (ushort*)srcPtr;
                byte width = 16;
                v = (ushort)(*dest & *src);
                ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF);
                state.Flags &= (ushort)~flag;
                SET_COMM_FLAGS(ref state, v, width);
                if (dest != (ushort*)0)
                    *dest = v;
            }
            return 0;
        }

        public static unsafe uint Op_AND_AI(ref State state, uint param)
        {
            const byte width = 8;
            byte srcData, v;
            byte* dest = (byte*)state.AX.Address, src = &srcData;
            uint ret = READ_INSTR8(ref state, ref srcData);
            if (ret != 0)
                return ret;
            v = (byte)(*dest & *src);
            ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF);
            state.Flags &= (ushort)~flag;
            SET_COMM_FLAGS(ref state, v, width);
            if (dest != (byte*)0)
                *dest = v;
            return 0;
        }

        public static unsafe uint Op_AND_AIX(ref State state, uint param)
        {
            if (state.Decoder.bOverrideOperand)
            {
                uint srcData, v;
                uint* dest = (uint*)state.AX.Address, src = &srcData;
                byte width = 32;
                uint ret = READ_INSTR32(ref state, ref srcData);
                if (ret != 0)
                    return ret;
                v = (uint)(*dest & *src);
                ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF);
                state.Flags &= (ushort)~flag;
                SET_COMM_FLAGS(ref state, v, width);
                if (dest != (uint*)0)
                    *dest = v;
            }
            else
            {
                ushort srcData, v;
                ushort* dest = (ushort*)state.AX.Address, src = &srcData;
                byte width = 16;
                uint ret = READ_INSTR16(ref state, ref srcData);
                if (ret != 0)
                    return ret;
                v = (ushort)(*dest & *src);
                ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF);
                state.Flags &= (ushort)~flag;
                SET_COMM_FLAGS(ref state, v, width);
                if (dest != (ushort*)0)
                    *dest = v;
            }
            return 0;
        }

        public static unsafe uint Op_SUB_RM(ref State state, uint param)
        {
            uint ret;
            const byte width = 8;
            byte v;
            byte* dest = (byte*)0, src = (byte*)0;
            ret = Int_ParseModRM(ref state, &dest, &src, 0);
            if (ret != 0)
                return ret;
            const uint c = 0;
            uint r = *src;
            v = (byte)(*dest - r);
            ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF | Flags.FLAG_AF);
            state.Flags &= (ushort)~flag;
            state.Flags |= (ushort)((*dest < r) ? Flags.FLAG_CF : 0);
            state.Flags |= (ushort)(((*dest & 15) < (r & 15) - c) ? Flags.FLAG_AF : 0);
            uint _sub_tmp = (uint)(((*dest ^ r) & (*dest ^ v)) & (1 << (width - 1)));
            if (_sub_tmp != 0)
                state.Flags |= Flags.FLAG_OF;
            SET_COMM_FLAGS(ref state, v, width);
            if (dest != (byte*)0)
                *dest = v;
            return 0;
        }

        public static unsafe uint Op_SUB_RMX(ref State state, uint param)
        {
            uint ret;
            void* destPtr = (void*)0, srcPtr = (void*)0;
            ret = Int_ParseModRMX(ref state, (ushort**)&destPtr, (ushort**)&srcPtr, 0);
            if (ret != 0)
                return ret;
            if (state.Decoder.bOverrideOperand)
            {
                uint v;
                uint* dest = (uint*)destPtr, src = (uint*)srcPtr;
                byte width = 32;
                const uint c = 0;
                uint r = *src;
                v = (uint)(*dest - r);
                ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF | Flags.FLAG_AF);
                state.Flags &= (ushort)~flag;
                state.Flags |= (ushort)((*dest < r) ? Flags.FLAG_CF : 0);
                state.Flags |= (ushort)(((*dest & 15) < (r & 15) - c) ? Flags.FLAG_AF : 0);
                uint _sub_tmp = (uint)(((*dest ^ r) & (*dest ^ v)) & (1 << (width - 1)));
                if (_sub_tmp != 0)
                    state.Flags |= Flags.FLAG_OF;
                SET_COMM_FLAGS(ref state, v, width);
                if (dest != (uint*)0)
                    *dest = v;
            }
            else
            {
                ushort v;
                ushort* dest = (ushort*)destPtr, src = (ushort*)srcPtr;
                byte width = 16;
                const uint c = 0;
                uint r = *src;
                v = (ushort)(*dest - r);
                ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF | Flags.FLAG_AF);
                state.Flags &= (ushort)~flag;
                state.Flags |= (ushort)((*dest < r) ? Flags.FLAG_CF : 0);
                state.Flags |= (ushort)(((*dest & 15) < (r & 15) - c) ? Flags.FLAG_AF : 0);
                uint _sub_tmp = (uint)(((*dest ^ r) & (*dest ^ v)) & (1 << (width - 1)));
                if (_sub_tmp != 0)
                    state.Flags |= Flags.FLAG_OF;
                SET_COMM_FLAGS(ref state, v, width);
                if (dest != (ushort*)0)
                    *dest = v;
            }
            return 0;
        }

        public static unsafe uint Op_SUB_MR(ref State state, uint param)
        {
            uint ret;
            const byte width = 8;
            byte v;
            byte* dest = (byte*)0, src = (byte*)0;
            ret = Int_ParseModRM(ref state, &src, &dest, 1);
            if (ret != 0)
                return ret;
            const uint c = 0;
            uint r = *src;
            v = (byte)(*dest - r);
            ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF | Flags.FLAG_AF);
            state.Flags &= (ushort)~flag;
            state.Flags |= (ushort)((*dest < r) ? Flags.FLAG_CF : 0);
            state.Flags |= (ushort)(((*dest & 15) < (r & 15) - c) ? Flags.FLAG_AF : 0);
            uint _sub_tmp = (uint)(((*dest ^ r) & (*dest ^ v)) & (1 << (width - 1)));
            if (_sub_tmp != 0)
                state.Flags |= Flags.FLAG_OF;
            SET_COMM_FLAGS(ref state, v, width);
            if (dest != (byte*)0)
                *dest = v;
            return 0;
        }

        public static unsafe uint Op_SUB_MRX(ref State state, uint param)
        {
            uint ret;
            void* destPtr = (void*)0, srcPtr = (void*)0;
            ret = Int_ParseModRMX(ref state, (ushort**)&srcPtr, (ushort**)&destPtr, 1);
            if (ret != 0)
                return ret;
            if (state.Decoder.bOverrideOperand)
            {
                uint v;
                uint* dest = (uint*)destPtr, src = (uint*)srcPtr;
                byte width = 32;
                const uint c = 0;
                uint r = *src;
                v = (uint)(*dest - r);
                ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF | Flags.FLAG_AF);
                state.Flags &= (ushort)~flag;
                state.Flags |= (ushort)((*dest < r) ? Flags.FLAG_CF : 0);
                state.Flags |= (ushort)(((*dest & 15) < (r & 15) - c) ? Flags.FLAG_AF : 0);
                uint _sub_tmp = (uint)(((*dest ^ r) & (*dest ^ v)) & (1 << (width - 1)));
                if (_sub_tmp != 0)
                    state.Flags |= Flags.FLAG_OF;
                SET_COMM_FLAGS(ref state, v, width);
                if (dest != (uint*)0)
                    *dest = v;
            }
            else
            {
                ushort v;
                ushort* dest = (ushort*)destPtr, src = (ushort*)srcPtr;
                byte width = 16;
                const uint c = 0;
                uint r = *src;
                v = (ushort)(*dest - r);
                ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF | Flags.FLAG_AF);
                state.Flags &= (ushort)~flag;
                state.Flags |= (ushort)((*dest < r) ? Flags.FLAG_CF : 0);
                state.Flags |= (ushort)(((*dest & 15) < (r & 15) - c) ? Flags.FLAG_AF : 0);
                uint _sub_tmp = (uint)(((*dest ^ r) & (*dest ^ v)) & (1 << (width - 1)));
                if (_sub_tmp != 0)
                    state.Flags |= Flags.FLAG_OF;
                SET_COMM_FLAGS(ref state, v, width);
                if (dest != (ushort*)0)
                    *dest = v;
            }
            return 0;
        }

        public static unsafe uint Op_SUB_AI(ref State state, uint param)
        {
            const byte width = 8;
            byte srcData, v;
            byte* dest = (byte*)state.AX.Address, src = &srcData;
            uint ret = READ_INSTR8(ref state, ref srcData);
            if (ret != 0)
                return ret;
            const uint c = 0;
            uint r = *src;
            v = (byte)(*dest - r);
            ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF | Flags.FLAG_AF);
            state.Flags &= (ushort)~flag;
            state.Flags |= (ushort)((*dest < r) ? Flags.FLAG_CF : 0);
            state.Flags |= (ushort)(((*dest & 15) < (r & 15) - c) ? Flags.FLAG_AF : 0);
            uint _sub_tmp = (uint)(((*dest ^ r) & (*dest ^ v)) & (1 << (width - 1)));
            if (_sub_tmp != 0)
                state.Flags |= Flags.FLAG_OF;
            SET_COMM_FLAGS(ref state, v, width);
            if (dest != (byte*)0)
                *dest = v;
            return 0;
        }

        public static unsafe uint Op_SUB_AIX(ref State state, uint param)
        {
            if (state.Decoder.bOverrideOperand)
            {
                uint srcData, v;
                uint* dest = (uint*)state.AX.Address, src = &srcData;
                byte width = 32;
                uint ret = READ_INSTR32(ref state, ref srcData);
                if (ret != 0)
                    return ret;
                const uint c = 0;
                uint r = *src;
                v = (uint)(*dest - r);
                ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF | Flags.FLAG_AF);
                state.Flags &= (ushort)~flag;
                state.Flags |= (ushort)((*dest < r) ? Flags.FLAG_CF : 0);
                state.Flags |= (ushort)(((*dest & 15) < (r & 15) - c) ? Flags.FLAG_AF : 0);
                uint _sub_tmp = (uint)(((*dest ^ r) & (*dest ^ v)) & (1 << (width - 1)));
                if (_sub_tmp != 0)
                    state.Flags |= Flags.FLAG_OF;
                SET_COMM_FLAGS(ref state, v, width);
                if (dest != (uint*)0)
                    *dest = v;
            }
            else
            {
                ushort srcData, v;
                ushort* dest = (ushort*)state.AX.Address, src = &srcData;
                byte width = 16;
                uint ret = READ_INSTR16(ref state, ref srcData);
                if (ret != 0)
                    return ret;
                const uint c = 0;
                uint r = *src;
                v = (ushort)(*dest - r);
                ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF | Flags.FLAG_AF);
                state.Flags &= (ushort)~flag;
                state.Flags |= (ushort)((*dest < r) ? Flags.FLAG_CF : 0);
                state.Flags |= (ushort)(((*dest & 15) < (r & 15) - c) ? Flags.FLAG_AF : 0);
                uint _sub_tmp = (uint)(((*dest ^ r) & (*dest ^ v)) & (1 << (width - 1)));
                if (_sub_tmp != 0)
                    state.Flags |= Flags.FLAG_OF;
                SET_COMM_FLAGS(ref state, v, width);
                if (dest != (ushort*)0)
                    *dest = v;
            }
            return 0;
        }

        public static unsafe uint Op_XOR_RM(ref State state, uint param)
        {
            uint ret;
            const byte width = 8;
            byte v;
            byte* dest = (byte*)0, src = (byte*)0;
            ret = Int_ParseModRM(ref state, &dest, &src, 0);
            if (ret != 0)
                return ret;
            v = (byte)(*dest ^ *src);
            ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF);
            state.Flags &= (ushort)~flag;
            SET_COMM_FLAGS(ref state, v, width);
            if (dest != (byte*)0)
                *dest = v;
            return 0;
        }

        public static unsafe uint Op_XOR_RMX(ref State state, uint param)
        {
            uint ret;
            void* destPtr = (void*)0, srcPtr = (void*)0;
            ret = Int_ParseModRMX(ref state, (ushort**)&destPtr, (ushort**)&srcPtr, 0);
            if (ret != 0)
                return ret;
            if (state.Decoder.bOverrideOperand)
            {
                uint v;
                uint* dest = (uint*)destPtr, src = (uint*)srcPtr;
                byte width = 32;
                v = (uint)(*dest ^ *src);
                ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF);
                state.Flags &= (ushort)~flag;
                SET_COMM_FLAGS(ref state, v, width);
                if (dest != (uint*)0)
                    *dest = v;
            }
            else
            {
                ushort v;
                ushort* dest = (ushort*)destPtr, src = (ushort*)srcPtr;
                byte width = 16;
                v = (ushort)(*dest ^ *src);
                ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF);
                state.Flags &= (ushort)~flag;
                SET_COMM_FLAGS(ref state, v, width);
                if (dest != (ushort*)0)
                    *dest = v;
            }
            return 0;
        }

        public static unsafe uint Op_XOR_MR(ref State state, uint param)
        {
            uint ret;
            const byte width = 8;
            byte v;
            byte* dest = (byte*)0, src = (byte*)0;
            ret = Int_ParseModRM(ref state, &src, &dest, 1);
            if (ret != 0)
                return ret;
            v = (byte)(*dest ^ *src);
            ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF);
            state.Flags &= (ushort)~flag;
            SET_COMM_FLAGS(ref state, v, width);
            if (dest != (byte*)0)
                *dest = v;
            return 0;
        }

        public static unsafe uint Op_XOR_MRX(ref State state, uint param)
        {
            uint ret;
            void* destPtr = (void*)0, srcPtr = (void*)0;
            ret = Int_ParseModRMX(ref state, (ushort**)&srcPtr, (ushort**)&destPtr, 1);
            if (ret != 0)
                return ret;
            if (state.Decoder.bOverrideOperand)
            {
                uint v;
                uint* dest = (uint*)destPtr, src = (uint*)srcPtr;
                byte width = 32;
                v = (uint)(*dest ^ *src);
                ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF);
                state.Flags &= (ushort)~flag;
                SET_COMM_FLAGS(ref state, v, width);
                if (dest != (uint*)0)
                    *dest = v;
            }
            else
            {
                ushort v;
                ushort* dest = (ushort*)destPtr, src = (ushort*)srcPtr;
                byte width = 16;
                v = (ushort)(*dest ^ *src);
                ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF);
                state.Flags &= (ushort)~flag;
                SET_COMM_FLAGS(ref state, v, width);
                if (dest != (ushort*)0)
                    *dest = v;
            }
            return 0;
        }

        public static unsafe uint Op_XOR_AI(ref State state, uint param)
        {
            const byte width = 8;
            byte srcData, v;
            byte* dest = (byte*)state.AX.Address, src = &srcData;
            uint ret = READ_INSTR8(ref state, ref srcData);
            if (ret != 0)
                return ret;
            v = (byte)(*dest ^ *src);
            ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF);
            state.Flags &= (ushort)~flag;
            SET_COMM_FLAGS(ref state, v, width);
            if (dest != (byte*)0)
                *dest = v;
            return 0;
        }

        public static unsafe uint Op_XOR_AIX(ref State state, uint param)
        {
            if (state.Decoder.bOverrideOperand)
            {
                uint srcData, v;
                uint* dest = (uint*)state.AX.Address, src = &srcData;
                byte width = 32;
                uint ret = READ_INSTR32(ref state, ref srcData);
                if (ret != 0)
                    return ret;
                v = (uint)(*dest ^ *src);
                ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF);
                state.Flags &= (ushort)~flag;
                SET_COMM_FLAGS(ref state, v, width);
                if (dest != (uint*)0)
                    *dest = v;
            }
            else
            {
                ushort srcData, v;
                ushort* dest = (ushort*)state.AX.Address, src = &srcData;
                byte width = 16;
                uint ret = READ_INSTR16(ref state, ref srcData);
                if (ret != 0)
                    return ret;
                v = (ushort)(*dest ^ *src);
                ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF);
                state.Flags &= (ushort)~flag;
                SET_COMM_FLAGS(ref state, v, width);
                if (dest != (ushort*)0)
                    *dest = v;
            }
            return 0;
        }

        public static unsafe uint Op_CMP_RM(ref State state, uint param)
        {
            uint ret;
            const byte width = 8;
            byte v;
            byte* dest = (byte*)0, src = (byte*)0;
            ret = Int_ParseModRM(ref state, &dest, &src, 0);
            if (ret != 0)
                return ret;
            const uint c = 0;
            byte r = *src;
            v = (byte)(*dest - r);
            ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF | Flags.FLAG_AF);
            state.Flags &= (ushort)~flag;
            state.Flags |= (ushort)((*dest < r) ? Flags.FLAG_CF : 0);
            state.Flags |= (ushort)(((*dest & 15) < (r & 15) - c) ? Flags.FLAG_AF : 0);
            uint _sub_tmp = (uint)(((*dest ^ r) & (*dest ^ v)) & (1 << (width - 1)));
            if (_sub_tmp != 0)
                state.Flags |= Flags.FLAG_OF;
            dest = null;
            SET_COMM_FLAGS(ref state, v, width);
            if (dest != (byte*)0)
                *dest = v;
            return 0;
        }

        public static unsafe uint Op_CMP_RMX(ref State state, uint param)
        {
            uint ret;
            void* destPtr = (void*)0, srcPtr = (void*)0;
            ret = Int_ParseModRMX(ref state, (ushort**)&destPtr, (ushort**)&srcPtr, 0);
            if (ret != 0)
                return ret;
            if (state.Decoder.bOverrideOperand)
            {
                uint v;
                uint* dest = (uint*)destPtr, src = (uint*)srcPtr;
                byte width = 32;
                const uint c = 0;
                uint r = *src;
                v = (uint)(*dest - r);
                ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF | Flags.FLAG_AF);
                state.Flags &= (ushort)~flag;
                state.Flags |= (ushort)((*dest < r) ? Flags.FLAG_CF : 0);
                state.Flags |= (ushort)(((*dest & 15) < (r & 15) - c) ? Flags.FLAG_AF : 0);
                uint _sub_tmp = (uint)(((*dest ^ r) & (*dest ^ v)) & (1 << (width - 1)));
                if (_sub_tmp != 0)
                    state.Flags |= Flags.FLAG_OF;
                dest = null;
                SET_COMM_FLAGS(ref state, v, width);
                if (dest != (uint*)0)
                    *dest = v;
            }
            else
            {
                ushort v;
                ushort* dest = (ushort*)destPtr, src = (ushort*)srcPtr;
                byte width = 16;
                const uint c = 0;
                ushort r = *src;
                v = (ushort)(*dest - r);
                ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF | Flags.FLAG_AF);
                state.Flags &= (ushort)~flag;
                state.Flags |= (ushort)((*dest < r) ? Flags.FLAG_CF : 0);
                state.Flags |= (ushort)(((*dest & 15) < (r & 15) - c) ? Flags.FLAG_AF : 0);
                uint _sub_tmp = (uint)(((*dest ^ r) & (*dest ^ v)) & (1 << (width - 1)));
                if (_sub_tmp != 0)
                    state.Flags |= Flags.FLAG_OF;
                dest = null;
                SET_COMM_FLAGS(ref state, v, width);
                if (dest != (ushort*)0)
                    *dest = v;
            }
            return 0;
        }

        public static unsafe uint Op_CMP_MR(ref State state, uint param)
        {
            uint ret;
            const byte width = 8;
            byte v;
            byte* dest = (byte*)0, src = (byte*)0;
            ret = Int_ParseModRM(ref state, &src, &dest, 1);
            if (ret != 0)
                return ret;
            const uint c = 0;
            byte r = *src;
            v = (byte)(*dest - r);
            ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF | Flags.FLAG_AF);
            state.Flags &= (ushort)~flag;
            state.Flags |= (ushort)((*dest < r) ? Flags.FLAG_CF : 0);
            state.Flags |= (ushort)(((*dest & 15) < (r & 15) - c) ? Flags.FLAG_AF : 0);
            uint _sub_tmp = (uint)(((*dest ^ r) & (*dest ^ v)) & (1 << (width - 1)));
            if (_sub_tmp != 0)
                state.Flags |= Flags.FLAG_OF;
            dest = null;
            SET_COMM_FLAGS(ref state, v, width);
            if (dest != (byte*)0)
                *dest = v;
            return 0;
        }

        public static unsafe uint Op_CMP_MRX(ref State state, uint param)
        {
            uint ret;
            void* destPtr = (void*)0, srcPtr = (void*)0;
            ret = Int_ParseModRMX(ref state, (ushort**)&srcPtr, (ushort**)&destPtr, 1);
            if (ret != 0)
                return ret;
            if (state.Decoder.bOverrideOperand)
            {
                uint v;
                uint* dest = (uint*)destPtr, src = (uint*)srcPtr;
                byte width = 32;
                const uint c = 0;
                uint r = *src;
                v = (uint)(*dest - r);
                ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF | Flags.FLAG_AF);
                state.Flags &= (ushort)~flag;
                state.Flags |= (ushort)((*dest < r) ? Flags.FLAG_CF : 0);
                state.Flags |= (ushort)(((*dest & 15) < (r & 15) - c) ? Flags.FLAG_AF : 0);
                uint _sub_tmp = (uint)(((*dest ^ r) & (*dest ^ v)) & (1 << (width - 1)));
                if (_sub_tmp != 0)
                    state.Flags |= Flags.FLAG_OF;
                dest = null;
                SET_COMM_FLAGS(ref state, v, width);
                if (dest != (uint*)0)
                    *dest = v;
            }
            else
            {
                ushort v;
                ushort* dest = (ushort*)destPtr, src = (ushort*)srcPtr;
                byte width = 16;
                const uint c = 0;
                ushort r = *src;
                v = (ushort)(*dest - r);
                ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF | Flags.FLAG_AF);
                state.Flags &= (ushort)~flag;
                state.Flags |= (ushort)((*dest < r) ? Flags.FLAG_CF : 0);
                state.Flags |= (ushort)(((*dest & 15) < (r & 15) - c) ? Flags.FLAG_AF : 0);
                uint _sub_tmp = (uint)(((*dest ^ r) & (*dest ^ v)) & (1 << (width - 1)));
                if (_sub_tmp != 0)
                    state.Flags |= Flags.FLAG_OF;
                dest = null;
                SET_COMM_FLAGS(ref state, v, width);
                if (dest != (ushort*)0)
                    *dest = v;
            }
            return 0;
        }

        public static unsafe uint Op_CMP_AI(ref State state, uint param)
        {
            const byte width = 8;
            byte srcData, v;
            byte* dest = (byte*)state.AX.Address, src = &srcData;
            uint ret = READ_INSTR8(ref state, ref srcData);
            if (ret != 0)
                return ret;
            const uint c = 0;
            byte r = *src;
            v = (byte)(*dest - r);
            ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF | Flags.FLAG_AF);
            state.Flags &= (ushort)~flag;
            state.Flags |= (ushort)((*dest < r) ? Flags.FLAG_CF : 0);
            state.Flags |= (ushort)(((*dest & 15) < (r & 15) - c) ? Flags.FLAG_AF : 0);
            uint _sub_tmp = (uint)(((*dest ^ r) & (*dest ^ v)) & (1 << (width - 1)));
            if (_sub_tmp != 0)
                state.Flags |= Flags.FLAG_OF;
            dest = null;
            SET_COMM_FLAGS(ref state, v, width);
            if (dest != (byte*)0)
                *dest = v;
            return 0;
        }

        public static unsafe uint Op_CMP_AIX(ref State state, uint param)
        {
            if (state.Decoder.bOverrideOperand)
            {
                uint srcData, v;
                uint* dest = (uint*)state.AX.Address, src = &srcData;
                byte width = 32;
                uint ret = READ_INSTR32(ref state, ref srcData);
                if (ret != 0)
                    return ret;
                const uint c = 0;
                uint r = *src;
                v = (uint)(*dest - r);
                ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF | Flags.FLAG_AF);
                state.Flags &= (ushort)~flag;
                state.Flags |= (ushort)((*dest < r) ? Flags.FLAG_CF : 0);
                state.Flags |= (ushort)(((*dest & 15) < (r & 15) - c) ? Flags.FLAG_AF : 0);
                uint _sub_tmp = (uint)(((*dest ^ r) & (*dest ^ v)) & (1 << (width - 1)));
                if (_sub_tmp != 0)
                    state.Flags |= Flags.FLAG_OF;
                dest = null;
                SET_COMM_FLAGS(ref state, v, width);
                if (dest != (uint*)0)
                    *dest = v;
            }
            else
            {
                ushort srcData, v;
                ushort* dest = (ushort*)state.AX.Address, src = &srcData;
                byte width = 16;
                uint ret = READ_INSTR16(ref state, ref srcData);
                if (ret != 0)
                    return ret;
                const uint c = 0;
                ushort r = *src;
                v = (ushort)(*dest - r);
                ushort flag = (Flags.FLAG_PF | Flags.FLAG_ZF | Flags.FLAG_SF | Flags.FLAG_OF | Flags.FLAG_CF | Flags.FLAG_AF);
                state.Flags &= (ushort)~flag;
                state.Flags |= (ushort)((*dest < r) ? Flags.FLAG_CF : 0);
                state.Flags |= (ushort)(((*dest & 15) < (r & 15) - c) ? Flags.FLAG_AF : 0);
                uint _sub_tmp = (uint)(((*dest ^ r) & (*dest ^ v)) & (1 << (width - 1)));
                if (_sub_tmp != 0)
                    state.Flags |= Flags.FLAG_OF;
                dest = null;
                SET_COMM_FLAGS(ref state, v, width);
                if (dest != (ushort*)0)
                    *dest = v;
            }
            return 0;
        }

        #endregion

        #region ALU Part 2

        public static unsafe uint Op_TEST_MR(ref State state, uint param)
        {
            uint ret;
            const byte width = 8;
            byte v;
            byte* dest = (byte*)0, src = (byte*)0;
            ret = Int_ParseModRM(ref state, &src, &dest, 1);
            if (ret != 0)
                return ret;
            v = (byte)((*dest) & (*src));
            ushort flag = (Flags.FLAG_OF | Flags.FLAG_CF);
            state.Flags &= (ushort)~flag;
            dest = null;
            SET_COMM_FLAGS(ref state, v, width);
            if (dest != (byte*)0)
                *dest = v;
            return 0;
        }

        public static unsafe uint Op_TEST_MRX(ref State state, uint param)
        {
            uint ret;
            void* destPtr = (void*)0, srcPtr = (void*)0;
            ret = Int_ParseModRMX(ref state, (ushort**)&srcPtr, (ushort**)&destPtr, 1);
            if (ret != 0)
                return ret;
            if (state.Decoder.bOverrideOperand)
            {
                uint v;
                uint* dest = (uint*)destPtr, src = (uint*)srcPtr;
                byte width = 32;
                v = (byte)((*dest) & (*src));
                ushort flag = (Flags.FLAG_OF | Flags.FLAG_CF);
                state.Flags &= (ushort)~flag;
                dest = null;
                SET_COMM_FLAGS(ref state, v, width);
                if (dest != (uint*)0)
                    *dest = v;
            }
            else
            {
                ushort v;
                ushort* dest = (ushort*)destPtr, src = (ushort*)srcPtr;
                byte width = 16;
                v = (byte)((*dest) & (*src));
                ushort flag = (Flags.FLAG_OF | Flags.FLAG_CF);
                state.Flags &= (ushort)~flag;
                dest = null;
                SET_COMM_FLAGS(ref state, v, width);
                if (dest != (ushort*)0)
                    *dest = v;
            }
            return 0;
        }

        public static unsafe uint Op_TEST_AI(ref State state, uint param)
        {
            const byte width = 8;
            byte srcData, v;
            byte* dest = (byte*)state.AX.Address, src = &srcData;
            uint ret = READ_INSTR8(ref state, ref srcData);
            if (ret != 0)
                return ret;
            v = (byte)((*dest) & (*src));
            ushort flag = (Flags.FLAG_OF | Flags.FLAG_CF);
            state.Flags &= (ushort)~flag;
            dest = null;
            SET_COMM_FLAGS(ref state, v, width);
            if (dest != (byte*)0)
                *dest = v;
            return 0;
        }

        public static unsafe uint Op_TEST_AIX(ref State state, uint param)
        {
            if (state.Decoder.bOverrideOperand)
            {
                uint srcData, v;
                uint* dest = (uint*)state.AX.Address, src = &srcData;
                byte width = 32;
                uint ret = READ_INSTR32(ref state, ref srcData);
                if (ret != 0)
                    return ret;
                v = (byte)((*dest) & (*src));
                ushort flag = (Flags.FLAG_OF | Flags.FLAG_CF);
                state.Flags &= (ushort)~flag;
                dest = null;
                SET_COMM_FLAGS(ref state, v, width);
                if (dest != (uint*)0)
                    *dest = v;
            }
            else
            {
                ushort srcData, v;
                ushort* dest = (ushort*)state.AX.Address, src = &srcData;
                byte width = 16;
                uint ret = READ_INSTR16(ref state, ref srcData);
                if (ret != 0)
                    return ret;
                v = (byte)((*dest) & (*src));
                ushort flag = (Flags.FLAG_OF | Flags.FLAG_CF);
                state.Flags &= (ushort)~flag;
                dest = null;
                SET_COMM_FLAGS(ref state, v, width);
                if (dest != (ushort*)0)
                    *dest = v;
            }
            return 0;
        }


        #endregion

        #region ALU Part 3



        #endregion

    }
}