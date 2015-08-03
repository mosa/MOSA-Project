// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Diagnostics;

namespace Mosa.Kernel.x86.Helpers
{
	public delegate void ExceptionHandler(string message);

	public static class Assert
	{
		private static void AssertError(string message)
		{
			Panic.Error(message);
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
