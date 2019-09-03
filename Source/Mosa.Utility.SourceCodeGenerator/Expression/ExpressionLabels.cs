// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Utility.SourceCodeGenerator.Expression
{
	public class ExpressionLabels
	{
		protected Dictionary<string, ExpressionLabel> Labels = new Dictionary<string, ExpressionLabel>();

		public ExpressionLabels(ExpressionNode node)
		{
			AddPositions(node);
		}

		public LabelPosition GetFirst(string label)
		{
			var expressionLabel = Labels[label];

			return expressionLabel.LabelPositions[0];
		}

		protected void AddPosition(string label, int nodeNbr, int operandIndex)
		{
			if (!Labels.TryGetValue(label, out ExpressionLabel expressionLabel))
			{
				expressionLabel = new ExpressionLabel(label);
				Labels.Add(label, expressionLabel);
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
				}

				if (operand.IsExpressionNode)
				{
					AddPositions(operand.Node);
				}
			}
		}
	}
}
