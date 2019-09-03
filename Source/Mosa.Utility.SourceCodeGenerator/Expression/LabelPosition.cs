// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.SourceCodeGenerator.Expression
{
	public struct LabelPosition
	{
		public int NodeNbr;
		public int OperandIndex;

		public LabelPosition(int nodeNbr, int operandIndex)
		{
			NodeNbr = nodeNbr;
			OperandIndex = operandIndex;
		}
	}
}
