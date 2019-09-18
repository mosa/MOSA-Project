// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.Kernel.x86
{
	public enum ThreadStatus { Empty = 0, Running, Terminating, Terminated, Waiting };

	internal class Thread
	{
		public ThreadStatus Status = ThreadStatus.Empty;
		public Pointer StackTop;
		public Pointer StackBottom;
		public Pointer StackStatePointer;
		public uint Ticks;
	}
}
