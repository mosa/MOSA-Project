/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using Mosa.Compiler.Common;
using System.Collections.Generic;
using System.IO;

namespace Mosa.Compiler.Linker
{
	/// <summary>
	/// Interface to the linker
	/// </summary>
	/// <remarks>
	/// The linker collects runtime specific requests in order to resolve a metadata object to its physical virtual
	/// address in memory. All link requests require the metadata object, the request virtual address
	/// and a relative flag. These are used to either resolve the request immediately or patch the code during
	/// a later linker stage, when all methods and fields have been compiled.
	/// </remarks>
	public interface ILinker
	{
		#region Properties

		/// <summary>
		/// Gets the base address.
		/// </summary>
		/// <value>The base address.</value>
		long BaseAddress { get; }

		/// <summary>
		/// Gets or sets the entry point symbol.
		/// </summary>
		/// <value>The entry point symbol.</value>
		LinkerSymbol EntryPoint { get; set; }

		/// <summary>
		/// Gets the load alignment of sections.
		/// </summary>
		/// <value>The load alignment.</value>
		uint LoadSectionAlignment { get; }

		/// <summary>
		/// Retrieves the collection of sections created during compilation.
		/// </summary>
		/// <value>The sections collection.</value>
		ExtendedLinkerSection[] Sections { get; }

		/// <summary>
		/// Retrieves the collection of symbols known by the linker.
		/// </summary>
		/// <value>The symbol collection.</value>
		ICollection<LinkerSymbol> Symbols { get; }

		/// <summary>
		/// Gets or sets the output file of the linker.
		/// </summary>
		/// <value>The output file.</value>
		string OutputFile { get; set; }

		/// <summary>
		/// Gets the alignment of sections.
		/// </summary>
		/// <value>The virtual section alignment.</value>
		uint SectionAlignment { get; }

		/// <summary>
		/// Flag is the target platform is little-endian
		/// </summary>
		Endianness Endianness { get; set; }

		/// <summary>
		/// Gets or sets the machine id (depends on platform)
		/// </summary>
		/// <value>
		/// </value>
		uint MachineID { get; set; }

		#endregion Properties

		#region Methods

		/// <summary>
		/// Allocates a symbol of the given name in the specified section.
		/// </summary>
		/// <param name="name">The name of the symbol.</param>
		/// <param name="section">The executable section to allocate from.</param>
		/// <param name="size">The number of bytes to allocate. If zero, indicates an unknown amount of memory is required.</param>
		/// <param name="alignment">The alignment. A value of zero indicates the use of a default alignment for the section.</param>
		/// <returns>A stream, which can be used to populate the section.</returns>
		Stream Allocate(string name, SectionKind section, int size, int alignment);

		/// <summary>
		/// Adds the section.
		/// </summary>
		/// <param name="section">The section.</param>
		void AddSection(ExtendedLinkerSection section);

		/// <summary>
		/// Gets the section.
		/// </summary>
		/// <param name="sectionKind">Kind of the section.</param>
		/// <returns>The section of the requested kind.</returns>
		LinkerSection GetSection(SectionKind sectionKind);

		/// <summary>
		/// Retrieves a linker symbol.
		/// </summary>
		/// <param name="symbolName">The name of the symbol to retrieve.</param>
		/// <returns>The named linker symbol.</returns>
		/// <exception cref="System.ArgumentNullException"><paramref name="symbolName"/> is null.</exception>
		/// <exception cref="System.ArgumentException">There's no symbol of the given name.</exception>
		LinkerSymbol GetSymbol(string symbolName);

		/// <summary>
		/// Issues a linker request for the given runtime method.
		/// </summary>
		/// <param name="linkType">The type of link required.</param>
		/// <param name="patches">The patches.</param>
		/// <param name="symbolName">The symbol name the patched code belongs to.</param>
		/// <param name="symbolOffset">The offset inside the method where the patch is placed.</param>
		/// <param name="methodRelativeBase">The base virtualAddress, if a relative link is required.</param>
		/// <param name="targetSymbol">The linker symbol name to link against.</param>
		/// <param name="targetOffset">An offset to apply to the link target.</param>
		void Link(LinkType linkType, Patch[] patches, string symbolName, int symbolOffset, int methodRelativeBase, string targetSymbol, long targetOffset);

		/// <summary>
		/// Commits the linker and generates the final linked file
		/// </summary>
		void Commit();

		#endregion Methods
	}
}