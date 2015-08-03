// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Test.Numbers
{
	public static class B
	{
		public static IEnumerable<bool> Series
		{
			get
			{
				yield return true;
				yield return false;
			}
		}
	}
}