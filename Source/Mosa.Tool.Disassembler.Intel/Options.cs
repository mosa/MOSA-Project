// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using System.IO;

namespace Mosa.Tool.Disassembler.Intel
{
	public class Options
	{
		public string InputFile { get; set; }
		public string OutputFile { get; set; }
		public ulong FileOffset { get; set; }
		public ulong Length { get; set; }
		public ulong StartingAddress { get; set; }

		public Options()
		{
			FileOffset = 0;
			StartingAddress = 0;
		}

		public void LoadArguments(string[] args)
		{
			for (int i = 0; i < args.Length; i++)
			{
				var arg = args[i];

				switch (arg.ToLower())
				{
					case "-offset": FileOffset = args[++i].ParseHexOrDecimal(); continue;
					case "-address": StartingAddress = args[++i].ParseHexOrDecimal(); continue;
					case "-len": StartingAddress = args[++i].ParseHexOrDecimal(); continue;
					case "-o": OutputFile = args[++i].Trim(); continue;
					case "-output": OutputFile = args[++i].Trim(); continue;

					default: break;
				}

				InputFile = Path.Combine(Directory.GetCurrentDirectory(), arg);
			}
		}
	}
}
