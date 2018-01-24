// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Expression
{
	public enum NodeType
	{
		Instruction,
		FixedIntegerConstant,
		FixedDoubleConstant,
		VirtualRegister,
		PhyiscalRegister,
		ConstantVariable,
		OperandVariable, // can be virtual/physical register or constant
		TypeVariable,
		Expression,
		Any
	}
}
