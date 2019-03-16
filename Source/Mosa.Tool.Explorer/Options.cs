// Copyright (c) MOSA Project. Licensed under the New BSD License.

using CommandLine;
using System.Collections.Generic;

namespace Mosa.Tool.Explorer
{
	internal class Options
	{
		[Option("inline-off")]
		public bool InlineOff { get; set; }

		[Option("threading-off")]
		public bool ThreadingOff { get; set; }

		[Option("no-code")]
		public bool NoCode { get; set; }

		[Option("no-ssa")]
		public bool NoSSA { get; set; }

		[Option("no-ir-optimizations")]
		public bool NoIROptimizations { get; set; }

		[Option("no-sparse")]
		public bool NoSparse { get; set; }

		[Option("scanner")]
		public bool EnableMethodScanner { get; set; }

		[Option("x64")]
		public bool X64 { get; set; }

		[Option("x86")]
		public bool X86 { get; set; }

		[Option("esp32")]
		public bool ESP32 { get; set; }

		[Option("armv6")]
		public bool ARMv6 { get; set; }

		[Option("filter")]
		public string Filter { get; set; }

		[Value(0)]
		public IEnumerable<string> Files { get; set; }

		public Options()
		{
			X86 = true;
			X64 = false;
			ESP32 = false;
		}
	}
}
