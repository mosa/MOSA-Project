/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (grover) <michael.ruck@michaelruck.de>
 */

using System;
using System.IO;

using Mosa.Compiler.Linker;

namespace Mosa.Test.System
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class TestLinkerSection : LinkerSectionExtended
	{

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="TestLinkerSection"/> class.
		/// </summary>
		/// <param name="kind">The kind of the section.</param>
		/// <param name="name">The name.</param>
		/// <param name="address">The address.</param>
		public TestLinkerSection(SectionKind kind, string name, long address) :
			base(kind, name, address)
		{
			// Allocate 8Mb for this stream
			VirtualMemoryStream vms = new VirtualMemoryStream(1024 * 1024 * 8);
			base.VirtualAddress = vms.Base;
			stream = vms;
		}

		#endregion // Construction

		#region Methods

		/// <summary>
		/// Allocates a stream of the specified size from the section.
		/// </summary>
		/// <param name="size">The size.</param>
		/// <param name="alignment">The alignment.</param>
		/// <returns></returns>
		public override Stream Allocate(int size, int alignment)
		{
			return stream;
		}

		#endregion // Methods

	}
}
