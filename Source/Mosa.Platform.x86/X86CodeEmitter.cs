// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Linker;
using System;
using System.Diagnostics;

namespace Mosa.Platform.x86
{
	/// <summary>
	/// An x86 machine code emitter.
	/// </summary>
	public sealed class X86CodeEmitter : BaseCodeEmitter
	{
		/// <summary>
		/// Calls the specified target.
		/// </summary>
		/// <param name="symbolOperand">The symbol operand.</param>
		public void EmitCallSite(Operand symbolOperand)
		{
			linker.Link(
				LinkType.RelativeOffset,
				PatchType.I4,
				SectionKind.Text,
				MethodName,
				(int)codeStream.Position,
				SectionKind.Text,
				symbolOperand.Name,
				-4
			);

			codeStream.WriteZeroBytes(4);
		}

		/// <summary>
		/// Emits relative branch code.
		/// </summary>
		/// <param name="code">The branch instruction code.</param>
		/// <param name="dest">The destination label.</param>
		public void EmitRelativeBranch(byte[] code, int dest)
		{
			codeStream.Write(code, 0, code.Length);
			EmitRelativeBranchTarget(dest);
		}

		/// <summary>
		/// Emits the relative branch target.
		/// </summary>
		/// <param name="label">The label.</param>
		public void EmitRelativeBranchTarget(int label)
		{
			// The relative offset of the label
			int relOffset = 0;

			// The position in the code stream of the label

			// Did we see the label?
			if (TryGetLabel(label, out int position))
			{
				// Yes, calculate the relative offset
				relOffset = position - ((int)codeStream.Position + 4);
			}
			else
			{
				// Forward jump, we can't resolve yet - store a patch
				AddPatch(label, (int)codeStream.Position);
			}

			// Emit the relative jump offset (zero if we don't know it yet!)
			codeStream.Write(relOffset, Endianness.Little);
		}

		public override void ResolvePatches()
		{
			// Save the current position
			long currentPosition = codeStream.Position;

			foreach (var p in patches)
			{
				if (!TryGetLabel(p.Label, out int labelPosition))
				{
					throw new ArgumentException("Missing label while resolving patches.", "label=" + labelPosition.ToString());
				}

				codeStream.Position = p.Position;

				// Compute relative branch offset
				int relOffset = labelPosition - ((int)p.Position + 4);

				// Write relative offset to stream
				var bytes = BitConverter.GetBytes(relOffset);
				codeStream.Write(bytes, 0, bytes.Length);
			}

			// Reset the position
			codeStream.Position = currentPosition;
		}

		/// <summary>
		/// Emits a far jump to next instruction.
		/// </summary>
		public void EmitFarJumpToNextInstruction()
		{
			codeStream.WriteByte(0xEA);

			linker.Link(LinkType.AbsoluteAddress, PatchType.I4, SectionKind.Text, MethodName, (int)codeStream.Position, SectionKind.Text, MethodName, (int)codeStream.Position + 6);

			codeStream.WriteZeroBytes(4);
			codeStream.WriteByte(0x08);
			codeStream.WriteByte(0x00);
		}

		#region Legacy Opcode Methods

		/// <summary>
		/// Emits the specified op code.
		/// </summary>
		/// <param name="opCode">The op code.</param>
		internal void Emit(LegacyOpCode opCode)
		{
			// Write the opcode
			codeStream.Write(opCode.Code, 0, opCode.Code.Length);
		}

		/// <summary>
		/// Emits the specified op code.
		/// </summary>
		/// <param name="opCode">The op code.</param>
		/// <param name="dest">The destination operand.</param>
		internal void Emit(LegacyOpCode opCode, Operand dest)
		{
			// Write the opcode
			codeStream.Write(opCode.Code, 0, opCode.Code.Length);

			// Write the mod R/M byte
			byte? modRM = CalculateModRM(opCode.RegField, dest, null);
			if (modRM != null)
			{
				codeStream.WriteByte(modRM.Value);
			}

			// Add immediate bytes
			if (dest.IsConstant)
				WriteImmediate(dest);
			else if (dest.IsSymbol)
				WriteDisplacement(dest);
		}

		/// <summary>
		/// Emits the given code.
		/// </summary>
		/// <param name="opCode">The op code.</param>
		/// <param name="dest">The destination operand.</param>
		/// <param name="src">The source operand.</param>
		internal void Emit(LegacyOpCode opCode, Operand dest, Operand src)
		{
			// Write the opcode
			codeStream.Write(opCode.Code, 0, opCode.Code.Length);

			Debug.Assert(!(dest == null && src == null));

			// Write the mod R/M byte
			byte? modRM = CalculateModRM(opCode.RegField, dest, src);
			if (modRM != null)
			{
				codeStream.WriteByte(modRM.Value);
			}

			// Add immediate bytes
			if (dest.IsConstant)
				WriteImmediate(dest);
			else if (dest.IsSymbol)
				WriteDisplacement(dest);
			else if (src?.IsResolvedConstant == true)
				WriteImmediate(src);
			else if (src != null && (src.IsSymbol || src.IsStaticField))
				WriteDisplacement(src);
		}

