// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.RSP.Command
{
	public class ReadMemory : BaseCommand
	{
		public ulong Address { get; private set; }
		public int SentBytes { get; private set; }

		protected override string PackArguments { get { return Address.ToString("x") + "," + SentBytes.ToString("x"); } }

		public ReadMemory(ulong address, int bytes, CallBack callBack = null) : base("m", callBack)
		{
			Address = address;
			SentBytes = bytes;
		}

		internal override void Decode()
		{
			StandardErrorCheck();

			if (!IsResponseOk)
				return;
		}
	}
}
