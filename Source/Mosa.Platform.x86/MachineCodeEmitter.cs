/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Scott Balmos <sbalmos@fastmail.fm>
 */

using System;
using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Operands;
using Mosa.Compiler.Linker;
using Mosa.Compiler.Metadata;

namespace Mosa.Platform.x86
{
	/// <summary>
	/// An x86 machine code emitter.
	/// </summary>
	public sealed class MachineCodeEmitter : BaseCodeEmitter, IDisposable
	{

		/// <summary>
		/// Initializes a new instance of the <see cref="MachineCodeEmitter"/> class.
		/// </summary>
		public MachineCodeEmitter()
		{
		}

		#region Code Generation

		/// <summary>
		/// Emits relative branch code.
		/// </summary>
		/// <param name="code">The branch instruction code.</param>
		/// <param name="dest">The destination label.</param>
		public void EmitBranch(byte[] code, int dest)
		{
			codeStream.Write(code, 0, code.Length);
			EmitRelativeBranchTarget(dest);
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
		/// Emits the specified op code.
		/// </summary>
		/// <param name="opCode">The op code.</param>
		/// <param name="dest">The dest.</param>
		public void Emit(OpCode opCode, Operand dest)
		{
			byte? sib, modRM;
			Operand displacement;

			// Write the opcode
			codeStream.Write(opCode.Code, 0, opCode.Code.Length);

			// Write the mod R/M byte
			modRM = CalculateModRM(opCode.RegField, dest, null, out sib, out displacement);
			if (modRM != null)
			{
				codeStream.WriteByte(modRM.Value);
				if (sib.HasValue)
					codeStream.WriteByte(sib.Value);
			}

			// Add displacement to the code
			if (displacement != null)
				WriteDisplacement(displacement);
		}

		/// <summary>
		/// Emits the given code.
		/// </summary>
		/// <param name="opCode">The op code.</param>
		/// <param name="dest">The destination operand.</param>
		/// <param name="src">The source operand.</param>
		public void Emit(OpCode opCode, Operand dest, Operand src)
		{
			byte? sib, modRM;
			Operand displacement;

			// Write the opcode
			codeStream.Write(opCode.Code, 0, opCode.Code.Length);

			if (dest == null && src == null)
				return;

			// Write the mod R/M byte
			modRM = CalculateModRM(opCode.RegField, dest, src, out sib, out displacement);
			if (modRM != null)
			{
				codeStream.WriteByte(modRM.Value);
				if (sib.HasValue)
					codeStream.WriteByte(sib.Value);
			}

			// Add displacement to the code
			if (displacement != null)
				WriteDisplacement(displacement);

			// Add immediate bytes
			if (dest is ConstantOperand)
				WriteImmediate(dest);
			if (src is ConstantOperand)
				WriteImmediate(src);
			if (src is SymbolOperand)
				WriteDisplacement(src);
		}

		/// <summary>
		/// Emits the given code.
		/// </summary>
		/// <param name="opCode">The op code.</param>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The source.</param>
		/// <param name="third">The third.</param>
		public void Emit(OpCode opCode, Operand dest, Operand src, Operand third)
		{
			byte? sib = null, modRM = null;
			Operand displacement = null;

			// Write the opcode
			codeStream.Write(opCode.Code, 0, opCode.Code.Length);

			if (dest == null && src == null)
				return;

			// Write the mod R/M byte
			modRM = CalculateModRM(opCode.RegField, dest, src, out sib, out displacement);
			if (modRM != null)
			{
				codeStream.WriteByte(modRM.Value);
				if (sib.HasValue)
					codeStream.WriteByte(sib.Value);
			}

			// Add displacement to the code
			if (displacement != null)
				WriteDisplacement(displacement);

			// Add immediate bytes
			if (third is ConstantOperand)
				WriteImmediate(third);
		}

		/// <summary>
		/// Emits the displacement operand.
		/// </summary>
		/// <param name="displacement">The displacement operand.</param>
		public void WriteDisplacement(Operand displacement)
		{
			MemberOperand member = displacement as MemberOperand;
			LabelOperand label = displacement as LabelOperand;
			SymbolOperand symbol = displacement as SymbolOperand;

			int pos = (int)(codeStream.Position - codeStreamBasePosition);

			if (label != null)
			{
				linker.Link(LinkType.AbsoluteAddress | LinkType.NativeI4, compiler.Method.ToString(), pos, 0, label.Name, IntPtr.Zero);
				codeStream.Position += 4;
			}
			else if (member != null)
			{
				linker.Link(LinkType.AbsoluteAddress | LinkType.NativeI4, compiler.Method.ToString(), pos, 0, member.Member.ToString(), member.Offset);
				codeStream.Position += 4;
			}
			else if (symbol != null)
			{
				linker.Link(LinkType.AbsoluteAddress | LinkType.NativeI4, compiler.Method.ToString(), pos, 0, symbol.Name, IntPtr.Zero);
				codeStream.Position += 4;
			}
			else
			{
				codeStream.Write((displacement as MemoryOperand).Offset.ToInt32(), true);
			}

		}

		/// <summary>
		/// Emits an immediate operand.
		/// </summary>
		/// <param name="op">The immediate operand to emit.</param>
		private void WriteImmediate(Operand op)
		{

			if (op is LocalVariableOperand)
			{
				// Add the displacement
				codeStream.Write((op as StackOperand).Offset.ToInt32(), true);
			}
			else if (op is LabelOperand)
			{
				literals.Add(new Patch((op as LabelOperand).Label, codeStream.Position));
				codeStream.WriteZeroBytes(4); // or codeStream.Position += 4;
			}
			else if (op is MemoryOperand)
			{
				// Add the displacement
				codeStream.Write((op as MemoryOperand).Offset.ToInt32(), true);
			}
			else if (op is ConstantOperand)
			{
				// Add the immediate
				ConstantOperand co = (ConstantOperand)op;
				switch (op.Type.Type)
				{
					case CilElementType.I:
						try
						{
							if (co.Value is Token)
							{
								codeStream.Write(((Token)co.Value).ToInt32(), true);
							}
							else
							{
								codeStream.Write(Convert.ToInt32(co.Value), true);
							}
						}
						catch (OverflowException) // Odd??
						{
							codeStream.Write(Convert.ToUInt64(co.Value), true);
						}
						break;

					case CilElementType.I1:
						codeStream.WriteByte(Convert.ToByte(co.Value));
						break;
					case CilElementType.I2:
						codeStream.Write(Convert.ToInt16(co.Value), true);
						break;
					case CilElementType.I4:
						goto case CilElementType.I;
					case CilElementType.U1:
						codeStream.WriteByte(Convert.ToByte(co.Value));
						break;
					case CilElementType.Char:
						goto case CilElementType.U2;
					case CilElementType.U2:
						codeStream.Write((ushort)Convert.ToUInt64(co.Value), true);
						break;
					case CilElementType.Ptr:
					case CilElementType.U4:
						codeStream.Write((uint)Convert.ToUInt64(co.Value), true);
						break;
					case CilElementType.I8:
						codeStream.Write(Convert.ToInt64(co.Value), true);
						break;
					case CilElementType.U8:
						codeStream.Write(Convert.ToUInt64(co.Value), true);
						break;
					case CilElementType.R4:
						codeStream.Write(Endian.ConvertToUInt32(Convert.ToSingle(co.Value)), true);
						break;
					case CilElementType.R8:
						goto default;
					case CilElementType.Object:
						goto case CilElementType.I;
					default:
						throw new NotSupportedException(String.Format(@"CilElementType.{0} is not supported.", op.Type.Type));
				}
			}
			else if (op is RegisterOperand)
			{
				// Nothing to do...
			}
			else
			{
				throw new NotImplementedException();
			}

		}

		/// <summary>
		/// Emits the relative branch target.
		/// </summary>
		/// <param name="label">The label.</param>
		private void EmitRelativeBranchTarget(int label)
		{
			// The relative offset of the label
			int relOffset = 0;

			// The position in the code stream of the label
			long position;

			// Did we see the label?
			if (labels.TryGetValue(label, out position))
			{
				// Yes, calculate the relative offset
				relOffset = (int)position - ((int)codeStream.Position + 4);
			}
			else
			{
				// Forward jump, we can't resolve yet - store a patch
				patches.Add(new Patch(label, codeStream.Position));
			}

			// Emit the relative jump offset (zero if we don't know it yet!)
			codeStream.Write(relOffset, true);
		}

		/// <summary>
		/// Emits a far jump to next instruction.
		/// </summary>
		public void EmitFarJumpToNextInstruction()
		{
			codeStream.WriteByte(0xEA);

			// HACK: Determines the IP address of current instruction, should use the linker instead
			LinkerSection linkerSection = linker.GetSection(SectionKind.Text);
			if (linkerSection != null) // To assist TypeExplorer, which returns null from GetSection method
			{
				codeStream.Write((int)(linkerSection.VirtualAddress.ToInt32() + linkerSection.Length + 6), true);
			}

			codeStream.WriteByte(0x08);
			codeStream.WriteByte(0x00);
		}

		/// <summary>
		/// Emits an immediate operand.
		/// </summary>
		/// <param name="op">The immediate operand to emit.</param>
		public void EmitImmediate(Operand op)
		{
			if (op is LocalVariableOperand)
			{
				// Add the displacement
				codeStream.Write((op as StackOperand).Offset.ToInt32(), true);
			}
			else if (op is LabelOperand)
			{
				literals.Add(new Patch((op as LabelOperand).Label, codeStream.Position));
				codeStream.WriteZeroBytes(4);
			}
			else if (op is MemoryOperand)
			{
				// Add the displacement
				codeStream.Write((op as MemoryOperand).Offset.ToInt32(), true);
			}
			else if (op is ConstantOperand)
			{
				// Add the immediate
				ConstantOperand co = (ConstantOperand)op;
				switch (op.Type.Type)
				{
					case CilElementType.I:
						try
						{
							codeStream.Write(Convert.ToInt32(co.Value), true);
						}
						catch (OverflowException)
						{
							codeStream.Write(Convert.ToUInt32(co.Value), true);
						}
						break;
					case CilElementType.I1:
						codeStream.WriteByte(Convert.ToByte(co.Value));
						break;

					case CilElementType.I2:
						codeStream.Write(Convert.ToInt16(co.Value), true);
						break;
					case CilElementType.I4:
						goto case CilElementType.I;
					case CilElementType.U1:
						codeStream.WriteByte(Convert.ToByte(co.Value));
						break;
					case CilElementType.Char:
						goto case CilElementType.U2;
					case CilElementType.U2:
						codeStream.Write(Convert.ToUInt16(co.Value), true);
						break;
					case CilElementType.U4:
						codeStream.Write(Convert.ToUInt32(co.Value), true);
						break;
					case CilElementType.I8:
						codeStream.Write(Convert.ToInt64(co.Value), true);
						break;
					case CilElementType.U8:
						codeStream.Write(Convert.ToUInt64(co.Value), true);
						break;
					case CilElementType.R4:
						codeStream.Write(Endian.ConvertToUInt32(Convert.ToSingle(co.Value)), true);
						break;
					case CilElementType.R8:
						goto default;
					default:
						throw new NotSupportedException();
				}
			}
			else if (op is RegisterOperand)
			{
				// Nothing to do...
			}
			else
			{
				throw new NotImplementedException();
			}
		}

		/// <summary>
		/// Calculates the value of the modR/M byte and SIB bytes.
		/// </summary>
		/// <param name="regField">The modR/M regfield value.</param>
		/// <param name="op1">The destination operand.</param>
		/// <param name="op2">The source operand.</param>
		/// <param name="sib">A potential SIB byte to emit.</param>
		/// <param name="displacement">An immediate displacement to emit.</param>
		/// <returns>The value of the modR/M byte.</returns>
		private static byte? CalculateModRM(byte? regField, Operand op1, Operand op2, out byte? sib, out Operand displacement)
		{
			byte? modRM = null;

			displacement = null;

			// FIXME: Handle the SIB byte
			sib = null;

			RegisterOperand rop1 = op1 as RegisterOperand, rop2 = op2 as RegisterOperand;
			MemoryOperand mop1 = op1 as MemoryOperand, mop2 = op2 as MemoryOperand;

			// Normalize the operand order
			if (rop1 == null && rop2 != null)
			{
				// Swap the memory operands
				rop1 = rop2; rop2 = null;
				mop2 = mop1; mop1 = null;
			}

			if (regField != null)
				modRM = (byte)(regField.Value << 3);

			if (rop1 != null && rop2 != null)
			{
				// mod = 11b, reg = rop1, r/m = rop2
				modRM = (byte)((3 << 6) | (rop1.Register.RegisterCode << 3) | rop2.Register.RegisterCode);
			}
			// Check for register/memory combinations
			else if (mop2 != null && mop2.Base != null)
			{
				// mod = 10b, reg = rop1, r/m = mop2
				modRM = (byte)(modRM.GetValueOrDefault() | (2 << 6) | (byte)mop2.Base.RegisterCode);
				if (rop1 != null)
					modRM |= (byte)(rop1.Register.RegisterCode << 3);
				displacement = mop2;
				if (mop2.Base.RegisterCode == 4)
					sib = 0xA4;
			}
			else if (mop2 != null)
			{
				// mod = 10b, r/m = mop1, reg = rop2
				modRM = (byte)(modRM.GetValueOrDefault() | 5);
				if (rop1 != null)
					modRM |= (byte)(rop1.Register.RegisterCode << 3);
				displacement = mop2;
			}
			else if (mop1 != null && mop1.Base != null)
			{
				// mod = 10b, r/m = mop1, reg = rop2
				modRM = (byte)(modRM.GetValueOrDefault() | (2 << 6) | mop1.Base.RegisterCode);
				if (rop2 != null)
					modRM |= (byte)(rop2.Register.RegisterCode << 3);
				displacement = mop1;
				if (mop1.Base.RegisterCode == 4)
					sib = 0xA4;
			}
			else if (mop1 != null)
			{
				// mod = 10b, r/m = mop1, reg = rop2
				modRM = (byte)(modRM.GetValueOrDefault() | 5);
				if (rop2 != null)
					modRM |= (byte)(rop2.Register.RegisterCode << 3);
				displacement = mop1;
			}
			else if (rop1 != null)
			{
				modRM = (byte)(modRM.GetValueOrDefault() | (3 << 6) | rop1.Register.RegisterCode);
				//if (op2 is SymbolOperand)
				//    displacement = op2;
			}

			return modRM;
		}

		#endregion // Code Generation
	}
}
