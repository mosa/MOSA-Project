/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Linker;
using Mosa.Compiler.Linker.PE;
using Mosa.Compiler.Linker.Elf32;
using Mosa.Compiler.Linker.Elf64;

namespace Mosa.Compiler.Framework.Linker
{
	/// <summary>
	/// 
	/// </summary>
	public static class LinkerFactory
	{
		public static ILinker Create(LinkerType linkerType, CompilerOptions compilerOptions, IArchitecture architecture)
		{
			ILinker linker = Create(linkerType);

			// setup all the common attributes
			linker.OutputFile = compilerOptions.OutputFile;
			linker.MachineID = architecture.ElfMachineType;
			linker.Endianness = architecture.Endianness;

			// TODO: add specific options based on type

			return linker;
		}

		private static ILinker Create(LinkerType linkerType)
		{
			switch (linkerType)
			{
				case LinkerType.PE: return new PELinker();
				case LinkerType.Elf32: return new Elf32Linker();
				case LinkerType.Elf64: return new Elf64Linker();
				default: throw new LinkerException("unknown linker type");
			}
		}
	}
}
