// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Text;

namespace Mosa.Utility.RSP
{
	public abstract class BaseCommand
	{
		public string CommandName { get; protected set; }

		public string Pack { get { return CommandName + PackArguments; } }

		protected virtual string PackArguments { get { return string.Empty; } }

		public bool IsResponseOk { get; internal set; } = false;

		public byte[] ResponseData { get; internal set; }

		public string ResponseAsString { get { return Encoding.UTF8.GetString(ResponseData); } }

		public CallBack Callback { get; private set; }

		public BaseCommand(string commandName, CallBack callBack)
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

		public byte GetReceivedByte(int i)
		{
			return GDBClient.HexToDecimal(ResponseData[i * 2], ResponseData[(i * 2) + 1]);
		}

		public ushort GetReceivedWord(int i)
		{
			return (ushort)(GetReceivedByte(i) | (GetReceivedByte(i + 2) << 8));
		}

		public ushort GetReceivedInteger(int i)
		{
			return (ushort)(GetReceivedWord(i) | (GetReceivedWord(i + 3) << 16));
		}

		public ulong GetReceivedLong(int i)
		{
			return (ulong)(GetReceivedInteger(i) | (GetReceivedInteger(i + 7) << 32));
		}
	}
}
