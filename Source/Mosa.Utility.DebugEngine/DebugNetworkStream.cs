/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;

namespace Mosa.Utility.DebugEngine
{
	public class DebugNetworkStream : NetworkStream
	{
		public DebugNetworkStream(Socket socket, bool ownsSocket) :
			base(socket, ownsSocket)
		{
		}

		public bool IsConnected { get { return this.Socket.Connected; } }
	}
}
