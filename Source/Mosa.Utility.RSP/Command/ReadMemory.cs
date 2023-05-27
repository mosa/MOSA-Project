// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.RSP.Command;

public class ReadMemory : GDBCommand
{
	public ulong Address { get; }

	public uint SentBytes { get; }

	protected override string PackArguments => $"{Address:x},{SentBytes:x}";

	public ReadMemory(ulong address, uint bytes, CallBack callBack = null) : base("m", callBack)
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
