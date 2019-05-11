// Copyright (c) MOSA Project. Licensed under the New BSD License.

using CommandLine;
using Mosa.Utility.BootImage;
using System;
using System.IO;

namespace Mosa.Tool.CreateBootImage
{
	/// <summary>
	/// Mosa.Tool.CreateBootImage
	/// </summary>
	internal static class Program
	{
		private static string UsageString;

		static Program()
		{
			UsageString = @"Example: Mosa.Tool.CreateBootImage.exe -o Mosa.HelloWorld.x86.img --mbr ../Tools/syslinux/3.72/mbr.bin --boot ../Tools/syslinux/3.72/ldlinux.bin --volume-label MOSABOOT --blocks 25000 --filesystem fat16 --syslinux --img ../Tools/syslinux/3.72/ldlinux.sys ../Tools/syslinux/3.72/mboot.c32 ../Demos/syslinux.cfg Mosa.HelloWorld.x86.bin,main.exe";
		}

		private static Options ParseOptions(string[] args)
		{
			if (args.Length >= 2 && args[0] == "--configfile")
				return ParseOptions(File.ReadAllText(args[1]).Split(new char[] { '\n', '\r', ' ' }, StringSplitOptions.RemoveEmptyEntries));

			ParserResult<Options> result = new Parser(config => config.HelpWriter = Console.Out).ParseArguments<Options>(args);

			if (result.Tag == ParserResultType.NotParsed)
			{
				return null;
			}

			return ((Parsed<Options>)result).Value;
		}

		/// <summary>
		/// Main
		/// </summary>
		/// <param name="args">The args.</param>
		/// <returns></returns>
		private static int Main(string[] args)
		{
			Console.WriteLine();
			Console.WriteLine("CreateBootImage v1.6 [www.mosa-project.org]");
			Console.WriteLine("Copyright 2015. New BSD License.");
			Console.WriteLine("Written by Philipp Garcia (phil@thinkedge.com)");
			Console.WriteLine();

			try
			{
				var opt = ParseOptions(args);
				var bootImageOptions = opt?.BootImageOptions;

				if (bootImageOptions == null)
				{
					Console.WriteLine(UsageString);
					return -1; //Errors will be printed by the command line library
				}

				Console.WriteLine(opt.ToString());

				Console.WriteLine("Building image...");

				Generator.Create(bootImageOptions);

				Console.WriteLine("Completed!");
			}
			catch (Exception e)
			{
				Console.Error.WriteLine("ERROR: " + e.ToString());
				return -1;
			}

			return 0;
		}
	}
}
