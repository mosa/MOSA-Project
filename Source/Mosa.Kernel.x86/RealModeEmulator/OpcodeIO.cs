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
        /// Read unsigned byte from port
        /// </summary>
        /// <param name="port">Port address</param>
        /// <returns>Value as unsigned byte</returns>
        public static byte inb(ushort port)
        {
            return Native.In8(port);
        }

        /// <summary>
        /// Read unsigned short from port
        /// </summary>
        /// <param name="port">Port address</param>
        /// <returns>Value as unsigned short</returns>
        public static ushort inw(ushort port)
        {
            return Native.In16(port);
        }

        /// <summary>
        /// Read unsigned integer from port
        /// </summary>
        /// <param name="port">Port address</param>
        /// <returns>Value as unsigned integer</returns>
        public static uint inl(ushort port)
        {
            return Native.In32(port);
        }

        /// <summary>
        /// Write unsigned byte to port
        /// </summary>
        /// <param name="port">Port address</param>
        /// <param name="value">Value as unsigned byte</param>
        public static void outb(ushort port, byte value)
        {
            Native.Out8(port, value);
        }

        /// <summary>
        /// Write unsigned short to port
        /// </summary>
        /// <param name="port">Port address</param>
        /// <param name="value">Value as unsigned short</param>
        public static void outw(ushort port, ushort value)
        {
            Native.Out16(port, value);
        }

        /// <summary>
        /// Write unsigned integer to port
        /// </summary>
        /// <param name="port">Port address</param>
        /// <param name="value">Value as unsigned integer</param>
        public static void outl(ushort port, uint value)
        {
            Native.Out32(port, value);
        }

        public static uint Op_IN_ADx(ref State state, uint param)
        {
            state.AX.L = inb((state.DX.W));
            return ErrorCodes.ERR_OK;
        }

        public static uint Op_IN_ADxX(ref State state, uint param)
        {
            if (state.Decoder.bOverrideOperand)
            {
                state.AX.D = inl(state.DX.W);
            }
            else
            {
                state.AX.W = inw(state.DX.W);
            }

            return ErrorCodes.ERR_OK;
        }

        public static unsafe uint Op_IN_AI(ref State state, uint param)
        {
            byte port = 0;
            uint err = Int_Read8(ref state, state.CS, (ushort)(state.IP + state.Decoder.IPOffset), &port);
            if (err != ErrorCodes.ERR_OK) return err;
            state.Decoder.IPOffset++;
            state.AX.L = inb(port);

            return ErrorCodes.ERR_OK;
        }

        public static unsafe uint Op_IN_AIX(ref State state, uint param)
        {
            byte port = 0;
            uint err = Int_Read8(ref state, state.CS, (ushort)(state.IP + state.Decoder.IPOffset), &port);
            if (err != ErrorCodes.ERR_OK) return err;
            state.Decoder.IPOffset++;

            if (state.Decoder.bOverrideOperand)
            {
                state.AX.D = inl(port);
            }
            else
            {
                state.AX.W = inw(port);
            }

            return ErrorCodes.ERR_OK;
        }

        public static uint Op_OUT_DxA(ref State state, uint param)
        {
            outb(state.DX.W, state.AX.L);
            return ErrorCodes.ERR_OK;
        }
        public static uint Op_OUT_DxAX(ref State state, uint param)
        {
            if (state.Decoder.bOverrideOperand)
            {
                outl(state.DX.W, state.AX.D);
            }
            else
            {
                outw(state.DX.W, state.AX.W);
            }

            return ErrorCodes.ERR_OK;
        }

        public static unsafe uint Op_OUT_AI(ref State state, uint param)
        {
            byte port = 0;
            uint err = Int_Read8(ref state, state.CS, (ushort)(state.IP + state.Decoder.IPOffset), &port);
            if (err != ErrorCodes.ERR_OK) return err;
            state.Decoder.IPOffset++;
            outb(port, state.AX.L);
            return ErrorCodes.ERR_OK;
        }

        public static unsafe uint Op_OUT_AIX(ref State state, uint param)
        {
            byte port = 0;
            uint err = Int_Read8(ref state, state.CS, (ushort)(state.IP + state.Decoder.IPOffset), &port);
            if (err != ErrorCodes.ERR_OK) return err;
            state.Decoder.IPOffset++;

            if (state.Decoder.bOverrideOperand)
            {
                outl(port, state.AX.D);
            }
            else
            {
                outw(port, state.AX.W);
            }

            return ErrorCodes.ERR_OK;
        }
    }
}