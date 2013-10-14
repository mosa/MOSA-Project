/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

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

		[Theory]
		[InlineData((int)10, int.MaxValue)]
		[InlineData((int)0, (int)0)]
		[InlineData((int)0, (int)1)]
		[InlineData((int)1, (int)0)]
		[InlineData((int)200, (int)100)]
		[InlineData((int)100, (int)200)]
		[InlineData((int)-200, (int)100)]
		[InlineData((int)-100, (int)200)]
		[InlineData((int)200, (int)-100)]
		[InlineData((int)100, (int)-200)]
		[InlineData((int)int.MaxValue, (int)int.MaxValue)]
		[InlineData((int)int.MaxValue, (int)int.MinValue)]
		[InlineData((int)int.MinValue, (int)int.MinValue)]
		[InlineData((int)int.MaxValue, (int)int.MaxValue)]
		public void CmpSigned(int a, int b)
		{
			Add(Opcode.Mov, 1, CPU.EAX, a);
			Add(Opcode.Mov, 1, CPU.EBX, b);
			Add(Opcode.Cmp, 1, CPU.EAX, CPU.EBX);

			Monitor.AddBreakPoint(Address);

			CPU.Execute();

			long r = (long)a - (long)b;

			bool sign = (a - b) < 0;
			bool zero = (a - b) == 0;
			bool overflow = r < int.MinValue || r > int.MaxValue;
			//			bool carry = r > int.MaxValue;

			Assert.True(CPU.FLAGS.Sign == sign, "Expected: Sign = " + sign.ToString());
			Assert.True(CPU.FLAGS.Zero == zero, "Expected: Zero = " + zero.ToString());
			Assert.True(CPU.FLAGS.Overflow == overflow, "Expected: Overflow = " + overflow.ToString());
			//			Assert.True(CPU.FLAGS.Carry == carry, "Expected: Carry = " + carry.ToString());
		}

		[Theory]
		[InlineData((uint)10, uint.MaxValue)]
		[InlineData((uint)0, (uint)0)]
		[InlineData((uint)0, (uint)1)]
		[InlineData((uint)1, (uint)0)]
		[InlineData((uint)200, (uint)100)]
		[InlineData((uint)100, (uint)200)]
		[InlineData((uint)uint.MaxValue, (uint)uint.MaxValue)]
		[InlineData((uint)uint.MaxValue, (uint)uint.MinValue)]
		[InlineData((uint)uint.MinValue, (uint)uint.MinValue)]
		[InlineData((uint)uint.MaxValue, (uint)uint.MaxValue)]
		public void CmpUnsigned(uint a, uint b)
		{
			Add(Opcode.Mov, 1, CPU.EAX, a);
			Add(Opcode.Mov, 1, CPU.EBX, b);
			Add(Opcode.Cmp, 1, CPU.EAX, CPU.EBX);

			Monitor.AddBreakPoint(Address);

			CPU.Execute();

			ulong r = (ulong)a - (ulong)b;

			bool zero = (a - b) == 0;
			bool carry = r > uint.MaxValue;

			Assert.True(CPU.FLAGS.Zero == zero, "Expected: Zero = " + zero.ToString());
			Assert.True(CPU.FLAGS.Carry == carry, "Expected: Carry = " + carry.ToString());
		}

		[Theory]
		[InlineData((uint)10, (uint)0, false)]
		[InlineData((uint)0, (uint)1, true)]
		[InlineData((uint)0, (uint)0, false)]
		[InlineData((uint)200, (uint)1, false)]
		[InlineData((uint)200, (uint)31, false)]
		[InlineData((uint)uint.MaxValue, (uint)3, false)]
		[InlineData((uint)uint.MaxValue, (uint)3, true)]
		[InlineData((uint)uint.MaxValue, (uint)31, true)]
		public void RcrU4U4(uint a, uint b, bool carry)
		{
			Add(Opcode.Mov, 1, CPU.EAX, a);
			if (carry) Add(Opcode.Stc, 1);
			Add(Opcode.Rcr, 1, CPU.EAX, b);

			Monitor.AddBreakPoint(Address);

			CPU.Execute();

			uint u = a;

			for (int i = 0; i < b; i++)
			{
				bool c = (u & 0x1) == 1;
				u = u >> 1;
				if (carry)
					u = u | ((uint)1 << 31);
				carry = c;
			}

			Assert.Equal(CPU.EAX.Value, u);
		}
	}
}