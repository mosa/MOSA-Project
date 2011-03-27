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

namespace Mosa.Runtime.Metadata.Tables
{
	/// <summary>
	/// 
	/// </summary>
	public struct FileRow
	{
		#region Data members

		/// <summary>
		/// 
		/// </summary>
		private FileAttributes flags;

		/// <summary>
		/// 
		/// </summary>
		private HeapIndexToken name;

		/// <summary>
		/// 
		/// </summary>
		private HeapIndexToken hashValue;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="FileRow"/> struct.
		/// </summary>
		/// <param name="flags">The flags.</param>
		/// <param name="name">The name string idx.</param>
		/// <param name="hashValue">The hash value.</param>
		public FileRow(FileAttributes flags, HeapIndexToken name, HeapIndexToken hashValue)
		{
			this.flags = flags;
			this.name = name;
			this.hashValue = hashValue;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the flags.
		/// </summary>
		/// <value>The flags.</value>
		public FileAttributes Flags
		{
			get { return flags; }
		}

		/// <summary>
		/// Gets the name string idx.
		/// </summary>
		/// <value>The name string idx.</value>
		public HeapIndexToken Name
		{
			get { return name; }
		}

		/// <summary>
		/// Gets the hash value BLOB idx.
		/// </summary>
		/// <value>The hash value BLOB idx.</value>
		public HeapIndexToken HashValue
		{
			get { return hashValue; }
		}

		#endregion // Properties
	}
}
