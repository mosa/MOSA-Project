// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Kernel.x86
{
	public enum ThreadStatus { Empty = 0, Running, Terminating, Terminated, Waiting };

	internal class Thread
	{
		public ThreadStatus Status = ThreadStatus.Empty;
		public uint StackTop;
		public uint StackBottom;
		public uint StackStatePointer;
		public uint Ticks;
	}
}
