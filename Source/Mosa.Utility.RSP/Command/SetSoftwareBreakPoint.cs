// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.RSP.Command
{
	public class SetSoftwareBreakPoint : SetBreakPoint
	{
		public SetSoftwareBreakPoint(ulong address, byte size, CallBack callBack = null) : base(address, size, 0, callBack)
		{
		}
	}
}
