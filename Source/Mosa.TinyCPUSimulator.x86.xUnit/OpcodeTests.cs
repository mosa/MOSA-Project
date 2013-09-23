/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.TinyCPUSimulator.x86;
using Mosa.TinyCPUSimulator.x86.Emulate;
using Xunit;
using Xunit.Extensions;
	 
namespace Mosa.TinyCPUSimulator.x86.xUnit
{
	public class OpcodeTests : BaseSetup<CPUx86>
	{
		public override void Initialize()
		{
			CPU.AddMemory(0x400000, 0x100000, 2);  // 4-5Mb reserved

			CPU.AddDevice(new PowerUp(CPU));
			CPU.AddDevice(new Multiboot(CPU));
		}
		
		[Theory]
		[InlineData((uint)10, uint.MaxValue)]
		[InlineData((uint)0, (uint)0)]
		[InlineData((uint)200, (uint)100)]
		public void AddU4U4(uint a, uint b)
		{
			Add(Opcode.Mov, 1, CPU.EAX, a);
			Add(Opcode.Mov, 1, CPU.EBX, b);
			Add(Opcode.Add, 1, CPU.EAX, CPU.EBX);

			Monitor.AddBreakPoint(Address);

			CPU.Execute();

			ulong r = (ulong)a + (ulong)b;

			Assert.Equal(CPU.EAX.Value, (a + b) & uint.MaxValue);
			Assert.Equal(CPU.FLAGS.Carry, r > uint.MaxValue);
			Assert.False(CPU.FLAGS.Sign);
		}

		[Theory]
		[InlineData((uint)10, uint.MaxValue)]
		[InlineData((uint)0, (uint)0)]
		[InlineData((uint)200, (uint)100)]
		public void MulU4U4(uint a, uint b)
		{
			Add(Opcode.Mov, 1, CPU.EAX, a);
			Add(Opcode.Mov, 1, CPU.EBX, b);
			Add(Opcode.Mul, 1, CPU.EBX);

			Monitor.AddBreakPoint(Address);

			CPU.Execute();

			ulong r = (ulong)a * (ulong)b;

			Assert.Equal(CPU.EAX.Value, r & uint.MaxValue);
		}

		[Theory]
		[InlineData((int)10)]
		[InlineData((int)0)]
		[InlineData((int)200)]
		[InlineData((int)-2025)]
		public void Cvtsi2sd(int a)
		{
			Add(Opcode.Mov, 1, CPU.EAX, a);
			Add(Opcode.Cvtsi2sd, 1, CPU.XMM0, CPU.EAX);

			Monitor.AddBreakPoint(Address);

			CPU.Execute();

			Assert.Equal(CPU.XMM0.Value, a);
		}

		[Theory]
		[InlineData((int)10)]
		[InlineData((int)0)]
		[InlineData((int)200)]
		[InlineData((int)-2025)]
		public void Cvtsi2sdAndCvtsd2si(int a)
		{
			Add(Opcode.Mov, 1, CPU.EAX, a);
			Add(Opcode.Cvtsi2sd, 1, CPU.XMM0, CPU.EAX);
			Add(Opcode.Cvttsd2si, 1, CPU.EBX, CPU.XMM0);

			Monitor.AddBreakPoint(Address);

			CPU.Execute();

			Assert.Equal(CPU.EBX.Value, (uint)a);
		}
	}
}