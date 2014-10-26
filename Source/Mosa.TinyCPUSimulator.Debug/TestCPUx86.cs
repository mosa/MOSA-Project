/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *
 */

using Mosa.TinyCPUSimulator.x86;
using Mosa.TinyCPUSimulator.x86.Emulate;

namespace Mosa.TinyCPUSimulator.Debug
{
	internal class TestCPUx86 : BaseSetup<CPUx86>
	{
		public TestCPUx86()
		{
			CPU.AddDevice(new PowerUp(CPU));
			CPU.AddDevice(new Multiboot(CPU));
			//CPU.AddDevice(new BochDebug(CPU, System.Console.Out));
		}

		public void RunTest()
		{
			Add(Opcode.Mov, 1, CPU.EAX, 0x100);
			Add(Opcode.Mov, 1, CPU.EBX, 0x200);
			Add(Opcode.Add, 1, CPU.EAX, CPU.EBX);

			Add(Opcode.Mov, 1, CreateMemoryAddressOperand(32, CPU.EBX, CPU.EAX, 1, 0), CPU.EBX);

			CPU.Monitor.AddBreakPoint(Address);

			CPU.Execute();

			return;
		}
	}
}