// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.RSP.Command;

public class SetAccessWatchPoint : SetBreakPoint
{
	public SetAccessWatchPoint(ulong address, byte size, CallBack callBack = null) : base(address, size, 4, callBack)
	{
	}
}
