/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Ki (kiootic) <kiootic@gmail.com>
 */

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
