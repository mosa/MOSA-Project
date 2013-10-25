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
	public class SimStandardPCAdapter : SimAdapter
	{
		public SimStandardPCAdapter()
		{
		}

		public override void Initialize()
		{
			var primaryDisplay = new DisplayForm(800, 600);
			primaryDisplay.Show();
			primaryDisplay.StartTimer();

			CPU.AddDevice(new PowerUp(CPU));
			CPU.AddDevice(new CMOS(CPU));
			CPU.AddDevice(new VGAConsole(CPU, primaryDisplay));
			CPU.AddDevice(new Multiboot(CPU));
			
			CPU.AddDevice(new MosaKernel(CPU));
		}

	}
}