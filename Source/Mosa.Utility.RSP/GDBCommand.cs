// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Text;

namespace Mosa.Utility.RSP
{
	public abstract class GDBCommand
	{
		public string CommandName { get; protected set; }

		public string Pack { get { return CommandName + PackArguments; } }

		protected virtual string PackArguments { get { return string.Empty; } }

		public bool IsResponseOk { get; internal set; } = false;

		public byte[] ResponseData { get; internal set; }

		public string ResponseAsString { get { return Encoding.UTF8.GetString(ResponseData); } }

		public CallBack Callback { get; private set; }

		public GDBCommand(string commandName, CallBack callBack)
		{
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

		public ulong GetInteger(int index, int size)
		{
			index = index * 2;
			ulong value = 0;

			for (int i = index + (size * 2) - 2; i >= index; i = i - 2)
			{
				var b = GDBClient.HexToDecimal(ResponseData[i + 1], ResponseData[i]);
				value = value << 8;
				value = value | b;
			}

			return value;
		}
	}
}
