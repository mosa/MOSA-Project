// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Utility.RSP
{
	public abstract class BaseCommand
	{
		public string Command { get; private set; }

		public string Pack { get { return Command + PackArguments; } }

		protected virtual string PackArguments { get { return string.Empty; } }

		public List<byte> ResponseData { get; internal set; }

		public CallBack Callback { get; private set; }

		public BaseCommand(string command, CallBack callBack)
		{
			Command = command;
			Callback = callBack;
			ResponseData = new List<byte>();
		}
	}
}
