// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Expression
{
	public sealed class NodeParser
	{
		private Node Root;
		private SymbolDictionary Map;
		private int Index = 0;

		public static Node Parse(List<Token> tokens, SymbolDictionary map)
		{
			if (tokens.Count == 0)
				return null;

			var parse = new NodeParser(tokens, map);

			return parse.Root;
		}

		private NodeParser(List<Token> tokens, SymbolDictionary map)
		{
			Map = map;

			if (tokens.Count != 0)
			{
				Root = ParseOperand(tokens);
			}
		}

		private Node ParseOperand(List<Token> tokens)
		{
			var token = tokens[Index++];

			if (token.TokenType == TokenType.CloseParens)
			{
				return null;
			}

			if (token.TokenType == TokenType.OpenParens)
			{
				return ParseNewExpression(tokens);
			}
			else if (token.TokenType == TokenType.SignedIntegerConstant)
			{
				return new Node((long)token.Integer);
			}
			else if (token.TokenType == TokenType.UnsignedIntegerConstant)
			{
				return new Node((ulong)token.Integer);
			}
			else if (token.TokenType == TokenType.DoubleConstant)
			{
				return new Node((long)token.Double);
			}
			else if (token.TokenType == TokenType.OpenBracket)
			{
				return ParseBracketExpression(tokens);
			}
			else if (token.TokenType == TokenType.PhysicalRegister)
			{
				var physicalRegister = Map.GetPhysicalRegister(token.Value);

				if (physicalRegister != null)
				{
					return new Node(physicalRegister);
				}

				throw new CompilerException("ExpressionEvaluation: Invalid parse: error at " + token.Position.ToString() + " unknown register name: " + token.Value);
			}
			else if (token.TokenType == TokenType.VirtualRegister)
			{
				// not available with syntax yet
				return new Node(NodeType.VirtualRegister, token.Value);
			}
			else if (token.TokenType == TokenType.OperandVariable)
			{
				return new Node(NodeType.OperandVariable, token.Value);
			}

			throw new CompilerException("ExpressionEvaluation: Invalid parse: error at " + token.Position.ToString() + " unexpected token: " + token);
		}

		private Node ParseNewExpression(List<Token> tokens)
		{
			var token = tokens[Index];

			if (token.TokenType == TokenType.InstructionName)
			{
				return ParseInstructionNode(tokens);
			}
			else if (token.TokenType == TokenType.ConstLiteral)
			{
				// the next token should be a variable
				var next = tokens[++Index];

				if (next.TokenType != TokenType.OperandVariable)
					return null; // error

				var node = new Node(NodeType.ConstantVariable, next.Value);

				// next token should be a close parentheses
				var next2 = tokens[++Index];

				if (next2.TokenType != TokenType.CloseParens)
					return null; // error

				Index++;
				return node;
			}

			throw new CompilerException("ExpressionEvaluation: Invalid parse: error at " + token.Position.ToString() + " unexpected token: " + token);
		}

		private Node ParseInstructionNode(List<Token> tokens)
		{
			var token = tokens[Index++];

			var instruction = Map.GetInstruction(token.Value);

			var node = new Node(instruction);

			while (Index < tokens.Count)
			{
				var operand = ParseOperand(tokens);

				if (operand == null)
					return node;

				node.AddNode(operand);
			}

			throw new CompilerException("ExpressionEvaluation: Invalid parse: error at " + token.Position.ToString() + " unexpected token: " + token);
		}

		private Node ParseBracketExpression(List<Token> tokens)
		{
			var bracketedTokens = new List<Token>();

			for (; Index < tokens.Count; Index++)
			{
				if (tokens[Index].TokenType == TokenType.CloseBracket)
				{
					break;
				}

				bracketedTokens.Add(tokens[Index]);
			}

			var expressionNode = ExpressionParser.Parse(bracketedTokens);

			return new Node(expressionNode);
		}
	}
}
