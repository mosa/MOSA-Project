/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Linker;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Vm;
using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platforms.x86
{
	/// <summary>
	/// An x86 machine code emitter.
	/// </summary>
	public static class X86
    {
        #region Add
        /// <summary>
		/// Adds the specified dest.
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Add(Operand dest, Operand src)
		{
			if ((dest is RegisterOperand) && (src is ConstantOperand)) return Add(dest as RegisterOperand, src as ConstantOperand);
			if ((dest is RegisterOperand) && (src is RegisterOperand)) return Add(dest as RegisterOperand, src as RegisterOperand);
			if ((dest is RegisterOperand) && (src is MemoryOperand)) return Add(dest as RegisterOperand, src as MemoryOperand);
			if ((dest is MemoryOperand) && (src is RegisterOperand)) return Add(dest as MemoryOperand, src as RegisterOperand);

			throw new ArgumentException(@"No opcode for operand type.");
		}

		/// <summary>
		/// Adds the specified dest.
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Add(RegisterOperand dest, ConstantOperand src)
		{
			return new OpCode(new byte[] { 0x81 }, 0);
		}

		/// <summary>
		/// Adds the specified dest.
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Add(RegisterOperand dest, RegisterOperand src)
		{
			return new OpCode(new byte[] { 0x03 }, null);
		}

		/// <summary>
		/// Adds the specified dest.
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Add(RegisterOperand dest, MemoryOperand src)
		{
			if ((src.Type.Type == CilElementType.I1) || (src.Type.Type == CilElementType.U1))
				return new OpCode(new byte[] { 0x02 }, null);

			return new OpCode(new byte[] { 0x03 }, null);
		}

		/// <summary>
		/// Adds the specified dest.
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Add(MemoryOperand dest, RegisterOperand src)
		{
			if ((dest.Type.Type == CilElementType.I1) || (dest.Type.Type == CilElementType.U1))
				return new OpCode(new byte[] { 0x00 }, null);

			return new OpCode(new byte[] { 0x01 }, null);
		}
        #endregion

        #region And
        /// <summary>
        /// Ands the specified dest.
        /// </summary>
        /// <param name="dest">The dest.</param>
        /// <param name="src">The SRC.</param>
        /// <returns></returns>
        public static OpCode And(Operand dest, Operand src)
        {
            if ((dest is RegisterOperand) && (src is ConstantOperand)) return And(dest as RegisterOperand, src as ConstantOperand);
            if ((dest is MemoryOperand) && (src is ConstantOperand)) return And(dest as MemoryOperand, src as ConstantOperand);
            if ((dest is RegisterOperand) && (src is MemoryOperand)) return And(dest as RegisterOperand, src as MemoryOperand);
            if ((dest is RegisterOperand) && (src is RegisterOperand)) return And(dest as RegisterOperand, src as RegisterOperand);
            if ((dest is MemoryOperand) && (src is RegisterOperand)) return And(dest as MemoryOperand, src as RegisterOperand);

            throw new ArgumentException(@"No opcode for operand type.");
        }

        /// <summary>
        /// Ands the specified dest.
        /// </summary>
        /// <param name="dest">The dest.</param>
        /// <param name="src">The SRC.</param>
        /// <returns></returns>
        public static OpCode And(RegisterOperand dest, ConstantOperand src)
        {
            return new OpCode(new byte[] { 0x81 }, 4);
        }

        /// <summary>
        /// Ands the specified dest.
        /// </summary>
        /// <param name="dest">The dest.</param>
        /// <param name="src">The SRC.</param>
        /// <returns></returns>
        public static OpCode And(MemoryOperand dest, ConstantOperand src)
        {
            return new OpCode(new byte[] { 0x81 }, 4);
        }

        /// <summary>
        /// Ands the specified dest.
        /// </summary>
        /// <param name="dest">The dest.</param>
        /// <param name="src">The SRC.</param>
        /// <returns></returns>
        public static OpCode And(RegisterOperand dest, MemoryOperand src)
        {
            return new OpCode(new byte[] { 0x23 }, null);
        }

        /// <summary>
        /// Ands the specified dest.
        /// </summary>
        /// <param name="dest">The dest.</param>
        /// <param name="src">The SRC.</param>
        /// <returns></returns>
        public static OpCode And(RegisterOperand dest, RegisterOperand src)
        {
            return new OpCode(new byte[] { 0x23 }, null);
        }

        /// <summary>
        /// </summary>
        /// <param name="dest">The dest.</param>
        /// <param name="src">The SRC.</param>
        /// <returns></returns>
        public static OpCode And(MemoryOperand dest, RegisterOperand src)
        {
            return new OpCode(new byte[] { 0x21 }, null);
        }
        #endregion

        #region Div
        /// <summary>
        /// Returns the matching OpCode for DIV
        /// </summary>
        /// <param name="dest">Destination operand</param>
        /// <param name="src">Source Operand</param>
        /// <returns>The matching OpCode</returns>
        public static OpCode Div(Operand dest, Operand src)
        {
            if (src is RegisterOperand) return Div(dest, src as RegisterOperand);
            if (src is MemoryOperand)   return Div(dest, src as MemoryOperand);

            throw new ArgumentException(@"No opcode for operand type.");
        }

        /// <summary>
        /// Returns the matching OpCode for DIV
        /// </summary>
        /// <param name="dest">Destination operand</param>
        /// <param name="src">Source Operand</param>
        /// <returns>The matching OpCode</returns>
        public static OpCode Div(Operand dest, RegisterOperand src)
        {
            if ((src.Type.Type == CilElementType.U1) || (src.Type.Type == CilElementType.I1))
                return new OpCode(new byte[] { 0xF6 }, 6);
            else if ((src.Type.Type == CilElementType.U2) || (src.Type.Type == CilElementType.I2))
                return new OpCode(new byte[] { 0x66, 0xF7 }, 6);
            
            return new OpCode(new byte[] { 0xF7 }, 6);
        }

        /// <summary>
        /// Returns the matching OpCode for DIV
        /// </summary>
        /// <param name="dest">Destination operand</param>
        /// <param name="src">Source Operand</param>
        /// <returns>The matching OpCode</returns>
        public static OpCode Div(Operand dest, MemoryOperand src)
        {
            if ((src.Type.Type == CilElementType.U1) || (src.Type.Type == CilElementType.I1))
                return new OpCode(new byte[] { 0xF6 }, 6);
            else if ((src.Type.Type == CilElementType.U2) || (src.Type.Type == CilElementType.I2))
                return new OpCode(new byte[] { 0x66, 0xF7 }, 6);

            return new OpCode(new byte[] { 0xF7 }, 6);
        }
        #endregion

        #region Move
        /// <summary>
        /// Moves the specified dest.
        /// </summary>
        /// <param name="dest">The dest.</param>
        /// <param name="src">The SRC.</param>
        /// <returns></returns>
        public static OpCode Move(Operand dest, Operand src)
        {
            if ((dest is RegisterOperand) && (src is ConstantOperand)) return Move(dest as RegisterOperand, src as ConstantOperand);
            if ((dest is MemoryOperand) && (src is ConstantOperand)) return Move(dest as MemoryOperand, src as ConstantOperand);
            if ((dest is RegisterOperand) && (src is RegisterOperand)) return Move(dest as RegisterOperand, src as RegisterOperand);
            if ((dest is RegisterOperand) && (src is MemoryOperand)) return Move(dest as RegisterOperand, src as MemoryOperand);
            if ((dest is MemoryOperand) && (src is RegisterOperand)) return Move(dest as MemoryOperand, src as RegisterOperand);

            throw new ArgumentException(@"No opcode for operand type.");
        }

        /// <summary>
        /// Moves the specified dest.
        /// </summary>
        /// <param name="dest">The dest.</param>
        /// <param name="src">The SRC.</param>
        /// <returns></returns>
        public static OpCode Move(RegisterOperand dest, ConstantOperand src)
        {
            return new OpCode(new byte[] { 0xC7 }, 0);
        }

        /// <summary>
        /// Moves the specified dest.
        /// </summary>
        /// <param name="dest">The dest.</param>
        /// <param name="src">The SRC.</param>
        /// <returns></returns>
        public static OpCode Move(MemoryOperand dest, ConstantOperand src)
        {
            return new OpCode(new byte[] { 0xC7 }, 0);
        }

        /// <summary>
        /// Moves the specified dest.
        /// </summary>
        /// <param name="dest">The dest.</param>
        /// <param name="src">The SRC.</param>
        /// <returns></returns>
        public static OpCode Move(RegisterOperand dest, RegisterOperand src)
        {
            return new OpCode(new byte[] { 0x8B }, null);
        }

        /// <summary>
        /// Moves the specified dest.
        /// </summary>
        /// <param name="dest">The dest.</param>
        /// <param name="src">The SRC.</param>
        /// <returns></returns>
        public static OpCode Move(RegisterOperand dest, MemoryOperand src)
        {
            if (src.Type.Type == CilElementType.U1)
                return new OpCode(new byte[] { 0x0F, 0xB6 }, null);

            if (src.Type.Type == CilElementType.I1)
                return new OpCode(new byte[] { 0x0F, 0xBE }, null);

            if (src.Type.Type == CilElementType.U2)
                return new OpCode(new byte[] { 0x0F, 0xB7 }, null);

            if (src.Type.Type == CilElementType.I2)
                return new OpCode(new byte[] { 0x0F, 0xBF }, null);

            return new OpCode(new byte[] { 0x8B }, null);
        }

        /// <summary>
        /// Moves the specified dest.
        /// </summary>
        /// <param name="dest">The dest.</param>
        /// <param name="src">The SRC.</param>
        /// <returns></returns>
        public static OpCode Move(MemoryOperand dest, RegisterOperand src)
        {
            if ((dest.Type.Type == CilElementType.I1) || (dest.Type.Type == CilElementType.U1))
                return new OpCode(new byte[] { 0x88 }, null);

            return new OpCode(new byte[] { 0x89 }, null);
        }
        #endregion

        #region Neg
        /// <summary>
        /// Negates the specified dest.
        /// </summary>
        /// <param name="dest">The dest.</param>
        /// <returns></returns>
        public static OpCode Neg(Operand dest)
        {
            if (dest is RegisterOperand) return Neg(dest as RegisterOperand);
            if (dest is MemoryOperand) return Neg(dest as MemoryOperand);

            throw new ArgumentException(@"No opcode for operand type.");
        }

        /// <summary>
        /// Negates the specified dest.
        /// </summary>
        /// <param name="dest">The dest.</param>
        /// <returns></returns>
        public static OpCode Neg(RegisterOperand dest)
        {
            return new OpCode(new byte[] { 0xF7 }, 3);
        }

        /// <summary>
        /// Negates the specified dest.
        /// </summary>
        /// <param name="dest">The dest.</param>
        /// <returns></returns>
        public static OpCode Neg(MemoryOperand dest)
        {
            if ((dest.Type.Type == CilElementType.I1) || (dest.Type.Type == CilElementType.U1))
                return new OpCode(new byte[] { 0xF6 }, 3);

            return new OpCode(new byte[] { 0xF7 }, 3);
        }
        #endregion

        #region Or
        /// <summary>
        /// Ors the specified dest.
        /// </summary>
        /// <param name="dest">The dest.</param>
        /// <param name="src">The SRC.</param>
        /// <returns></returns>
        public static OpCode Or(Operand dest, Operand src)
        {
            if ((dest is RegisterOperand) && (src is ConstantOperand)) return Or(dest as RegisterOperand, src as ConstantOperand);
            if ((dest is RegisterOperand) && (src is MemoryOperand)) return Or(dest as RegisterOperand, src as MemoryOperand);
            if ((dest is RegisterOperand) && (src is RegisterOperand)) return Or(dest as RegisterOperand, src as RegisterOperand);
            if ((dest is MemoryOperand) && (src is RegisterOperand)) return Or(dest as MemoryOperand, src as RegisterOperand);

            throw new ArgumentException(@"No opcode for operand type.");
        }

        /// <summary>
        /// Ors the specified dest.
        /// </summary>
        /// <param name="dest">The dest.</param>
        /// <param name="src">The SRC.</param>
        /// <returns></returns>
        public static OpCode Or(RegisterOperand dest, ConstantOperand src)
        {
            return new OpCode(new byte[] { 0x81 }, 1);
        }

        /// <summary>
        /// Ors the specified dest.
        /// </summary>
        /// <param name="dest">The dest.</param>
        /// <param name="src">The SRC.</param>
        /// <returns></returns>
        public static OpCode Or(RegisterOperand dest, MemoryOperand src)
        {
            return new OpCode(new byte[] { 0x0B }, null);
        }

        /// <summary>
        /// Ors the specified dest.
        /// </summary>
        /// <param name="dest">The dest.</param>
        /// <param name="src">The SRC.</param>
        /// <returns></returns>
        public static OpCode Or(RegisterOperand dest, RegisterOperand src)
        {
            return new OpCode(new byte[] { 0x0B }, null);
        }

        /// <summary>
        /// Ors the specified dest.
        /// </summary>
        /// <param name="dest">The dest.</param>
        /// <param name="src">The SRC.</param>
        /// <returns></returns>
        public static OpCode Or(MemoryOperand dest, RegisterOperand src)
        {
            return new OpCode(new byte[] { 0x09 }, null);
        }
        #endregion

        #region SHL
        /// <summary>
        /// SHLs the specified dest.
        /// </summary>
        /// <param name="dest">The dest.</param>
        /// <param name="src">The SRC.</param>
        /// <returns></returns>
        public static OpCode Shl(Operand dest, Operand src)
        {
            if ((dest is RegisterOperand) && (src is RegisterOperand)) return Shl(dest as RegisterOperand, src as RegisterOperand);
            if ((dest is MemoryOperand) && (src is RegisterOperand)) return Shl(dest as MemoryOperand, src as RegisterOperand);
            if ((dest is RegisterOperand) && (src is ConstantOperand)) return Shl(dest as RegisterOperand, src as ConstantOperand);
            if ((dest is MemoryOperand) && (src is ConstantOperand)) return Shl(dest as MemoryOperand, src as ConstantOperand);

            throw new ArgumentException(@"No opcode for operand type.");
        }

        /// <summary>
        /// SHLs the specified dest.
        /// </summary>
        /// <param name="dest">The dest.</param>
        /// <param name="src">The SRC.</param>
        /// <returns></returns>
        public static OpCode Shl(RegisterOperand dest, RegisterOperand src)
        {
            return new OpCode(new byte[] { 0xD3 }, 4);
        }

        /// <summary>
        /// SHLs the specified dest.
        /// </summary>
        /// <param name="dest">The dest.</param>
        /// <param name="src">The SRC.</param>
        /// <returns></returns>
        public static OpCode Shl(MemoryOperand dest, RegisterOperand src)
        {
            //if ((dest.Type.Type == CilElementType.I1) || (dest.Type.Type == CilElementType.U1))
            //    return new OpCode(new byte[] { 0xD2 }, 4);

            return new OpCode(new byte[] { 0xD3 }, 4);
        }

        /// <summary>
        /// SHLs the specified dest.
        /// </summary>
        /// <param name="dest">The dest.</param>
        /// <param name="src">The SRC.</param>
        /// <returns></returns>
        public static OpCode Shl(RegisterOperand dest, ConstantOperand src)
        {
            return new OpCode(new byte[] { 0xC1 }, 4);
        }

        /// <summary>
        /// SHLs the specified dest.
        /// </summary>
        /// <param name="dest">The dest.</param>
        /// <param name="src">The SRC.</param>
        /// <returns></returns>
        public static OpCode Shl(MemoryOperand dest, ConstantOperand src)
        {
            //if ((dest.Type.Type == CilElementType.I1) || (dest.Type.Type == CilElementType.U1))
            //    return new OpCode(new byte[] { 0xC0 }, 4);

            return new OpCode(new byte[] { 0xC1 }, 4);
        }
        #endregion

        #region SHR
        /// <summary>
        /// SHRs the specified dest.
        /// </summary>
        /// <param name="dest">The dest.</param>
        /// <param name="src">The SRC.</param>
        /// <returns></returns>
        public static OpCode Shr(Operand dest, Operand src)
        {
            if ((dest is RegisterOperand) && (src is RegisterOperand)) return Shr(dest as RegisterOperand, src as RegisterOperand);
            if ((dest is MemoryOperand) && (src is RegisterOperand)) return Shr(dest as MemoryOperand, src as RegisterOperand);
            if ((dest is RegisterOperand) && (src is ConstantOperand)) return Shr(dest as RegisterOperand, src as ConstantOperand);
            if ((dest is MemoryOperand) && (src is ConstantOperand)) return Shr(dest as MemoryOperand, src as ConstantOperand);

            throw new ArgumentException(@"No opcode for operand type.");
        }

        /// <summary>
        /// SHRs the specified dest.
        /// </summary>
        /// <param name="dest">The dest.</param>
        /// <param name="src">The SRC.</param>
        /// <returns></returns>
        public static OpCode Shr(RegisterOperand dest, RegisterOperand src)
        {
            return new OpCode(new byte[] { 0xD3 }, 5);
        }

        /// <summary>
        /// SHRs the specified dest.
        /// </summary>
        /// <param name="dest">The dest.</param>
        /// <param name="src">The SRC.</param>
        /// <returns></returns>
        public static OpCode Shr(MemoryOperand dest, RegisterOperand src)
        {
            //if ((dest.Type.Type == CilElementType.I1) || (dest.Type.Type == CilElementType.U1))
            //    return new OpCode(new byte[] { 0xD2 }, 4);

            return new OpCode(new byte[] { 0xD3 }, 5);
        }

        /// <summary>
        /// SHRs the specified dest.
        /// </summary>
        /// <param name="dest">The dest.</param>
        /// <param name="src">The SRC.</param>
        /// <returns></returns>
        public static OpCode Shr(RegisterOperand dest, ConstantOperand src)
        {
            return new OpCode(new byte[] { 0xC1 }, 5);
        }

        /// <summary>
        /// SHRs the specified dest.
        /// </summary>
        /// <param name="dest">The dest.</param>
        /// <param name="src">The SRC.</param>
        /// <returns></returns>
        public static OpCode Shr(MemoryOperand dest, ConstantOperand src)
        {
            //if ((dest.Type.Type == CilElementType.I1) || (dest.Type.Type == CilElementType.U1))
            //    return new OpCode(new byte[] { 0xC0 }, 4);

            return new OpCode(new byte[] { 0xC1 }, 5);
        }
        #endregion

        #region Sub
        /// <summary>
		/// Subs the specified dest.
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Sub(Operand dest, Operand src)
		{
			if ((dest is Operand) && (src is ConstantOperand)) return Sub(dest as Operand, src as ConstantOperand);
			if ((dest is RegisterOperand) && (src is Operand)) return Sub(dest as RegisterOperand, src as Operand);
			if ((dest is MemoryOperand) && (src is RegisterOperand)) return Sub(dest as MemoryOperand, src as RegisterOperand);

			throw new ArgumentException(@"No opcode for operand type.");
		}

		/// <summary>
		/// Subs the specified dest.
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Sub(Operand dest, ConstantOperand src)
		{
			return new OpCode(new byte[] { 0x81 }, 5);
		}

		/// <summary>
		/// Subs the specified dest.
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Sub(RegisterOperand dest, Operand src)
		{
			return new OpCode(new byte[] { 0x2B }, null);
		}

		/// <summary>
		/// Subs the specified dest.
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Sub(MemoryOperand dest, RegisterOperand src)
		{
			if ((dest.Type.Type == CilElementType.I1) || (dest.Type.Type == CilElementType.U1))
				return new OpCode(new byte[] { 0x28 }, null);

			return new OpCode(new byte[] { 0x29 }, null);
        }
        #endregion

        #region Xor
        /// <summary>
		/// Xors the specified dest.
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Xor(Operand dest, Operand src)
		{
			if ((dest is RegisterOperand) && (src is ConstantOperand)) return Xor(dest as RegisterOperand, src as ConstantOperand);
			if ((dest is RegisterOperand) && (src is MemoryOperand)) return Xor(dest as RegisterOperand, src as MemoryOperand);
			if ((dest is RegisterOperand) && (src is RegisterOperand)) return Xor(dest as RegisterOperand, src as RegisterOperand);
			if ((dest is MemoryOperand) && (src is RegisterOperand)) return Xor(dest as MemoryOperand, src as RegisterOperand);

			throw new ArgumentException(@"No opcode for operand type.");
		}

		/// <summary>
		/// Ors the specified dest.
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Xor(RegisterOperand dest, ConstantOperand src)
		{
			return new OpCode(new byte[] { 0x81 }, 6);
		}

		/// <summary>
		/// Xors the specified dest.
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Xor(RegisterOperand dest, MemoryOperand src)
		{
			return new OpCode(new byte[] { 0x33 }, null);
		}

		/// <summary>
		/// Xors the specified dest.
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Xor(RegisterOperand dest, RegisterOperand src)
		{
			return new OpCode(new byte[] { 0x33 }, null);
		}

		/// <summary>
		/// Xors the specified dest.
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Xor(MemoryOperand dest, RegisterOperand src)
		{
			return new OpCode(new byte[] { 0x31 }, null);
        }
        #endregion
    }
}

