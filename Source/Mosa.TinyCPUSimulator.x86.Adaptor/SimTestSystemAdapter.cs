// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.TinyCPUSimulator.x86.Emulate;

namespace Mosa.TinyCPUSimulator.x86.Adaptor
{
	public class SimTestSystemAdapter : SimAdapter
	{
		public SimTestSystemAdapter()
		{
			CPU.AddDevice(new PowerUp(CPU));
			CPU.AddDevice(new CMOS(CPU));
			CPU.AddDevice(new Multiboot(CPU));
			CPU.AddDevice(new MosaTestMemory(CPU));
			CPU.AddDevice(new MosaImage(CPU));

			//CPU.AddDevice(new BochDebug(CPU));
			//CPU.AddDevice(new MosaDebug(CPU));
		}
	}
}
