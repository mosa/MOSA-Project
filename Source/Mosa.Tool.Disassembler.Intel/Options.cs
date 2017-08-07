// Copyright (c) MOSA Project. Licensed under the New BSD License.

using CommandLine;
using Mosa.Compiler.Common;

namespace Mosa.Tool.Disassembler.Intel
{
	public class Options
	{
		[Value(0, Required = true, Hidden = true)]
		public string InputFile { get; set; }

		[Option('o', "output", Required = true)]
		public string OutputFile { get; set; }

		[Option('f', "offset", Default = "0", Required = true)]
		public string FileOffsetString { get; set; }

		[Option('l', "len", Required = true)]
		public string LengthString { get; set; }

		[Option('a', "address", Default = "0", Required = true)]
		public string StartingAddressString { get; set; }

		//TODO: Better way of parsing these?
		public ulong FileOffset
		{
			get { return FileOffsetString.ParseHexOrDecimal(); }
		}

		public ulong Length
		{
			get { return LengthString.ParseHexOrDecimal(); }
		}

		public ulong StartingAddress
		{
			get { return StartingAddressString.ParseHexOrDecimal(); }
		}
	}
}