		/// <summary>
		/// Emits the given code.
		/// </summary>
		/// <param name="opCode">The op code.</param>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The source.</param>
		/// <param name="third">The third.</param>
		internal void Emit(LegacyOpCode opCode, Operand dest, Operand src, Operand third)
		{
			// Write the opcode
			codeStream.Write(opCode.Code, 0, opCode.Code.Length);

			Debug.Assert(!(dest == null && src == null));

			// Write the mod R/M byte
			byte? modRM = CalculateModRM(opCode.RegField, dest, src);
			if (modRM != null)
			{
				codeStream.WriteByte(modRM.Value);
			}

			// Add immediate bytes
			if (third?.IsConstant == true)
				WriteImmediate(third);
		}

		/// <summary>
		/// Emits the displacement operand.
		/// </summary>
		/// <param name="displacement">The displacement operand.</param>
		private void WriteDisplacement(Operand displacement)
		{
			if (displacement.IsLabel)
			{
				Debug.Assert(displacement.IsUnresolvedConstant);

				// FIXME! remove assertion
				Debug.Assert(displacement.Offset == 0);

				linker.Link(LinkType.AbsoluteAddress, PatchType.I4, SectionKind.Text, MethodName, (int)codeStream.Position, SectionKind.ROData, displacement.Name, 0);
				codeStream.WriteZeroBytes(4);
			}
			else if (displacement.IsStaticField)
			{
				Debug.Assert(displacement.IsUnresolvedConstant);
				var section = displacement.Field.Data != null ? SectionKind.ROData : SectionKind.BSS;

				linker.Link(LinkType.AbsoluteAddress, PatchType.I4, SectionKind.Text, MethodName, (int)codeStream.Position, section, displacement.Field.FullName, 0);
				codeStream.WriteZeroBytes(4);
			}
			else if (displacement.IsSymbol)
			{
				Debug.Assert(displacement.IsUnresolvedConstant);
				var section = (displacement.Method != null) ? SectionKind.Text : SectionKind.ROData;

				// First try finding the symbol in the expected section
				var symbol = linker.FindSymbol(displacement.Name, section);

				// If no symbol found, look in all sections
				if (symbol == null)
				{
					symbol = linker.FindSymbol(displacement.Name);
				}

				// Otherwise create the symbol in the expected section
				if (symbol == null)
				{
					symbol = linker.GetSymbol(displacement.Name, section);
				}

				linker.Link(LinkType.AbsoluteAddress, PatchType.I4, SectionKind.Text, MethodName, (int)codeStream.Position, symbol, 0);
				codeStream.WriteZeroBytes(4);
			}
			else
			{
				codeStream.Write((int)displacement.Offset, Endianness.Little);
			}
		}

		/// <summary>
		/// Emits an immediate operand.
		/// </summary>
		/// <param name="op">The immediate operand to emit.</param>
		private void WriteImmediate(Operand op)
		{
			Debug.Assert(!op.IsCPURegister);

			if (op.IsStackLocal)
			{
				codeStream.Write((int)op.Offset, Endianness.Little);
				return;
			}

			Debug.Assert(op.IsResolvedConstant);

			if (op.IsI1)
				codeStream.WriteByte((byte)op.ConstantSignedInteger);
			else if (op.IsU1 || op.IsBoolean)
				codeStream.WriteByte(Convert.ToByte(op.ConstantUnsignedInteger));
			else if (op.IsU2 || op.IsChar)
				codeStream.Write(Convert.ToUInt16(op.ConstantUnsignedInteger), Endianness.Little);
			else if (op.IsI2)
				codeStream.Write(Convert.ToInt16(op.ConstantSignedInteger), Endianness.Little);
			else if (op.IsI4 || op.IsI)
				codeStream.Write(Convert.ToInt32(op.ConstantSignedInteger), Endianness.Little);
			else if (op.IsU4 || op.IsPointer || op.IsU || !op.IsValueType)
				codeStream.Write(Convert.ToUInt32(op.ConstantUnsignedInteger), Endianness.Little);
			else if (op.IsI8 || op.IsU8)
				codeStream.Write(Convert.ToUInt32(op.ConstantUnsignedInteger), Endianness.Little);  // FUTURE: Remove me
			else
				throw new CompilerException();
		}

		/// <summary>
		/// Calculates the value of the modR/M byte and SIB bytes.
		/// </summary>
		/// <param name="regField">The modR/M regfield value.</param>
		/// <param name="op1">The destination operand.</param>
		/// <param name="op2">The source operand.</param>
		/// <returns>The value of the modR/M byte.</returns>
		private static byte? CalculateModRM(byte? regField, Operand op1, Operand op2)
		{
			byte? modRM = null;

			bool op1IsRegister = op1?.IsCPURegister == true;
			bool op2IsRegister = op2?.IsCPURegister == true;

			Debug.Assert(!(!op1IsRegister && op2IsRegister));

			if (regField != null)
				modRM = (byte)(regField.Value << 3);

			if (op1IsRegister && op2IsRegister)
			{
				// mod = 11b, reg = rop1, r/m = rop2
				return (byte)((3 << 6) | (op1.Register.RegisterCode << 3) | op2.Register.RegisterCode);
			}
			else if (op1IsRegister)
			{
				return (byte)(modRM.GetValueOrDefault() | (3 << 6) | op1.Register.RegisterCode);
			}

			return modRM;
		}

		#endregion Legacy Opcode Methods
	}
}
