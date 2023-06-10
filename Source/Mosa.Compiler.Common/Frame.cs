// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;

namespace Mosa.Compiler.Common
{
	public static class Frame
	{
		public static string MethodName
		{
			get
			{
				var stackTrace = new StackTrace();
				return stackTrace.GetFrame(1).GetMethod().Name;
			}
		}
	}
}
