/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (grover) <michael.ruck@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.Linker;

namespace Mosa.Test.System
{
	/// <summary>
	/// A specialized linker for in-memory tests. This linker performs live linking in memory without
	/// respect to an executable format.
	/// </summary>
	/// <remarks>
	/// It is similar to the Jit linker. TODO: Move most of this code to the Jit linker and reuse 
	/// the Jit linker.
	/// </remarks>
	public class TestAssemblyLinker : BaseLinkerStage, IPipelineStage
	{
		#region Data members

		/// <summary>
		/// Holds the linker sections of the assembly linker.
		/// </summary>
		private List<LinkerSection> sections;

		//private readonly AllocateMemoryDelegate allocateMemoryHandler;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="TestAssemblyLinker"/> class.
		/// </summary>
		public unsafe TestAssemblyLinker()
		{
			int maxSections = (int)SectionKind.Max;
			sections = new List<LinkerSection>(maxSections);
			for (int i = 0; i < maxSections; i++)
				sections.Add(new TestLinkerSection((SectionKind)i, String.Empty, IntPtr.Zero));

			//this.allocateMemoryHandler = new AllocateMemoryDelegate(global::Mosa.Test.System.HostedRuntime.AllocateMemory);
		}

		#endregion // Construction

		#region BaseLinkerStage Overrides

		/// <summary>
		/// Gets the load alignment of sections.
		/// </summary>
		/// <value>The load alignment.</value>
		public override long LoadSectionAlignment
		{
			get { return 1; }
		}

		/// <summary>
		/// Gets the virtual alignment of sections.
		/// </summary>
		/// <value>The virtual section alignment.</value>
		public override long VirtualSectionAlignment
		{
			get { return 1; }
		}

		/// <summary>
		/// Retrieves a linker section by its type.
		/// </summary>
		/// <param name="sectionKind">The type of the section to retrieve.</param>
		/// <returns>The retrieved linker section.</returns>
		public override LinkerSection GetSection(SectionKind sectionKind)
		{
			return sections[(int)sectionKind];
		}

		/// <summary>
		/// Retrieves the collection of sections created during compilation.
		/// </summary>
		/// <value>The sections collection.</value>
		public override ICollection<LinkerSection> Sections
		{
			get { return (ICollection<LinkerSection>)sections.AsReadOnly(); }
		}

		/// <summary>
		/// Allocates a symbol of the given name in the specified section.
		/// </summary>
		/// <param name="section">The executable section to allocate from.</param>
		/// <param name="size">The number of bytes to allocate. If zero, indicates an unknown amount of memory is required.</param>
		/// <param name="alignment">The alignment. A value of zero indicates the use of a default alignment for the section.</param>
		/// <returns>
		/// A stream, which can be used to populate the section.
		/// </returns>
		protected override Stream Allocate(SectionKind section, int size, int alignment)
		{
			TestLinkerSection tle = (TestLinkerSection)sections[(int)section];
			return tle.Allocate(size, alignment);
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
		public override Stream Allocate(string name, SectionKind section, int size, int alignment)
		{
			LinkerStream stream = (LinkerStream)base.Allocate(name, section, size, alignment);
			try
			{
				VirtualMemoryStream vms = (VirtualMemoryStream)stream.BaseStream;
				LinkerSymbol symbol = GetSymbol(name);
				symbol.VirtualAddress = new IntPtr(vms.Base.ToInt64() + vms.Position);
				//Trace.WriteLine("Symbol " + name + " located at 0x" + symbol.VirtualAddress.ToInt32().ToString("x08"));
			}
			catch
			{
				stream.Dispose();
				throw;
			}

			return stream;
		}

		/// <summary>
		/// A request to patch already emitted code by storing the calculated virtualAddress value.
		/// </summary>
		/// <param name="linkType">Type of the link.</param>
		/// <param name="methodAddress">The virtual virtualAddress of the method whose code is being patched.</param>
		/// <param name="methodOffset">The value to store at the position in code.</param>
		/// <param name="methodRelativeBase">The method relative base.</param>
		/// <param name="targetAddress">The position in code, where it should be patched.</param>
		protected unsafe override void ApplyPatch(LinkType linkType, long methodAddress, long methodOffset, long methodRelativeBase, long targetAddress)
		{
			long value;
			switch (linkType & LinkType.KindMask)
			{
				case LinkType.RelativeOffset:
					value = targetAddress - (methodAddress + methodRelativeBase);
					break;
				case LinkType.AbsoluteAddress:
					value = targetAddress;
					break;
				default:
					throw new NotSupportedException();
			}
			long address = methodAddress + methodOffset;
			// Position is a raw memory virtualAddress, we're just storing value there
			Debug.Assert(0 != value && value == (int)value);
			int* pAddress = (int*)address;
			*pAddress = (int)value;
		}

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private unsafe delegate void* AllocateMemoryDelegate(uint size);

		protected override void AddVmCalls(IDictionary<string, LinkerSymbol> virtualMachineCalls)
		{
			//AddVmCall(virtualMachineCalls, allocateMemoryHandler, @"Mosa.Internal.Runtime.AllocateMemory(U4 size)");
		}

		protected unsafe void AddVmCall(IDictionary<string, LinkerSymbol> virtualMachineCalls, Delegate handler, string method)
		{
			IntPtr allocate = Marshal.GetFunctionPointerForDelegate(handler);

			long virtualAddress = allocate.ToInt64();
			//Trace.WriteLine(String.Format("\t{0} at 0x{1:x08}", method, virtualAddress));

			LinkerSymbol symbol = new LinkerSymbol(method, SectionKind.Text, virtualAddress);
			symbol.VirtualAddress = new IntPtr(symbol.SectionAddress);

			virtualMachineCalls.Remove(method);
			virtualMachineCalls.Add(method, symbol);
		}

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		public override void Run()
		{
			// Adjust the symbol addresses
			// __grover, 01/02/2009: Copied from ObjectFileLayoutStage
			foreach (LinkerSymbol symbol in Symbols)
			{
				LinkerSection ls = GetSection(symbol.Section);
				symbol.Offset = ls.Offset + symbol.SectionAddress;
				symbol.VirtualAddress = new IntPtr(ls.VirtualAddress.ToInt64() + symbol.SectionAddress);
			}

			// Now run the linker
			base.Run();
		}

		#endregion // BaseLinkerStage Overrides
	}
}
