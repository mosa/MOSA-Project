// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Internal;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Mosa.Platform.Internal.x86
{
	public static class RuntimeExceptions
	{
		public static void ThrowIndexOutOfRangeException()
		{
			throw new IndexOutOfRangeException();
		}
	}
}
