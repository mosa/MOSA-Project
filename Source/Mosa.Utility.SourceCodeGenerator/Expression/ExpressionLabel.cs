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

		public List<LabelPosition> Positions { get; } = new List<LabelPosition>();

		public void Add(int nodeNbr, int operandIndex)
		{
			Positions.Add(new LabelPosition(nodeNbr, operandIndex));
		}
	}
}
