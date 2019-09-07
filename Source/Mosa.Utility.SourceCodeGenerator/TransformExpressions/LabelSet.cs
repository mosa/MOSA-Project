// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Utility.SourceCodeGenerator.TransformExpressions
{
	public class LabelSet
	{
		protected Dictionary<string, Label> LabelLookupByName = new Dictionary<string, Label>();

		public List<string> Labels { get; } = new List<string>();

		public LabelSet(InstructionNode node)
		{
			AddPositions(node);
		}

		public LabelPosition GetFirstPosition(string name)
		{
			var label = LabelLookupByName[name];

			return label.Positions[0];
		}

		public Label GetExpressionLabel(string name)
		{
			return LabelLookupByName[name];
		}

		protected void AddPosition(string name, int nodeNbr, int operandIndex)
		{
			if (!LabelLookupByName.TryGetValue(name, out Label expressionLabel))
			{
				expressionLabel = new Label(name);
				LabelLookupByName.Add(name, expressionLabel);
			}

			expressionLabel.Add(nodeNbr, operandIndex);
		}

		protected void AddPositions(InstructionNode node)
		{
			foreach (var operand in node.Operands)
			{
				if (operand.IsLabel)
				{
					AddPosition(operand.LabelName, node.NodeNbr, operand.Index);

					if (!Labels.Contains(operand.LabelName))
					{
						Labels.Add(operand.LabelName);
					}
				}

				if (operand.IsInstruction)
				{
					AddPositions(operand.InstructionNode);
				}
			}
		}
	}
}
