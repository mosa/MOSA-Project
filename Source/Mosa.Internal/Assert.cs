using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mosa.Kernel.Helpers
{
	//public static class InternalPanic
	//{
	//	public static ExceptionHandler ExceptionHandler;

	//	public static void Error(string message)
	//	{
	//		if (ExceptionHandler != null)
	//			ExceptionHandler(message);
	//	}
	//}

	public delegate void ExceptionHandler(string message);

	public static class Assert
	{
		private static void AssertError(string message)
		{
			//InternalPanic.Error(message);
		}

		public static void InRange(uint value, uint length)
		{
			if (value >= length)
				AssertError("Out of Range");
		}
	}
}