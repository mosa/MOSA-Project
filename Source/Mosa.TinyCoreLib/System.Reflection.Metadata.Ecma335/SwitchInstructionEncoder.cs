using System.Runtime.InteropServices;

namespace System.Reflection.Metadata.Ecma335;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public readonly struct SwitchInstructionEncoder
{
	public void Branch(LabelHandle label)
	{
	}
}
