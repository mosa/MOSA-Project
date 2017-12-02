// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Expression
{
	public class Tokenizer
	{
		public string Expression { get; protected set; }

		protected int Index { get; set; } = 0;

		public List<Token> Tokens { get; protected set; } = new List<Token>();

		public bool HasError { get; set; } = false;

		internal static List<KeyValuePair<string, TokenType>> operands = new List<KeyValuePair<string, TokenType>>()
		{
			new KeyValuePair<string, TokenType>("(", TokenType.OpenParens),
			new KeyValuePair<string, TokenType>(")", TokenType.CloseParens),
			new KeyValuePair<string, TokenType>("->", TokenType.Transform),
			new KeyValuePair<string, TokenType>("!=", TokenType.CompareNotEqual),
			new KeyValuePair<string, TokenType>("==", TokenType.CompareEqual),
			new KeyValuePair<string, TokenType>(">=", TokenType.CompareGreaterThanOrEqual),
			new KeyValuePair<string, TokenType>("<=", TokenType.CompareLessThanOrEqual),
			new KeyValuePair<string, TokenType>("&&", TokenType.And),
			new KeyValuePair<string, TokenType>("||", TokenType.Or),
			new KeyValuePair<string, TokenType>("<", TokenType.CompareLessThan),
			new KeyValuePair<string, TokenType>(">", TokenType.CompareGreaterThan),
			new KeyValuePair<string, TokenType>(",", TokenType.Comma),
			new KeyValuePair<string, TokenType>("!", TokenType.Not),
			new KeyValuePair<string, TokenType>("+", TokenType.Addition),
			new KeyValuePair<string, TokenType>("-", TokenType.Subtract),
			new KeyValuePair<string, TokenType>("*", TokenType.Multiplication),
			new KeyValuePair<string, TokenType>("/", TokenType.Division),
			new KeyValuePair<string, TokenType>("%", TokenType.Modulus),
			new KeyValuePair<string, TokenType>(",", TokenType.Comma),
			new KeyValuePair<string, TokenType>("?", TokenType.Questionmark),
			new KeyValuePair<string, TokenType>(":", TokenType.Colon),
			new KeyValuePair<string, TokenType>("_", TokenType.Underscore),
			new KeyValuePair<string, TokenType>("[", TokenType.OpenBracket),
			new KeyValuePair<string, TokenType>("]", TokenType.CloseBracket),
		};

		public Tokenizer(string expression)
		{
			Expression = expression;
			Parse();
		}

		private void Parse()
		{
			while (Index < Expression.Length && !HasError)
			{
				char c = Expression[Index];
				char n = Index + 1 < Expression.Length ? Expression[Index + 1] : ' ';

				if (c == ' ') // whitespace
				{
					Index++;
					continue;
				}

				if ((c >= '0' && c <= '9') || (c == '-' && n >= '0' && n <= '9'))
				{
					ExtractNumber();
					continue;
				}

				if (ExtractOperand())
					continue;

				if ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
				{
					ExtractIdentifier();
					continue;
				}

				HasError = true; // AddError("error at " + Index.ToString() + ": syntax error");
			}
		}

		private bool ExtractOperand()
		{
			foreach (var op in operands)
			{
				if (Match(op.Key, op.Value))
				{
					return true;
				}
			}

			return false;
		}

		private bool Match(string symbol, TokenType tokenType)
		{
			if (Index + symbol.Length > Expression.Length)
				return false;

			var part = Expression.Substring(Index, symbol.Length);

			if (symbol != part)
				return false;

			Index += symbol.Length;

			Tokens.Add(new Token(tokenType));

			return true;
		}

		private void ExtractNumber()
		{
			bool decimalsymbol = false;
			int start = Index;

			while (Index < Expression.Length)
			{
				char c = Expression[Index];

				if (c == '-')
				{
					Index++;
					continue;
				}

				if (c == '.')
				{
					if (decimalsymbol)
					{
						// AddError("error at " + Index.ToString() + ": too many decimal characters");
						HasError = true;
						return;
					}

					decimalsymbol = true;
					Index++;
					continue;
				}

				if (c >= '0' && c <= '9')
				{
					Index++;
					continue;
				}

				break;
			}

			var value = Expression.Substring(start, Index - start);

			if (decimalsymbol)
				Tokens.Add(new Token(TokenType.FloatConstant, value, Index));
			else
				Tokens.Add(new Token(TokenType.IntegerConstant, value, Index));
		}

		private void ExtractIdentifier()
		{
			int start = Index;

			while (Index < Expression.Length)
			{
				char c = Expression[Index];

				if ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || (c >= '0' && c <= '9') || (c == '.'))
				{
					Index++;
					continue;
				}

				break;
			}

			var value = Expression.Substring(start, Index - start);

			// special case for true/false
			if (value == "true")
			{
				Tokens.Add(new Token(TokenType.BooleanTrueConstant));
			}
			else if (value == "false")
			{
				Tokens.Add(new Token(TokenType.BooleanFalseConstant));
			}

			Tokens.Add(new Token(TokenType.Identifier, value, Index));
		}

		public override string ToString()
		{
			return Expression;
		}
	}
}
