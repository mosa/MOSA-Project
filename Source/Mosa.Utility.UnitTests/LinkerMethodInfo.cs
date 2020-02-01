// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;
using System;

namespace Mosa.Utility.UnitTests
{
	public class LinkerMethodInfo
	{
		public MosaMethod MosaMethod { get; set; }
		public IntPtr MosaMethodAddress { get; set; }

		public string MethodNamespaceName { get; set; }
		public string MethodTypeName { get; set; }
		public string MethodName { get; set; }
	}
}
