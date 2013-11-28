/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

namespace Mosa.Compiler.Linker
{
	/// <summary>
	/// Abstract class, that represents sections in an executable file provided by the linker.
	/// </summary>
	public abstract class LinkerSection
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LinkerSection" /> class.
		/// </summary>
		/// <param name="sectionKind">The kind of the section.</param>
		/// <param name="name">The name.</param>
		/// <param name="virtualAddress">The virtualAddress.</param>
		protected LinkerSection(SectionKind sectionKind, string name, long virtualAddress)
		{
			this.VirtualAddress = virtualAddress;
			this.SectionKind = sectionKind;
			this.Name = name;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the virtual address of the section.
		/// </summary>
		/// <value>The virtual address.</value>
		public long VirtualAddress { get; set; }

		/// <summary>
		/// Gets the length of the section in bytes.
		/// </summary>
		/// <value>The length of the section in bytes.</value>
		public abstract long Length { get; }

		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the file offset of the section.
		/// </summary>
		/// <value>The file offset.</value>
		public long Offset { get; set; }

		/// <summary>
		/// Gets the kind of the section.
		/// </summary>
		/// <value>The kind of the section.</value>
		public SectionKind SectionKind { get; set; }

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return Name;
		}

		#endregion Properties
	}
}