/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */


namespace Mosa.Compiler.Metadata.Tables
{
	/// <summary>
	/// 
	/// </summary>
	public struct ClassLayoutRow
	{
		#region Data members

		/// <summary>
		/// 
		/// </summary>
		private short packingSize;

		/// <summary>
		/// 
		/// </summary>
		private int classSize;

		/// <summary>
		/// 
		/// </summary>
		private Token parent;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="ClassLayoutRow"/> struct.
		/// </summary>
		/// <param name="packingSize">Size of the packing.</param>
		/// <param name="classSize">Size of the class.</param>
		/// <param name="parent">The parent type def idx.</param>
		public ClassLayoutRow(short packingSize, int classSize, Token parent)
		{
			this.packingSize = packingSize;
			this.classSize = classSize;
			this.parent = parent;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the size of the packing.
		/// </summary>
		/// <value>The size of the packing.</value>
		public short PackingSize
		{
			get { return packingSize; }
		}

		/// <summary>
		/// Gets the size of the class.
		/// </summary>
		/// <value>The size of the class.</value>
		public int ClassSize
		{
			get { return classSize; }
		}

		/// <summary>
		/// Gets the parent type def idx.
		/// </summary>
		/// <value>The parent type def idx.</value>
		public Token Parent
		{
			get { return parent; }
		}

		#endregion // Properties
	}
}
