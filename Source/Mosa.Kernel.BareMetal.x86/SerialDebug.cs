// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Kernel.BareMetal.x86;

public static class SerialDebug
{
	public static void Setup() => Serial.Setup(Serial.COM2);

	public static void Write(byte c) => Serial.Write(Serial.COM2, c);
}
