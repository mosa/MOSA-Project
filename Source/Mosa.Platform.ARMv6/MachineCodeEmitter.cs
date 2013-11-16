/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
using System;

namespace Mosa.Platform.ARMv6
{
	/// <summary>
	/// An AVR32 machine code emitter.
	/// </summary>
	public sealed class MachineCodeEmitter : BaseCodeEmitter, ICodeEmitter
	{
		#region Code Generation Members

		protected override void ResolvePatches()
		{
			// TODO: Check x86 Implementation
		}

		/// <summary>
		/// Writes the unsigned short.
		/// </summary>
		/// <param name="data">The data.</param>
		public void Write(ushort data)
		{
			codeStream.WriteByte((byte)((data >> 8) & 0xFF));
			codeStream.WriteByte((byte)(data & 0xFF));
		}

		/// <summary>
		/// Writes the unsigned int.
		/// </summary>
		/// <param name="data">The data.</param>
		public void Write(uint data)
		{
			codeStream.WriteByte((byte)((data >> 24) & 0xFF));
			codeStream.WriteByte((byte)((data >> 16) & 0xFF));
			codeStream.WriteByte((byte)((data >> 8) & 0xFF));
			codeStream.WriteByte((byte)(data & 0xFF));
		}

		#endregion Code Generation Members

		#region Instruction Format Emitters

		public byte GetConditionCode(ConditionCode condition)
		{
			switch (condition)
			{
				case ConditionCode.Equal: return Bits.b0000;
				case ConditionCode.GreaterOrEqual: return Bits.b1010;
				case ConditionCode.GreaterThan: return Bits.b1100;
				case ConditionCode.LessOrEqual: return Bits.b1101;
				case ConditionCode.LessThan: return Bits.b1011;
				case ConditionCode.NotEqual: return Bits.b0001;

				case ConditionCode.UnsignedGreaterOrEqual: return Bits.b0010;
				case ConditionCode.UnsignedGreaterThan: return Bits.b1000;
				case ConditionCode.UnsignedLessOrEqual: return Bits.b1001;
				case ConditionCode.UnsignedLessThan: return Bits.b0011;

				case ConditionCode.NotSigned: return Bits.b0000;
				case ConditionCode.Signed: return Bits.b0000;

				case ConditionCode.Zero: return Bits.b0101;

				case ConditionCode.Overflow: return Bits.b0110;
				case ConditionCode.NoOverflow: return Bits.b0111;

				case ConditionCode.Always: return Bits.b1110;

				default: throw new NotSupportedException();
			}
		}

		// TODO: Add additional instruction formats

		#endregion Instruction Format Emitters
	}
}