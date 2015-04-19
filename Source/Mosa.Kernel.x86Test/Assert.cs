using System;
using System.Diagnostics;
using Mosa.Platform.Internal.x86;

namespace Mosa.Kernel.x86Test
{
	public delegate void ExceptionHandler(string message);

	public static class Assert
	{
		private static void AssertError(string message)
		{
			Runtime.DebugOutput(message);
		}

		[Conditional("DEBUG")]
		public static void Error(string message)
		{
			AssertError(message);
		}

		[Conditional("DEBUG")]
		public static void InRange(uint value, uint length)
		{
			if (value >= length)
				AssertError("Out of Range");
		}

		[Conditional("DEBUG")]
		public static void True(bool condition)
		{
			if (!condition)
				AssertError("Assert.True failed");
		}

		[Conditional("DEBUG")]
		public static void True(bool condition, string userMessage)
		{
			if (!condition)
				AssertError(userMessage);
		}

		[Conditional("DEBUG")]
		public static void False(bool condition)
		{
			if (condition)
				AssertError("Assert.False failed");
		}

		[Conditional("DEBUG")]
		public static void False(bool condition, string userMessage)
		{
			if (condition)
				AssertError(userMessage);
		}
	}
}