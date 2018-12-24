// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.UnitTests;

namespace Mosa.TestWorld.x86.Tests
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
			testMethods.Add(ReflectionTests.DeclaringTypeTest);
			testMethods.Add(ReflectionTests.ElementTypeTest);
			testMethods.Add(ReflectionTests.TypeActivator);
		}
	}
}
