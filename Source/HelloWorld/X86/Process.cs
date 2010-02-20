/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

using System;
using System.Runtime.InteropServices;

using Mosa.Platforms.x86;

namespace Mosa.Kernel.X86
{
	/// <summary>
	/// 
	/// </summary>
	public static class State
	{
		public static readonly int Empty = 0;
		public static readonly int Running = 1;
		public static readonly int Terminating = 2;
		public static readonly int Terminated = 3;
	}

	/// <summary>
	/// 
	/// </summary>
	//[StructLayout(LayoutKind.Explicit)]
	public struct Process
	{
		//[FieldOffset(0)]
		public uint ProcessId;
		//[FieldOffset(4)]
		public uint MemoryMap;
		//[FieldOffset(8)]
		public int Status;
	}
}
