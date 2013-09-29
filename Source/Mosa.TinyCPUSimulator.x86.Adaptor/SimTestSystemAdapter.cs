/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.TypeSystem;
using Mosa.Platform.x86;
using Mosa.TinyCPUSimulator.Adaptor;
using Mosa.TinyCPUSimulator.x86.Emulate;
using System.Collections.Generic;

namespace Mosa.TinyCPUSimulator.x86.Adaptor
{
	public class SimTestSystemAdapter : SimAdapter
	{
		public SimTestSystemAdapter()
		{
		}

		public override void Initialize()
		{
			base.Initialize();

			ulong freeMemPtr = 0x21700000;
			ulong freeMem = 0xF0000000;

			CPU.AddMemory(freeMemPtr, 0x0000000F, 1); // Must match Mosa.Kernel.Test.KernelMemory

			CPU.AddMemory(freeMem, 0x200000, 1);

			CPU.Write32(freeMemPtr, (uint)freeMem);
		}

	}
}