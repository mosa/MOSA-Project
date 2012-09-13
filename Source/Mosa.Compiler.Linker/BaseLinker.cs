/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Bruce Markham (illuminus) <illuminus86@gmail.com>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Mosa.Compiler.Common;

namespace Mosa.Compiler.Linker
{
	/// <summary>
	/// This compilation stage links all external labels together, which were previously registered.
	/// </summary>
	public abstract class BaseLinker : ILinker
	{

		#region Data members

		/// <summary>
		/// Holds the base virtualAddress of the link result.
		/// </summary>
		private long baseAddress;

		/// <summary>
		/// Holds the entry point of the compiled binary.
		/// </summary>
		private LinkerSymbol entryPoint;

		/// <summary>
		/// Holds all unresolved link requests.
		/// </summary>
		private readonly Dictionary<string, List<LinkRequest>> linkRequests = new Dictionary<string, List<LinkRequest>>();

		/// <summary>
		/// 
		/// </summary>
		private readonly List<LinkerSectionExtended> sections = new List<LinkerSectionExtended>();

		/// <summary>
		/// Holds the output file of the linker.
		/// </summary>
		private string outputFile;

		/// <summary>
		/// A dictionary containing all symbol seen in the assembly.
		/// </summary>
		private readonly Dictionary<string, LinkerSymbol> symbols = new Dictionary<string, LinkerSymbol>();

		/// <summary>
		/// Flag is the target platform is little-endian
		/// </summary>
		private Endianness endianness;

		/// <summary>
		/// Gets or sets the machine type (depends on platform)
		/// </summary>
		/// <value>
		/// </value>
		private uint machineID;

		/// <summary>
		/// Holds the file alignment used for this ELF32 file.
		/// </summary>
		private uint loadSectionAlignment;

		/// <summary>
		/// Flag, if the symbols have been resolved.
		/// </summary>
		private bool symbolsResolved;

		/// <summary>
		/// Holds the section alignment
		/// </summary>
		private uint sectionAlignment;

		#endregion // Data members

		#region Properties

		/// <summary>
		/// Gets the base virtualAddress.
		/// </summary>
		/// <value>The base virtualAddress.</value>
		public long BaseAddress
		{
			get { return baseAddress; }
		}

		/// <summary>
		/// Gets the entry point symbol.
		/// </summary>
		/// <value>The entry point symbol.</value>
		public LinkerSymbol EntryPoint
		{
			get { return entryPoint; }
			set { entryPoint = value; }
		}

