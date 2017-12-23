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

		public void AddPhysicalRegisters(PhysicalRegister[] registers)
		{
			foreach (var entry in registers)
			{
				physicalRegisterMap.Add(entry.ToString(), entry);
			}
		}

		public TransformRule CreateExpressionTree(string expression)
		{
			var tokenized = new Tokenizer(expression);

			int transformEnd = tokenized.Tokens.Count - 1;
			int matchEnd = tokenized.Tokens.Count - 1;

			int transformPosition = tokenized.FindFirst(TokenType.Transform);
			int andPosition = tokenized.FindFirst(TokenType.And);

			if (transformPosition != -1)
				matchEnd = transformPosition - 1;

			if (andPosition != -1 && andPosition < matchEnd)
				matchEnd = andPosition;

			var matchTokens = tokenized.GetPart(1, matchEnd);
			var transformTokens = tokenized.GetPart(transformPosition + 1, transformEnd);

			var match = StartParse(matchTokens);
			var transform = StartParse(transformTokens);

			var tree = new TransformRule(match, transform);

			return tree;
		}

		protected Node StartParse(List<Token> tokens)
		{
			int at = 0;
			return StartParse(tokens, ref at);
		}

		protected Node StartParse(List<Token> tokens, ref int at)
		{
			var word = tokens[at];

			if (word.TokenType == TokenType.InstructionName)
			{
				return ParseInstructionNode(tokens, ref at);
			}

			return null; // error
		}

		protected Node Parse(List<Token> tokens, ref int at)
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

				var node = new Node(NodeType.ConstantVariable, next.Value);

				// next token should be a close paran
				var next2 = tokens[++at];

				if (next2.TokenType != TokenType.CloseParens)
					return null; // error

				at++;
				return node;
			}

			return null; // error
		}

		protected Node ParseInstructionNode(List<Token> tokens, ref int at)
		{
			var word = tokens[at++];

			var instruction = instructionMap[word.Value];

			var node = new Node(instruction);

			while (at < tokens.Count)
			{
				var token = tokens[at++];

				if (token.TokenType == TokenType.CloseParens)
				{
					return node;
				}

				Node parentNode = null;

				if (token.TokenType == TokenType.OpenParens)
				{
					parentNode = Parse(tokens, ref at);

					if (parentNode == null)
						return null;
				}
				else if (token.TokenType == TokenType.SignedIntegerConstant)
				{
					parentNode = new Node((long)token.Integer);
				}
				else if (token.TokenType == TokenType.UnsignedIntegerConstant)
				{
					parentNode = new Node((ulong)token.Integer);
				}
				else if (token.TokenType == TokenType.DoubleConstant)
				{
					parentNode = new Node((long)token.Double);
				}
				else if (token.TokenType == TokenType.OpenBracket)
				{
					var bracketedTokens = new List<Token>();

					for (; at < tokens.Count; at++)
					{
						if (tokens[at].TokenType == TokenType.CloseBracket)
						{
							break;
						}

						bracketedTokens.Add(tokens[at]);
					}

					if (tokens.Count <= at)
					{
						throw new CompilerException("Invalid expression: missing closing bracket");
					}

					var expressionNode = ExpressionParser.Parse(bracketedTokens);

					parentNode = new Node(expressionNode);
				}
				else if (token.TokenType == TokenType.OperandVariable)
				{
					if (physicalRegisterMap.TryGetValue(token.Value, out PhysicalRegister physicalRegister))
					{
						parentNode = new Node(physicalRegister);
					}
					else if (token.Value[0] == 'v' && token.Value.Length > 1)
					{
						parentNode = new Node(NodeType.VirtualRegister, token.Value);
					}
					else
					{
						parentNode = new Node(NodeType.OperandVariable, token.Value);
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
