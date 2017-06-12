// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.RSP.Command
{
	public class ReadMemory : BaseCommand
	{
		public ulong Address;
		public int Bytes;

		protected override string PackArguments { get { return Address.ToString("x") + "," + Bytes.ToString("x"); } }

		public ReadMemory(ulong address, int bytes, CallBack callBack = null) : base("m", callBack)
		{
			Address = address;
			Bytes = bytes;
		}
	}
}
