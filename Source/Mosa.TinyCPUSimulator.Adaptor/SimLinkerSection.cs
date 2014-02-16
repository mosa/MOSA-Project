/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Linker;
using System.IO;

namespace Mosa.TinyCPUSimulator.Adaptor
{
	/// <summary>
	///
	/// </summary>
	public sealed class SimLinkerSection : ExtendedLinkerSection
	{
		public readonly byte[] Memory;

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="SimLinkerSection" /> class.
		/// </summary>
		/// <param name="kind">The kind of the section.</param>
		/// <param name="name">The name.</param>
		/// <param name="address">The address.</param>
		/// <param name="size">The size.</param>
		public SimLinkerSection(SectionKind kind, string name, uint address, uint size) :
			base(kind, name, 0)
		{
			Memory = new byte[size];

			VirtualAddress = address;

			stream = new MemoryStream(Memory, true);
		}

		#endregion Construction
	}
}