		/// <summary>
		/// Gets the load alignment of sections.
		/// </summary>
		/// <value>The load alignment.</value>
		public uint LoadSectionAlignment
		{
			get { return loadSectionAlignment; }
			protected set { loadSectionAlignment = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether symbols are resolved.
		/// </summary>
		/// <value>
		///   <c>true</c> if [symbols are resolved]; otherwise, <c>false</c>.
		/// </value>
		public bool SymbolsResolved
		{
			get { return symbolsResolved; }
			protected set { symbolsResolved = value; }
		}

		/// <summary>
		/// Gets or sets the output file of the linker.
		/// </summary>
		/// <value>The output file.</value>
		public string OutputFile
		{
			get { return outputFile; }
			set { outputFile = value; }
		}

		/// <summary>
		/// Retrieves the collection of sections created during compilation.
		/// </summary>
		/// <value>The sections collection.</value>
		public IList<LinkerSectionExtended> Sections { get { return sections; } }

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
		public uint SectionAlignment
		{
			get { return sectionAlignment; }
			protected set { sectionAlignment = value; }
		}

		/// <summary>
		/// Flag is the target platform is little-endian
		/// </summary>
		public Endianness Endianness
		{
			get { return endianness; }
			set { endianness = value; }
		}

		/// <summary>
		/// Gets or sets the machine type (depends on platform)
		/// </summary>
		/// <value>
		/// </value>
		public uint MachineID
		{
			get { return machineID; }
			set { machineID = value; }
		}

		#endregion // Properties

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="BaseLinker"/>.
		/// </summary>
		protected BaseLinker()
		{
			baseAddress = 0x00400000; // Use the Win32 default for now, FIXME
			sectionAlignment = 0x1000; // default 1K
			symbolsResolved = false;
		}

		#endregion // Construction

		#region ILinker Members

		/// <summary>
		/// Executes the linker and generates the final linked file
		/// </summary>
		void ILinker.GeneratedFile()
		{
			// Layout the sections
			LayoutSections();

			// Resolve all symbols 
			Resolve();

			// Persist the file now
			CreateFile();
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
			LinkerSectionExtended linkerSection = (LinkerSectionExtended)GetSection(section);
			Stream stream = linkerSection.Allocate(size, alignment);

			// Create a linker symbol for the name
			LinkerSymbol symbol = new LinkerSymbol(name, section, stream.Position);

			//symbol.VirtualAddress = linkerSection.VirtualAddress + stream.Position;

			Debug.Assert(!symbols.ContainsKey(symbol.Name));

			// Save the symbol for later use
			symbols.Add(symbol.Name, symbol);

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
		/// <param name="symbolName">The name of the symbol to retrieve.</param>
		/// <returns>The named linker symbol.</returns>
		/// <exception cref="System.ArgumentNullException"><paramref name="symbolName"/> is null.</exception>
		/// <exception cref="System.ArgumentException">There's no symbol of the given name.</exception>
		LinkerSymbol ILinker.GetSymbol(string symbolName)
		{
			if (symbolName == null)
				throw new ArgumentNullException(@"symbolName");

			LinkerSymbol result;
			if (!symbols.TryGetValue(symbolName, out result))
				throw new ArgumentException(@"Symbol not compiled.", @"member");

			return result;
		}

		/// <summary>
		/// Determines if a given symbol name is already in use by the linker.
		/// </summary>
		/// <param name="symbolName">The symbol name.</param>
		/// <returns><c>true</c> if the symbol name is already used; <c>false</c> otherwise.</returns>
		bool ILinker.HasSymbol(string symbolName)
		{
			return symbols.ContainsKey(symbolName);
		}

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
		void ILinker.Link(LinkType linkType, Patch[] patches, string symbolName, int symbolOffset, int methodRelativeBase, string targetSymbol, long targetOffset)
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

			list.Add(new LinkRequest(linkType, patches, symbolName, symbolOffset, methodRelativeBase, targetSymbol, targetOffset));
		}

		#endregion //  ILinker Members

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
		protected virtual void LayoutSections()
		{
			return;
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
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		protected void Resolve()
		{
			long address;

			// Check if we have unresolved requests and try to link them
			List<string> members = new List<string>(linkRequests.Keys);
			foreach (string member in members)
			{
				// Is the runtime member resolved?
				if (IsResolved(member, out address))
				{
					// Yes, patch up the method
					List<LinkRequest> link = linkRequests[member];
					PatchRequests(address, link);
					linkRequests.Remove(member);
				}
			}

			Debug.Assert(linkRequests.Count == 0, @"AssemblyLinker has found unresolved symbols.");
			if (linkRequests.Count != 0)
			{
				StringBuilder sb = new StringBuilder();
				sb.AppendLine(@"Unresolved symbols:");
				foreach (string member in linkRequests.Keys)
				{
					sb.AppendFormat("\t{0}\r\n", member);
				}

				throw new LinkerException(sb.ToString());
			}
		}

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
		/// Determines whether the specified symbol is resolved.
		/// </summary>
		/// <param name="symbol">The symbol.</param>
		/// <param name="virtualAddress">The virtualAddress.</param>
		/// <returns>
		///   <c>true</c> if the specified symbol is resolved; otherwise, <c>false</c>.
		/// </returns>
		protected bool IsResolved(string symbol, out long virtualAddress)
		{
			virtualAddress = 0;

			if (!symbolsResolved)
				return false;

			LinkerSymbol linkerSymbol;
			if (symbols.TryGetValue(symbol, out linkerSymbol))
			{
				virtualAddress = linkerSymbol.VirtualAddress;
			}

			return (virtualAddress != 0);
		}

		/// <summary>
		/// Patches all requests in the given link request list.
		/// </summary>
		/// <param name="virtualAddress">The virtualAddress of the member.</param>
		/// <param name="requests">A list of requests to patch.</param>
		private void PatchRequests(long virtualAddress, IEnumerable<LinkRequest> requests)
		{
			long methodAddress;

			foreach (LinkRequest request in requests)
			{
				if (!IsResolved(request.LinkSymbol, out methodAddress))
					throw new InvalidOperationException(@"Method not compiled - but making link requests??");

				// Patch the code stream
				ApplyPatch(
					request.LinkType,
					request.Patches,
					methodAddress,
					request.SymbolOffset,
					request.MethodRelativeBase,
					virtualAddress + request.TargetOffset);
			}
		}

		/// <summary>
		/// A request to patch already emitted code by storing the calculated virtual address value.
		/// </summary>
		/// <param name="linkType">Type of the link.</param>
		/// <param name="patches">The patches.</param>
		/// <param name="methodAddress">The virtual virtualAddress of the method whose code is being patched.</param>
		/// <param name="methodOffset">The value to store at the position in code.</param>
		/// <param name="methodRelativeBase">The method relative base.</param>
		/// <param name="targetAddress">The position in code, where it should be patched.</param>
		/// <exception cref="System.InvalidOperationException"></exception>
		private void ApplyPatch(LinkType linkType, Patch[] patches, long methodAddress, long methodOffset, long methodRelativeBase, long targetAddress)
		{
			if (!SymbolsResolved)
				throw new InvalidOperationException(@"Can't apply patches - symbols not resolved.");

			// Retrieve the text section
			LinkerSectionExtended text = GetSection(SectionKind.Text) as LinkerSectionExtended;

			// Calculate the patch offset
			long offset = (methodAddress - text.VirtualAddress) + methodOffset;

			if ((linkType & LinkType.KindMask) == LinkType.AbsoluteAddress)
			{
				// FIXME: Need a .reloc section with a relocation entry if the module is moved in virtual memory
				// the runtime loader must patch this link request, we'll fail it until we can do relocations.
				//throw new NotSupportedException(@".reloc section not supported.");
			}
			else
			{
				// Change the absolute into a relative offset
				targetAddress = targetAddress - (methodAddress + methodRelativeBase);
			}

			ulong value = Patch.GetResult(patches, (ulong)targetAddress);
			ulong mask = Patch.GetFinalMask(patches);

			// Save the stream position
			text.ApplyPatch(offset, linkType, value, mask, Endianness);
		}

		#endregion // Internals
	}
}
