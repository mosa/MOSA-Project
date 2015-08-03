// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.TinyCPUSimulator
{
	public class SimRegister
	{
		public int Index { get; private set; }

		public RegisterType RegisterType { get; private set; }

		public bool Physical { get; private set; }

		public int Size { get; private set; }

		public string Name { get; private set; }

		public SimRegister(string name, int index, RegisterType registerType, int size, bool physical)
		{
			this.Index = index;
			this.RegisterType = registerType;
			this.Size = size;
			this.Physical = physical;
			this.Name = name;
		}

		public SimRegister(string name, int index, RegisterType registerType, int size)
			: this(name, index, registerType, size, false)
		{
		}

		public override string ToString()
		{
			return Name;
		}
	}
}