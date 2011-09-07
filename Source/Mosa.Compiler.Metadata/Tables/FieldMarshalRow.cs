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
	public struct FieldMarshalRow
	{
		#region Data members

		/// <summary>
		/// 
		/// </summary>
		private Token parent;

		/// <summary>
		/// 
		/// </summary>
		private HeapIndexToken nativeType;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="FieldMarshalRow"/> struct.
		/// </summary>
		/// <param name="parent">The parent.</param>
		/// <param name="nativeType">The native type BLOB idx.</param>
		public FieldMarshalRow(Token parent, HeapIndexToken nativeType)
		{
			this.parent = parent;
			this.nativeType = nativeType;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the parent.
		/// </summary>
		/// <value>The parent.</value>
		public Token Parent
		{
			get { return parent; }
		}

		/// <summary>
		/// Gets the type of the native.
		/// </summary>
		/// <value>The type of the native.</value>
		public HeapIndexToken NativeType
		{
			get { return nativeType; }
		}

		#endregion // Properties
	}
}
