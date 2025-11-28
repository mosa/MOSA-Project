// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.DeviceSystem.HardwareAbstraction;
using Mosa.DeviceSystem.Services;
using Mosa.Kernel.BareMetal;
using Mosa.Runtime.Plug;

namespace Mosa.BareMetal.CoolWorld.Console;

public static class ConsoleMode
{
	public static DeviceService DeviceService { get; private set; }

	public static void Initialize()
	{
		Debug.WriteLine("ConsoleMode::Initialize()");

		DeviceService = Kernel.BareMetal.Kernel.ServiceManager.GetFirstService<DeviceService>();

		System.Console.BackgroundColor = ConsoleColor.Black;
		System.Console.ForegroundColor = ConsoleColor.White;
		System.Console.Clear();

		AppManager.Execute("ShowISA");
		AppManager.Execute("ShowPCI");
		AppManager.Execute("ShowDisks");
		AppManager.Execute("ShowFS");
		AppManager.Execute("Shell");

		System.Console.WriteLine("User has decided to exit out of shell.");
		System.Console.WriteLine("Shutting down...");

		var pcService = Kernel.BareMetal.Kernel.ServiceManager.GetFirstService<PCService>();
		var success = pcService.Shutdown();

		if (!success) System.Console.WriteLine("Error while trying to shut down.");

		for (; ; ) HAL.Yield();
	}

	public static void InBrackets(string message, ConsoleColor outerColor, ConsoleColor innerColor)
	{
		var restore = System.Console.ForegroundColor;
		System.Console.ForegroundColor = outerColor;
		System.Console.Write("[");
		System.Console.ForegroundColor = innerColor;
		System.Console.Write(message);
		System.Console.ForegroundColor = outerColor;
		System.Console.Write("]");
		System.Console.ForegroundColor = restore;
	}

	public static void Bullet(ConsoleColor color)
	{
		var restore = System.Console.ForegroundColor;
		System.Console.ForegroundColor = color;
		System.Console.Write("*");
		System.Console.ForegroundColor = restore;
	}
}
