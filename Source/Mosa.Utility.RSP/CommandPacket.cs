// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.RSP
{
	public abstract class CommandPacket
	{
		public string Command { get; private set; }

		public string Pack { get { return Command + PackArguments; } }

		protected virtual string PackArguments { get { return string.Empty; } }

		public CommandPacket(string command)
		{
			this.Command = command;
		}
	}
}
