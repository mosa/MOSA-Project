// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.x86;

namespace Mosa.Kernel.x86
{
	/// <summary>
	/// Unit Test Runner
	/// </summary>
	public static class UnitTestRunner
	{
		private const uint TestStackAddress = 0x1000;

		private static uint counter = 0;

		private static int testReady = 0;
		private static uint testID = 0;
		private static uint testParameters = 0;
		private static uint testMethodAddress = 0;
		private static uint testResultType = 0;

		private static uint testResultReady = 0;
		private static uint testResultU4 = 0;
		private static ulong testResultU8 = 0;
		private static double testResultR2 = 0;

		public static void Setup()
		{
			ResetUnitTest();
		}

		public static void EnterTestReadyLoop()
		{
			while (true)
			{
				if (testReady != 0)
				{
					testReady = 0;
					testResultReady = 0;

					uint esp = TestStackAddress + (testParameters * 4);

					switch (testResultType)
					{
						case 0:
							{
								Native.FrameCall(testMethodAddress, esp);
								break;
							}
						case 1:
							{
								testResultU4 = Native.FrameCallRetU4(testMethodAddress, esp);
								break;
							}
						case 2:
							{
								testResultU8 = Native.FrameCallRetU8(testMethodAddress, esp);
								break;
							}
						case 3:
							{
								testResultR2 = Native.FrameCallRetR2(testMethodAddress, esp);
								break;
							}

						default: break;
					}

					testResultReady = 1;

					Native.Int(255);
				}
			}
		}

		public static void ResetUnitTest()
		{
			testReady = 0;
			testID = 0;
			testParameters = 0;
			testMethodAddress = 0;

			testResultU4 = 0;
			testResultU8 = 0;
			testResultR2 = 0;
		}

		public static void SetUnitTestParameter(uint index, uint value)
		{
			Native.Set32(TestStackAddress + (index * 4), value);

			testParameters = testParameters > index ? testParameters : index;
		}

		public static void SetUnitTestMethodAddress(uint address)
		{
			testMethodAddress = address;
		}

		public static void SetUnitTestID(uint id)
		{
			testID = id;
		}

		public static void StartTest()
		{
			testResultReady = 0;
			testReady = 1;
		}

		public static void AbortUnitTest()
		{
			testReady = 0;

			// TODO
			// Very Complex - the stack needs to be manipulated to re-enter EnterTestReadyLoop()
		}

		public static ulong GetResults()
		{
			switch (testResultType)
			{
				case 0: return 0;
				case 1: return testResultU4;
				case 2: return testResultU8;
				case 3: return 0; // TODO
				default: return 0;
			}
		}
	}
}
