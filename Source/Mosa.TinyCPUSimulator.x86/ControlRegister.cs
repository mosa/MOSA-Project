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
	public class ControlRegister : Register32Bit
	{
		public ControlRegister(string name, int index)
			: base(name, index, RegisterType.ControlRegister, false)
		{
		}
	}
}