// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;
using System;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Expression
{
	public enum ParseType { Expression, Instructions };

	public sealed class Tokenizer
	{
		private readonly List<Token> Tokens = new List<Token>();
		private readonly string Expression;
		private readonly ParseType ParseType;
		private int Index;

		private static readonly List<KeyValuePair<string, TokenType>> operands = new List<KeyValuePair<string, TokenType>>()
		{
			new KeyValuePair<string, TokenType>("(", TokenType.OpenParens),
			new KeyValuePair<string, TokenType>(")", TokenType.CloseParens),

			//new KeyValuePair<string, TokenType>("->", TokenType.Transform),
			new KeyValuePair<string, TokenType>("!=", TokenType.CompareNotEqual),
			new KeyValuePair<string, TokenType>("==", TokenType.CompareEqual),
			new KeyValuePair<string, TokenType>(">=", TokenType.CompareGreaterThanOrEqual),
			new KeyValuePair<string, TokenType>("<=", TokenType.CompareLessThanOrEqual),
			new KeyValuePair<string, TokenType>("&&", TokenType.And),
			new KeyValuePair<string, TokenType>("||", TokenType.Or),
			new KeyValuePair<string, TokenType>(">>", TokenType.ShiftRight),
			new KeyValuePair<string, TokenType>("<<", TokenType.ShiftLeft),
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
			new KeyValuePair<string, TokenType>(".", TokenType.Period),
			new KeyValuePair<string, TokenType>("#", TokenType.Hash),
			new KeyValuePair<string, TokenType>("[", TokenType.OpenBracket),
			new KeyValuePair<string, TokenType>("]", TokenType.CloseBracket),
		};

		public static List<Token> Parse(string expression, ParseType parseType)
		{
			var tokenizer = new Tokenizer(expression, parseType);

			return tokenizer.Tokens;
		}

		private Tokenizer(string expression, ParseType parseType)
		{
			Expression = expression;
			ParseType = parseType;
			Index = 0;

			Parse();
			Lower();
		}

		private void Parse()
		{
			while (Index < Expression.Length)
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

				throw new CompilerException("ExpressionEvaluation: tokenizer: error at " + Index.ToString() + ": syntax error");
			}
		}

		private Token Peek(int index, int offset = 0)
		{
			int i = index + offset;

			if (i >= Tokens.Count || i < 0)
				return Token.Unknown;

			return Tokens[i];
		}

		/// <summary>
		/// Lowers tokens to more specific types of tokens
		/// </summary>
		private void Lower()
		{
			for (int i = 0; i < Tokens.Count; i++)
			{
				var token = Tokens[i];

				if (token.TokenType == TokenType.Identifier && string.Equals(token.Value, "const", StringComparison.OrdinalIgnoreCase))
				{
					Tokens[i] = new Token(TokenType.ConstLiteral, token.Value);
				}
			}

			var parseType = ParseType;

			for (int i = 0; i < Tokens.Count; i++)
			{
				var current = Tokens[i].TokenType;

				if (current == TokenType.Null)
					continue;

				if (current == TokenType.OpenBracket)
				{
					parseType = ParseType.Expression;
					continue;
				}
				else if (current == TokenType.CloseBracket)
				{
					parseType = ParseType;
					continue;
				}

				var prev = Peek(i, -1).TokenType;
				var next = Peek(i, 1).TokenType;
				var next2 = Peek(i, 2).TokenType;

				if (parseType == ParseType.Expression)
				{
					if (current == TokenType.Identifier && next == TokenType.Period && next2 == TokenType.Identifier)
					{
						// x.IsRegister
						Tokens[i] = new Token(TokenType.OperandVariable, Tokens[i].Value);
					}
					else if (current == TokenType.Identifier && next == TokenType.OpenParens)
					{
						// IsPowerOfTwo(
						Tokens[i] = new Token(TokenType.Method, Tokens[i].Value);
					}
					else if (current == TokenType.Identifier && next == TokenType.Colon && next2 == TokenType.Identifier)
					{
						// Math:IsPowerOfTwo(
						Tokens[i] = new Token(TokenType.Method, Tokens[i].Value + ':' + Tokens[i + 2].Value);
						Tokens[i + 1] = new Token(TokenType.Null);
						Tokens[i + 2] = new Token(TokenType.Null);
					}
					else if (current == TokenType.Identifier)
					{
						// x
						Tokens[i] = new Token(TokenType.OperandVariable, Tokens[i].Value);
					}
				}
				else if (parseType == ParseType.Instructions)
				{
					if (prev == TokenType.OpenParens && current == TokenType.Identifier)
					{
						Tokens[i] = new Token(TokenType.Instruction, Tokens[i].Value);
					}
					else if (prev == TokenType.Hash && current == TokenType.Identifier)
					{
						Tokens[i - 1] = new Token(TokenType.Null);
						Tokens[i] = new Token(TokenType.PhysicalRegister, Tokens[i].Value);
					}
					else if (prev == TokenType.CompareLessThan && current == TokenType.Identifier && prev == TokenType.CompareGreaterThan)
					{
						Tokens[i - 1] = new Token(TokenType.Null);
						Tokens[i] = new Token(TokenType.TypeVariable, Tokens[i + 1].Value);
						Tokens[i + 1] = new Token(TokenType.Null);
					}
					else if (prev == TokenType.Instruction && current == TokenType.Period && next == TokenType.Identifier)
					{
						Tokens[i - 1] = new Token(TokenType.Instruction, Tokens[i - 1].Value + '.' + Tokens[i + 1].Value);
						Tokens[i] = new Token(TokenType.Null);
						Tokens[i + 1] = new Token(TokenType.Null);
					}
					else if (current == TokenType.Identifier)
					{
						Tokens[i] = new Token(TokenType.OperandVariable, Tokens[i].Value);
					}
				}
			}

			for (int i = Tokens.Count - 1; i >= 0; i--)
			{
				if (Tokens[i].TokenType == TokenType.Null)
				{
					Tokens.RemoveAt(i);
				}
			}
		}

		private bool ExtractOperand()
		{
			foreach (var op in operands)
			{
				if (Match(op.Key, op.Value))
					return true;
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
			bool hex = false;
			int start = Index;

			while (Index < Expression.Length)
			{
				char c = Expression[Index];

				if (c == '-')
				{
					Index++;
					continue;
				}

				if (c == 'x')
				{
					Index++;
					hex = true;
					continue;
				}

				if (c == '.')
				{
					if (decimalsymbol)
					{
						throw new CompilerException("ExpressionEvaluation: tokenizer: error at " + Index.ToString() + ": too many decimal characters");
					}

					decimalsymbol = true;
					Index++;
					continue;
				}

				if ((c >= '0' && c <= '9') || (c == 'd' || c == 'u'))
				{
					Index++;
					continue;
				}

				break;
			}

			var value = Expression.Substring(start, Index - start);

			Tokens.Add(new Token(value, Index));
		}

		private void ExtractIdentifier()
		{
			int start = Index;

			while (Index < Expression.Length)
			{
				char c = Expression[Index];

				if ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || (c >= '0' && c <= '9'))
				{
					Index++;
					continue;
				}

				break;
			}

			var value = Expression.Substring(start, Index - start);

			// special case for true/false/null
			var special = value.ToLower();

			if (special == "true")
			{
				Tokens.Add(new Token(1u, Index));
				return;
			}
			else if (special == "false" || special == "null")
			{
				Tokens.Add(new Token((ulong)0, Index));
				return;
			}

			Tokens.Add(new Token(TokenType.Identifier, value, Index));
		}

		public override string ToString()
		{
			return Expression;
		}
	}
}
