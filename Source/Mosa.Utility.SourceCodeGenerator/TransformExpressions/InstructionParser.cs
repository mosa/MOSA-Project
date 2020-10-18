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
					node.Condition = InstructionParser.ParseCondition(tokens, ref index);
				}
				else
				{
					throw new Exception($"parsing error {token}");
				}
			}

			throw new Exception($"parsing error incomplete");
		}

		public static ConditionCode ParseCondition(List<Token> tokens, ref int index)
		{
			var condition = ConditionCode.Always;

			for (; ; index++)
			{
				var t = tokens[index].TokenType;

				if (t == TokenType.CloseCurly)
					break;

				if (condition == ConditionCode.Always)
				{
					switch (t)
					{
						case TokenType.Greater:
							condition = ConditionCode.Greater;
							break;

						case TokenType.Less:
							condition = ConditionCode.Less;
							break;

						case TokenType.Not:
							condition = ConditionCode.NotEqual;
							break;

						case TokenType.Equal:
							condition = ConditionCode.Equal;
							break;
					}
				}
				else if (t == TokenType.Equal)
				{
					switch (condition)
					{
						case ConditionCode.Greater:
							condition = ConditionCode.GreaterOrEqual;
							break;

						case ConditionCode.Less:
							condition = ConditionCode.LessOrEqual;
							break;
					}
				}
				else if (t == TokenType.Word)
				{
					var text = tokens[index].Value.ToLower();

					if (text == "u")
					{
						switch (condition)
						{
							case ConditionCode.Greater:
								condition = ConditionCode.UnsignedGreater;
								break;

							case ConditionCode.Less:
								condition = ConditionCode.UnsignedLess;
								break;

							case ConditionCode.GreaterOrEqual:
								condition = ConditionCode.UnsignedGreaterOrEqual;
								break;

							case ConditionCode.LessOrEqual:
								condition = ConditionCode.UnsignedLessOrEqual;
								break;
						}
					}
				}
			}

			return condition;
		}
	}
}
