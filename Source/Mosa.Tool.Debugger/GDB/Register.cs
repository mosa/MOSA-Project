// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Tool.Debugger.GDB
{
	public class Register
	{
		public RegisterDefinition Definition { get; private set; }

		public string Name { get { return Definition.Name; } }

		public uint Size { get { return Definition.Size; } }

		public ulong Value { get; private set; }
		public ulong ValueExtended { get; private set; }

		public Register(RegisterDefinition definition, ulong value)
		{
			Definition = definition;
			Value = value;
		}

		public string ToHex()
		{
			return BasePlatform.ToHex(Value, Size);
		}
	}
}
