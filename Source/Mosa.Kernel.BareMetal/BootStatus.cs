// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Kernel.BareMetal;

public static class BootStatus
{
	public static bool IsGCEnabled;

	public static void Initalize()
	{
		IsGCEnabled = false;
	}
}
