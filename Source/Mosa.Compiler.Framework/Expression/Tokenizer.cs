// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Expression
{
	public class Tokenizer
	{
		public string Expression { get; protected set; }

		protected int Index { get; set; } = 0;

		public List<Token> Tokens { get; protected set; } = new List<Token>();

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

				throw new CompilerException("tokenizer: error at " + Index.ToString() + ": syntax error");
			}

			Rewrite();
		}

		private Token Peek(int index, int offset = 0)
		{
			int i = index + offset;

			if (Tokens.Count >= i)
				return Token.Unknown;

			return Tokens[i];
		}

		private bool Match(int index, TokenType[] tokens)
		{
			for (int i = 0; i < tokens.Length; i++)
			{
				if (Peek(index, i).TokenType != tokens[index + i])
					return false;
			}

			return true;
		}

		private static readonly TokenType[] TypeVariableList = new TokenType[]
		{
			// <Identifier>  ->  TypeVariable
			TokenType.OpenBracket , TokenType.Identifier , TokenType.CloseBracket
		};

		private static readonly TokenType[] TypeMethodList = new TokenType[]
		{
			// {TypeVariable}.{Identifier}  ->  {TypeVariable}.{MethodName}
			TokenType.TypeVariable , TokenType.Period , TokenType.Identifier
		};

		private static readonly TokenType[] ClassMethodList = new TokenType[]
		{
			// {Identifier}:{Identifier}  ->  {ClassName}:{MethodName}
			TokenType.Identifier , TokenType.Colon , TokenType.Identifier
		};

		private static readonly TokenType[] MethodNameList = new TokenType[]
		{
			// {Identifier}(  ->  {MethodName}(
			TokenType.Identifier , TokenType.OpenParens
		};

		private void Rewrite()
		{
			bool criteria = false;
			bool transform = false;

			for (int i = 0; i < Tokens.Count; i++)
			{
				var token = Peek(i);

				if (token.TokenType == TokenType.And)
				{
					criteria = true;
				}
				else if (token.TokenType == TokenType.Transform)
				{
					transform = true;
					criteria = false;
				}

				if (Match(i, TypeVariableList))
				{
					// <Identifier>  ->  TypeVariable
					Tokens[i] = new Token(TokenType.TypeVariable, Tokens[i + 1].Value);
					Tokens.RemoveRange(i + 1, 2);
				}
				if (Match(i, TypeMethodList))
				{
					// {TypeVariable}.{Identifier}  ->  {TypeVariable}.{MethodName}
					Tokens[i + 2] = new Token(TokenType.MethodName, Tokens[i + 2].Value);
				}
				if (Match(i, ClassMethodList))
				{
					// {Identifier}:{Identifier}  ->  {ClassName}:{MethodName}
					Tokens[i] = new Token(TokenType.ClassName, Tokens[i].Value);
					Tokens[i + 2] = new Token(TokenType.MethodName, Tokens[i + 2].Value);
				}
				if (Match(i, MethodNameList))
				{
					// {Identifier}(  ->  {MethodName}(
					Tokens[i] = new Token(TokenType.MethodName, Tokens[i].Value);
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
						throw new CompilerException("tokenizer: error at " + Index.ToString() + ": too many decimal characters");
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

				if ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || (c >= '0' && c <= '9'))
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
