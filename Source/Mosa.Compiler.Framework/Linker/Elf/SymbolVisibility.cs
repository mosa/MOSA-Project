// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Linker.Elf
{
	/// <summary>
	/// SymbolVisibility
	/// </summary>
	public enum SymbolVisibility
	{
		Default = 0,
		Internal = 1,
		Hidden = 2,
		Protected = 3,
		Exported = 4,
		Singleton = 5,
		Eliminate = 6,
	}
}
