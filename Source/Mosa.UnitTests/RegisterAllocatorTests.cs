// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTests;

public static class RegisterAllocatorTests
{
	[MosaUnitTest]
	public static int Pressure8()
	{
		var a = 10;
		var b = 20;
		var c = 30;
		var d = 40;
		var e = 50;
		var f = 60;
		var g = 70;
		var h = 80;
		var j = 90;
		var k = 100;

		if (a >> 2 == 5)
			k++;
		else
			j++;

		var z = ((((a >> b) + c + d * e - f) * g - h) * j * k) >> 2;

		return z;
	}

	public static int Pressure7(int a, int b, int c, int d, int e, int f, int g)
	{
		var h = 70;
		var j = 90;
		var k = 100;

		if (a >> 2 == 5)
			k++;
		else
			j++;

		var z = (((((a >> b) + c + d * e - f) * g - h) * j * k) >> 2) + a + b + c + d + e + f * g;

		return z;
	}

	public static int Pressure7B(int a, int b, int c, int d, int e, int f, int g)
	{
		var h = 70;
		var j = 90;
		var k = 100;

		if (a >> 2 == 5)
		{
			k++; a++; c--;
		}
		else
		{
			j++; c++; d--;
		}

		var z = (((((a >> b) + c * d * e - f) * g - h) * j * k) >> 2) + a % b + c * d + e % (f * g + 1);

		if (z % 2 == c)
		{
			z--;
			z = z + d * e;
		}
		else
		{
			z = z * a / (e + 1);
		}

		return z;
	}

	public static int Pressure7C(int a, int b, int c, int d, int e, int f, int g)
	{
		var z = a + b + c + d + e + f + g;
		var q = 0;

		for (var i = 1; i < 20; i++)
		{
			z = z + a * b * c * d * e * f * g + i * q;
			q = q + z - 1;
		}

		return z * 10;
	}

	public static int Pressure9(int a, int b, int c, int d, int e, int f, int g)
	{
		var a1 = a;
		var b1 = b;
		var c1 = c;
		var d1 = d;
		var e1 = e;
		var f1 = f;
		var g1 = g;

		var z = a1 + b1 + c1 + d1 + e1 + f1 + g1;

		for (var i = 1; i < 20; i++)
		{
			z = z + a1 * b1 * c1 * d1 * e1 * f1 * g1 + i;
		}

		return z;
	}
}
