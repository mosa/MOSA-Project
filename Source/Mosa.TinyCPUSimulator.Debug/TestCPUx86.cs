// Copyright (c) MOSA Project. Licensed under the New BSD License.

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
			Add(Opcode.Mov, 32, CPU.EAX, 0x100, 1);
			Add(Opcode.Mov, 32, CPU.EBX, 0x200, 1);
			Add(Opcode.Add, 32, CPU.EAX, CPU.EBX, 1);

			Add(Opcode.Mov, 32, CreateMemoryAddressOperand(32, CPU.EBX, CPU.EAX, 1, 0), CPU.EBX, 1);

			CPU.Monitor.AddBreakPoint(Address);

			CPU.Execute();

			return;
		}
	}
}