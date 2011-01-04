/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

using System;

namespace Mosa.HelloWorld.Tests
{

	public class Test 
	{

		public static bool CallOrderU4U4(uint a, uint b)
		{
			return (a == 1 && b == 2);
		}

		public static uint CallOrderU4U4_2(uint a, uint b)
		{
			return a + (b * 10);
		}
	}

}
