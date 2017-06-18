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
	}
}
