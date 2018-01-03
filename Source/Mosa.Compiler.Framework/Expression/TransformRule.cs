// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Expression
{
	public class TransformRule
	{
		public Node InstructionMatchExpression { get; }
		public ExpressionNode CriteriaExpression { get; }
		public Node TransformationExpression { get; }

		public int OperandVariableCount { get; protected set; }
		public int TypeVariableCount { get; protected set; }

		public TransformRule(Node instructionMatch, ExpressionNode criteriaExpression, Node transform, int operandVariableCount, int typeVariableCount)
		{
			InstructionMatchExpression = instructionMatch;
			CriteriaExpression = criteriaExpression;
			TransformationExpression = transform;
			TypeVariableCount = typeVariableCount;
			OperandVariableCount = operandVariableCount;
		}

		public bool Validate(InstructionNode node)
		{
			var operands = new Operand[OperandVariableCount];
			var types = new MosaType[TypeVariableCount];

			// validate the instructions and operands match
			bool match = InstructionMatchExpression.Match(node, operands, types);

			if (!match)
				return false;

			// validate against any criteria
			if (CriteriaExpression == null)
				return true;

			var result = ExpressionEvaluator.Evaluate(CriteriaExpression, operands, types);

			if (result.IsTrue)
				return true;

			return false;
		}
	}
}
