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
	public class MethodDefRow
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="MethodDefRow"/> struct.
		/// </summary>
		/// <param name="rva">The rva.</param>
		/// <param name="implFlags">The impl flags.</param>
		/// <param name="flags">The flags.</param>
		/// <param name="paramList">The param list.</param>
		public MethodDefRow(uint rva, MethodImplAttributes implFlags, MethodAttributes flags, HeapIndexToken nameString,
								HeapIndexToken signatureBlob, Token paramList)
		{
			Rva = rva;
			ImplFlags = implFlags;
			Flags = flags;
			NameString = nameString;
			SignatureBlob = signatureBlob;
			ParamList = paramList;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the rva.
		/// </summary>
		/// <value>The rva.</value>
		public uint Rva { get; private set; }

		/// <summary>
		/// Gets the impl flags.
		/// </summary>
		/// <value>The impl flags.</value>
		public MethodImplAttributes ImplFlags { get; private set; }

		/// <summary>
		/// Gets the flags.
		/// </summary>
		/// <value>The flags.</value>
		public MethodAttributes Flags { get; private set; }

		/// <summary>
		/// Gets the name string.
		/// </summary>
		/// <value>
		/// The name string.
		/// </value>
		public HeapIndexToken NameString { get; private set; }

		/// <summary>
		/// Gets the signature BLOB.
		/// </summary>
		/// <value>
		/// The signature BLOB.
		/// </value>
		public HeapIndexToken SignatureBlob { get; private set; }

		/// <summary>
		/// Gets the param list.
		/// </summary>
		/// <value>The param list.</value>
		public Token ParamList { get; private set; }

		#endregion Properties
	}
}