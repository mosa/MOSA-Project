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

		Number,     // constants:
		IntegerConstant,

		//LongConstant,
		DoubleConstant,

		FloatConstant,

		And,    // boolean logic:
		Or,
		Not,

		//Questionmark,
		//Colon,

		//Addition,   // math:
		//Subtract,
		//Multiplication,
		//Division,
		//Modulus,
		//Negate,

		//CompareEqual,   // comparisons:
		//CompareNotEqual,
		//CompareGreaterThanOrEqual,
		//CompareLessThanOrEqual,
		//CompareLessThan,
		//CompareGreaterThan,

		//If,
		//ShiftRight,
		//ShiftLeft,

		//Instruction,
		//TypeVariable,
		//OperandVariable,

		//PhysicalRegister,
		//VirtualRegister,

		//Null,
	}
}
