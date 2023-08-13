// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.DeviceSystem;
using Mosa.DeviceSystem.Service;
using Mosa.Kernel.BareMetal;
using Mosa.Runtime.Plug;

namespace Mosa.BareMetal.HelloWorld;

public static class Program
{
	[Plug("Mosa.Runtime.StartUp::BootOptions")]
	public static void SetBootOptions()
	{
		BootSettings.EnableDebugOutput = true;
		//BootOptions.EnableVirtualMemory = true;
		//BootOptions.EnableMinimalBoot = true;
	}

	public static DeviceService DeviceService { get; private set; }

	public static void EntryPoint()
	{
		Debug.WriteLine("Program::EntryPoint()");
		Debug.WriteLine("##PASS##");

		DeviceService = Kernel.BareMetal.Kernel.ServiceManager.GetFirstService<DeviceService>();

		Console.BackgroundColor = ConsoleColor.Black;
		Console.ForegroundColor = ConsoleColor.White;
		Console.Clear();

		Debug.WriteLine("##PASS##");

		AppManager.Execute("ShowISA");
		AppManager.Execute("ShowPCI");
		AppManager.Execute("ShowDisks");
		AppManager.Execute("ShowFS");
		AppManager.Execute("Shell");

		Console.WriteLine("User has decided to exit out of shell.");
		Console.WriteLine("Shutting down...");

		var pcService = Kernel.BareMetal.Kernel.ServiceManager.GetFirstService<PCService>();
		pcService.Shutdown();

		for (; ; ) HAL.Yield();
	}

	public static void InBrackets(string message, ConsoleColor outerColor, ConsoleColor innerColor)
	{
		var restore = Console.ForegroundColor;
		Console.ForegroundColor = outerColor;
		Console.Write("[");
		Console.ForegroundColor = innerColor;
		Console.Write(message);
		Console.ForegroundColor = outerColor;
		Console.Write("]");
		Console.ForegroundColor = restore;
	}

	public static void Bullet(ConsoleColor color)
	{
		var restore = Console.ForegroundColor;
		Console.ForegroundColor = color;
		Console.Write("*");
		Console.ForegroundColor = restore;
	}
}
