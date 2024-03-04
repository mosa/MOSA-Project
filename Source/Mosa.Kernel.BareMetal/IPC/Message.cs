// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Kernel.BareMetal.IPC;

public class Message
{
	public object Data;
	public Thread Thread;

	public Message(object requestData, Thread thread = null)
	{
		Data = requestData;
		Thread = thread;
	}
}
