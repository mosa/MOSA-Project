/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.TinyCPUSimulator.x86
{
	public class Legacy8BitLowRegister : Register32Bit
	{
		private GeneralPurposeRegister register;

		public Legacy8BitLowRegister(string name, GeneralPurposeRegister register)
			: base(name, register.Index, register.RegisterType, 8, register.Physical)
		{
			this.register = register;
		}

		public override uint Value { get { return (byte)(register.Value & 0xFF); } set { register.Value = (register.Value & 0xFFFFFF00) | value; } }
	}
}