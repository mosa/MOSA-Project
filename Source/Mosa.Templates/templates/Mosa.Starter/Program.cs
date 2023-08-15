// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.Kernel.BareMetal;

namespace Mosa.Starter;

public static class Program
{
	[Plug("Mosa.Runtime.StartUp::BootOptions")]
	public static void SetBootOptions()
	{
		BootSettings.EnableDebugOutput = true;
		//BootSettings.EnableVirtualMemory = true;
		//BootSettings.EnableMinimalBoot = true;
	}

	public static void EntryPoint()
	{
		Debug.WriteLine("Program::EntryPoint()");

		Console.ResetColor();
		Console.Clear();
		Console.WriteLine("Hello World!");

		for (; ; )
		{ }
	}
}
