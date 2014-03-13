/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *
 */

using System;
using Mosa.Test.Collection.x86.xUnit;

namespace Mosa.TinyCPUSimulator.Debug
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			Test8();
		}

		private static void Test8()
		{
			var fixture = new DoubleFixture();

			fixture.IsNaN(Double.NaN);
		}

		private static void Test7()
		{
			var fixture = new DoubleFixture();

			fixture.CeqR8R8(200, 100);
		}

		private static void Test6()
		{
			var fixture = new DoubleFixture();

			fixture.AddR8R8(200, 100);
		}

		private static void Test5()
		{
			var fixture = new UInt32Fixture();

			fixture.AddU4U4(200, 100);
		}

		private static void Test4()
		{
			var fixture = new Int16Fixture();

			fixture.LdelemI2(0, -32768);
		}

		private static void Test3()
		{
			var fixture = new UInt64Fixture();

			fixture.DivU8U8(18446744073709551615, 4294967294);
		}

		private static void Test1()
		{
			var test = new TestCPUx86();

			test.RunTest();
		}
	}
}