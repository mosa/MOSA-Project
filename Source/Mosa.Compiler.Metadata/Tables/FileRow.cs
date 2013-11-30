/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.IO;

namespace Mosa.Compiler.Metadata.Tables
{
	/// <summary>
	///
	/// </summary>
	public class FileRow
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="FileRow" /> struct.
		/// </summary>
		/// <param name="flags">The flags.</param>
		/// <param name="name">The name.</param>
		/// <param name="hashValue">The hash value.</param>
		public FileRow(FileAttributes flags, HeapIndexToken name, HeapIndexToken hashValue)
		{
			Flags = flags;
			Name = name;
			HashValue = hashValue;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the flags.
		/// </summary>
		/// <value>The flags.</value>
		public FileAttributes Flags { get; private set; }

		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		public HeapIndexToken Name { get; private set; }

		/// <summary>
		/// Gets the hash value.
		/// </summary>
		/// <value>The hash value.</value>
		public HeapIndexToken HashValue { get; private set; }

		#endregion Properties
	}
}