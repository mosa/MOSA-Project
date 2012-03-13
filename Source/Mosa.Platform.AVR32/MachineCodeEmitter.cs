/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Pascal Delprat (pdelprat) <pascal.delprat@online.fr>  
 */

using System;
using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Operands;
using Mosa.Compiler.Linker;
using Mosa.Compiler.Metadata;

namespace Mosa.Platform.AVR32
{
	/// <summary>
	/// An AVR32 machine code emitter.
	/// </summary>
	public sealed class MachineCodeEmitter : BaseCodeEmitter, ICodeEmitter, IDisposable
	{

		public MachineCodeEmitter()
		{
		}

		#region Code Generation Members

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

		private bool Is32Opcode(uint opcode)
		{
			return ((opcode & 0xE0000000) == 0xE000000);
		}

		public void WriteOpcode(uint opcode)
		{
			if (Is32Opcode(opcode))
				Write(opcode);
			else
				Write((ushort)opcode);
		}

		/// <summary>
		/// Calls the specified target.
		/// </summary>
		/// <param name="symbolOperand">The symbol operand.</param>
		public void Call(SymbolOperand symbolOperand)
		{
			linker.Link(
				LinkType.RelativeOffset | LinkType.NativeI4,
				compiler.Method.ToString(),
				(int)(codeStream.Position - codeStreamBasePosition),
				(int)(codeStream.Position - codeStreamBasePosition) + 4,
				symbolOperand.Name,
				IntPtr.Zero
			);

			codeStream.Position += 4;
		}

		/// <summary>
		/// Emit with format 9.2.1 
		/// </summary>
		/// <param name="opcode"></param>
		/// <param name="firstRegister"></param>
		/// <param name="secondRegister"></param>
		public void EmitTwoRegisterInstructions(byte opcode, byte firstRegister, byte secondRegister)
		{
			ushort buffer = 0;

			buffer |= (ushort)((opcode & 0xE0) << 8);
			buffer |= (ushort)((opcode & 0x1F) << 4);
			buffer |= (ushort)(firstRegister << 9);
			buffer |= (ushort)(secondRegister);

			Write(buffer);
		}

		/// <summary>
		/// Emit with format 9.2.2 
		/// </summary>
		/// <param name="opcode"></param>
		/// <param name="register"></param>
		public void EmitSingleRegisterInstructions(byte opcode, byte register)
		{
			ushort buffer = 0;

			buffer |= (ushort)0x05C00;
			buffer |= (ushort)((opcode & 0x1F) << 4);
			buffer |= (ushort)(register);

			Write(buffer);
		}

		/// <summary>
		/// Emit with format 9.2.3 
		/// </summary>
		/// <param name="opcode"></param>
		/// <param name="first"></param>
		public void EmitReturnAndTest(byte opcode)
		{
			ushort buffer = 0;

			buffer |= (ushort)0x05E00;
			buffer |= (ushort)((opcode & 0x01) << 0x08);
			buffer |= (ushort)((0x0F) << 0x04);                         // Cond4 = always
			buffer |= (ushort)(0x0E);                                   // sp then r12 = 0

			Write(buffer);
		}

		/// <summary>
		/// Emit with format 9.2.4
		/// </summary>
		/// <param name="opcode"></param>
		/// <param name="k8"></param>
		/// <param name="register"></param>
		public void EmitK8immediateAndSingleRegister(byte opcode, sbyte k8, byte register)
		{
			ushort buffer = 0;

			buffer |= 0x2000;
			buffer |= (ushort)((opcode & 0x01) << 12);
			buffer |= (ushort)((k8 & 0xFF) << 4);
			buffer |= (ushort)(register);

			Write(buffer);
		}

		/// <summary>
		/// Emit with format 9.2.6 Bis // Seems to be a error on doc 32000.pdf
		/// </summary>
		/// <param name="k6"></param>
		/// <param name="register"></param>
		public void EmitK6immediateAndSingleRegister(sbyte k6, byte register)
		{
			ushort buffer = 0;

			buffer |= 0x5800;
			buffer |= (ushort)((k6 & 0x3F) << 4);
			buffer |= (ushort)(register);

			Write(buffer);
		}

		/// <summary>
		/// Emit with format 9.2.7
		/// </summary>
		/// <param name="pointerRegister"></param>
		/// <param name="k5"></param>
		/// <param name="destinationRegister"></param>
		public void EmitDisplacementLoadWithK5Immediate(byte pointerRegister, sbyte k5, byte destinationRegister)
		{
			ushort buffer = 0;

			buffer |= 0x6000;
			buffer |= (ushort)(pointerRegister << 9);
			buffer |= (ushort)((k5 & 0x1F) << 4);
			buffer |= (ushort)(destinationRegister);

			Write(buffer);
		}

		/// <summary>
		/// Emit with format 9.2.13
		/// </summary>
		/// <param name="opcode"></param>
		/// <param name="label"></param>
		public void EmitRelativeJumpAndCall(byte opcode, int label)
		{
			ushort value = (ushort)(label >> 1);
			ushort buffer = 0;

			buffer |= 0xC000;
			buffer |= (ushort)((value & 0xFF) << 4);
			buffer |= (ushort)(0x01 << 3);
			buffer |= (ushort)((opcode & 0x1) << 2);
			buffer |= (ushort)((value & 0x300) >> 8);

			Write(buffer);
		}

