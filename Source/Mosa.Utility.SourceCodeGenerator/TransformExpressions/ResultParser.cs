// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;

namespace Mosa.Utility.SourceCodeGenerator.TransformExpressions
{
	public static class ResultParser
	{
		public static InstructionNode Parse(List<Token> tokens)
		{
			return ParseInstructionNode(tokens, 0, 0).node;
		}

		private static (InstructionNode node, int end, int nodeNbr) ParseInstructionNode(List<Token> tokens, int start, int nodeNbr)
		{
			var node = new InstructionNode() { NodeNbr = nodeNbr };
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
				else if (token.TokenType == TokenType.OpenBracket)
				{
					var innerNode = ParseExpression(tokens, index + 1);

					index = innerNode.end;

					node.Operands.Add(new Operand(innerNode.method, node.Operands.Count));
				}
				else
				{
					throw new Exception($"parsing error {token}");
				}
			}

			return (node, index, nodeNbr);
		}

		private static (Method method, int end) ParseExpression(List<Token> tokens, int start)
		{
			var method = new Method();
			var length = tokens.Count;

			int index = start;
			for (; index < length;)
			{
				var token = tokens[index++];

				if (token.TokenType == TokenType.Not && method.MethodName == null)
				{
					method.IsNegated = true;
				}
				else if (token.TokenType == TokenType.Word && method.MethodName == null)
				{
					method.MethodName = token.Value;
					index++; // skip open paran
				}
				else if (token.TokenType == TokenType.Word && method.MethodName != null)
				{
					// peak ahead
					var next = tokens[index];

					if (next.TokenType == TokenType.OpenParens)
					{
						var innerNode = ParseExpression(tokens, index + 2);

						index = innerNode.end;

						method.Parameters.Add(new Operand(innerNode.method, method.Parameters.Count));
					}
					else
					{
						method.Parameters.Add(new Operand(new Token(TokenType.Label, token.Position, token.Value), method.Parameters.Count));
					}
				}
				else if (token.TokenType == TokenType.IntegerConstant)
				{
					method.Parameters.Add(new Operand(token, method.Parameters.Count));
				}
				else if (token.TokenType == TokenType.LongConstant)
				{
					method.Parameters.Add(new Operand(token, method.Parameters.Count));
				}
				else if (token.TokenType == TokenType.DoubleConstant)
				{
					method.Parameters.Add(new Operand(token, method.Parameters.Count));
				}
				else if (token.TokenType == TokenType.FloatConstant)
				{
					method.Parameters.Add(new Operand(token, method.Parameters.Count));
				}
				else if (token.TokenType == TokenType.Comma)
				{
					// skip
				}
				else if (token.TokenType == TokenType.CloseParens)
				{
					return (method, index);
				}
				else if (token.TokenType == TokenType.CloseBracket)
				{
					return (method, index);
				}
				else
				{
					throw new Exception($"parsing error {token}");
				}
			}

			return (method, index);
		}
	}
}
