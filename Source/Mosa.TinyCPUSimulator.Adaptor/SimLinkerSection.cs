﻿/*
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
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="SimLinkerSection" /> class.
		/// </summary>
		/// <param name="kind">The kind of the section.</param>
		/// <param name="name">The name.</param>
		/// <param name="address">The address.</param>
		/// <param name="size">The size.</param>
		/// <param name="simAdapter">The sim adapter.</param>
		public SimLinkerSection(SectionKind kind, string name, uint address, uint size, ISimAdapter simAdapter) :
			base(kind, name, 0)
		{
			byte[] ram = new byte[size];

			RAMBank rambank = new RAMBank(ram, address, size, 1);

			simAdapter.SimCPU.AddMemory(rambank);

			VirtualAddress = address;
			stream = new MemoryStream(ram, true);
		}

		#endregion Construction
	}
}