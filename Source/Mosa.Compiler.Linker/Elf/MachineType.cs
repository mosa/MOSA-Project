// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Linker.Elf
{
	/// <summary>
	///
	/// </summary>
	public enum MachineType : ushort
	{
		/// <summary>
		/// No machine
		/// </summary>
		NoMachine = 0x0,

		/// <summary>
		/// Intel Architecture
		/// </summary>
		Intel386 = 0x3,

		/// <summary>
		/// Advanced RISC Machines ARM
		/// </summary>
		ARM = 40,

		/// <summary>
		/// Intel IA-64 processor architecture
		/// </summary>
		IA_64 = 50
	}
}
