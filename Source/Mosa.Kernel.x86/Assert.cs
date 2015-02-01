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
	}
}