// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.UnitTests;

namespace Mosa.Demo.TestWorld.x86.Tests
{
	public class ReflectionTest : KernelTest
	{
		public ReflectionTest()
			: base("Reflection")
		{
			testMethods.Add(ReflectionTests.PointerTest);
			testMethods.Add(ReflectionTests.HandleTest);
			testMethods.Add(ReflectionTests.FindTypeOfTest);
			testMethods.Add(ReflectionTests.FindTypeByNameTest);
			testMethods.Add(ReflectionTests.CompareTypeHandlesTest);
			testMethods.Add(ReflectionTests.TypeHandleFromObjectTest);
			testMethods.Add(ReflectionTests.DeclaringTypeTest1);
			testMethods.Add(ReflectionTests.DeclaringTypeTest2);
			testMethods.Add(ReflectionTests.ElementTypeTest);
			testMethods.Add(ReflectionTests.TypeActivator);
		}
	}
}
