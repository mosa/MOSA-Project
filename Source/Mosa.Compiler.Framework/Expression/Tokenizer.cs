// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;
using System.Collections.Generic;
using System;

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

			if (i >= Tokens.Count)
				return Token.Unknown;

			return Tokens[i];
		}

		private bool Match(int index, TokenType[] tokens)
		{
			for (int i = 0; i < tokens.Length; i++)
			{
				if (index + i > Tokens.Count)
					return false;

				if (Peek(index, i).TokenType != tokens[i])
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

		private static readonly TokenType[] InstructionNameList = new TokenType[]
		{
			// ({Identifier}  ->  ({InstructionName}
			TokenType.OpenParens , TokenType.Identifier
		};

		private static readonly TokenType[] InstructionFamilyNameList = new TokenType[]
		{
			// ({Identifier}.{Identifier}  ->  ({InstructionFamily}.{InstructionName}
			TokenType.OpenParens , TokenType.Identifier , TokenType.Period, TokenType.Identifier, TokenType.OpenParens
		};

		/// <summary>
		/// Rewrites the tokens by:
		///    1. resolving identifiers into specific tokens
		///    2. adding tokens to
		/// </summary>
		private void Rewrite()
		{
			for (int i = 0; i < Tokens.Count; i++)
			{
				if (Tokens[i].TokenType == TokenType.Identifier && string.Equals(Tokens[i].Value, "const", StringComparison.OrdinalIgnoreCase))
				{
					Tokens[i] = new Token(TokenType.ConstLiteral, Tokens[i].Value);
				}
			}

			bool instructionMatch = true;
			bool criteria = false;
			bool transform = false;

			for (int i = 0; i < Tokens.Count; i++)
			{
				var token = Peek(i);

				if (token.TokenType == TokenType.And)
				{
					instructionMatch = false;
					criteria = true;
				}
				else if (token.TokenType == TokenType.Transform)
				{
					instructionMatch = true;
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
				if (criteria && Match(i, MethodNameList))
				{
					// {Identifier}(  ->  {MethodName}(
					Tokens[i] = new Token(TokenType.MethodName, Tokens[i].Value);
				}
				if (!criteria && Match(i, InstructionFamilyNameList))
				{
					// ({Identifier}.{Identifier}(  ->  ({InstructionFamily}.{InstructionName}
					Tokens[i + 1] = new Token(TokenType.InstructionFamily, Tokens[i + 1].Value);
					Tokens[i + 3] = new Token(TokenType.InstructionName, Tokens[i + 3].Value);
				}
				if (!criteria && Match(i, InstructionNameList))
				{
					// ({Identifier}  ->  ({InstructionName}
					Tokens[i + 1] = new Token(TokenType.InstructionName, Tokens[i + 1].Value);
				}
			}

			for (int i = 0; i < Tokens.Count; i++)
			{
				if (Tokens[i].TokenType == TokenType.Identifier)
				{
					Tokens[i] = new Token(TokenType.OperandVariable, Tokens[i].Value);
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
						throw new CompilerException("tokenizer: error at " + Index.ToString() + ": too many decimal characters");
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

		public int FindFirst(TokenType token)
		{
			for (int i = 0; i < Tokens.Count; i++)
			{
				if (Tokens[i].TokenType == token)
					return i;
			}

			return -1;
		}

		public List<Token> GetPart(int start, int end)
		{
			var tokens = new List<Token>(end - start);

			for (int i = start; i < end; i++)
			{
				tokens.Add(Tokens[i]);
			}

			return tokens;
		}
	}
}