		/// <summary>
		/// Emit with format 9.2.20
		/// </summary>
		/// <param name="opcode"></param>
		/// <param name="firstSourceRegister"></param>
		/// <param name="secondSourceRegister"></param>
		/// <param name="destinationRegister"></param>
		public void EmitThreeRegistersUnshifted(byte opcode, byte firstSourceRegister, byte secondSourceRegister, byte destinationRegister)
		{
			ushort buffer = 0;

			buffer |= 0xE000;
			buffer |= (ushort)((firstSourceRegister & 0x0F) << 25);
			buffer |= (ushort)((secondSourceRegister & 0x0F) << 16);
			buffer |= (ushort)((opcode & 0xFF) << 4);
			buffer |= (ushort)(destinationRegister & 0x0F);

			Write(buffer);
		}

		/// <summary>
		/// Emit with format 9.2.23
		/// </summary>
		/// <param name="opcode"></param>
		/// <param name="sourceRegister"></param>
		/// <param name="destinationRegister"></param>
		/// <param name="k8"></param>
		public void EmitTwoRegisterOperandsWithK8Immediate(byte opcode, byte sourceRegister, byte destinationRegister, sbyte k8)
		{
			ushort buffer = 0;

			buffer |= 0xE000;
			buffer |= (ushort)((sourceRegister & 0x0F) << 25);
			buffer |= (ushort)((destinationRegister & 0x0F) << 16);
			buffer |= (ushort)((opcode & 0x0F) << 8);
			buffer |= (ushort)(k8 & 0xFF);

			Write(buffer);
		}

		/// <summary>
		/// Emit with format 9.2.28
		/// </summary>
		/// <param name="opcode"></param>
		/// <param name="register"></param>
		/// <param name="k16"></param>
		public void EmitRegisterOperandWithK16(ushort opcode, byte register, ushort k16)
		{
			uint buffer = 0;

			buffer |= 0xE0000000;
			buffer |= (uint)((opcode & 0x01FF) << 20);
			buffer |= (uint)(register  << 16);
			buffer |= (uint)(k16);

			Write(buffer);
		}

		/// <summary>
		/// Emit with format 9.2.30
		/// </summary>
		/// <param name="opcode"></param>
		/// <param name="registerOrCond"></param>
		/// <param name="k20"></param>
		public void EmitRegisterOrConditionCodeAndK21(byte opcode, byte registerOrCond, int k20)
		{
			uint buffer = 0;

			buffer |= 0xE0000000;
			buffer |= (uint)((k20 & 0xF0000) << 8);
			buffer |= (uint)((opcode & 0x1F) << 21);
			buffer |= (uint)((k20 & 0x10000) << 4);
			buffer |= (uint)(k20 & 0xFFFF);
			buffer |= (uint)(registerOrCond << 16);

			Write(buffer);
		}

		/// <summary>
		/// Emit with format 9.2.31
		/// </summary>
		/// <param name="opcode"></param>
		/// <param name="label"></param>
		public void EmitNoRegisterAndK21(byte opcode, int label)
		{
			uint buffer = 0;

			buffer |= 0xE0000000;
			buffer |= (uint)((label & 0xF0000) << 8);
			buffer |= (uint)((opcode & 0xF0) << 17);
			buffer |= (uint)((label & 0x10000) << 4);
			buffer |= (uint)(label & 0xFFFF);
			buffer |= (uint)((opcode & 0x0F) << 16);

			Write(buffer);
		}

		/// <summary>
		/// Emit with format 9.2.32
		/// </summary>
		/// <param name="opcode"></param>
		/// <param name="firstRegister"></param>
		/// <param name="secondRegister"></param>
		/// <param name="k16"></param>
		public void EmitTwoRegistersAndK16(byte opcode, byte firstRegister, byte secondRegister, short k16)
		{
			uint buffer = 0;

			buffer |= 0xE0000000;
			buffer |= (uint)(firstRegister << 25);
			buffer |= (uint)((opcode & 0x1F) << 20);
			buffer |= (uint)(secondRegister << 16);
			buffer |= (uint)(k16 & 0xFFFF);

			Write(buffer);
		}

		/// <summary>
		/// Emit with format 9.2.50
		/// </summary>
		/// <param name="opcode"></param>
		/// <param name="firstRegister"></param>
		/// <param name="secondRegister"></param>
		/// <param name="k16"></param>
		public void EmitTwoRegistersWithK4(byte firstRegister, byte secondRegister, sbyte k4)
		{
			uint buffer = 0;

			buffer |= 0x8000;
			buffer |= (uint)((firstRegister & 0x0F) << 9);
			buffer |= (uint)(0x01 << 8);
			buffer |= (uint)((k4 & 0x0F) << 4);
			buffer |= (uint)(secondRegister & 0x0F);

			Write(buffer);
		}

		#endregion

	}
}
