// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Utility.SourceCodeGenerator.Expression
{
	public class ExpressionLabels
	{
		protected Dictionary<string, ExpressionLabel> ExpressionalLabels = new Dictionary<string, ExpressionLabel>();

		public List<string> Labels { get; } = new List<string>();

		public ExpressionLabels(ExpressionNode node)
		{
			AddPositions(node);
		}

		public LabelPosition GetFirstPosition(string label)
		{
			var expressionLabel = ExpressionalLabels[label];

			return expressionLabel.Positions[0];
		}

		public ExpressionLabel GetExpressionLabel(string label)
		{
			return ExpressionalLabels[label];
		}

		protected void AddPosition(string label, int nodeNbr, int operandIndex)
		{
			if (!ExpressionalLabels.TryGetValue(label, out ExpressionLabel expressionLabel))
			{
				expressionLabel = new ExpressionLabel(label);
				ExpressionalLabels.Add(label, expressionLabel);
			}

			expressionLabel.Add(nodeNbr, operandIndex);
		}

		protected void AddPositions(ExpressionNode node)
		{
			foreach (var operand in node.Operands)
			{
				if (operand.IsLabel)
				{
					AddPosition(operand.Label, node.NodeNbr, operand.Index);

					if (!Labels.Contains(operand.Label))
						Labels.Add(operand.Label);
				}

				if (operand.IsExpressionNode)
				{
					AddPositions(operand.Node);
				}
			}
		}
	}
}
