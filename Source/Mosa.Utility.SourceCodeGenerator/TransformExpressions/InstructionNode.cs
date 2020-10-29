// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Utility.SourceCodeGenerator.TransformExpressions
{
	public class InstructionNode
	{
		public string InstructionName { get; set; }

		public string ResultType { get; set; }

		public ConditionCode Condition { get; set; } = ConditionCode.Always;

		public List<ConditionCode> Conditions { get; set; } = new List<ConditionCode>() { ConditionCode.Always };

		public List<Operand> Operands { get; } = new List<Operand>();

		public int NodeNbr { get; set; }

		public InstructionNode Parent { get; set; }

		public override string ToString()
		{
			if (string.IsNullOrWhiteSpace(ResultType))
				return $"{NodeNbr} : {InstructionName}";
			else
				return $"{NodeNbr} : {InstructionName}<{ResultType}>";
		}

		public InstructionNode Clone(InstructionNode parent)
		{
			var node = new InstructionNode()
			{
				InstructionName = InstructionName,
				ResultType = ResultType,
				NodeNbr = NodeNbr,
				Parent = parent,
				Condition = Condition,
				Conditions = Conditions
			};

			foreach (var operand in Operands)
			{
				node.Operands.Add(operand.Clone(node));
			}

			return node;
		}
	}
}
