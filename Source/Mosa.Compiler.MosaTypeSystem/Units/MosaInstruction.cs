// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.MosaTypeSystem
{
	public class MosaInstruction
	{
		public int Offset { get; private set; }

		public ushort OpCode { get; private set; }

		public object Operand { get; private set; }

		public int? Previous { get; private set; }

		public int? Next { get; private set; }

		public MosaInstruction(int offset, ushort opCode, object operand, int? prev, int? next)
		{
			this.Offset = offset;
			this.OpCode = opCode;
			this.Operand = operand;
			this.Previous = prev;
			this.Next = next;
		}
	}
}
