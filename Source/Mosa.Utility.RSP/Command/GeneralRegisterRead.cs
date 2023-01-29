// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.RSP.Command;

public class GeneralRegisterRead : GDBCommand
{
	public GeneralRegisterRead(CallBack callBack = null) : base("g", callBack)
	{
	}
}
