// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.RSP.Command;

public class ClearHardwareBreakPoint : ClearBreakPoint
{
	public ClearHardwareBreakPoint(ulong address, byte size, CallBack callBack = null) : base(address, size, 1, callBack)
	{
	}
}
