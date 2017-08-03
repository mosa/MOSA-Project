// Copyright (c) MOSA Project. Licensed under the New BSD License.

using CommandLine;
using Mosa.Compiler.Common;
using Mosa.Utility.BootImage;
using System;
using System.Collections.Generic;
using System.IO;

namespace Mosa.Tool.CreateBootImage
{
	/// <summary>
	///
	/// </summary>
	internal class Program
	{
		public static BootImageOptions Parse(string filename)
		{
			//TODO: The CommandLineParser library doesn't support multiple options with the same name, so multple "--file" doesn't work -> HACK
			string[] lines = File.ReadAllLines(filename);

			List<string> parse = new List<string>();
			List<IncludeFile> files = new List<IncludeFile>();

			foreach(string line in lines)
			{
				string[] parts = line.Split(new char[] { '\n', '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);
				if(line.StartsWith("--file"))
				{
					if (parts.Length > 2)
						files.Add(new IncludeFile(parts[1], parts[2]));
					else
						files.Add(new IncludeFile(parts[1]));
				}
				else
				{
					parse.AddRange(parts);
				}
			}

			Options options = ParseOptions(parse.ToArray());
			if (options == null)
				return null;

			options.BootImageOptions.IncludeFiles = files;
			return options.BootImageOptions;
		}

		private static Options ParseOptions(string[] args)
		{
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

			bool valid = args.Length == 2;

			if (valid)
				valid = System.IO.File.Exists(args[0]);

			if (!valid)
			{
				Console.WriteLine("Usage: CreateBootImage <boot.config file> <image name>");
				Console.Error.WriteLine("ERROR: Missing arguments");
				return -1;
			}

			Console.WriteLine("Building image...");

			try
			{
				var bootImageOptions = Parse(args[0]);

				if (bootImageOptions == null)
				{
					Console.WriteLine("Usage: CreateBootImage <boot.config file> <image name>");
					Console.Error.WriteLine("ERROR: Invalid options");
					return -1;
				}

				bootImageOptions.DiskImageFileName = args[1];

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
