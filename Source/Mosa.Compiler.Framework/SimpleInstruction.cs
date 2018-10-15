// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework
{
	public class SimpleInstruction
	{
		public BaseInstruction Instruction { get; set; }
		public Operand Result { get; set; }
		public Operand Result2 { get; set; }

		public Operand Operand1 { get; set; }
		public Operand Operand2 { get; set; }
		public Operand Operand3 { get; set; }

		public int OperandCount { get { return Operand1 == null ? 0 : Operand2 == null ? 1 : Operand3 == null ? 2 : 3; } }
		public int ResultCount { get { return Result == null ? 0 : Result2 == null ? 1 : 2; } }

		public SimpleInstruction()
		{
		}

		public SimpleInstruction(BaseInstruction instruction, Operand result = null, Operand operand1 = null, Operand operand2 = null, Operand operand3 = null)
		{
			Instruction = instruction;
			Result = result;
			Operand1 = operand1;
			Operand2 = operand2;
			Operand3 = operand3;
		}
	}
}
