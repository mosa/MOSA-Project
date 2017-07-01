// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.RSP.Command
{
	public class ClearSoftwareBreakPoint : ClearBreakPoint
	{
		public ClearSoftwareBreakPoint(ulong address, byte size, CallBack callBack = null) : base(address, size, 0, callBack)
		{
		}
	}
}
