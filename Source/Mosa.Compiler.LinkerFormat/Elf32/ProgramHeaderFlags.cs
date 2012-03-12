/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

using System;

namespace Mosa.Compiler.LinkerFormat.Elf32
{
	/// <summary>
	/// 
	/// </summary>
	[Flags]
	public enum ProgramHeaderFlags : uint
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
