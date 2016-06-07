// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.UnitTest.System;

namespace Mosa.Workspace.UnitTest.Debug
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var testSystem = UnitTestSystemFixture.UnitTestSystem;

			testSystem.Initialize();

			int value = 0;

			value = testSystem.Run<int>("Mosa.Test.Collection", "Int32Tests", "AddI4I4", 1, 2);
			value = testSystem.Run<int>("Mosa.Test.Collection", "Int32Tests", "AddI4I4", 3, 4);
			value = testSystem.Run<int>("Mosa.Test.Collection", "Int32Tests", "AddI4I4", 5, 6);
			value = testSystem.Run<int>("Mosa.Test.Collection", "Int32Tests", "AddI4I4", 7, 8);
			value = testSystem.Run<int>("Mosa.Test.Collection", "Int32Tests", "AddI4I4", 9, 0);

			return;
		}
	}
}
