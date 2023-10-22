// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework;

/// <summary>
/// Instruction Node Addition
/// </summary>
public sealed class NodeAddition
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
