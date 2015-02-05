using Mosa.Internal.Plug;
using System.Diagnostics;

namespace Mosa.Kernel.x86.Helpers
{
	[Type("System.Diagnostics.Debug")]
	internal class DebugImplementation
	{
		//Can removed after Bugfix in PlugSystem
		[Conditional("DEBUG")]
		public static void Assert(bool condition, string message)
		{
			Mosa.Kernel.x86.Helpers.Assert.True(condition, message);
		}

		[Conditional("DEBUG")]
		public static void Assert(bool condition, string message, string detailMessage)
		{
			Mosa.Kernel.x86.Helpers.Assert.True(condition, message);
		}

		//Can removed after Bugfix in PlugSystem
		[Conditional("DEBUG")]
		public static void Fail(string message)
		{
			Panic.Error(message);
		}

		[Conditional("DEBUG")]
		public static void Fail(string message, string detailMessage)
		{
			Panic.Error(message);
		}

		[Conditional("DEBUG")]
		public static void Write(object value, string category)
		{
			if (value == null)
				return;
			Screen.Write(value.ToString());
		}

		//Can removed after Bugfix in PlugSystem
		[Conditional("DEBUG")]
		public static void Write(string message)
		{
			Screen.Write(message);
		}

		[Conditional("DEBUG")]
		public static void Write(string message, string category)
		{
			Screen.Write(message);
		}

		[Conditional("DEBUG")]
		public static void WriteLine(object value, string category)
		{
			if (value == null)
				return;
			Screen.Write(value.ToString());
			Screen.NextLine();
		}

		//Can removed after Bugfix in PlugSystem
		[Conditional("DEBUG")]
		public static void WriteLine(string message)
		{
			Screen.Write(message);
			Screen.NextLine();
		}

		[Conditional("DEBUG")]
		public static void WriteLine(string message, string category)
		{
			Screen.Write(message);
			Screen.NextLine();
		}

		[Conditional("DEBUG")]
		//[Method("System.Diagnostics.Debug.Print")]
		public static void Print(string message)
		{
			Panic.Error("print");
			Screen.Write(message);
		}
	}
}