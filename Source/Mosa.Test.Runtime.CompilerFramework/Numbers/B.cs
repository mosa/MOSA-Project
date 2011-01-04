/*
* (c) 2008 MOSA - The Managed Operating System Alliance
*
* Licensed under the terms of the New BSD License.
*
* Authors:
*  Phil Garcia (tgiphil) <phil@thinkedge.com>
*/

using System;
using System.Collections.Generic;
using System.Text;

using MbUnit.Framework;

namespace Mosa.Test.Runtime.CompilerFramework.Numbers
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
