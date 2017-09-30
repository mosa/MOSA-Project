// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.MosaTypeSystem
{
	public class MosaInstruction
	{
		public int Offset { get; }

		public ushort OpCode { get; }

		public object Operand { get; }

		public int? Previous { get; }

		public int? Next { get; }

		public MosaInstruction(int offset, ushort opCode, object operand, int? prev, int? next)
		{
			Offset = offset;
			OpCode = opCode;
			Operand = operand;
			Previous = prev;
			Next = next;
		}
	}
}
