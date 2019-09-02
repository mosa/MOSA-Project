// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Utility.SourceCodeGenerator.Expression
{
	public class ExpressionLabel
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

		public string Label { get; }

		public ExpressionLabel(string label)
		{
			Label = label;
		}

		public List<LabelPosition> LabelPositions { get; } = new List<LabelPosition>();

		public void Add(int nodeNbr, int operandIndex)
		{
			LabelPositions.Add(new LabelPosition(nodeNbr, operandIndex));
		}
	}
}
