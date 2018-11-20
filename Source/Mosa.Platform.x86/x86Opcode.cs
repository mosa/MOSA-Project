// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.IO;

namespace Mosa.Platform.x86
{
	public class x86Opcode // Experiment
	{
		public byte? InstructionPrefix;
		public byte? Rex;
		public byte? AddressPrefix;
		public byte? OperandPrefix;
		public byte? Opcode;
		public byte? Opcode2;
		public byte? Opcode3;

		public byte? ModRM;
		public byte? SIB;

		public int DisplacementSize = 0;
		public ulong? Displacement;

		public int ImmediateSize = 0;
		public ulong? Immediate;

		public void Emit(Stream stream)
		{
			if (InstructionPrefix.HasValue)
			{
				stream.WriteByte(InstructionPrefix.Value);
			}

			if (Rex.HasValue)
			{
				stream.WriteByte(Rex.Value);
			}

			if (AddressPrefix.HasValue)
			{
				stream.WriteByte(AddressPrefix.Value);
			}

			if (OperandPrefix.HasValue)
			{
				stream.WriteByte(OperandPrefix.Value);
			}

			if (Opcode.HasValue)
			{
				stream.WriteByte(Opcode.Value);
			}

			if (Opcode2.HasValue)
			{
				stream.WriteByte(Opcode2.Value);
			}

			if (Opcode3.HasValue)
			{
				stream.WriteByte(Opcode3.Value);
			}

			if (ModRM.HasValue)
			{
				stream.WriteByte(ModRM.Value);
			}

			if (SIB.HasValue)
			{
				stream.WriteByte(SIB.Value);
			}

			switch (ImmediateSize)
			{
				case 0: break;
				case 1: stream.WriteByte((byte)SIB.Value); break;
				case 2: stream.WriteByte((byte)SIB.Value); stream.WriteByte((byte)(SIB.Value >> 8)); break;
				case 4: stream.WriteByte((byte)SIB.Value); stream.WriteByte((byte)(SIB.Value >> 8)); stream.WriteByte((byte)(SIB.Value >> 16)); break;
				case 8: stream.WriteByte((byte)SIB.Value); stream.WriteByte((byte)(SIB.Value >> 8)); stream.WriteByte((byte)(SIB.Value >> 16)); stream.WriteByte((byte)(SIB.Value >> 24)); break;
				default: break;
			}

			switch (DisplacementSize)
			{
				case 0: break;
				case 1: stream.WriteByte((byte)Immediate.Value); break;
				case 2: stream.WriteByte((byte)Immediate.Value); stream.WriteByte((byte)(Immediate.Value >> 8)); break;
				case 4: stream.WriteByte((byte)Immediate.Value); stream.WriteByte((byte)(Immediate.Value >> 8)); stream.WriteByte((byte)(Immediate.Value >> 16)); break;
				case 8: stream.WriteByte((byte)Immediate.Value); stream.WriteByte((byte)(Immediate.Value >> 8)); stream.WriteByte((byte)(Immediate.Value >> 16)); stream.WriteByte((byte)(Immediate.Value >> 24)); break;
				default: break;
			}
		}

		public void Clear()
		{
			InstructionPrefix = null;
			Rex = null;
			AddressPrefix = null;
			OperandPrefix = null;
			AddressPrefix = null;
			Opcode = null;
			Opcode2 = null;
			Opcode3 = null;
			ModRM = null;
			SIB = null;
			DisplacementSize = 0;
			Displacement = null;
			ImmediateSize = 0;
			Immediate = null;
		}

		public void SetOpcode(byte opcode)
		{
			Opcode = opcode;
		}
	}
}
