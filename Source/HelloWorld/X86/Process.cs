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
	[StructLayout(LayoutKind.Explicit)]
	public struct Process
	{
		public enum State { Empty = 0, Running = 1, Terminating = 2, Terminated = 3 } 

		[FieldOffset(0)]
		public uint ProcessId;
		[FieldOffset(4)]
		public uint MemoryMap;
		[FieldOffset(8)]
		public State Status;
	}
}
