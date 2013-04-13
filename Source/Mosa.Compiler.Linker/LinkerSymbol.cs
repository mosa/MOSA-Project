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
		#region Data members

		/// <summary>
		/// Holds the virtual address of the symbol.
		/// </summary>
		private long virtualAddress;

		/// <summary>
		/// Holds the length of the linker symbol in bytes.
		/// </summary>
		private long length;

		/// <summary>
		/// Holds the name of the linker symbol.
		/// </summary>
		private string name;

		/// <summary>
		/// Holds the symbol offset in the file.
		/// </summary>
		private long offset;

		/// <summary>
		/// Holds the section containing the linker symbol.
		/// </summary>
		private SectionKind section;

		/// <summary>
		/// The section start relative virtualAddress.
		/// </summary>
		private long sectionAddress;

		#endregion Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LinkerSymbol"/> class.
		/// </summary>
		/// <param name="name">The name of the symbol.</param>
		/// <param name="section">The section holding the symbol.</param>
		/// <param name="sectionAddress">Holds the section relative address of the symbol.</param>
		/// <exception cref="T:System.ArgumentException"><paramref name="name"/> is empty.</exception>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="name"/> is null.</exception>
		public LinkerSymbol(string name, SectionKind section, long sectionAddress)
		{
			Debug.Assert(!String.IsNullOrEmpty(name), @"LinkerSymbol requires a proper name.");
			if (name == null)
				throw new ArgumentNullException(@"name");
			if (name.Length == 0)
				throw new ArgumentException(@"Name can't be empty.", @"name");

			this.name = name;
			this.section = section;
			this.sectionAddress = sectionAddress;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the virtualAddress of the linker symbol.
		/// </summary>
		/// <value>The virtualAddress of the linker symbol.</value>
		public long VirtualAddress
		{
			get { return this.virtualAddress; }
			set { this.virtualAddress = value; }
		}

		/// <summary>
		/// Gets or sets the length of the linker symbol in bytes.
		/// </summary>
		/// <value>The length in bytes.</value>
		public long Length
		{
			get { return this.length; }
			set { this.length = value; }
		}

		/// <summary>
		/// Retrieves the symbol name.
		/// </summary>
		/// <value>The name of the linker symbol.</value>
		public string Name
		{
			get { return this.name; }
		}

		/// <summary>
		/// Gets or sets the offset of the symbol in the file.
		/// </summary>
		/// <value>The symbol offset.</value>
		public long Offset
		{
			get { return this.offset; }
			set { this.offset = value; }
		}

		/// <summary>
		/// Gets the section holding the symbol.
		/// </summary>
		/// <value>The section.</value>
		public SectionKind Section
		{
			get { return this.section; }
		}

		/// <summary>
		/// Gets the section start relative virtualAddress of the symbol.
		/// </summary>
		/// <value>The section start relative virtualAddress.</value>
		public long SectionAddress
		{
			get { return this.sectionAddress; }
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return this.name;
		}

		#endregion Properties
	}
}