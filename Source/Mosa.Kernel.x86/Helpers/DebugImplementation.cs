// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.Plug;
using System.Diagnostics;

namespace Mosa.Kernel.x86.Helpers
{
	//[Type("System.Diagnostics.Debug")]
	internal static class DebugImplementation
	{
		[Conditional("DEBUG")]
		[Method("System.Diagnostics.Debug::Assert")]
		public static void Assert(bool condition, string message, string detailMessage)
		{
			Helpers.Assert.True(condition, message);
		}

		[Conditional("DEBUG")]
		[Method("System.Diagnostics.Debug::Fail")]
		public static void Fail(string message, string detailMessage)
		{
			Panic.Error(message);
		}

		[Conditional("DEBUG")]
		[Method("System.Diagnostics.Debug::Write")]
		public static void Write(object value, string category)
		{
			if (value == null)
				return;
			Screen.Write(value.ToString());
		}

		[Conditional("DEBUG")]
		[Method("System.Diagnostics.Debug::Write")]
		public static void Write(string message, string category)
		{
			Screen.Write(message);
		}

		[Conditional("DEBUG")]
		[Method("System.Diagnostics.Debug::WriteLine")]
		public static void WriteLine(object value, string category)
		{
			if (value == null)
				return;
			Screen.Write(value.ToString());
			Screen.NextLine();
		}

		[Conditional("DEBUG")]
		[Method("System.Diagnostics.Debug::WriteLine")]
		public static void WriteLine(string message, string category)
		{
			Screen.Write(message);
			Screen.NextLine();
		}

		[Conditional("DEBUG")]
		[Method("System.Diagnostics.Debug::Print")]
		public static void Print(string message)
		{
			Panic.Error("print");
			Screen.Write(message);
		}
	}
}
