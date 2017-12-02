// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Expression
{
	/// <summary>
	/// TokenType
	/// </summary>
	public enum TokenType
	{
		Unknown,

		OpenParens, // syntax:
		CloseParens,
		OpenBracket,
		CloseBracket,
		Comma,
		Transform,
		Underscore,

		And,    // boolean logic:
		Or,
		Not,
		Questionmark,
		Colon,

		Addition,   // math:
		Subtract,
		Multiplication,
		Division,
		Modulus,
		Negate,

		IntegerConstant, // literals:
		FloatConstant,
		BooleanTrueConstant,
		BooleanFalseConstant,

		CompareEqual,   // comparisons:
		CompareNotEqual,
		CompareGreaterThanOrEqual,
		CompareLessThanOrEqual,
		CompareLessThan,
		CompareGreaterThan,

		Identifier,  // temporary

		Variable,
		VirtualRegister,
		ReservedWord,
		TypeName,
		PhysicalRegister,
	}
}
