// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.RSP.Command
{
	public class Continue : GDBCommand
	{
		public Continue(CallBack callBack = null) : base("c", callBack)
		{
		}
	}
}
