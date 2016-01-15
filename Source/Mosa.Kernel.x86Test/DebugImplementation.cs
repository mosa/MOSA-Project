// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.Plug;
using System.Diagnostics;

namespace Mosa.Kernel.x86Test
{
	[Type("System.Diagnostics.Debug")]
	internal class DebugImplementation
	{
		[Conditional("DEBUG")]
		public static void Assert(bool condition, string message, string detailMessage)
		{
			Mosa.Kernel.x86Test.Assert.True(condition, message);
		}

		[Conditional("DEBUG")]
		public static void Fail(string message, string detailMessage)
		{
			Mosa.Kernel.x86Test.Assert.Error(message);
		}

		[Conditional("DEBUG")]
		public static void Write(object value, string category)
		{
			if (value == null)
				return;
			Mosa.Kernel.x86Test.Assert.Error(value.ToString());
		}

		[Conditional("DEBUG")]
		public static void Write(string message, string category)
		{
			Mosa.Kernel.x86Test.Assert.Error(message);
		}

		[Conditional("DEBUG")]
		public static void WriteLine(object value, string category)
		{
			if (value == null)
				return;
			Mosa.Kernel.x86Test.Assert.Error(value.ToString());
			Mosa.Kernel.x86Test.Assert.Error("\r\n");
		}

		[Conditional("DEBUG")]
		public static void WriteLine(string message, string category)
		{
			Mosa.Kernel.x86Test.Assert.Error(message);
			Mosa.Kernel.x86Test.Assert.Error("\r\n");
		}

		[Conditional("DEBUG")]
		public static void Print(string message)
		{
			Mosa.Kernel.x86Test.Assert.Error("print");
			Mosa.Kernel.x86Test.Assert.Error("\r\n");
		}
	}
}
