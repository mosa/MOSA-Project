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
	public static class Codes
	{
		public const int Connecting = 10;
		public const int Connected = 11;
		public const int Disconnected = 12;

		public const int UnknownData = 99;

		public const int Alive = 1000;
		public const int Ping = 1001;
		public const int InformationalMessage = 1002;
		public const int WarningMessage = 1003;
		public const int ErrorMessage = 1004;
		public const int SendNumber = 1005;
		public const int ReadMemory = 1010;
		public const int WriteMemory = 1011;
		public const int ReadCR3 = 1012;
		public const int Scattered32BitReadMemory = 1013;

	}
}