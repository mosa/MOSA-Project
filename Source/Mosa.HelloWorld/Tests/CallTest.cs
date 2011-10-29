using System;
using Mosa.Test.Collection;

namespace Mosa.HelloWorld.Tests
{
	public class CallTest : KernelTest
	{
		public CallTest()
			: base("Call") 
		{
			testMethods.Add(CallU1_0);
			testMethods.Add(CallU1_1);
			testMethods.Add(CallU1_255);
		}

		public static bool CallU1_0()
		{
			return CallTests.CallU1((byte)0);
		}
		public static bool CallU1_1()
		{
			return CallTests.CallU1((byte)1);
		}
		public static bool CallU1_255()
		{
			return CallTests.CallU1((byte)255);
		}
	}
}
