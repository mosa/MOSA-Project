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
using System.Linq;
using System.Text;

namespace Mosa.Utility.DebugEngine
{
	public delegate void SenderMesseageDelegate(DebugMessage response);

	public class DebugMessage
	{
		public int ID { get; internal set; }
		public int Code { get; private set; }
		public byte[] CommandData { get; private set; }
		public byte[] ResponseData { get; internal set; }
		public object Other { get; private set; }

		public object Sender { get; protected set; }
		public SenderMesseageDelegate SenderMethod { get; protected set; }

		public int Checksum { get { return 0; } } // TODO

		public DebugMessage(int code, byte[] data)
		{
			this.Code = code;
			this.CommandData = data;
		}

		public DebugMessage(int code, int[] data)
		{
			this.Code = code;
			this.CommandData = new byte[data.Length * 4];

			int index = 0;
			foreach (int i in data)
			{
				this.CommandData[index++] = (byte)((i >> 24) & 0xFF);
				this.CommandData[index++] = (byte)((i >> 16) & 0xFF);
				this.CommandData[index++] = (byte)((i >> 8) & 0xFF);
				this.CommandData[index++] = (byte)(i & 0xFF);
			}
		}

		public DebugMessage(int code, byte[] data, object sender, SenderMesseageDelegate senderMethod)
			: this(code, data)
		{
			this.Sender = sender;
			this.SenderMethod = senderMethod;
		}

		public DebugMessage(int code, int[] data, object sender, SenderMesseageDelegate senderMethod)
			: this(code, data)
		{
			this.Sender = sender;
			this.SenderMethod = senderMethod;
		}

		public int GetInt32(int index)
		{
			return (ResponseData[index] << 24) | (ResponseData[index + 1] << 16) | (ResponseData[index + 2] << 8) | (ResponseData[index + 3]);
		}

		public uint GetUInt32(int index)
		{
			return (uint)((ResponseData[index] << 24) | (ResponseData[index + 1] << 16) | (ResponseData[index + 2] << 8) | (ResponseData[index + 3]));
		}

	}
}
