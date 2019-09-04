// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;
using System;
using System.Globalization;

namespace Mosa.Utility.SourceCodeGenerator.TransformExpressions
{
	public class Token
	{
		public TokenType TokenType { get; protected set; }
		public string Value { get; protected set; }
		public int Position { get; protected set; } = -1;

		public bool IsInteger { get { return TokenType == TokenType.IntegerConstant || TokenType == TokenType.LongConstant; } }
		public bool IsFloat { get { return TokenType == TokenType.FloatConstant; } }
		public bool IsDouble { get { return TokenType == TokenType.DoubleConstant; } }

		public ulong Long { get; set; }

		public double Double { get; }
		public double Float { get; }
		public uint Integer { get { return (uint)Long; } set { Long = value; } }

		public Token(TokenType tokenType, int index)
		{
			TokenType = tokenType;
			Position = index;
		}

		public Token(TokenType tokenType, int index, string value)
			: this(tokenType, index)
		{
			Value = value;
		}

		public Token(TokenType tokenType, int index, string value, double d)
			: this(tokenType, index, value)
		{
			Double = d;
		}

		public Token(TokenType tokenType, int index, string value, float f)
			: this(tokenType, index, value)
		{
			Float = f;
		}

		public Token(TokenType tokenType, int index, string value, ulong l)
			: this(tokenType, index, value)
		{
			Long = l;
		}

		public override string ToString()
		{
			return TokenType.ToString() + (Value != null ? " = " + Value : string.Empty);
		}
	}
}
