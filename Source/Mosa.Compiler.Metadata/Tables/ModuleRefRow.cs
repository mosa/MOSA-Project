/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Compiler.Metadata.Tables
{
	/// <summary>
	///
	/// </summary>
	public class ModuleRefRow
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="ModuleRefRow" /> struct.
		/// </summary>
		/// <param name="nameString">The name string.</param>
		public ModuleRefRow(HeapIndexToken nameString)
		{
			NameString = nameString;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the name string.
		/// </summary>
		/// <value>
		/// The name string.
		/// </value>
		public HeapIndexToken NameString { get; private set; }

		#endregion Properties
	}
}