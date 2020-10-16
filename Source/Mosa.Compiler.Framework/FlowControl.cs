// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Specifies flow-control properties of an instruction.
	/// </summary>
	public enum FlowControl
	{
		/// <summary>
		/// The instruction always continues execution on the next instruction.
		/// </summary>
		Next,

		/// <summary>
		/// The instruction invokes another method.
		/// </summary>
		Call,

		/// <summary>
		/// The instruction is an unconditional branch.
		/// </summary>
		UnconditionalBranch,

		/// <summary>
		/// The instruction is a conditional branch, which never falls through.
		/// </summary>
		ConditionalBranch,

		/// <summary>
		/// The instruction is a conditional branch, which may fall-through.
		/// </summary>
		Switch,

		/// <summary>
		/// The instruction breaks the control-flow.
		/// </summary>
		Break,

		/// <summary>
		/// The instruction returns from the method
		/// </summary>
		Return,

		/// <summary>
		/// The instruction throws an exception.
		/// </summary>
		Throw,

		/// <summary>
		/// Leaves a try block
		/// </summary>
		Leave,

		/// <summary>
		/// End of finally block
		/// </summary>
		EndFinally,

		/// <summary>
		/// End of filter block
		/// </summary>
		EndFilter,
	}
}
