// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.SourceCodeGenerator.TransformExpressions
{
	/// <summary>
	/// TokenType
	/// </summary>
	public enum TokenType
	{
		Unknown,

		Word,  // temporary - until deducted
		Method,
		Instruction,
		Label,

		OpenParens, // syntax:
		CloseParens,
		OpenBracket,
		CloseBracket,
		Comma,
		Underscore,
		Period,
		Hash,
		Minus,
		Dollar,

		Greater,
		Less,
		Equal,

		GreaterEqual,
		LessEqual,
		NotEqual,
		Always,

		Number,     // constants:
		IntegerConstant,

		//LongConstant,
		DoubleConstant,

		FloatConstant,

		And,    // boolean logic:
		Or,
		Not,

		OpenCurly,
		CloseCurly,
	}
}
