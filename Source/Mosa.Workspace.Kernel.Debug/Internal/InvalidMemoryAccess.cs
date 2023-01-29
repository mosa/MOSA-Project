// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Workspace.Kernel.Internal;

public class InvalidMemoryAccess : Exception
{
	public ulong Address { get; private set; }

	public InvalidMemoryAccess(ulong address)
	{
		Address = address;
	}

	public override string ToString()
	{
		return "Invalid Memory Access at 0x" + Address.ToString("X");
	}
}
