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
using Mosa.Compiler.Linker;

namespace Mosa.Test.System
{
	/// <summary>
	/// A specialized linker for in-memory tests. This linker performs live linking in memory without
	/// respect to an executable format.
	/// </summary>
	public class TestLinker : BaseLinker
	{

		#region Data members

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="TestLinker"/> class.
		/// </summary>
		public TestLinker()
		{
			for (int i = 0; i < (int)SectionKind.Max; i++)
				Sections.Add(new TestLinkerSection((SectionKind)i, String.Empty, 0));

			LoadSectionAlignment = 1;
			SectionAlignment = 1;
		}

		#endregion // Construction

		#region BaseLinkerStage Overrides

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
			return ((TestLinkerSection)GetSection(section)).Allocate(size, alignment);
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

			VirtualMemoryStream vms = (VirtualMemoryStream)stream.BaseStream;
			LinkerSymbol symbol = GetSymbol(name);
			symbol.VirtualAddress = vms.Base + vms.Position;

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
		/// <exception cref="System.NotSupportedException"></exception>
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

		#endregion // BaseLinkerStage Overrides
	}
}
