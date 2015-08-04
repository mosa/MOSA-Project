// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.TinyCPUSimulator
{
	public class Register32Bit : SimRegister
	{
		public virtual uint Value { get; set; }

		public Register32Bit(string name, int index, RegisterType registerType, int size, bool physical)
			: base(name, index, registerType, size, physical)
		{
		}

		public Register32Bit(string name, int index, RegisterType registerType, bool physical)
			: base(name, index, registerType, 32, physical)
		{
		}

		public Register32Bit(string name, int index, RegisterType registerType)
			: base(name, index, registerType, 32, true)
		{
		}

		public bool GetBit(int bit)
		{
			return (Value & (1 << bit)) != 0;
		}

		public void SetBit(byte bit, bool value)
		{
			if (value)
			{
				Value |= (uint)(1 << bit);
			}
			else
			{
				Value &= (uint)(~(1 << bit));
			}
		}

		public override string ToString()
		{
			return base.ToString() + " = 0x" + Value.ToString("X");
		}
	}
}
