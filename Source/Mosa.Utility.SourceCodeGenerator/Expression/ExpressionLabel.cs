// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Utility.SourceCodeGenerator.Expression
{
	public partial class ExpressionLabel
	{
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
