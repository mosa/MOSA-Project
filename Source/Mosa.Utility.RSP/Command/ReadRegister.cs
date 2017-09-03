// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.RSP.Command
{
	public class ReadRegister : GDBCommand
	{
		public int Register { get; }

		protected override string PackArguments { get { return Register.ToString("x"); } }

		public ReadRegister(int register, CallBack callBack = null) : base("p", callBack)
		{
			Register = register;
		}

		internal override void Decode()
		{
			StandardErrorCheck();

			if (!IsResponseOk)
				return;
		}
	}
}
