// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.TinyCPUSimulator.x86
{
	public sealed class Legacy8BitHighRegister : Register32Bit
	{
		private GeneralPurposeRegister register;

		public Legacy8BitHighRegister(string name, GeneralPurposeRegister register)
			: base(name, register.Index, register.RegisterType, 8, register.Physical)
		{
			this.register = register;
		}

		public override uint Value { get { return (byte)((register.Value >> 8) & 0xFF); } set { register.Value = (uint)((register.Value & 0xFFFF00FF) | (uint)(value << 8)); } }
	}
}
