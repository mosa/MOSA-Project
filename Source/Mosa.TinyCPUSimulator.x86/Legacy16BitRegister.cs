/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Diagnostics;

namespace Mosa.TinyCPUSimulator.x86
{
	public class Legacy16BitRegister : Register32Bit
	{
		private GeneralPurposeRegister register;

		public Legacy16BitRegister(string name, GeneralPurposeRegister register)
			: base(name, register.Index, register.RegisterType, 16, register.Physical)
		{
			Debug.Assert(register != null);

			this.register = register;
		}

		public override uint Value { get { return (byte)(register.Value & 0xFFFF); } set { register.Value = (register.Value & 0xFFFF0000) | value; } }
	}
}