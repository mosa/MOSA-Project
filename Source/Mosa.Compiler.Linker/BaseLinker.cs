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
using Mosa.Compiler.LinkerFormat.Elf;

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
		private readonly Dictionary<string, List<LinkRequest>> linkRequests;

		/// <summary>
		/// Holds the output file of the linker.
		/// </summary>
		private string outputFile;

		/// <summary>
		/// A dictionary containing all symbol seen in the assembly.
		/// </summary>
		private readonly Dictionary<string, LinkerSymbol> symbols;
	
		/// <summary>
		/// Flag is the target platform is little-endian
		/// </summary>
		protected bool IsLittleEndian;

		/// <summary>
		/// 
		/// </summary>
		protected MachineType Machine;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="BaseLinker"/>.
		/// </summary>
		protected BaseLinker()
		{
			baseAddress = 0x00400000; // Use the Win32 default for now, FIXME
			linkRequests = new Dictionary<string, List<LinkRequest>>();
			symbols = new Dictionary<string, LinkerSymbol>();
		}

		#endregion // Construction

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		public virtual void Run()
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

		#region Methods

		/// <summary>
		/// A request to patch already emitted code by storing the calculated virtualAddress value.
		/// </summary>
		/// <param name="linkType">Type of the link.</param>
		/// <param name="methodAddress">The method address.</param>
		/// <param name="methodOffset">The value to store at the position in code.</param>
		/// <param name="methodRelativeBase">The method relative base.</param>
		/// <param name="targetAddress">The position in code, where it should be patched.</param>
		protected abstract void ApplyPatch(LinkType linkType, long methodAddress, long methodOffset, long methodRelativeBase, long targetAddress);

		#endregion // Methods

		#region ILinker Members

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
		public abstract long LoadSectionAlignment
		{
			get;
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
		public abstract ICollection<LinkerSection> Sections
		{
			get;
		}

		/// <summary>
		/// Retrieves the collection of _symbols known by the linker.
		/// </summary>
		/// <value>The symbol collection.</value>
		public ICollection<LinkerSymbol> Symbols
		{
			get { return symbols.Values; }
		}

		/// <summary>
		/// Gets the virtual alignment of sections.
		/// </summary>
		/// <value>The virtual section alignment.</value>
		public abstract long VirtualSectionAlignment
		{
			get;
		}

		/// <summary>
		/// Allocates a symbol of the given name in the specified section.
		/// </summary>
		/// <param name="name">The name of the symbol.</param>
		/// <param name="section">The executable section to allocate From.</param>
		/// <param name="size">The number of bytes to allocate. If zero, indicates an unknown amount of memory is required.</param>
		/// <param name="alignment">The alignment. A value of zero indicates the use of a default alignment for the section.</param>
		/// <returns>
		/// A stream, which can be used to populate the section.
		/// </returns>
		public virtual Stream Allocate(string name, SectionKind section, int size, int alignment)
		{
			try
			{
				Stream baseStream = Allocate(section, size, alignment);

				// Create a linker symbol for the name
				LinkerSymbol symbol = new LinkerSymbol(name, section, baseStream.Position);
				
				// Save the symbol for later use
				if (!symbols.ContainsKey(symbol.Name)) // FIXME: Remove this line when generic patch is fixed! It duplicates generic types
					symbols.Add(symbol.Name, symbol);

				// Wrap the stream to catch premature disposal
				Stream result = new LinkerStream(symbol, baseStream, size);

				return result;
			}
			catch (ArgumentException argx)
			{
				throw new LinkerException(String.Format(@"Symbol {0} defined multiple times.", name), argx);
			}

		}

		/// <summary>
		/// Allocates a symbol of the given name in the specified section.
		/// </summary>
		/// <param name="section">The executable section to allocate From.</param>
		/// <param name="size">The number of bytes to allocate. If zero, indicates an unknown amount of memory is required.</param>
		/// <param name="alignment">The alignment. A value of zero indicates the use of a default alignment for the section.</param>
		/// <returns>
		/// A stream, which can be used to populate the section.
		/// </returns>
		protected abstract Stream Allocate(SectionKind section, int size, int alignment);

		/// <summary>
		/// Gets the section.
		/// </summary>
		/// <param name="sectionKind">Kind of the section.</param>
		/// <returns>The section of the requested kind.</returns>
		public abstract LinkerSection GetSection(SectionKind sectionKind);

		/// <summary>
		/// Retrieves a linker symbol.
		/// </summary>
		/// <param name="symbolName">The name of the symbol to retrieve.</param>
		/// <returns>The named linker symbol.</returns>
		/// <exception cref="System.ArgumentNullException"><paramref name="symbolName"/> is null.</exception>
		/// <exception cref="System.ArgumentException">There's no symbol of the given name.</exception>
		public LinkerSymbol GetSymbol(string symbolName)
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
		public bool HasSymbol(string symbolName)
		{
			return symbols.ContainsKey(symbolName);
		}

		/// <summary>
		/// Issues a linker request for the given runtime method.
		/// </summary>
		/// <param name="linkType">The type of link required.</param>
		/// <param name="symbolName">The method the patched code belongs to.</param>
		/// <param name="methodOffset">The offset inside the method where the patch is placed.</param>
		/// <param name="methodRelativeBase">The base virtualAddress, if a relative link is required.</param>
		/// <param name="targetSymbol">The linker symbol to link against.</param>
		/// <param name="offset">An offset to apply to the link target.</param>
		/// <returns>
		/// The return value is the preliminary virtualAddress to place in the generated machine
		/// code. On 32-bit systems, only the lower 32 bits are valid. The above are not used. An implementation of
		/// IAssemblyLinker may not rely on 64-bits being stored in the memory defined by position.
		/// </returns>
		public virtual void Link(LinkType linkType, string symbolName, int methodOffset, int methodRelativeBase, string targetSymbol, IntPtr offset)
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

			list.Add(new LinkRequest(linkType, symbolName, methodOffset, methodRelativeBase, targetSymbol, offset));
		}

		#endregion // ILinker Members

		#region Internals

		/// <summary>
		/// Determines whether the specified symbol is resolved.
		/// </summary>
		/// <param name="symbol">The symbol.</param>
		/// <param name="virtualAddress">The virtualAddress.</param>
		/// <returns>
		/// 	<c>true</c> if the specified symbol is resolved; otherwise, <c>false</c>.
		/// </returns>
		protected virtual bool IsResolved(string symbol, out long virtualAddress)
		{
			virtualAddress = 0;
			LinkerSymbol linkerSymbol;
			if (symbols.TryGetValue(symbol, out linkerSymbol))
			{
				virtualAddress = linkerSymbol.VirtualAddress.ToInt64();
			}

			return (0 != virtualAddress);
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
					methodAddress,
					request.MethodOffset,
					request.MethodRelativeBase,
					virtualAddress + request.Offset.ToInt64()
				);
			}
		}

		#endregion // Internals
	}
}
