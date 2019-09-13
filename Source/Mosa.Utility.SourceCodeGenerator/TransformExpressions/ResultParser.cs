// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;

namespace Mosa.Utility.SourceCodeGenerator.TransformExpressions
{
	public static class ResultParser
	{
		public static InstructionNode Parse(List<Token> tokens)
		{
			return ParseInstructionNode(tokens, 0, 0, null).node;
		}

		private static (InstructionNode node, int end, int nodeNbr) ParseInstructionNode(List<Token> tokens, int start, int nodeNbr, InstructionNode parent)
		{
			var node = new InstructionNode() { NodeNbr = nodeNbr, Parent = parent };
			var length = tokens.Count;

			for (int index = start; index < length; index++)
			{
				var token = tokens[index];

				if (token.TokenType == TokenType.OpenParens)
				{
					if (node.InstructionName == null)
					{
						index++;
						node.InstructionName = tokens[index].Value;
					}
					else
					{
						var childNode = ParseInstructionNode(tokens, index, nodeNbr + 1, node);

						index = childNode.end;
						nodeNbr = childNode.nodeNbr;

						node.Operands.Add(new Operand(childNode.node, node.Operands.Count));
					}
				}
				else if (token.TokenType == TokenType.CloseParens)
				{
					return (node, index, nodeNbr);
				}
				else if (token.TokenType == TokenType.Word)
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
				else if (token.TokenType == TokenType.OpenBracket)
				{
					var (method, end) = ParseExpression(tokens, index + 1);

					index = end;

					node.Operands.Add(new Operand(method, node.Operands.Count));
				}
				else if (token.TokenType == TokenType.Greater)
				{
					index++; // skip to next token, which should be label
					node.ResultType = tokens[index].Value;
					index++; // skip to next token, which should be less
				}
				else
				{
					throw new Exception($"parsing error {token}");
				}
			}

			throw new Exception($"parsing error incomplete");
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
					index++; // skip open param
				}
				else if (token.TokenType == TokenType.Word && method.MethodName != null)
				{
					// peak ahead
					var next = tokens[index];

					if (next.TokenType == TokenType.OpenParens)
					{
						var innerNode = ParseExpression(tokens, index - 1);

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

			throw new Exception($"parsing error unexpected end");
		}
	}
}
