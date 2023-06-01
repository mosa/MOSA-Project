// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Tool.Debugger.GDB;

public enum RegisterType
{ Int, Float };

public class RegisterDefinition
{
	public string Name { get; set; }

	public int Index { set; get; }

	public int Offset { set; get; }

	public uint Size { set; get; }

	public bool Display { set; get; }

	public RegisterType RegisterType { get; set; }

	public RegisterDefinition(int index, string name, uint size, int offset, bool display = false, RegisterType registerType = RegisterType.Int)
	{
		Index = index;
		Name = name;
		Size = size;
		Offset = offset;
		Display = display;
		RegisterType = registerType;
	}
}
