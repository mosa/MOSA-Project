// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.Plug;

namespace Mosa.Kernel.BareMetal.Korlib;

public class EnvironmentPlug
{
	[Plug("System.Environment::FailFast")]
	public static void FailFast(string message)
	{
		Debug.WriteLine("*** FailFast ***");
		Debug.WriteLine(message);
		Debug.Fatal();
	}

	[Plug("System.Environment::Exit")]
	public static void Exit(int exitCode) => Debug.Fatal();

	[Plug("System.Environment::GetProcessorCount")]
	public static int GetProcessorCount() => 1;
}
