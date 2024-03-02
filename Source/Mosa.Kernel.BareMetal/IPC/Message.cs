// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Kernel.BareMetal.IPC;

public class Message
{
	public uint ReceiverID;
	public uint SenderID;

	public object Data;
}
