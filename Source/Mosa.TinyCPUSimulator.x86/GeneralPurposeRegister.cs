// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.TinyCPUSimulator.x86
{
	public class GeneralPurposeRegister : Register32Bit
	{
		public GeneralPurposeRegister(string name, int index)
			: base(name, index, RegisterType.GeneralPurpose, false)
		{
		}
	}
}
