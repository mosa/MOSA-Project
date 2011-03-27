/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Mosa.Runtime.Metadata;

namespace Mosa.Runtime.Metadata.Tables
{
	/// <summary>
	/// 
	/// </summary>
	public struct ManifestResourceRow
	{
		#region Data members

		/// <summary>
		/// 
		/// </summary>
		private uint _offset;

		/// <summary>
		/// 
		/// </summary>
		private ManifestResourceAttributes _flags;

		/// <summary>
		/// 
		/// </summary>
		private HeapIndexToken _nameStringIdx;

		/// <summary>
		/// 
		/// </summary>
		private Token _implementation;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="ManifestResourceRow"/> struct.
		/// </summary>
		/// <param name="offset">The offset.</param>
		/// <param name="flags">The flags.</param>
		/// <param name="nameStringIndex">Index of the name string.</param>
		/// <param name="implementation">The implementation table idx.</param>
		public ManifestResourceRow(uint offset, ManifestResourceAttributes flags, HeapIndexToken nameStringIndex,
			Token implementation)
		{
			_offset = offset;
			_flags = flags;
			_nameStringIdx = nameStringIndex;
			_implementation = implementation;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the offset.
		/// </summary>
		/// <value>The offset.</value>
		public uint Offset
		{
			get { return _offset; }
		}

		/// <summary>
		/// Gets the flags.
		/// </summary>
		/// <value>The flags.</value>
		public ManifestResourceAttributes Flags
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
		/// Gets the implementation table idx.
		/// </summary>
		/// <value>The implementation table idx.</value>
		public Token Implementation
		{
			get { return _implementation; }
		}

		#endregion // Properties
	}
}
