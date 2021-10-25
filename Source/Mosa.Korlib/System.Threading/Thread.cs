// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System.Threading
{
	public enum ThreadStatus { Empty = 0, Running, Terminating, Terminated, Waiting };

	public delegate void ThreadStart();

	public delegate void ParameterizedThreadStart(object obj);
}
