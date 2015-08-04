// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.TinyCPUSimulator.x86
{
	public class ControlRegister : Register32Bit
	{
		public ControlRegister(string name, int index)
			: base(name, index, RegisterType.ControlRegister, false)
		{
		}
	}
}
