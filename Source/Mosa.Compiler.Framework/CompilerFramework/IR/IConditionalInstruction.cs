/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */


namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// Marks an IR instruction as being conditional.
	/// </summary>
	public interface IConditionalInstruction
	{
		/// <summary>
		/// Gets the condition code.
		/// </summary>
		/// <value>The condition code.</value>
		ConditionCode ConditionCode { get; set; }
	}
}
