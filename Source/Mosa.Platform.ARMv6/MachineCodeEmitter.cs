// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
using System;
using System.Diagnostics;

namespace Mosa.Platform.ARMv6
{
	public enum Indexing { Pre, Post };

	public enum TransferSize { Byte, Word };

	public enum TransferType { Store, Load };

	public enum WriteBack { NoWriteBack, Write }

	public enum OffsetDirection { Up, Down };

	/// <summary>
	/// An ARMv6 machine code emitter.
	/// </summary>
	public sealed class MachineCodeEmitter : BaseCodeEmitter
	{
		#region Code Generation Members

		public override void ResolvePatches()
		{
			// TODO: Check x86 Implementation
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

		public static byte GetConditionCode(ConditionCode condition)
		{
			switch (condition)
			{
				case ConditionCode.Always: return Bits.b1110;
				case ConditionCode.Never: return Bits.b1111;
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
				case ConditionCode.Positive: return Bits.b0101;

				default: throw new NotSupportedException();
			}
		}

		public static byte GetShiftTypeCode(ShiftType shiftType)
		{
			switch (shiftType)
			{
				case ShiftType.LogicalLeft: return Bits.b00;
				case ShiftType.LogicalRight: return Bits.b01;
				case ShiftType.ArithmeticRight: return Bits.b10;
				case ShiftType.RotateRight: return Bits.b11;
				default: throw new NotSupportedException();
			}
		}

		public void EmitBranch(ConditionCode conditionCode, int register)
		{
			Debug.Assert(register <= 0xF);

			uint value = 0;

			value |= (uint)(GetConditionCode(conditionCode) << 28);
			value |= (uint)0x12FFF10;
			value |= (uint)register;

			Write(value);
		}

		public void EmitBranch(ConditionCode conditionCode, int offset, bool link)
		{
			Debug.Assert(offset <= 0xFFF);

			uint value = 0;

			value |= (uint)(GetConditionCode(conditionCode) << 28);
			value |= (uint)(Bits.b0101 << 25);
			value |= (uint)(link ? 1 : 0 << 24);
			value |= (uint)offset;

			Write(value);
		}

		public void EmitInstructionWithRegister(ConditionCode conditionCode, byte opcode, bool setCondition, int firstRegister, int destinationRegister, ShiftType secondShiftType, int secondRegister)
		{
			Debug.Assert(opcode <= 0xF);
			Debug.Assert(destinationRegister <= 0xF);
			Debug.Assert(firstRegister <= 0xF);
			Debug.Assert(secondRegister <= 0xF);

			uint value = 0;

			value |= (uint)(GetConditionCode(conditionCode) << 28);
			value |= (uint)(0x0 << 25);
			value |= (uint)(opcode << 21);
			value |= (uint)(setCondition ? 1 : 0 << 20);
			value |= (uint)(firstRegister << 16);
			value |= (uint)(destinationRegister << 12);
			value |= (uint)(GetShiftTypeCode(secondShiftType) << 4);
			value |= (uint)secondRegister;

			Write(value);
		}

		public void EmitInstructionWithImmediate(ConditionCode conditionCode, byte opcode, bool setCondition, int firstRegister, int destinationRegister, int rotate, int immediate)
		{
			Debug.Assert(opcode <= 0xF);
			Debug.Assert(destinationRegister <= 0xF);
			Debug.Assert(firstRegister <= 0xF);
			Debug.Assert(rotate <= 0xF);
			Debug.Assert(immediate <= 0xFF);

			uint value = 0;

			value |= (uint)(GetConditionCode(conditionCode) << 28);
			value |= (uint)(0x1 << 25);
			value |= (uint)(opcode << 21);
			value |= (uint)(setCondition ? 1 : 0 << 20);
			value |= (uint)(firstRegister << 16);
			value |= (uint)(destinationRegister << 12);
			value |= (uint)(rotate << 8);
			value |= (uint)immediate;

			Write(value);
		}

		public void EmitMultiply(ConditionCode conditionCode, bool setCondition, int firstRegister, int destinationRegister, int secondRegister)
		{
			Debug.Assert(destinationRegister <= 0xF);
			Debug.Assert(secondRegister <= 0xF);
			Debug.Assert(firstRegister <= 0xF);

			uint value = 0;

			value |= (uint)(GetConditionCode(conditionCode) << 28);
			value |= (uint)(setCondition ? 1 : 0 << 20);
			value |= (uint)(destinationRegister << 16);
			value |= (uint)(firstRegister << 12);
			value |= (uint)(Bits.b1001 << 4);
			value |= (uint)(secondRegister << 8);

			Write(value);
		}

		public void EmitMultiplyWithAccumulate(ConditionCode conditionCode, bool setCondition, int firstRegister, int destinationRegister, int secondRegister, int accumulateRegister)
		{
			Debug.Assert(destinationRegister <= 0xF);
			Debug.Assert(secondRegister <= 0xF);
			Debug.Assert(firstRegister <= 0xF);

			uint value = 0;

			value |= (uint)(GetConditionCode(conditionCode) << 28);
			value |= (uint)(1 << 21);
			value |= (uint)(setCondition ? 1 : 0 << 20);
			value |= (uint)(destinationRegister << 16);
			value |= (uint)(firstRegister << 12);
			value |= (uint)(secondRegister << 8);
			value |= (uint)(Bits.b1001 << 4);
			value |= (uint)accumulateRegister;

			Write(value);
		}

		public void EmitSingleDataTransfer(ConditionCode conditionCode, Indexing indexing, OffsetDirection offsetDirection, TransferSize transferSize, WriteBack writeBack, TransferType transferType, int firstRegister, int destinationRegister, uint immediate)
		{
			Debug.Assert(destinationRegister <= 0xF);
			Debug.Assert(firstRegister <= 0xF);
			Debug.Assert(immediate <= 0xFFF);

			uint value = 0;

			value |= (uint)(GetConditionCode(conditionCode) << 28);
			value |= (uint)(1 << 26);
			value |= (uint)(1 << 25);
			value |= (uint)((indexing == Indexing.Post ? 0 : 1) << 24);
			value |= (uint)((transferSize == TransferSize.Word ? 0 : 1) << 23);
			value |= (uint)((offsetDirection == OffsetDirection.Down ? 0 : 1) << 22);
			value |= (uint)((writeBack == WriteBack.NoWriteBack ? 0 : 1) << 21);
			value |= (uint)((transferType == TransferType.Store ? 0 : 1) << 20);
			value |= (uint)(destinationRegister << 12);
			value |= (uint)(firstRegister << 16);
			value |= (uint)immediate;

			Write(value);
		}

		public void EmitSingleDataTransfer(ConditionCode conditionCode, Indexing indexing, OffsetDirection offsetDirection, TransferSize transferSize, WriteBack writeBack, TransferType transferType, int firstRegister, int destinationRegister, ShiftType secondShiftType, int secondRegister)
		{
			Debug.Assert(destinationRegister <= 0xF);
			Debug.Assert(firstRegister <= 0xF);
			Debug.Assert(secondRegister <= 0xF);

			uint value = 0;

			value |= (uint)(GetConditionCode(conditionCode) << 28);
			value |= (uint)(1 << 26);
			value |= (uint)(1 << 25);
			value |= (uint)((indexing == Indexing.Post ? 0 : 1) << 24);
			value |= (uint)((transferSize == TransferSize.Word ? 0 : 1) << 23);
			value |= (uint)((offsetDirection == OffsetDirection.Down ? 0 : 1) << 22);
			value |= (uint)((writeBack == WriteBack.NoWriteBack ? 0 : 1) << 21);
			value |= (uint)((transferType == TransferType.Store ? 0 : 1) << 20);
			value |= (uint)(destinationRegister << 12);
			value |= (uint)(firstRegister << 16);
			value |= (uint)(GetShiftTypeCode(secondShiftType) << 4);
			value |= (uint)secondRegister;

			Write(value);
		}

		// TODO: Add additional instruction formats

		#endregion Instruction Format Emitters
	}
}