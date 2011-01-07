/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.Linker.Elf32
{
	/// <summary>
	/// 
	/// </summary>
	[Flags]
	public enum Elf32ProgramHeaderFlags : uint
	{
		/// <summary>
		/// 
		/// </summary>
		Execute = 0x1,
		/// <summary>
		/// 
		/// </summary>
		Write = 0x2,
		/// <summary>
		/// 
		/// </summary>
		Read = 0x4,
		/// <summary>
		/// 
		/// </summary>
		MaskProc = 0xF0000000,
	}
}
