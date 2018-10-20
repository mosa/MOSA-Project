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

		public ConditionCode ConditionCode { get; set; } = ConditionCode.Undefined;

		public int OperandCount { get { return Operand1 == null ? 0 : Operand2 == null ? 1 : Operand3 == null ? 2 : 3; } }
		public int ResultCount { get { return Result == null ? 0 : Result2 == null ? 1 : 2; } }
	}
}
