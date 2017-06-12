// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.RSP.Command
{
	public class SetBreakPoint : BaseCommand
	{
		public ulong Address;

		protected override string PackArguments { get { return Address.ToString("x") + ",S"; } }

		public SetBreakPoint(ulong address, bool set, CallBack callBack = null) : base("B", callBack)
		{
			Address = address;
		}
	}
}
