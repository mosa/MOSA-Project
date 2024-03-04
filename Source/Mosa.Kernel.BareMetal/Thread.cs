﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Threading;
using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal;

public class Thread
{
	public readonly uint Index;

	public uint ID;

	public ThreadStatus Status = ThreadStatus.Empty;
	public Pointer StackTop;
	public Pointer StackBottom;
	public Pointer StackStatePointer;
	public uint Sequence = 0;
	public uint Ticks = 0;

	public Thread(uint index)
	{
		Index = index;
	}
}
