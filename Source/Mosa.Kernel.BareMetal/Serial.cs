// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Kernel.BareMetal
{
	public class Serial
	{
		public static void Write(int serial, byte b)
		{
			Platform.Serial.Write(serial, b);
		}

		public static int Read(int serial)
		{
			return Platform.Serial.Read(serial);
		}

		public static bool IsDataReady(int serial)
		{
			return Platform.Serial.IsDataReady(serial);
		}
	}
}
