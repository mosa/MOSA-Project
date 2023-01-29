// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.RSP.Command;

public class GetRegisters : GDBCommand
{
	public GetRegisters(CallBack callBack = null) : base("g", callBack)
	{
	}

	internal override void Decode()
	{
		StandardErrorCheck();

		if (!IsResponseOk)
			return;
	}
}
