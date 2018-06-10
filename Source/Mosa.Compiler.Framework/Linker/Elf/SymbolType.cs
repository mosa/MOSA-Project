// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Linker.Elf
{
	/// <summary>
	/// SymbolType
	/// </summary>
	public enum SymbolType
	{
		NotSpecified = 0,
		Object = 1,
		Function = 2,
		Section = 3,
		File = 4,
		Common = 5,
		TLS = 6,
	}
}
