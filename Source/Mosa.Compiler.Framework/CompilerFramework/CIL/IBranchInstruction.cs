/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// Base interface for CIL branch instructions.
	/// </summary>
	public interface IBranchInstruction
	{
		#region Properties

		/// <summary>
		/// Determines if the branch is conditional.
		/// </summary>
		bool IsConditional { get; }

		#endregion // Properties
	}
}
