/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

using Mosa.Runtime;
using Mosa.Runtime.Linker;
using Mosa.Runtime.Vm;
using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Metadata;

namespace Test.Mosa.Runtime.CompilerFramework.BaseCode
{
    /// <summary>
    /// A specialized linker for in-memory tests. This linker performs live linking in memory without
    /// respect to an executable format.
    /// </summary>
    /// <remarks>
    /// It is similar to the Jit linker. TODO: Move most of this code to the Jit linker and reuse 
    /// the Jit linker.
    /// </remarks>
	public class TestAssemblyLinker : AssemblyLinkerStageBase, IPipelineStage
    {
        #region Data members

        /// <summary>
        /// Holds the linker sections of the assembly linker.
        /// </summary>
        private List<LinkerSection> _sections;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="TestAssemblyLinker"/> class.
        /// </summary>
        public TestAssemblyLinker()
        {
            int maxSections = (int)SectionKind.Max;
            _sections = new List<LinkerSection>(maxSections);
            for (int i = 0; i < maxSections; i++)
                _sections.Add(new TestLinkerSection((SectionKind)i, String.Empty, IntPtr.Zero));
        }

        #endregion // Construction

		#region IPipelineStage

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		string IPipelineStage.Name { get { return @"Test Linker"; } }

		#endregion // IPipelineStage Members

        #region AssemblyLinkerStageBase Overrides

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
            return _sections[(int)sectionKind];
        }

        /// <summary>
        /// Retrieves the collection of sections created during compilation.
        /// </summary>
        /// <value>The sections collection.</value>
        public override ICollection<LinkerSection> Sections
        {
            get { return (ICollection<LinkerSection>)_sections.AsReadOnly(); }
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
            TestLinkerSection tle = (TestLinkerSection)_sections[(int)section];
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
                LinkerSymbol symbol = this.GetSymbol(name);
                symbol.VirtualAddress = new IntPtr(vms.Base.ToInt64() + vms.Position);
            }
            catch
            {
                stream.Dispose();
                throw;
            }

            return stream;
        }

        /// <summary>
        /// Allocates memory in the specified section.
        /// </summary>
        /// <param name="member">The metadata member to allocate space for.</param>
        /// <param name="section">The executable section to allocate from.</param>
        /// <param name="size">The number of bytes to allocate. If zero, indicates an unknown amount of memory is required.</param>
        /// <param name="alignment">The alignment. A value of zero indicates the use of a default alignment for the section.</param>
        /// <returns>
        /// A stream, which can be used to populate the section.
        /// </returns>
        public override Stream Allocate(RuntimeMember member, SectionKind section, int size, int alignment)
        {
            string name = CreateSymbolName(member);

            LinkerStream stream = (LinkerStream)base.Allocate(name, section, size, alignment);
            try
            {
                VirtualMemoryStream vms = (VirtualMemoryStream)stream.BaseStream;

                // Save the member address
                member.Address = new IntPtr(vms.Base.ToInt64() + vms.Position);

                LinkerSymbol symbol = this.GetSymbol(name);
                symbol.VirtualAddress = member.Address;
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

        [UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        private delegate IntPtr AllocateDelegate(int module, TokenTypes token, int count);

        protected override void AddVmCalls(IDictionary<string, LinkerSymbol> virtualMachineCalls)
        {
            Trace.WriteLine(@"TestAssemblyLinker adding VM calls:");
            long baseAddress = this._sections[(int)SectionKind.Text].VirtualAddress.ToInt64();

            AllocateDelegate handler = new AllocateDelegate(RuntimeBase.AllocateArray);
            IntPtr allocate = Marshal.GetFunctionPointerForDelegate(handler);

            const string allocateMethod = @"Mosa.Runtime.RuntimeBase.AllocateArray(I4 moduleLoadIndex,ValueType token,I4 elements)";
            long virtualAddress = allocate.ToInt64();
            Trace.WriteLine(String.Format("\t{0} at 0x{1:x08}", allocateMethod, virtualAddress));

            LinkerSymbol symbol = new LinkerSymbol(allocateMethod, SectionKind.Text, virtualAddress);
            symbol.VirtualAddress = new IntPtr(symbol.SectionAddress);
            virtualMachineCalls.Add(allocateMethod, symbol);
        }

        /// <summary>
        /// Performs stage specific processing on the compiler context.
        /// </summary>
        public override void Run()
        {
            // Adjust the symbol addresses
            // __grover, 01/02/2009: Copied from ObjectFileLayoutStage
            foreach (LinkerSymbol symbol in this.Symbols)
            {
                LinkerSection ls = GetSection(symbol.Section);
                symbol.Offset = ls.Offset + symbol.SectionAddress;
                symbol.VirtualAddress = new IntPtr(ls.VirtualAddress.ToInt64() + symbol.SectionAddress);
            }

            // Now run the linker
            base.Run();
        }

        #endregion // AssemblyLinkerStageBase Overrides
    }
}
