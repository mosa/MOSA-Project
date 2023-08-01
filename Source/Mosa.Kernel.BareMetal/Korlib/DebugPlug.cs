// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.Plug;

namespace Mosa.Kernel.BareMetal.Korlib;

public class DebugPlug
{
	[Plug("System.Debug::WriteLine")]
	public static void WriteLine(string message)
	{
		Debug.WriteLine("DEBUG: ");
		Debug.WriteLine(message);
	}

	[Plug("System.Debug::Write")]
	public static void Write(string message)
	{
		Debug.Write(message);
	}
}
