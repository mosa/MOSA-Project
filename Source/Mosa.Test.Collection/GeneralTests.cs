/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Test.Collection
{
	public static class GeneralTests
	{
		public static int VariablePressure8()
		{
			int a = 10;
			int b = 20;
			int c = 30;
			int d = 40;
			int e = 50;
			int f = 60;
			int g = 70;
			int h = 80;
			int i = 80;
			int j = 90;
			int k = 100;

			if (a >> 2 == 5)
				k++;
			else
				j++;

			int z = ((((((a >> b) + c + (d * e) - f) * g) - h) * j) * k) >> 2;

			return z;
		}
	}
}