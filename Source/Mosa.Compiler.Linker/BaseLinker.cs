/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Bruce Markham (illuminus) <illuminus86@gmail.com>
 */

using Mosa.Compiler.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Mosa.Compiler.Linker
{
	/// <summary>
	/// This compilation stage links all external labels together, which were previously registered.
	/// </summary>
	public abstract class BaseLinker : ILinker
	{
		#region Data members

		/// <summary>
		/// Holds all unresolved link requests.
		/// </summary>
		private readonly Dictionary<string, List<LinkRequest>> linkRequests = new Dictionary<string, List<LinkRequest>>();

		/// <summary>
		///
		/// </summary>
		private readonly ExtendedLinkerSection[] sections = new ExtendedLinkerSection[4];

		/// <summary>
		/// A dictionary containing all symbol seen in the assembly.
		/// </summary>
		private readonly Dictionary<string, LinkerSymbol> symbols = new Dictionary<string, LinkerSymbol>();

		#endregion Data members

		#region Properties

		/// <summary>
		/// Gets the base virtualAddress.
		/// </summary>
		/// <value>The base virtualAddress.</value>
		public long BaseAddress { get; private set; }

		/// <summary>
		/// Gets the entry point symbol.
		/// </summary>
		/// <value>The entry point symbol.</value>
		public LinkerSymbol EntryPoint { get; set; }

		/// <summary>
		/// Gets the load alignment of sections.
		/// </summary>
		/// <value>The load alignment.</value>
		public uint LoadSectionAlignment { get; protected set; }

		/// <summary>
		/// Gets or sets a value indicating whether symbols are resolved.
		/// </summary>
		/// <value>
		///   <c>true</c> if [symbols are resolved]; otherwise, <c>false</c>.
		/// </value>
		public bool SymbolsResolved { get; protected set; }

		/// <summary>
		/// Gets or sets the output file of the linker.
		/// </summary>
		/// <value>The output file.</value>
		public string OutputFile { get; set; }

		/// <summary>
		/// Retrieves the collection of sections created during compilation.
		/// </summary>
		/// <value>The sections collection.</value>
		public ExtendedLinkerSection[] Sections { get { return sections; } }

		/// <summary>
		/// Retrieves the collection of _symbols known by the linker.
		/// </summary>
		/// <value>The symbol collection.</value>
		public ICollection<LinkerSymbol> Symbols
		{
			get { return symbols.Values; }
		}

		/// <summary>
		/// Gets or sets the section alignment in bytes.
		/// </summary>
		/// <value>The section alignment in bytes.</value>
		public uint SectionAlignment { get; set; }

		/// <summary>
		/// Flag is the target platform is little-endian
		/// </summary>
		public Endianness Endianness { get; set; }

		/// <summary>
		/// Gets or sets the machine type (depends on platform)
		/// </summary>
		public uint MachineID { get; set; }

		#endregion Properties

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="BaseLinker"/>.
		/// </summary>
		protected BaseLinker()
		{
			BaseAddress = 0x00400000; // Use the Win32 default for now, FIXME
			SectionAlignment = 0x1000; // default 1K
			SymbolsResolved = false;
		}

		#endregion Construction

		#region ILinker Members

		/// <summary>
		/// Executes the linker and generates the final linked file
		/// </summary>
		void ILinker.GeneratedFile()
		{
			// Layout the sections
			LayoutSymbols();

			// Resolve all symbols
			Resolve();

			// Persist the file now
			CreateFile();
		}

		/// <summary>
		/// Adds the section.
		/// </summary>
		/// <param name="section">The section.</param>
		public void AddSection(ExtendedLinkerSection section)
		{
			sections[(int)section.SectionKind] = section;
		}

		/// <summary>
		/// Allocates a symbol of the given name in the specified section.
		/// </summary>
		/// <param name="name">The name of the symbol.</param>
		/// <param name="section">The executable section to allocate from.</param>
		/// <param name="size">The number of bytes to allocate. If zero, indicates an unknown amount of memory is required.</param>
		/// <param name="alignment">The alignment. A value of zero indicates the use of a default alignment for the section.</param>
		/// <returns>
		/// A stream, which can be used to populate the section.
		/// </returns>
		Stream ILinker.Allocate(string name, SectionKind section, int size, int alignment)
		{
			ExtendedLinkerSection linkerSection = (ExtendedLinkerSection)GetSection(section);
			Stream stream = linkerSection.Allocate(size, alignment);

			// Create a linker symbol for the name
			LinkerSymbol symbol = new LinkerSymbol(name, section, stream.Position);

			//symbol.VirtualAddress = linkerSection.VirtualAddress + stream.Position;

			// HACK -
			if (!symbols.ContainsKey(symbol.Name))
				symbols.Add(symbol.Name, symbol);

			// Debug.Assert(!symbols.ContainsKey(symbol.Name));

			// Save the symbol for later use
			//symbols.Add(symbol.Name, symbol);

			return new LinkerStream(symbol, stream, size);
		}

		/// <summary>
		/// Gets the section.
		/// </summary>
		/// <param name="sectionKind">Kind of the section.</param>
		/// <returns>The section of the requested kind.</returns>
		LinkerSection ILinker.GetSection(SectionKind sectionKind)
		{
			return GetSection(sectionKind);
		}

		/// <summary>
		/// Retrieves a linker symbol.
		/// </summary>
		/// <param name="name">The name of the symbol to retrieve.</param>
		/// <returns>The named linker symbol.</returns>
		/// <exception cref="System.ArgumentNullException"><paramref name="name"/> is null.</exception>
		/// <exception cref="System.ArgumentException">There's no symbol of the given name.</exception>
		LinkerSymbol ILinker.GetSymbol(string name)
		{
			return this.GetSymbol(name);
		}

		/// <summary>
		/// Issues a linker request for the given runtime method.
		/// </summary>
		/// <param name="linkType">The type of link required.</param>
		/// <param name="patches">The patches.</param>
		/// <param name="symbolName">The symbol name that is being patched.</param>
		/// <param name="symbolOffset">The offset inside the method where the patch is placed.</param>
		/// <param name="relativeBase">The base virtualAddress, if a relative link is required.</param>
		/// <param name="targetSymbol">The linker symbol name to link against.</param>
		/// <param name="targetOffset">An offset to apply to the link target.</param>
		void ILinker.Link(LinkType linkType, Patch[] patches, string symbolName, int symbolOffset, int relativeBase, string targetSymbol, long targetOffset)
		{
			Debug.Assert(symbolName != null, @"Symbol can't be null.");
			if (symbolName == null)
				throw new ArgumentNullException(@"symbol");

			List<LinkRequest> list;
			if (!linkRequests.TryGetValue(targetSymbol, out list))
			{
				list = new List<LinkRequest>();
				linkRequests.Add(targetSymbol, list);
			}

			list.Add(new LinkRequest(linkType, patches, symbolName, symbolOffset, relativeBase, targetSymbol, targetOffset));
		}

		#endregion ILinker Members

		/// <summary>
		/// Verifies the parameters.
		/// </summary>
		/// <returns></returns>
		/// <exception cref="System.ArgumentException"></exception>
		protected virtual void VerifyParameters()
		{
			if (String.IsNullOrEmpty(OutputFile))
				throw new ArgumentException(@"Invalid argument.", "compiler");
		}

		/// <summary>
		/// Layouts the sections.
		/// </summary>
		protected virtual void LayoutSymbols()
		{
			foreach (LinkerSymbol symbol in Symbols)
			{
				LinkerSection ls = GetSection(symbol.SectionKind);
				symbol.Offset = ls.Offset + symbol.SectionAddress;
				symbol.VirtualAddress = ls.VirtualAddress + symbol.SectionAddress;
			}

			// We've resolved all symbols, allow IsResolved to succeed
			SymbolsResolved = true;
		}

		/// <summary>
		/// Creates the final linked file.
		/// </summary>
		protected virtual void CreateFile()
		{
			return;
		}

		#region Internals

		/// <summary>
		/// Gets the section.
		/// </summary>
		/// <param name="sectionKind">Kind of the section.</param>
		/// <returns>The section of the requested kind.</returns>
		protected LinkerSection GetSection(SectionKind sectionKind)
		{
			return sections[(int)sectionKind];
		}

		/// <summary>
		/// Gets the symbol.
		/// </summary>
		/// <param name="name">Name of the symbol.</param>
		/// <returns></returns>
		private LinkerSymbol GetSymbol(string name)
		{
			if (name == null)
				throw new ArgumentNullException(@"symbolName");

			LinkerSymbol result;
			if (!symbols.TryGetValue(name, out result))
			{
				return null;
			}

			return result;
		}

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		protected void Resolve()
		{
			// Check if we have unresolved requests and try to link them
			foreach (var request in linkRequests)
			{
				var targetSymbol = GetSymbol(request.Key);

				if (targetSymbol == null)
				{
					throw new LinkerException("Missing Symbol: " + request.Key);
				}

				PatchRequests(request.Value, targetSymbol.VirtualAddress);
			}
		}

		/// <summary>
		/// Patches all requests in the given link request list.
		/// </summary>
		/// <param name="requests">A list of requests to patch.</param>
		/// <param name="targetAddress">The virtualAddress of the member.</param>
		private void PatchRequests(IEnumerable<LinkRequest> requests, long targetAddress)
		{
			foreach (LinkRequest request in requests)
			{
				var symbol = GetSymbol(request.SymbolName);

				ExtendedLinkerSection section = GetSection(symbol.SectionKind) as ExtendedLinkerSection;

				// Patch the code stream
				ApplyPatch(
					request.LinkType,
					request.Patches,
					section,
					symbol.VirtualAddress,
					request.SymbolOffset,
					request.SymbolRelativeBase,
					targetAddress + request.TargetOffset
				);
			}
		}

		/// <summary>
		/// A request to patch already emitted code by storing the calculated virtual address value.
		/// </summary>
		/// <param name="linkType">Type of the link.</param>
		/// <param name="patches">The patches.</param>
		/// <param name="symbolVirtualAddress">The virtual virtualAddress of the method whose code is being patched.</param>
		/// <param name="symbolOffset">The value to store at the position in code.</param>
		/// <param name="symbolRelativeBase">The method relative base.</param>
		/// <param name="targetAddress">The position in code, where it should be patched.</param>
		/// <exception cref="System.InvalidOperationException"></exception>
		private void ApplyPatch(LinkType linkType, Patch[] patches, ExtendedLinkerSection section, long symbolVirtualAddress, long symbolOffset, long symbolRelativeBase, long targetAddress)
		{
			if (!SymbolsResolved)
				throw new InvalidOperationException(@"Can't apply patches - symbols not resolved.");

			// Calculate the patch offset
			long offset = (symbolVirtualAddress - section.VirtualAddress) + symbolOffset;

			if ((linkType & LinkType.KindMask) == LinkType.AbsoluteAddress)
			{
				// FIXME: Need a .reloc section with a relocation entry if the module is moved in virtual memory
				// the runtime loader must patch this link request, we'll fail it until we can do relocations.
				//throw new NotSupportedException(@".reloc section not supported.");
			}
			else
			{
				// Change the absolute into a relative offset
				targetAddress = targetAddress - (symbolVirtualAddress + symbolRelativeBase);
			}

			ulong value = Patch.GetResult(patches, (ulong)targetAddress);
			ulong mask = Patch.GetFinalMask(patches);

			// Save the stream position
			section.ApplyPatch(offset, linkType, value, mask, Endianness);
		}

		#endregion Internals
	}
}