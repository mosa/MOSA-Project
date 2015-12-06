// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;

namespace Mosa.TinyCPUSimulator.x86
{
	public sealed class Legacy16BitRegister : Register32Bit
	{
		private GeneralPurposeRegister register;

		public Legacy16BitRegister(string name, GeneralPurposeRegister register)
			: base(name, register.Index, register.RegisterType, 16, register.Physical)
		{
			Debug.Assert(register != null);

			this.register = register;
		}

		public override uint Value { get { return (ushort)(register.Value & 0xFFFF); } set { register.Value = (register.Value & 0xFFFF0000) | value; } }
	}
}
