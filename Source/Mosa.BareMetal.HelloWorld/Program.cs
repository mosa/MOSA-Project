// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.DeviceSystem;
using Mosa.DeviceSystem.HardwareAbstraction;
using Mosa.DeviceSystem.Services;
using Mosa.Kernel.BareMetal;
using Mosa.Runtime.Plug;

namespace Mosa.BareMetal.HelloWorld;

public static class Program
{
	public static DeviceService DeviceService { get; private set; }

	public static void EntryPoint()
	{
		Debug.WriteLine("Program::EntryPoint()");
		Debug.WriteLine("##PASS##");

		DeviceService = Kernel.BareMetal.Kernel.ServiceManager.GetFirstService<DeviceService>();

		Console.BackgroundColor = ConsoleColor.Black;
		Console.ForegroundColor = ConsoleColor.White;
		Console.Clear();

		AppManager.Execute("ShowISA");
		AppManager.Execute("ShowPCI");
		AppManager.Execute("ShowDisks");
		AppManager.Execute("ShowFS");
		AppManager.Execute("Shell");

		Console.WriteLine("User has decided to exit out of shell.");
		Console.WriteLine("Shutting down...");

		var pcService = Kernel.BareMetal.Kernel.ServiceManager.GetFirstService<PCService>();
		var success = pcService.Shutdown();

		if (!success) Console.WriteLine("Error while trying to shut down.");

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

	[Plug("Mosa.Runtime.StartUp::BootOptions")]
	public static void SetBootOptions()
	{
		BootSettings.EnableDebugOutput = true;
	}
}
