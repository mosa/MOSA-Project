// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Expression
{
	public sealed class ExpressionParser
	{
		private List<Token> Tokens;
		private Token CurrentToken { get { return Tokens[Index]; } }
		private TokenType CurrentTokenType { get { return CurrentToken.TokenType; } }
		private bool IsOutOfTokens { get { return Index >= Tokens.Count; } }
		private int Index = 0;

		private ExpressionNode Root;

		public static ExpressionNode Parse(List<Token> tokens)
		{
			if (tokens.Count == 0)
				return null;

			var parse = new ExpressionParser(tokens);

			return parse.Root;
		}

		private ExpressionParser(List<Token> tokens)
		{
			Tokens = tokens ?? throw new CompilerException("ExpressionEvaluation: tokens parameter is null");

			Parse();
		}

		private void Parse()
		{
			Root = ParseAddSub();
		}

		private ExpressionNode ParseAddSub()
		{
			var lhs = ParseMulDiv();

			while (true)
			{
				if (IsOutOfTokens) return lhs;

				Token op = null;

				if (IsAddSubOperand(CurrentToken.TokenType))
				{
					op = CurrentToken;
					Index++;
				}
				else if (IsLogicalOperand(CurrentToken.TokenType))
				{
					op = CurrentToken;
					Index++;
				}
				else
				{
					return lhs;
				}

				var rhs = ParseMulDiv();

				lhs = new ExpressionNode(op, lhs, rhs);
			}
		}

		private ExpressionNode ParseMulDiv()
		{
			var lhs = ParseUnary();

			while (true)
			{
				if (IsOutOfTokens)
					return lhs;

				Token op = null;

				if (IsMulDivOperand(CurrentToken.TokenType))
				{
					op = CurrentToken;
					Index++;
				}
				else if (IsComparisonOperand(CurrentToken.TokenType))
				{
					op = CurrentToken;
					Index++;
				}

				//else if (CurrentToken.TokenType == TokenType.Equal)
				//{
				//	op = CurrentToken;
				//	Index++;
				//}
				else
				{
					return lhs;
				}

				var rhs = ParseUnary();

				lhs = new ExpressionNode(op, lhs, rhs);
			}
		}

		private ExpressionNode ParseUnary()
		{
			while (true)
			{
				if (IsOutOfTokens)
				{
					throw new CompilerException("ExpressionEvaluation: Invalid parse: parser unexpected end");
				}

				if (CurrentTokenType == TokenType.Not)
				{
					var token = CurrentToken;
					Index++;

					var rhs = ParseUnary();

					return new ExpressionNode(token, rhs);
				}

				if (CurrentTokenType == TokenType.OperandVariable)
				{
					var variableToken = new Token(CurrentTokenType, CurrentToken.Value, Index++);

					return new ExpressionNode(variableToken);
				}
				else if (CurrentTokenType == TokenType.Method || CurrentTokenType == TokenType.If)
				{
					var token = new Token(CurrentTokenType, CurrentToken.Value, Index++);

					Index++; // skip opening parentheses

					var parameters = new List<ExpressionNode>();

					while (CurrentTokenType != TokenType.CloseParens)
					{
						if (CurrentTokenType == TokenType.Comma)
						{
							Index++;
							continue;
						}

						var parameter = ParseAddSub();

						parameters.Add(parameter);
					}

					Index++; // skip closing parentheses

					return new ExpressionNode(token, parameters);
				}

				//else if (CurrentTokenType == TokenType.Questionmark)
				//{
				//	var questionToken = new Token(TokenType.If, null, Index++);

				//	var trueExpression = ParseAddSub();

				//	if (CurrentToken.TokenType != TokenType.Colon)
				//		throw new CompilerException("ExpressionEvaluation: Invalid parse: error at " + CurrentToken.Index.ToString() + " missing colon");

				//	Index++; // skip closing colon

				//	var falseExpression = ParseAddSub();

				//	var parameters = new List<ExpressionNode>
				//	{
				//		null, // todo
				//		trueExpression,
				//		falseExpression
				//	};

				//	var node = new ExpressionNode(questionToken, parameters);
				//	return node;
				//}

				return ParseLeaf();
			}
		}

		private ExpressionNode ParseLeaf()
		{
			if (IsConstant(CurrentToken.TokenType))
			{
				var node = new ExpressionNode(CurrentToken);
				Index++;
				return node;
			}

			if (CurrentToken.TokenType == TokenType.OpenParens)
			{
				Index++;

				var node = ParseAddSub();

				if (CurrentToken.TokenType != TokenType.CloseParens)
				{
					throw new CompilerException("ExpressionEvaluation: Invalid parse: error at " + CurrentToken.Position.ToString() + " missing closing parenthesis");
				}

				Index++;

				return node;
			}

			if (CurrentTokenType == TokenType.OperandVariable)
			{
				var node = new ExpressionNode(CurrentToken);
				Index++;
				return node;
			}

			throw new CompilerException("ExpressionEvaluation: Invalid parse: error at " + CurrentToken.Position.ToString() + " unexpected token: " + CurrentToken);
		}

		private static bool IsConstant(TokenType tokenType)
		{
			return tokenType == TokenType.SignedIntegerConstant
					|| tokenType == TokenType.UnsignedIntegerConstant
					|| tokenType == TokenType.DoubleConstant;
		}

		private static bool IsAddSubOperand(TokenType tokenType)
		{
			return tokenType == TokenType.Addition || tokenType == TokenType.Subtract;
		}

		private static bool IsMulDivOperand(TokenType tokenType)
		{
			return tokenType == TokenType.Multiplication || tokenType == TokenType.Division || tokenType == TokenType.Modulus;
		}

		private static bool IsLogicalOperand(TokenType tokenType)
		{
			return tokenType == TokenType.And || tokenType == TokenType.Or;
		}

		private static bool IsComparisonOperand(TokenType tokenType)
		{
			return tokenType == TokenType.CompareEqual
					|| tokenType == TokenType.CompareNotEqual
					|| tokenType == TokenType.CompareGreaterThanOrEqual
					|| tokenType == TokenType.CompareLessThanOrEqual
					|| tokenType == TokenType.CompareLessThan
					|| tokenType == TokenType.CompareGreaterThan;
		}
	}
}
