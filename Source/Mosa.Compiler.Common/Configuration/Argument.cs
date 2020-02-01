// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.IO;

namespace Mosa.Compiler.Common.Configuration
{
	public class Argument
	{
		public string Name { get; set; }

		public string Setting { get; set; }

		public string Value { get; set; }

		public bool IsList { get; set; } = false;
	}
}
