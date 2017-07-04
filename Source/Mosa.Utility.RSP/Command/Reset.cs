// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.RSP.Command
{
	public class Reset : GDBCommand
	{
		protected override string PackArguments { get { return "0"; } }

		public Reset() : base("R", null)
		{
		}
	}
}
