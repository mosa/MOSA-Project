
using System;

namespace Mosa.Compiler.Metadata
{

	[Flags]
	public enum EventAttributes : ushort
	{
		None = 0x0000,
		SpecialName = 0x0200,
		RTSpecialName = 0x0400
	}
}
