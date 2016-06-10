// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.UnitTest.System;

namespace Mosa.Workspace.UnitTest.Debug
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			using (var testSystem = UnitTestSystemFixture.UnitTestSystem)
			{
				testSystem.Initialize();

				//ulong value = 0;
				//value = testSystem.Run<int>("Mosa.Test.Collection", "Int32Tests", "AddI4I4", 1, 2);
				//value = testSystem.Run<int>("Mosa.Test.Collection", "Int32Tests", "AddI4I4", 3, 4);
				//value = testSystem.Run<int>("Mosa.Test.Collection", "Int32Tests", "AddI4I4", 5, 6);
				//value = testSystem.Run<int>("Mosa.Test.Collection", "Int32Tests", "AddI4I4", 7, 8);
				//value = testSystem.Run<int>("Mosa.Test.Collection", "Int32Tests", "AddI4I4", 9, 0);

				//value = testSystem.Run<ulong>("Mosa.Test.Collection", "UInt64Tests", "AddU8U8", 4294967295ul, 2ul);

				//long value = testSystem.Run<long>("Mosa.Test.Collection", "Int64Tests", "AddI8I8", -1l, -126l);

				//Mosa.Test.Collection.xUnit.Int64Fixture.AndI8I8(a: -1, b: -126)

				bool value = testSystem.Run<bool>("Mosa.Test.Collection", "GenericCallTests", "GenericCallU4", 10);
			}

			return;
		}
	}
}
