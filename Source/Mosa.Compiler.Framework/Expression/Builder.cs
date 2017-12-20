// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Mosa.Compiler.Framework.Expression
{
	public class Builder
	{
		protected Dictionary<string, BaseInstruction> instructionMap = new Dictionary<string, BaseInstruction>();
		protected Dictionary<string, PhysicalRegister> physicalRegisterMap = new Dictionary<string, PhysicalRegister>();

		public Builder()
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

			int end = tokenized.Tokens.Count - 1;

			int transformPosition = tokenized.FindFirst(TokenType.Transform);
			if (transformPosition != -1)
				end = transformPosition - 1;

			int andPosition = tokenized.FindFirst(TokenType.And);
			if (andPosition != -1 && andPosition < end)
				end = andPosition;

			var tokens = tokenized.GetPart(1, end);
			int at = 0;

			var match = StartParse(tokens, ref at);
			var transform = null as Node;

			var tree = new TransformRule(match, transform);

			return tree;
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
					//
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
