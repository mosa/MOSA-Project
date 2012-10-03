/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.IO;
using Mosa.Compiler.Common;

namespace Mosa.Compiler.Linker.PE
{
	/// <summary>
	/// An implementation of a portable executable linker section.
	/// </summary>
	public class PELinkerSection : ExtendedLinkerSection
	{

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="PELinkerSection"/> class.
		/// </summary>
		/// <param name="kind">The kind of the section.</param>
		/// <param name="name">The name of the section.</param>
		/// <param name="virtualAddress">The virtualAddress of the section.</param>
		public PELinkerSection(SectionKind kind, string name, long virtualAddress) :
			base(kind, name, virtualAddress)
		{
			stream = new MemoryStream();
		}

		#endregion // Construction

		#region Methods

		/// <summary>
		/// Stores the section contents to the given writer.
		/// </summary>
		/// <param name="writer">The writer.</param>
		public void Write(BinaryWriter writer)
		{
			stream.WriteTo(writer.BaseStream);
		}

		#endregion // Methods

	}
}
