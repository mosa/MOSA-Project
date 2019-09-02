// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Utility.SourceCodeGenerator.Expression
{
	public class ExpressionNode
	{
		public string InstructionName { get; set; }

		public List<ExpressionOperand> Operands { get; } = new List<ExpressionOperand>();

		public int NodeNbr { get; private set; }

		public static ExpressionNode Parse(List<Token> tokens)
		{
			return Parse(tokens, 0, 1).node;
		}

		protected static (ExpressionNode node, int end, int nodeNbr) Parse(List<Token> tokens, int start, int nodeNbr)
		{
			var node = new ExpressionNode() { NodeNbr = nodeNbr };
			var length = tokens.Count;

			// skip first open parens, if one exists
			if (tokens[start].TokenType == TokenType.OpenParens)
				start++;

			int index = start;
			for (; index < length; index++)
			{
				var token = tokens[index];

				if (token.TokenType == TokenType.CloseParens)
					break;

				if (token.TokenType == TokenType.Word && node.InstructionName == null)
				{
					node.InstructionName = token.Value;
				}
				else if (token.TokenType == TokenType.Word && node.InstructionName != null)
				{
					node.Operands.Add(new ExpressionOperand(token));
				}
				else if (token.TokenType == TokenType.IntegerConstant)
				{
					node.Operands.Add(new ExpressionOperand(token));
				}
				else if (token.TokenType == TokenType.LongConstant)
				{
					node.Operands.Add(new ExpressionOperand(token));
				}
				else if (token.TokenType == TokenType.DoubleConstant)
				{
					node.Operands.Add(new ExpressionOperand(token));
				}
				else if (token.TokenType == TokenType.FloatConstant)
				{
					node.Operands.Add(new ExpressionOperand(token));
				}
				else if (token.TokenType == TokenType.Underscore)
				{
					node.Operands.Add(new ExpressionOperand(token));
				}
				else if (token.TokenType == TokenType.OpenParens)
				{
					var innerNode = Parse(tokens, index + 1, ++nodeNbr);

					index = innerNode.end;
					nodeNbr = innerNode.nodeNbr;

					node.Operands.Add(new ExpressionOperand(innerNode.node));
				}
				else if (token.TokenType == TokenType.CloseBracket)
				{
					return (node, index + 1, nodeNbr);
				}
			}

			return (node, index, nodeNbr);
		}
	}
}
