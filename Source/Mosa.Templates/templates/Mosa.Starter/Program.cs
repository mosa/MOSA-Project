// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.Kernel.BareMetal;

namespace Mosa.Starter;

public static class Program
{
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
