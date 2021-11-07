// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.UnitTests;
using Mosa.UnitTests.Optimization;

namespace Mosa.BareMetal.HelloWorld.ARMv8A32
{
	/// <summary>
	/// Boot
	/// </summary>
	public static class Boot
	{
		/// <summary>
		/// Main
		/// </summary>
		public static void Main()
		{
			while (true)
			{ }
		}

		public static bool IncludeUnitTestAssembly()
		{
			return CommonTests.OptimizationTest1();
		}
	}
}
