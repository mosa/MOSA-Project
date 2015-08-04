// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.TinyCPUSimulator.x86
{
	// http://en.wikipedia.org/wiki/FLAGS_register

	public class FlagsRegister : Register32Bit
	{
		public FlagsRegister()
			: base("FLAGS", 0, RegisterType.StatusFlag, false)
		{
		}

		public bool Carry { get { return GetBit(0); } set { SetBit(0, value); } }

		public bool Parity { get { return GetBit(2); } set { SetBit(2, value); } }

		public bool Adjust { get { return GetBit(4); } set { SetBit(4, value); } }

		public bool Zero { get { return GetBit(6); } set { SetBit(6, value); } }

		public bool Sign { get { return GetBit(7); } set { SetBit(7, value); } }

		public bool TrapFlag { get { return GetBit(8); } set { SetBit(8, value); } }

		public bool InterruptEnable { get { return GetBit(9); } set { SetBit(9, value); } }

		public bool Direction { get { return GetBit(10); } set { SetBit(10, value); } }

		public bool Overflow { get { return GetBit(11); } set { SetBit(11, value); } }
	}
}
