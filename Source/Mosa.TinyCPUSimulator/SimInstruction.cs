/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.TinyCPUSimulator
{
	public class SimInstruction
	{
		public BaseOpcode Opcode { get; private set; }

		public SimOperand Operand1 { get; private set; }

		public SimOperand Operand2 { get; private set; }

		public SimOperand Operand3 { get; private set; }

		public SimOperand Operand4 { get; private set; }

		public int OperandCount { get; private set; }

		public byte OpcodeSize { get; private set; }

		public string Source { get; set; }

		private SimInstruction(BaseOpcode opcode, byte opcodeSize, int operandCount)
		{
			Opcode = opcode;
			OperandCount = operandCount;
			OpcodeSize = opcodeSize;
		}

		public SimInstruction(BaseOpcode opcode, byte opcodeSize)
			: this(opcode, opcodeSize, 0)
		{ }

		public SimInstruction(BaseOpcode opcode, byte opcodeSize, SimOperand operand1)
			: this(opcode, opcodeSize, 1)
		{
			this.Operand1 = operand1;
		}

		public SimInstruction(BaseOpcode opcode, byte opcodeSize, SimOperand operand1, SimOperand operand2)
			: this(opcode, opcodeSize, 2)
		{
			this.Operand1 = operand1;
			this.Operand2 = operand2;
		}

		public SimInstruction(BaseOpcode opcode, byte opcodeSize, SimOperand operand1, SimOperand operand2, SimOperand operand3)
			: this(opcode, opcodeSize, 3)
		{
			this.Operand1 = operand1;
			this.Operand2 = operand2;
			this.Operand3 = operand3;
		}

		public SimInstruction(BaseOpcode opcode, byte opcodeSize, SimOperand operand1, SimOperand operand2, SimOperand operand3, SimOperand operand4)
			: this(opcode, opcodeSize, 4)
		{
			this.Operand1 = operand1;
			this.Operand2 = operand2;
			this.Operand3 = operand3;
			this.Operand4 = operand4;
		}

		public override string ToString()
		{
			string s = Opcode.ToString();

			if (OpcodeSize >= 1 && Operand1 != null)
				s = s + " " + Operand1.ToString();
			if (OpcodeSize >= 2 && Operand2 != null)
				s = s + ", " + Operand2.ToString();
			if (OpcodeSize >= 3 && Operand3 != null)
				s = s + ", " + Operand3.ToString();
			if (OpcodeSize >= 4 && Operand4 != null)
				s = s + ", " + Operand4.ToString();

			return s;
		}
	}
}