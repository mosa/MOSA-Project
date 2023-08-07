// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.SourceCodeGenerator.TransformExpressions;

public class Token
{
	public TokenType TokenType { get; protected set; }

	public string Value { get; protected set; }

	public int Position { get; protected set; } = -1;

	public bool IsInteger => TokenType == TokenType.IntegerConstant;

	public bool IsFloat => TokenType == TokenType.FloatConstant;

	public bool IsDouble => TokenType == TokenType.DoubleConstant;

	public ulong Integer { get; set; }

	public double Double { get; }

	public double Float { get; }

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
		Integer = l;
	}

	public override string ToString()
	{
		return TokenType + (Value != null ? " = " + Value : string.Empty);
	}
}
