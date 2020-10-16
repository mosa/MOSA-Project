// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Linker.Elf
{
	/// <summary>
	/// Machine Type
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
		/// ESP32
		/// </summary>
		ESP32 = 999,    // FIXME

		/// <summary>
		/// Intel IA-64 processor architecture
		/// </summary>
		IA_64 = 50,

		/// <summary>
		/// Intel x86_64 processor architecture
		/// </summary>
		x86_64 = 62
	}
}
