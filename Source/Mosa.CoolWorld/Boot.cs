/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

using System;

using Mosa.Platform.x86.Intrinsic;
using Mosa.Kernel;
using Mosa.Kernel.x86;
using Mosa.DeviceSystem;

namespace Mosa.CoolWorld
{
	/// <summary>
	/// 
	/// </summary>
	public static class Boot
	{
		/// <summary>
		/// Main
		/// </summary>
		public static void Main()
		{
			Mosa.Kernel.x86.Kernel.Setup();

			Console.Initialize();

			Screen.GotoTop();
			Screen.Color = Colors.White;
			Screen.BackgroundColor = Colors.Green;

			Screen.Write(@"                   MOSA OS Version 0.1 - Compiler Version 1.0");
			Screen.FillLine();
			Screen.Color = Colors.White;
			Screen.BackgroundColor = Colors.Black;

			Console.WriteLine("> Initializing kernel...");
			BulletPoint(); Console.Write("Reading multiboot header..."); PrintDone();
			BulletPoint(); Console.Write("Programmable Interrupt Controller..."); PrintDone();
			BulletPoint(); Console.Write("Global Descriptor Table..."); PrintDone();
			BulletPoint(); Console.Write("Interrupt Descriptor Table..."); PrintDone();
			BulletPoint(); Console.Write("Memory..."); PrintDone();
			BulletPoint(); Console.Write("Virtual Paging..."); PrintDone();
			BulletPoint(); Console.Write("Process Manager..."); PrintDone();
			BulletPoint(); Console.Write("Task Manager..."); PrintDone();

			Mosa.Kernel.x86.Smbios.BiosInformationStructure biosInfo = new Kernel.x86.Smbios.BiosInformationStructure();
			Mosa.Kernel.x86.Smbios.CpuStructure cpuInfo = new Kernel.x86.Smbios.CpuStructure();

			Console.WriteLine("> Checking bios...");
			BulletPoint(); Console.Write("Vendor  "); InBrackets(biosInfo.BiosVendor, Colors.White, Colors.LightBlue); Console.WriteLine();
			BulletPoint(); Console.Write("Version "); InBrackets(biosInfo.BiosVersion, Colors.White, Colors.LightBlue); Console.WriteLine();

			Console.WriteLine("> Checking cpu...");
			BulletPoint(); Console.Write("Vendor  "); InBrackets(cpuInfo.Vendor, Colors.White, Colors.LightBlue); Console.WriteLine();
			BulletPoint(); Console.Write("Version "); InBrackets(cpuInfo.Version, Colors.White, Colors.LightBlue); Console.WriteLine();

			Console.WriteLine("> Initializing hardware abstraction layer...");
			Setup.Initialize();

			Console.WriteLine("> Adding hardware devices...");
			Setup.Start();

			Console.WriteLine("> System ready");

			Screen.Goto(24, 0);
			Screen.Color = Colors.White;
			Screen.BackgroundColor = Colors.Green;
			Screen.Write("          Copyright (C) 2008-2001 [Managed Operating System Alliance]");
			Screen.FillLine();

			Console.Update();

			while (true)
			{
			}
		}

		public static void PrintDone()
		{
			InBrackets("Done", Colors.White, Colors.LightGreen); Console.WriteLine();
		}

		public static void BulletPoint()
		{
			Screen.Color = Colors.Yellow;
			Console.Write("  * ");
			Screen.Color = Colors.White;
		}

		public static void InBrackets(string message, byte outerColor, byte innerColor)
		{
			Screen.Color = outerColor;
			Console.Write("[");
			Screen.Color = innerColor; 
			Console.Write(message);
			Screen.Color = outerColor; 
			Console.Write("]"); 
		}
	}
}
