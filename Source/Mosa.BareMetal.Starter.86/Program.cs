// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.Kernel.BareMetal;

namespace Mosa.BareMetal.Starter.x86;

public static class Program
{
	public static void Main()
	{
		Debug.WriteLine("##PASS##");

		Console.BackgroundColor = ConsoleColor.Black;
		Console.ForegroundColor = ConsoleColor.Yellow;
		Console.Clear();
		Console.WriteLine("Welcome to the MOSA Project!");

		// Add your code here!
	}
}
