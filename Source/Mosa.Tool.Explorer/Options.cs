using CommandLine;
using System.Collections.Generic;

namespace Mosa.Tool.Explorer
{
	class Options
	{
		[Option("inline")]
		public bool Inline { get; set; }

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

		[Value(0)]
		public IEnumerable<string> Files { get; set; }
	}
}
