// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;
using System.Collections.Generic;
using Mosa.Compiler.Framework.Expression;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Rules
	/// </summary>
	public static class Rules
	{
		public static readonly List<Rule> List = new List<Rule>() {
			new Rule() {
				Name = "MultipleBy1",
				Type = "StrengthReduction",
				Match = "(IR.MulUnsigned 1 x)",
				Transform = "x",
				Criteria = "",
				DefaultInstructionFamily = "IR",
				DefaultArchitectureFamily = "NA",
				IsOptimization  = true,
				IsTransformation  = false,
			},
			new Rule() {
				Name = "MultipleBy0",
				Type = "StrengthReduction",
				Match = "(IR.MulUnsigned 0 x)",
				Transform = "0",
				Criteria = "",
				DefaultInstructionFamily = "IR",
				DefaultArchitectureFamily = "NA",
				IsOptimization  = true,
				IsTransformation  = false,
			},
		};
	}
}
