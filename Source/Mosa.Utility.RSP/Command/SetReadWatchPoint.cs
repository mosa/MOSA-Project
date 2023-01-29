﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.RSP.Command;

public class SetReadWatchPoint : SetBreakPoint
{
	public SetReadWatchPoint(ulong address, byte size, CallBack callBack = null) : base(address, size, 3, callBack)
	{
	}
}
