// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Tool.GDBDebugger.GDB
{
	public enum RegisterType { Int, Float };

	public class RegisterDefinition
	{
		public string Name { get; set; }
		public int Index { set; get; }
		public int Offset { set; get; }
		public int Size { set; get; }

		public RegisterDefinition(int index, string name, int size, int offset)
		{
			Index = index;
			Name = name;
			Size = size;
			Offset = offset;
		}
	}
}
