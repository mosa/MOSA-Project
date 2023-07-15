// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Net.Sockets;

namespace Mosa.Utility.UnitTests.DebugEngine;

public class DebugNetworkStream : NetworkStream
{
	public DebugNetworkStream(Socket socket, bool ownsSocket) :
		base(socket, ownsSocket)
	{
		socket.NoDelay = true;
	}

	public bool IsConnected => Socket.Connected;
}
