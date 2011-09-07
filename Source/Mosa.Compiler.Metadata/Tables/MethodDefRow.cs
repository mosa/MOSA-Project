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
		private uint _rva;

		/// <summary>
		/// 
		/// </summary>
		private MethodImplAttributes _implFlags;

		/// <summary>
		/// 
		/// </summary>
		private MethodAttributes _flags;

		/// <summary>
		/// 
		/// </summary>
		private HeapIndexToken _nameStringIdx;

		/// <summary>
		/// 
		/// </summary>
		private HeapIndexToken _signatureBlobIdx;

		/// <summary>
		/// 
		/// </summary>
		private Token _paramList;

		#endregion // Data members

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
			this._rva = rva;
			this._implFlags = implFlags;
			this._flags = flags;
			this._nameStringIdx = nameStringIdx;
			this._signatureBlobIdx = signatureBlobIdx;
			this._paramList = paramList;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the rva.
		/// </summary>
		/// <value>The rva.</value>
		public uint Rva
		{
			get { return _rva; }
		}

		/// <summary>
		/// Gets the impl flags.
		/// </summary>
		/// <value>The impl flags.</value>
		public MethodImplAttributes ImplFlags
		{
			get { return _implFlags; }
		}

		/// <summary>
		/// Gets the flags.
		/// </summary>
		/// <value>The flags.</value>
		public MethodAttributes Flags
		{
			get { return _flags; }
		}

		/// <summary>
		/// Gets the name string idx.
		/// </summary>
		/// <value>The name string idx.</value>
		public HeapIndexToken NameStringIdx
		{
			get { return _nameStringIdx; }
		}

		/// <summary>
		/// Gets the signature BLOB idx.
		/// </summary>
		/// <value>The signature BLOB idx.</value>
		public HeapIndexToken SignatureBlobIdx
		{
			get { return _signatureBlobIdx; }
		}

		/// <summary>
		/// Gets the param list.
		/// </summary>
		/// <value>The param list.</value>
		public Token ParamList
		{
			get { return _paramList; }
		}

		#endregion // Properties
	}
}
