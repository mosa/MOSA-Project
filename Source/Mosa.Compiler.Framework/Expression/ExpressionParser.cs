// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Expression
{
	public class ExpressionParser
	{
		protected List<Token> Tokens { get; }
		protected Token CurrentToken { get { return Tokens[Index]; } }
		protected TokenType CurrentTokenType { get { return CurrentToken.TokenType; } }
		protected bool IsOutOfTokens { get { return Index >= Tokens.Count; } }
		protected int Index = 0;

		protected ExpressionNode Root { get; private set; }

		public static ExpressionNode Parse(List<Token> tokens)
		{
			var parse = new ExpressionParser(tokens);

			return parse.Root;
		}

		protected ExpressionParser(List<Token> tokens)
		{
			Tokens = tokens ?? throw new CompilerException("tokens parameter is null");

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
				//if (HasError) return null;

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
				//if (HasError) return null;

				if (IsOutOfTokens) return lhs;

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
				//if (HasError) return null;

				if (IsOutOfTokens)
				{
					throw new CompilerException("Invalid parse: parser unexpected end");
				}

				if (CurrentTokenType == TokenType.Not)
				{
					var token = CurrentToken;
					Index++;

					var rhs = ParseUnary();

					var node = new ExpressionNode(token, rhs);
					return node;
				}

				if (CurrentTokenType == TokenType.OperandVariable)
				{
					var variableToken = new Token(CurrentTokenType, CurrentToken.Value, Index);
					Index++;

					var node = new ExpressionNode(variableToken);
					return node;
				}
				else if (CurrentTokenType == TokenType.Method || CurrentTokenType == TokenType.If)
				{
					var token = new Token(CurrentTokenType, CurrentToken.Value, Index);
					Index++;

					Index++; // skip opening paraens

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

					Index++; // skip closing parens

					var node = new ExpressionNode(token, parameters);
					return node;
				}

				//else if (CurrentTokenType == TokenType.Questionmark)
				//{
				//	var questionToken = new Token(TokenType.If, null, Index);
				//	Index++;

				//	var trueExpression = ParseAddSub();

				//	if (CurrentToken.TokenType != TokenType.Colon)
				//	{
				//		ErrorMessage = "error at " + CurrentToken.Index.ToString() + " missing colon";
				//		return null;
				//	}

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
			if (IsLiteral(CurrentToken.TokenType))
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
					throw new CompilerException("Invalid parse: error at " + CurrentToken.Index.ToString() + " missing closing parenthesis");

				Index++;

				return node;
			}

			if (CurrentTokenType == TokenType.OperandVariable)
			{
				var node = new ExpressionNode(CurrentToken);
				Index++;
				return node;
			}

			throw new CompilerException("Invalid parse: error at " + CurrentToken.Index.ToString() + " unexpected token: " + CurrentToken);
		}

		protected static bool IsLiteral(TokenType tokenType)
		{
			return tokenType == TokenType.SignedIntegerConstant
					|| tokenType == TokenType.UnsignedIntegerConstant
					|| tokenType == TokenType.DoubleConstant;
		}

		protected static bool IsAddSubOperand(TokenType tokenType)
		{
			return tokenType == TokenType.Addition || tokenType == TokenType.Subtract;
		}

		protected static bool IsMulDivOperand(TokenType tokenType)
		{
			return tokenType == TokenType.Multiplication || tokenType == TokenType.Division || tokenType == TokenType.Modulus;
		}

		protected static bool IsLogicalOperand(TokenType tokenType)
		{
			return tokenType == TokenType.And || tokenType == TokenType.Or;
		}

		protected static bool IsComparisonOperand(TokenType tokenType)
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
