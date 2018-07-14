// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;
using System;
using System.Globalization;

namespace Mosa.Compiler.Framework.Expression
{
	public class Token
	{
		public static Token Unknown = new Token(TokenType.Unknown);

		public TokenType TokenType { get; protected set; }
		public string Value { get; protected set; }
		public int Position { get; protected set; } = -1;

		public bool IsInteger { get { return TokenType == TokenType.SignedIntegerConstant || TokenType == TokenType.UnsignedIntegerConstant; } }
		public bool IsSigned { get { return TokenType == TokenType.SignedIntegerConstant; } }
		public bool IsDouble { get { return TokenType == TokenType.DoubleConstant; } }

		public ulong Integer { get; }
		public double Double { get; }

		public Token(TokenType tokenType, string value = null, int index = -1)
		{
			TokenType = tokenType;
			Value = value;
			Position = index;
		}

		public Token(long i, int index = -1)
		{
			TokenType = TokenType.SignedIntegerConstant;
			Integer = (ulong)i;
			Position = index;
		}

		public Token(ulong u, int index = -1)
		{
			TokenType = TokenType.UnsignedIntegerConstant;
			Integer = u;
			Position = index;
		}

		public Token(double d, int index = -1)
		{
			TokenType = TokenType.DoubleConstant;
			Double = d;
			Position = index;
		}

		public Token(bool b, int index = -1)
		{
			TokenType = TokenType.UnsignedIntegerConstant;
			Integer = b ? 0u : 1u;
			Position = index;
		}

		public Token(TokenType tokenType) : this(tokenType, null)
		{
		}

		public Token(string text, int index = -1)
		{
			Position = index;
			Value = text;

			// start of unused - kept for possible future use
			string value = text.ToLower();

			if (value == "null")
			{
				TokenType = TokenType.UnsignedIntegerConstant;
				Integer = 0;
				return;
			}

			if (value == "false")
			{
				TokenType = TokenType.UnsignedIntegerConstant;
				Integer = 0;
				return;
			}

			if (value == "true")
			{
				TokenType = TokenType.UnsignedIntegerConstant;
				Integer = 1;
				return;
			}

			// end of unused

			bool signed = value[0] == '-';
			bool dec = value.Contains(".");
			bool unsign2 = value.EndsWith("u");
			bool dec2 = value.EndsWith("d");
			bool hex = value.StartsWith("0x");

			// Hex
			if (hex)
			{
				if (!UInt64.TryParse(value.Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out ulong v))
					throw new CompilerException("ExpressionEvaluation: Invalid long hex constant: " + text);

				TokenType = TokenType.UnsignedIntegerConstant;
				Integer = v;
				return;
			}

			// Double
			if (dec || dec2)
			{
				value = dec2 ? value.Substring(0, value.Length - 1) : value;

				if (!Double.TryParse(value, out double d))
					throw new CompilerException("ExpressionEvaluation: Invalid double constant: " + text);

				TokenType = TokenType.DoubleConstant;
				Double = d;
				return;
			}

			// Signed
			if (signed)
			{
				if (!Int64.TryParse(value, out long i))
					throw new CompilerException("ExpressionEvaluation: Invalid integer constant: " + text);

				TokenType = TokenType.SignedIntegerConstant;
				Integer = (ulong)i;
				return;
			}

			// Unsigned
			value = dec2 ? value.Substring(0, value.Length - 1) : value;

			if (!UInt64.TryParse(value, out ulong u))
				throw new CompilerException("ExpressionEvaluation: Invalid integer constant: " + text);

			TokenType = TokenType.UnsignedIntegerConstant;
			Integer = u;
		}

		public override string ToString()
		{
			return TokenType.ToString() + (Value != null ? " = " + Value : string.Empty);
		}
	}
}
