// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.RSP.Command
{
	public class ClearWriteWatchPoint : SetBreakPoint
	{
		public ClearWriteWatchPoint(ulong address, byte size, CallBack callBack = null) : base(address, size, 2, callBack)
		{
		}
	}
}
