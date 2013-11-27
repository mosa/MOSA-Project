/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

//using System.Reflection;

namespace Mosa.Compiler.Metadata.Tables
{
	/// <summary>
	///
	/// </summary>
	public struct MethodDefRow
	{
		#region Data members

		/// <summary>
		///
		/// </summary>
		private uint rva;

		/// <summary>
		///
		/// </summary>
		private MethodImplAttributes implFlags;

		/// <summary>
		///
		/// </summary>
		private MethodAttributes flags;

		/// <summary>
		///
		/// </summary>
		private HeapIndexToken nameStringIdx;

		/// <summary>
		///
		/// </summary>
		private HeapIndexToken signatureBlobIdx;

		/// <summary>
		///
		/// </summary>
		private Token paramList;

		#endregion Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="MethodDefRow"/> struct.
		/// </summary>
		/// <param name="rva">The rva.</param>
		/// <param name="implFlags">The impl flags.</param>
		/// <param name="flags">The flags.</param>
		/// <param name="nameStringIdx">The name string idx.</param>
		/// <param name="signatureBlobIdx">The signature BLOB idx.</param>
		/// <param name="paramList">The param list.</param>
		public MethodDefRow(uint rva, MethodImplAttributes implFlags, MethodAttributes flags, HeapIndexToken nameStringIdx,
								HeapIndexToken signatureBlobIdx, Token paramList)
		{
			this.rva = rva;
			this.implFlags = implFlags;
			this.flags = flags;
			this.nameStringIdx = nameStringIdx;
			this.signatureBlobIdx = signatureBlobIdx;
			this.paramList = paramList;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the rva.
		/// </summary>
		/// <value>The rva.</value>
		public uint Rva
		{
			get { return rva; }
		}

		/// <summary>
		/// Gets the impl flags.
		/// </summary>
		/// <value>The impl flags.</value>
		public MethodImplAttributes ImplFlags
		{
			get { return implFlags; }
		}

		/// <summary>
		/// Gets the flags.
		/// </summary>
		/// <value>The flags.</value>
		public MethodAttributes Flags
		{
			get { return flags; }
		}

		/// <summary>
		/// Gets the name string idx.
		/// </summary>
		/// <value>The name string idx.</value>
		public HeapIndexToken NameStringIdx
		{
			get { return nameStringIdx; }
		}

		/// <summary>
		/// Gets the signature BLOB idx.
		/// </summary>
		/// <value>The signature BLOB idx.</value>
		public HeapIndexToken SignatureBlobIdx
		{
			get { return signatureBlobIdx; }
		}

		/// <summary>
		/// Gets the param list.
		/// </summary>
		/// <value>The param list.</value>
		public Token ParamList
		{
			get { return paramList; }
		}

		#endregion Properties
	}
}