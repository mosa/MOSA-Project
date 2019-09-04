// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;

namespace Mosa.Utility.SourceCodeGenerator.TransformExpressions
{
	public static class InstructionParser
	{
		public static InstructionNode Parse(List<Token> tokens)
		{
			return ParseInstructionNode(tokens, 0, 0).node;
		}

		private static (InstructionNode node, int end, int nodeNbr) ParseInstructionNode(List<Token> tokens, int start, int nodeNbr)
		{
			// skip first open parens, if one exists
			if (tokens[start].TokenType == TokenType.OpenParens)
				start++;

			var node = new InstructionNode() { NodeNbr = nodeNbr };
			var length = tokens.Count;

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
					node.Operands.Add(new Operand(new Token(TokenType.Label, token.Position, token.Value), node.Operands.Count));
				}
				else if (token.TokenType == TokenType.IntegerConstant)
				{
					node.Operands.Add(new Operand(token, node.Operands.Count));
				}
				else if (token.TokenType == TokenType.LongConstant)
				{
					node.Operands.Add(new Operand(token, node.Operands.Count));
				}
				else if (token.TokenType == TokenType.DoubleConstant)
				{
					node.Operands.Add(new Operand(token, node.Operands.Count));
				}
				else if (token.TokenType == TokenType.FloatConstant)
				{
					node.Operands.Add(new Operand(token, node.Operands.Count));
				}
				else if (token.TokenType == TokenType.Underscore)
				{
					node.Operands.Add(new Operand(token, node.Operands.Count));
				}
				else if (token.TokenType == TokenType.OpenParens)
				{
					var innerNode = ParseInstructionNode(tokens, index + 1, ++nodeNbr);

					index = innerNode.end;
					nodeNbr = innerNode.nodeNbr;

					node.Operands.Add(new Operand(innerNode.node, node.Operands.Count));
				}
				else
				{
					throw new Exception($"parsing error {token}");
				}
			}

			return (node, index, nodeNbr);
		}
	}
}
