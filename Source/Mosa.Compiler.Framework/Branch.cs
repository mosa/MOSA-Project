/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Holds the branch target information
	/// </summary>
	public sealed class Branch
	{
		#region Data members

		/// <summary>
		/// Holds the branch targets instruction index.
		/// </summary>
		private int[] targets = null;

		#endregion

		#region Properties

		/// <summary>
		/// Returns the branch targets instruction index.
		/// </summary>
		public int[] Targets { get { return targets; } set { targets = value; } }

		#endregion // Properties

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="Branch"/> class.
		/// </summary>
		/// <param name="targets">The targets.</param>
		public Branch(uint targets)
		{
			this.targets = new int[targets];
		}

		#endregion // Construction

	}
}
