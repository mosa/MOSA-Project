// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;
using System;
using System.Collections.Generic;

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

		public Transform CreateExpressionTree(string expression)
		{
			var tokenized = new Tokenizer(expression);

			var tokens = tokenized.Tokens;
			int at = 1; // skip

			var root = ParseInstruction(tokens, ref at, tokens.Count);

			var tree = new Transform(root, null);

			return tree;
		}

		protected ExpressionNode ParseInstruction(List<Token> tokens, ref int at, int end)
		{
			var word = tokens[at++];

			if (word.TokenType != TokenType.Identifier)
				return null;

			if (string.Equals(word.Value, "const", StringComparison.OrdinalIgnoreCase))
			{
				ExpressionNode node = null;

				while (at < end)
				{
					var token = tokens[at++];

					if (token.TokenType == TokenType.CloseParens)
					{
						at++;
						return node;
					}
					else if (token.TokenType == TokenType.IntegerConstant && token.Value[0] != '-')
					{
						if (!UInt64.TryParse(token.Value, out ulong value))
							throw new CompilerException("Invalid ulong constant: " + token.Value);

						node = new ExpressionNode(value);
					}
					else if (token.TokenType == TokenType.IntegerConstant && token.Value[0] == '-')
					{
						if (!Int64.TryParse(token.Value, out long value))
							throw new CompilerException("Invalid long constant: " + token.Value);

						node = new ExpressionNode((ulong)value);
					}
					else if (token.TokenType == TokenType.FloatConstant)
					{
						if (!Double.TryParse(token.Value, out double value))
							throw new CompilerException("Invalid floating constant: " + token.Value);

						node = new ExpressionNode(value);
					}
					else if (token.TokenType == TokenType.Identifier)
					{
						node = new ExpressionNode(NodeType.VariableConstant, token.Value);
					}
					else
					{
						return null;
					}
				}

				return null;
			}
			else
			{
				var instruction = instructionMap[word.Value];

				if (instruction == null)
					return null;

				var node = new ExpressionNode(instruction);

				while (at < end)
				{
					var token = tokens[at++];

					if (token.TokenType == TokenType.CloseParens)
					{
						return node;
					}

					ExpressionNode parentNode = null;

					if (token.TokenType == TokenType.OpenParens)
					{
						parentNode = ParseInstruction(tokens, ref at, end);

						if (parentNode == null)
							return null;
					}
					else if (token.TokenType == TokenType.IntegerConstant && token.Value[0] != '-')
					{
						if (!UInt64.TryParse(token.Value, out ulong value))
							return null;

						parentNode = new ExpressionNode(value);
					}
					else if (token.TokenType == TokenType.IntegerConstant && token.Value[0] == '-')
					{
						if (!Int64.TryParse(token.Value, out long value))
							return null;

						parentNode = new ExpressionNode((ulong)value);
					}
					else if (token.TokenType == TokenType.FloatConstant)
					{
						if (!Double.TryParse(token.Value, out double value))
							return null;

						parentNode = new ExpressionNode(value);
					}
					else if (token.TokenType == TokenType.Identifier)
					{
						if (physicalRegisterMap.TryGetValue(token.Value, out PhysicalRegister physicalRegister))
						{
							parentNode = new ExpressionNode(physicalRegister);
						}
						else
						{
							if (token.Value[0] == 'v' && token.Value.Length > 1)
							{
								parentNode = new ExpressionNode(NodeType.VirtualRegister, token.Value);
							}
							else
							{
								parentNode = new ExpressionNode(NodeType.Variable, token.Value);
							}
						}
					}
					else
					{
						return null;
					}

					node.AddNode(parentNode);
				}

				return node;
			}
		}
	}
}
