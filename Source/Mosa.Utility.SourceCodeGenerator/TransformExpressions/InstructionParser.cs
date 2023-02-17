// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;

namespace Mosa.Utility.SourceCodeGenerator.TransformExpressions;

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
		var node = new InstructionNode { NodeNbr = nodeNbr };
		var length = tokens.Count;

		for (var index = start; index < length; index++)
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
				node.Conditions = InstructionParser.ParseConditions(tokens, ref index);
				node.Condition = node.Conditions[0];
			}
			else
			{
				throw new Exception($"parsing error {token}");
			}
		}

		throw new Exception($"parsing error incomplete");
	}

	public static List<ConditionCode> ParseConditions(List<Token> tokens, ref int index)
	{
		var conditions = new List<ConditionCode>();

		while (true)
		{
			var condition = ParseCondition(tokens, ref index);

			conditions.Add(condition);

			if (tokens[index].TokenType == TokenType.Comma)
			{
				index++;
				continue;
			}

			break;
		}

		if (conditions.Count == 0)
		{
			conditions.Add(ConditionCode.Always);
		}

		return conditions;
	}

	public static ConditionCode ParseCondition(List<Token> tokens, ref int index)
	{
		var condition = ConditionCode.Always;

		for (; ; index++)
		{
			var t = tokens[index].TokenType;

			if (t == TokenType.Comma)
				return condition;

			if (t == TokenType.CloseCurly)
				break;

			if (condition == ConditionCode.Always)
			{
				condition = t switch
				{
					TokenType.Greater => ConditionCode.Greater,
					TokenType.Less => ConditionCode.Less,
					TokenType.Not => ConditionCode.NotEqual,
					TokenType.Equal => ConditionCode.Equal,
					_ => condition
				};
			}
			else if (t == TokenType.Equal)
			{
				condition = condition switch
				{
					ConditionCode.Greater => ConditionCode.GreaterOrEqual,
					ConditionCode.Less => ConditionCode.LessOrEqual,
					_ => condition
				};
			}
			else if (t == TokenType.Word)
			{
				var text = tokens[index].Value.ToLowerInvariant();

				if (text == "u")
				{
					condition = condition switch
					{
						ConditionCode.Greater => ConditionCode.UnsignedGreater,
						ConditionCode.Less => ConditionCode.UnsignedLess,
						ConditionCode.GreaterOrEqual => ConditionCode.UnsignedGreaterOrEqual,
						ConditionCode.LessOrEqual => ConditionCode.UnsignedLessOrEqual,
						_ => condition
					};
				}

				//else if (text == "set-equal")
				//	return ConditionCode.SetWithEqual;
				//else if (text == "set-noequal")
				//	return ConditionCode.SetWithNoEqual;
			}
		}

		return condition;
	}
}
