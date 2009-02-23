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
		/// <summary>
		/// Negates the specified dest.
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <returns></returns>
		public static OpCode Neg(Operand dest)
		{
			if (dest is RegisterOperand) return Neg(dest as RegisterOperand);
			if (dest is MemoryOperand) return Neg(dest as MemoryOperand);

			throw new ArgumentException(@"Not such op code for operand type.");
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
			return new OpCode(new byte[] { 0xF7 }, 3);
		}

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

			throw new ArgumentException(@"Not such op code for operand type.");
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

			throw new ArgumentException(@"Not such op code for operand type.");
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

			throw new ArgumentException(@"Not such op code for operand type.");
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

	}
}

