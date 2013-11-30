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
	public class ClassLayoutRow
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="ClassLayoutRow" /> struct.
		/// </summary>
		/// <param name="packingSize">Size of the packing.</param>
		/// <param name="classSize">Size of the class.</param>
		/// <param name="parent">The parent.</param>
		public ClassLayoutRow(short packingSize, int classSize, Token parent)
		{
			PackingSize = packingSize;
			ClassSize = classSize;
			Parent = parent;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the size of the packing.
		/// </summary>
		/// <value>The size of the packing.</value>
		public short PackingSize { get; private set; }

		/// <summary>
		/// Gets the size of the class.
		/// </summary>
		/// <value>The size of the class.</value>
		public int ClassSize { get; private set; }

		/// <summary>
		/// Gets the parent.
		/// </summary>
		/// <value>
		/// The parent.
		/// </value>
		public Token Parent { get; private set; }

		#endregion Properties
	}
}