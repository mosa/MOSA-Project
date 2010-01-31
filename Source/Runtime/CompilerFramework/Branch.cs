/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// Holds the branch target information
	/// </summary>
	public class Branch : IBranch
	{
		#region Data members

		/// <summary>
		/// Holds the instruction offset.
		/// </summary>
		private int _offset = 0;

		/// <summary>
		/// Holds the branch targets instruction index.
		/// </summary>
		private int[] _branchTargets = null;

		#endregion

		#region Properties

		/// <summary>
		/// Retrieves the instruction offset.
		/// </summary>
		public int Offset { get { return _offset; } }

		/// <summary>
		/// Returns the branch targets instruction index.
		/// </summary>
		public int[] Targets { get { return _branchTargets; } set { _branchTargets = value; } }
		
		#endregion // Properties
		
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="Branch"/> class.
		/// </summary>
		/// <param name="targets">The targets.</param>
		public Branch(uint targets)
		{
			_branchTargets = new int[targets];
		}

		#endregion // Construction

	}
}
