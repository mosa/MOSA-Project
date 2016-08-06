// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Linker;
using Mosa.Compiler.MosaTypeSystem;
using Mosa.Utility.DebugEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.UnitTest.Engine
{
	internal class UnitTestMessage : IUnitTestMessage
	{
		public IList<int> MessageAsInts { get; set; }
		public IList<byte> MessageAsBytes { get; }

		public int DebugCode { get; set; }

		public UnitTestMessage(int debugCode, IList<int> message)
		{
			DebugCode = debugCode;
			MessageAsInts = message;
		}

		public UnitTestMessage(int debugCode, IList<byte> message)
		{
			DebugCode = debugCode;
			MessageAsBytes = message;
		}
	}
}
