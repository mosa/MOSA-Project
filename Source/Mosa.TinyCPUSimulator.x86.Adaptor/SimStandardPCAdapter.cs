/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.TinyCPUSimulator.Adaptor;
using Mosa.TinyCPUSimulator.x86.Emulate;

namespace Mosa.TinyCPUSimulator.x86.Adaptor
{
	public class SimStandardPCAdapter : SimAdapter
	{
		public SimStandardPCAdapter(ISimDisplay simDisplay)
		{
			CPU.AddDevice(new PowerUp(CPU));
			CPU.AddDevice(new CMOS(CPU));
			CPU.AddDevice(new VGAConsole(CPU, simDisplay));
			CPU.AddDevice(new Multiboot(CPU));
			CPU.AddDevice(new MosaKernel(CPU));
			CPU.AddDevice(new MosaImage(CPU));
		}
	}
}