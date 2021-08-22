// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Text;
using System.Threading;

namespace Mosa.Utility.RSP
{
	public abstract class GDBCommand
	{
		public int ID { get; protected set; }

		public string CommandName { get; protected set; }

		public string Pack { get { return CommandName + PackArguments; } }

		protected virtual string PackArguments { get { return string.Empty; } }

		public bool IsResponseOk { get; internal set; } = false;

		public byte[] ResponseData { get; internal set; }

		public string ResponseAsString { get { return Encoding.UTF8.GetString(ResponseData); } }

		public CallBack Callback { get; }

		private static int sequence = 0;

		public override string ToString()
		{
			return $"[{ID}] {CommandName}";
		}

		protected GDBCommand(string commandName, CallBack callBack)
		{
			ID = Interlocked.Increment(ref sequence);
			CommandName = commandName;
			Callback = callBack;
		}

		internal virtual void Decode()
		{
		}

		protected void StandardErrorCheck()
		{
			if (ResponseData.Length >= 1 && ResponseData[0] == 'E')
				IsResponseOk = false;
		}

		public int ReceivedBytes { get { return IsResponseOk ? ResponseData.Length / 2 : 0; } }

		public byte GetByte(int i)
		{
			return GDBClient.HexToDecimal(ResponseData[i * 2], ResponseData[(i * 2) + 1]);
		}

		public ulong GetInteger(int index, uint size)
		{
			index *= 2;
			ulong value = 0;

			for (int i = index + ((int)size * 2) - 2; i >= index; i -= 2)
			{
				var b = GDBClient.HexToDecimal(ResponseData[i], ResponseData[i + 1]);
				value <<= 8;
				value |= b;
			}

			return value;
		}

		public byte[] GetAllBytes()
		{
			var bytes = new byte[ResponseData.Length / 2];

			for (int i = 0; i < bytes.Length; i++)
			{
				bytes[i] = GetByte(i);
			}

			return bytes;
		}
	}
}
