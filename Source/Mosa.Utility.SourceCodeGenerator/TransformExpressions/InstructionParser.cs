// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;

namespace Mosa.Utility.SourceCodeGenerator.TransformExpressions
{
	public static class InstructionParser
	{
		public static InstructionNode Parse(List<Token> tokens)
		{
			if (tokens[0].TokenType != TokenType.OpenParens)
			{
				tokens.Insert(0, new Token(TokenType.OpenParens, 0));
				tokens.Add(new Token(TokenType.CloseParens, 0));
			}
			return ParseInstructionNode(tokens, 0, 0).node;
		}

		private static (InstructionNode node, int end, int nodeNbr) ParseInstructionNode(List<Token> tokens, int start, int nodeNbr)
		{
			var node = new InstructionNode() { NodeNbr = nodeNbr };
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
						var childNode = ParseInstructionNode(tokens, index, nodeNbr + 1);

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
				else if (token.TokenType == TokenType.OpenCurly)
				{
					index++; // skip to next token, which should be label

					for (; ; index++)
					{
						var t = tokens[index].TokenType;

						if (t == TokenType.CloseCurly)
							break;

						if (node.Condition == TokenType.Always)
						{
							node.Condition = t;
						}
						else if (t == TokenType.Equal)
						{
							if (node.Condition == TokenType.Greater)
								node.Condition = TokenType.GreaterEqual;
							else if (node.Condition == TokenType.Less)
								node.Condition = TokenType.LessEqual;
							else if (node.Condition == TokenType.Not)
								node.Condition = TokenType.NotEqual;
						}
					}
				}
				else
				{
					throw new Exception($"parsing error {token}");
				}
			}

			throw new Exception($"parsing error incomplete");
		}
	}
}
