// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;
using System;
using System.Collections.Generic;

namespace Mosa.Utility.SourceCodeGenerator.TransformExpressions
{
	public static class Tokenizer
	{
		public static List<Token> Parse(string expression)
		{
			var tokens = new List<Token>();
			int length = expression.Length;

			int index = 0;

			while (index < length)
			{
				char c = expression[index];

				if (c == ' ' | c == '\t')
				{
					// whitespace
					index++;
				}
				else if (IsDigit(c) || c == '-')
				{
					// Number
					int start = index++;

					while (index < length)
					{
						c = expression[index];

						if (IsDigit(c) || c == '.' || c == 'x' || c == 'l' || c == 'f' || c == 'd' || c == 'b' || c == 'u' || c == 'i')
						{
							index++;
							continue;
						}
						break;
					}

					var part = expression.Substring(start, index - start);

					var token = GetNumberToken(part, start);

					tokens.Add(token);
				}
				else if (IsCharacter(c))
				{
					// Word
					int start = index++;

					while (index < length)
					{
						c = expression[index];

						if (IsCharacter(c) || IsDigit(c) || c == '.')
						{
							index++;
							continue;
						}
						break;
					}

					tokens.Add(new Token(TokenType.Word, start, expression.Substring(start, index - start)));
				}
				else if (c == '.')
				{
					tokens.Add(new Token(TokenType.Period, index++, "."));
				}
				else if (c == '#')
				{
					tokens.Add(new Token(TokenType.Hash, index++, "#"));
				}
				else if (c == '(')
				{
					tokens.Add(new Token(TokenType.OpenParens, index++, "("));
				}
				else if (c == ')')
				{
					tokens.Add(new Token(TokenType.CloseParens, index++, ")"));
				}
				else if (c == '[')
				{
					tokens.Add(new Token(TokenType.OpenBracket, index++, "["));
				}
				else if (c == ']')
				{
					tokens.Add(new Token(TokenType.CloseBracket, index++, "]"));
				}
				else if (c == '$')
				{
					tokens.Add(new Token(TokenType.Dollar, index++, "$"));
				}
				else if (c == '-')
				{
					tokens.Add(new Token(TokenType.Minus, index++, "-"));
				}
				else if (c == '_')
				{
					tokens.Add(new Token(TokenType.Underscore, index++, "_"));
				}
				else if (c == '>')
				{
					tokens.Add(new Token(TokenType.Greater, index++, ">"));
				}
				else if (c == '<')
				{
					tokens.Add(new Token(TokenType.Less, index++, "<"));
				}
				else if (c == '&')
				{
					tokens.Add(new Token(TokenType.And, index++, "&"));
				}
				else if (c == '|')
				{
					tokens.Add(new Token(TokenType.Or, index++, "|"));
				}
				else if (c == '!')
				{
					tokens.Add(new Token(TokenType.Not, index++, "!"));
				}
				else if (c == ',')
				{
					tokens.Add(new Token(TokenType.Comma, index++, ","));
				}
				else
				{
					throw new CompilerException($"tokensizer: syntax error at {index}");
				}
			}

			return tokens;
		}

		private static Token GetNumberToken(string part, int index)
		{
			int length = part.Length;
			char last = (length > 1) ? part[length - 1] : ' ';

			if (last == 'd' || last == 'f' || last == 'l' || last == 'i' || last == 'u')
			{
				part = part.Substring(0, length - 1);
			}

			if (last == 'd')
			{
				// double
				return new Token(TokenType.DoubleConstant, index, part, Convert.ToDouble(part));
			}
			if (last == 'f')
			{
				// float
				return new Token(TokenType.FloatConstant, index, part, Convert.ToSingle(part));
			}
			else if (part.StartsWith("0x"))
			{
				// hex
				part = part.Substring(0, part.Length - 2);

				var value = ParseHex(part);

				return (last == 'i')
					? new Token(TokenType.IntegerConstant, index, part, (uint)value)
					: new Token(TokenType.LongConstant, index, part, value);
			}
			else if (part.StartsWith("0b"))
			{
				// binary
				part = part.Substring(0, part.Length - 2);

				var value = ParseBinary(part);

				return (last == 'i')
					? new Token(TokenType.IntegerConstant, index, part, (uint)value)
					: new Token(TokenType.LongConstant, index, part, value);
			}

			// integer
			var l = long.Parse(part);

			return (last == 'i' || last == 'u')
				? new Token(TokenType.IntegerConstant, index, part, (uint)l)
				: new Token(TokenType.LongConstant, index, part, l);
		}

		public static ulong ParseBinary(string s)
		{
			ulong l = 0;

			foreach (var c in s)
			{
				l <<= 1;
				l |= c == '1' ? 1ul : 0;
			}

			return l;
		}

		public static ulong ParseHex(string s)
		{
			ulong l = 0;

			foreach (var c in s)
			{
				l <<= 4;
				l |= Char.IsLower(c) ? (byte)(c - 'a') : (byte)(c - 'A');
			}

			return l;
		}

		public static bool IsCharacter(char c)
		{
			return (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z');
		}

		public static bool IsDigit(char c)
		{
			return c >= '0' && c <= '9';
		}
	}
}
