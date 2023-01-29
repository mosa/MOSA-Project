// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.SourceCodeGenerator.TransformExpressions;

public struct LabelPosition
{
	public readonly int NodeNbr;
	public readonly int OperandIndex;

	public LabelPosition(int nodeNbr, int operandIndex)
	{
		NodeNbr = nodeNbr;
		OperandIndex = operandIndex;
	}

	public override string ToString()
	{
		return $"{NodeNbr} @ {OperandIndex}";
	}
}
