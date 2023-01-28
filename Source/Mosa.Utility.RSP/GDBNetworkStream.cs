// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Net.Sockets;

namespace Mosa.Utility.RSP;

public class GDBNetworkStream : NetworkStream
{
	public GDBNetworkStream(Socket socket, bool ownsSocket) :
		base(socket, ownsSocket)
	{
		socket.NoDelay = true;
	}

	public bool IsConnected { get { return Socket.Connected; } }
}
