// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.TinyCPUSimulator.x86
{
	public sealed class GeneralPurposeRegister : Register32Bit
	{
		public GeneralPurposeRegister(string name, int index)
			: base(name, index, RegisterType.GeneralPurpose, false)
		{
		}
	}
}
