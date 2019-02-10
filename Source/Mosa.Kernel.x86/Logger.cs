// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.x86;

namespace Mosa.Kernel.x86
{
	/// <summary>
	/// Kernel log. Char only.
	/// </summary>
	public static class Logger
	{
		private static bool initialized = false;

		public static void Log(string message)
		{
			if (!initialized)
				Serial.SetupPort(Serial.COM1);

			Serial.Write(Serial.COM1, message);
			Serial.Write(Serial.COM1, "\n");
		}
	}
}
