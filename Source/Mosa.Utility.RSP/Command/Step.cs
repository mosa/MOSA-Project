// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.RSP.Command
{
	public class Step : GDBCommand
	{
		public Step(CallBack callBack = null) : base("s", callBack)
		{
		}
	}
}
