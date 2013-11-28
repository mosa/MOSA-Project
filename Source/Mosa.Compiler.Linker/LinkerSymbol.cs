/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *
 */

using System;
using System.Diagnostics;

namespace Mosa.Compiler.Linker
{
	/// <summary>
	/// Represents a single symbol for the linker.
	/// </summary>
	public sealed class LinkerSymbol
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LinkerSymbol" /> class.
		/// </summary>
		/// <param name="name">The name of the symbol.</param>
		/// <param name="section">The section holding the symbol.</param>
		/// <param name="sectionAddress">Holds the section relative address of the symbol.</param>
		/// <exception cref="System.ArgumentNullException">@name</exception>
		/// <exception cref="System.ArgumentException">@Name can't be empty.;@name</exception>
		/// <exception cref="T:System.ArgumentException"><paramref name="name" /> is empty.</exception>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="name" /> is null.</exception>
		public LinkerSymbol(string name, SectionKind section, long sectionAddress)
		{
			Debug.Assert(!String.IsNullOrEmpty(name), @"LinkerSymbol requires a proper name.");
			if (name == null)
				throw new ArgumentNullException(@"name");
			if (name.Length == 0)
				throw new ArgumentException(@"Name can't be empty.", @"name");

			this.Name = name;
			this.SectionKind = section;
			this.SectionAddress = sectionAddress;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the virtualAddress of the linker symbol.
		/// </summary>
		/// <value>The virtualAddress of the linker symbol.</value>
		public long VirtualAddress { get; set; }

		/// <summary>
		/// Gets or sets the length of the linker symbol in bytes.
		/// </summary>
		/// <value>The length in bytes.</value>
		public long Length { get; set; }

		/// <summary>
		/// Retrieves the symbol name.
		/// </summary>
		/// <value>The name of the linker symbol.</value>
		public string Name { get; private set; }

		/// <summary>
		/// Gets or sets the offset of the symbol in the file.
		/// </summary>
		/// <value>The symbol offset.</value>
		public long Offset { get; set; }

		/// <summary>
		/// Gets the section holding the symbol.
		/// </summary>
		/// <value>The section.</value>
		public SectionKind SectionKind { get; set; }

		/// <summary>
		/// Gets the section start relative virtualAddress of the symbol.
		/// </summary>
		/// <value>The section start relative virtualAddress.</value>
		public long SectionAddress { get; set; }

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return this.Name;
		}

		#endregion Properties
	}
}