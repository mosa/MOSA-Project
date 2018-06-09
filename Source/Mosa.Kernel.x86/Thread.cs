// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Kernel.x86
{
	public enum ThreadStatus { Empty = 0, Running, Terminating, Terminated, Waiting };

	internal class Thread
	{
		public ThreadStatus Status = ThreadStatus.Empty;
		public IntPtr StackTop;
		public IntPtr StackBottom;
		public IntPtr StackStatePointer;
		public uint Ticks;
	}
}
