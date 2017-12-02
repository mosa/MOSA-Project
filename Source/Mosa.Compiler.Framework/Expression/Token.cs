// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Expression
{
	public class Token
	{
		public TokenType TokenType { get; protected set; } = TokenType.Unknown;
		public string Value { get; protected set; } = null;
		public int Index { get; protected set; } = -1;

		public Token(TokenType tokenType, string value = null, int index = -1)
		{
			TokenType = tokenType;
			Value = value;
			Index = index;
		}

		public Token(TokenType tokenType) : this(tokenType, null)
		{
		}

		public override string ToString()
		{
			return TokenType.ToString() + (Value != null ? " = " + Value : string.Empty);
		}
	}
}
