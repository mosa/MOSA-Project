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
	public class FieldMarshalRow
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="FieldMarshalRow" /> struct.
		/// </summary>
		/// <param name="parent">The parent.</param>
		/// <param name="nativeType">Type of the native.</param>
		public FieldMarshalRow(Token parent, HeapIndexToken nativeType)
		{
			Parent = parent;
			NativeType = nativeType;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the parent.
		/// </summary>
		/// <value>The parent.</value>
		public Token Parent { get; private set; }

		/// <summary>
		/// Gets the type of the native.
		/// </summary>
		/// <value>The type of the native.</value>
		public HeapIndexToken NativeType { get; private set; }

		#endregion Properties
	}
}