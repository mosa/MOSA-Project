// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Mosa.Compiler.Framework.Expression
{
	public class ExpressionBuilder
	{
		protected Dictionary<string, BaseInstruction> instructionMap = new Dictionary<string, BaseInstruction>();
		protected Dictionary<string, PhysicalRegister> physicalRegisterMap = new Dictionary<string, PhysicalRegister>();

		public ExpressionBuilder()
		{
		}

		public void AddInstructions(Dictionary<string, BaseInstruction> map)
		{
			foreach (var entry in map)
			{
				instructionMap.Add(entry.Key, entry.Value);
			}
		}

		public void AddPhysicalRegisters(Dictionary<string, PhysicalRegister> map)
		{
			foreach (var entry in map)
			{
				physicalRegisterMap.Add(entry.Key, entry.Value);
			}
		}

		public TransformRule CreateExpressionTree(string expression)
		{
			var tokenized = new Tokenizer(expression);

			int end = tokenized.Expression.Length - 1;

			int transform = tokenized.FindFirst(TokenType.Transform);
			if (transform != -1)
				end = transform - 1;

			int and = tokenized.FindFirst(TokenType.And);
			if (and != -1 && and < end)
				end = and;

			var tokens = tokenized.GetPart(1, end);
			int at = 0;

			var root = Parse(tokens, ref at);

			var tree = new TransformRule(root, null);

			return tree;
		}

		protected ExpressionNode Parse(List<Token> tokens, ref int at)
		{
			var word = tokens[at];

			if (word.TokenType == TokenType.InstructionName)
			{
				return ParseInstructionNode(tokens, ref at);
			}
			else if (word.TokenType == TokenType.ConstLiteral)
			{
				// the next token should be a variable
				var next = tokens[++at];

				if (next.TokenType != TokenType.OperandVariable)
					return null; // error

				var node = new ExpressionNode(NodeType.VariableConstant, next.Value);

				// next token should be a close paran
				var next2 = tokens[++at];

				if (next2.TokenType != TokenType.CloseParens)
					return null; // error

				at++;
				return node;
			}
			else
			{
				return ParseConstantNode(tokens, ref at);
			}
		}

		protected ExpressionNode ParseInstructionNode(List<Token> tokens, ref int at)
		{
			var word = tokens[at++];

			var instruction = instructionMap[word.Value];

			var node = new ExpressionNode(instruction);

			while (at < tokens.Count)
			{
				var token = tokens[at++];

				if (token.TokenType == TokenType.CloseParens)
				{
					return node;
				}

				ExpressionNode parentNode = null;

				if (token.TokenType == TokenType.OpenParens)
				{
					parentNode = Parse(tokens, ref at);

					if (parentNode == null)
						return null;
				}
				else if (token.TokenType == TokenType.OperandVariable)
				{
					if (physicalRegisterMap.TryGetValue(token.Value, out PhysicalRegister physicalRegister))
					{
						parentNode = new ExpressionNode(physicalRegister);
					}
					else if (token.Value[0] == 'v' && token.Value.Length > 1)
					{
						parentNode = new ExpressionNode(NodeType.VirtualRegister, token.Value);
					}
					else
					{
						parentNode = new ExpressionNode(NodeType.Variable, token.Value);
					}
				}
				else
				{
					return ParseConstant(token);
				}

				node.AddNode(parentNode);
			}

			return node;
		}

		protected ExpressionNode ParseConstantNode(List<Token> tokens, ref int at)
		{
			var token = tokens[at++];

			if (token.TokenType == TokenType.CloseParens)
			{
				at++;
				return null;
			}
			else if (token.TokenType == TokenType.ConstLiteral)
			{
				return ParseConstant(tokens[at++]);
			}

			return null;
		}

		protected ExpressionNode ParseConstant(Token token)
		{
			if (token.TokenType == TokenType.IntegerConstant && token.Value[0] != '-')
			{
				if (!UInt64.TryParse(token.Value, out ulong value))
					throw new CompilerException("Invalid ulong constant: " + token.Value);

				return new ExpressionNode(value);
			}
			else if (token.TokenType == TokenType.IntegerConstant && token.Value[0] == '-')
			{
				if (!Int64.TryParse(token.Value, out long value))
					throw new CompilerException("Invalid long constant: " + token.Value);

				return new ExpressionNode((ulong)value);
			}
			else if (token.TokenType == TokenType.HexIntegerConstant && token.Value[0] != '-')
			{
				if (!UInt64.TryParse(token.Value, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out ulong value))
					throw new CompilerException("Invalid ulong hex constant: " + token.Value);

				return new ExpressionNode(value);
			}
			else if (token.TokenType == TokenType.HexIntegerConstant && token.Value[0] == '-')
			{
				if (!Int64.TryParse(token.Value, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out long value))
					throw new CompilerException("Invalid long hex constant: " + token.Value);

				return new ExpressionNode((ulong)value);
			}
			else if (token.TokenType == TokenType.FloatConstant)
			{
				if (!Double.TryParse(token.Value, out double value))
					throw new CompilerException("Invalid float constant: " + token.Value);

				return new ExpressionNode(value);
			}

			return null;
		}
	}
}
