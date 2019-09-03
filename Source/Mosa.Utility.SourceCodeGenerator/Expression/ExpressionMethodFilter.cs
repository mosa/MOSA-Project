// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Utility.SourceCodeGenerator.Expression
{
	public sealed class ExpressionMethodFilter
	{
		public string MethodName { get; private set; }

		public bool IsNegated { get; private set; } = false;

		public List<ExpressionOperand> Parameters { get; } = new List<ExpressionOperand>();

		public static List<ExpressionMethodFilter> ParseAll(List<Token> tokens)
		{
			var methods = new List<ExpressionMethodFilter>();

			int length = tokens.Count;
			int index = 0;

			while (index < length)
			{
				var (method, end) = Parse(tokens, index);

				index = end;
				methods.Add(method);
			}

			return methods;
		}

		protected static ExpressionMethodFilter Parse(List<Token> tokens)
		{
			return Parse(tokens, 0).method;
		}

		protected static (ExpressionMethodFilter method, int end) Parse(List<Token> tokens, int start)
		{
			var filter = new ExpressionMethodFilter();
			var length = tokens.Count;

			// skip ANDs
			while (start != length && tokens[start].TokenType == TokenType.And)
			{
				start++;
			}

			int index = start;
			for (; index < length;)
			{
				var token = tokens[index++];

				if (token.TokenType == TokenType.Not && filter.MethodName == null)
				{
					filter.IsNegated = true;
				}
				else if (token.TokenType == TokenType.Word && filter.MethodName == null)
				{
					filter.MethodName = token.Value;
					index++; // skip open paran
				}
				else if (token.TokenType == TokenType.Word && filter.MethodName != null)
				{
					// peak ahead
					var next = tokens[index];

					if (next.TokenType == TokenType.OpenParens)
					{
						var innerNode = Parse(tokens, index + 2);

						index = innerNode.end;

						filter.Parameters.Add(new ExpressionOperand(innerNode.method, filter.Parameters.Count));
					}
					else
					{
						filter.Parameters.Add(new ExpressionOperand(new Token(TokenType.Label, token.Position, token.Value), filter.Parameters.Count));
					}
				}
				else if (token.TokenType == TokenType.IntegerConstant)
				{
					filter.Parameters.Add(new ExpressionOperand(token, filter.Parameters.Count));
				}
				else if (token.TokenType == TokenType.LongConstant)
				{
					filter.Parameters.Add(new ExpressionOperand(token, filter.Parameters.Count));
				}
				else if (token.TokenType == TokenType.DoubleConstant)
				{
					filter.Parameters.Add(new ExpressionOperand(token, filter.Parameters.Count));
				}
				else if (token.TokenType == TokenType.FloatConstant)
				{
					filter.Parameters.Add(new ExpressionOperand(token, filter.Parameters.Count));
				}
				else if (token.TokenType == TokenType.Comma)
				{
					// skip
				}
				else if (token.TokenType == TokenType.CloseParens)
				{
					return (filter, index);
				}
				else
				{
					throw new System.Exception($"parsing error {token}");
				}
			}

			return (filter, index);
		}
	}
}
