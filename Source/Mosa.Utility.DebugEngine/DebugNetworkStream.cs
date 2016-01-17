// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Net.Sockets;

namespace Mosa.Utility.DebugEngine
{
	public class DebugNetworkStream : NetworkStream
	{
		public DebugNetworkStream(Socket socket, bool ownsSocket) :
			base(socket, ownsSocket)
		{
		}

		public bool IsConnected { get { return Socket.Connected; } }
	}
}
