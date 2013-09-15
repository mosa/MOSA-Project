/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.TinyCPUSimulator
{
	public class RegisterFloatingPoint : SimRegister
	{
		public virtual double Value { get; set; }

		public RegisterFloatingPoint(string name, int index, RegisterType registerType, bool physical)
			: base(name, index, registerType, 128, physical)
		{
		}

		public RegisterFloatingPoint(string name, int index, RegisterType registerType)
			: base(name, index, registerType, 128, true)
		{
		}

		public override string ToString()
		{
			return base.ToString() + " = 0x" + Value.ToString("D");
		}
	}
}