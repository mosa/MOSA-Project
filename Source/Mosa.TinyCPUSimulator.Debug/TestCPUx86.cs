using Mosa.TinyCPUSimulator.x86;
using Mosa.TinyCPUSimulator.x86.Emulate;

namespace Mosa.TinyCPUSimulator.Debug
{
	internal class TestCPUx86 : BaseSetup<CPUx86>
	{
		public override void Initialize()
		{
			CPU.AddDevice(new PowerUp(CPU));
			CPU.AddDevice(new Multiboot(CPU));
		}

		public void RunTest()
		{
			Add(Opcode.Mov, 1, CPU.EAX, 0x100);
			Add(Opcode.Mov, 1, CPU.EBX, 0x200);
			Add(Opcode.Add, 1, CPU.EAX, CPU.EBX);

			Add(Opcode.Mov, 1, CreateMemoryAddressOperand(32, CPU.EBX, CPU.EAX, 1, 0), CPU.EBX);

			Monitor.AddBreakPoint(Address);

			CPU.Execute();

			return;
		}
	}
}