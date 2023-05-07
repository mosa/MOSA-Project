// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework;

/// <summary>
/// Instruction Node Addition
/// </summary>
public sealed class InstructionNodeAddition
{
	#region Properties

	/// <summary>
	/// Gets or sets the phi blocks.
	/// </summary>
	/// <value>
	/// The phi blocks.
	/// </value>
	public List<BasicBlock> PhiBlocks { get; set; }

	/// <summary>
	/// Gets or sets  additional operands
	/// </summary>
	public Operand[] AdditionalOperands { get; set; }

	#endregion Properties
}
