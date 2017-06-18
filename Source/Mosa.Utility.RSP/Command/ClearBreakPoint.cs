// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.RSP.Command
{
	public class ClearBreakPoint : BaseCommand
	{
		protected byte Type;

		public ulong Address { get; private set; }
		public ulong Size { get; private set; }

		protected override string PackArguments { get { return Type.ToString() + "," + Address.ToString("x") + "," + Size.ToString("x"); } }

		public ClearBreakPoint(ulong address, byte size, byte type, CallBack callBack = null) : base("z", callBack)
		{
			Address = address;
			Size = size;
			Type = type;
		}
	}
}
