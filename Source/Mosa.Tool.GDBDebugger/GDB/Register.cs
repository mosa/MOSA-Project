// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Tool.GDBDebugger.GDB
{
	public class Register
	{
		public RegisterDefinition Definition { get; private set; }

		public string Name { get { return Definition.Name; } }

		public int Size { get { return Definition.Size; } }

		public ulong Value { get; private set; }
		public ulong ValueExtended { get; private set; }

		public Register(RegisterDefinition definition, ulong value)
		{
			Definition = definition;
			Value = value;
		}

		public string ToHex()
		{
			switch (Size)
			{
				case 1: return "0x" + ((uint)Value).ToString("X2");
				case 2: return "0x" + ((uint)Value).ToString("X4");
				case 4: return "0x" + ((uint)Value).ToString("X8");
				case 8: return "0x" + ((ulong)Value).ToString("X16");
				default: return "N/A";
			}
		}
	}
}
