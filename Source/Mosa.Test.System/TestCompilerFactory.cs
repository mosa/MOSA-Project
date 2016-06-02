// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Test.System
{
	public static class TestCompilerFactory
	{
		private static Dictionary<BaseTestPlatform, TestCompiler> testCompilers = new Dictionary<BaseTestPlatform, TestCompiler>();

		internal static TestCompiler GetTestCompiler(BaseTestPlatform basePlatform)
		{
			lock (testCompilers)
			{
				TestCompiler testCompiler = null;

				if (!testCompilers.TryGetValue(basePlatform, out testCompiler))
				{
					testCompiler = new TestCompiler(basePlatform);
					testCompilers.Add(basePlatform, testCompiler);
				}

				return testCompiler;
			}
		}
	}
}
