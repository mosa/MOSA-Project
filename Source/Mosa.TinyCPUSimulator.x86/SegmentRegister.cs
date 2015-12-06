// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.TinyCPUSimulator.x86
{
	public sealed class SegmentRegister : Register32Bit
	{
		public SegmentRegister(string name, int index)
			: base(name, index, RegisterType.SegmentRegister, false)
		{
		}
	}
}
