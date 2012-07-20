using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mosa.Utility.DebugEngine
{
	public class Message
	{
		public int ID { get; internal set; }
		public int Code { get; private set; }
		public byte[] CommandData { get; private set; }
		public byte[] ResponseData { get; internal set; }
		public object Other { get; private set; }

		public int Checksum
		{
			get
			{
				// TODO
				return 0;
			}
		}

		public Message(int code, byte[] data)
		{
			this.Code = code;
			this.CommandData = data;
		}

		public Message(int code, byte[] data, object other)
			: this(code, data)
		{
			this.Other = other;
		}
	}
}
