// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Test.Collection;
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

				for (int i = 0; i < 500; i++)
				{
					int value = testSystem.Run<int>("Mosa.Test.Collection", "Int32Tests", "AddI4I4", 1, 2);
					value = testSystem.Run<int>("Mosa.Test.Collection", "Int32Tests", "AddI4I4", 3, 4);
					value = testSystem.Run<int>("Mosa.Test.Collection", "Int32Tests", "AddI4I4", 5, 6);
					value = testSystem.Run<int>("Mosa.Test.Collection", "Int32Tests", "AddI4I4", 7, 8);
					value = testSystem.Run<int>("Mosa.Test.Collection", "Int32Tests", "AddI4I4", 9, 0);

					double a1 = 7;
					double b1 = 9;

					var d1 = testSystem.Run<double>("Mosa.Test.Collection", "DoubleTests", "AddR8R8", a1, b1);
					var d2 = DoubleTests.AddR8R8(a1, b1);

					float a2 = 7;
					float b2 = 9;

					var d1a = testSystem.Run<float>("Mosa.Test.Collection", "SingleTests", "AddR4R4", a2, b2);
					var d2a = SingleTests.AddR4R4(a2, b2);

					var b1b = testSystem.Run<bool>("Mosa.Test.Collection", "ValueTypeTests", "TestValueTypeStaticField");
					var b2b = ValueTypeTests.TestValueTypeStaticField();
				}

				//value = testSystem.Run<ulong>("Mosa.Test.Collection", "UInt64Tests", "AddU8U8", 4294967295ul, 2ul);

				//long value = testSystem.Run<long>("Mosa.Test.Collection", "Int64Tests", "AddI8I8", -1l, -126l);

				//Mosa.Test.Collection.xUnit.Int64Fixture.AndI8I8(a: -1, b: -126)

				//bool value = testSystem.Run<bool>("Mosa.Test.Collection", "GenericCallTests", "GenericCallU4", 10);

				return;
			}
		}
	}
}
