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
	public class GeneralPurposeRegister : Register32Bit
	{
		public GeneralPurposeRegister(string name, int index)
			: base(name, index, RegisterType.GeneralPurpose, false)
		{
		}
	}
}