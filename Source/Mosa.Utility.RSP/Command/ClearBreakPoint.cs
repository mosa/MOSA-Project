﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.RSP.Command;

public class ClearBreakPoint : GDBCommand
{
	protected byte Type;

	public ulong Address { get; }
	public ulong Size { get; }

	protected override string PackArguments { get { return Type.ToString() + "," + Address.ToString("x") + "," + Size.ToString("x"); } }

	public ClearBreakPoint(ulong address, byte size, byte type, CallBack callBack = null) : base("z", callBack)
	{
		Address = address;
		Size = size;
		Type = type;
	}
}
