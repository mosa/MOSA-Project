/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (grover) <michael.ruck@michaelruck.de>
 */

using Mosa.Compiler.Linker;

namespace Mosa.Test.System
{
	/// <summary>
	///
	/// </summary>
	public sealed class TestLinkerSection : ExtendedLinkerSection
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="TestLinkerSection" /> class.
		/// </summary>
		/// <param name="kind">The kind of the section.</param>
		/// <param name="name">The name.</param>
		public TestLinkerSection(SectionKind kind, string name) :
			base(kind, name, 0)
		{
			// Allocate 8Mb for this stream
			VirtualMemoryStream vms = new VirtualMemoryStream(1024 * 1024 * 8);
			base.VirtualAddress = vms.Base;
			stream = vms;
		}

		#endregion Construction
	}
}