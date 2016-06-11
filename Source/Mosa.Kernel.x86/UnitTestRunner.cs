// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.x86;

namespace Mosa.Kernel.x86
{
	/// <summary>
	/// Unit Test Runner
	/// </summary>
	public static class UnitTestRunner
	{
		private const uint MaxParameters = 8; // max 32-bit parameters

		private static uint counter = 0;

		private static int testReady = 0;
		private static int testResultReady = 0;
		private static int testResultReported = 0;

		private static int testID = 0;
		private static uint testParameters = 0;
		private static uint testMethodAddress = 0;
		private static uint testResultType = 0;

		private static uint testResultU4 = 0;
		private static ulong testResultU8 = 0;

		//private static double testResultR8 = 0;

		public static void Setup()
		{
			ResetUnitTest();
		}

		public static void EnterTestReadyLoop()
		{
			uint testCount = 0;

			DebugClient.Ready();

			Screen.Write("Waiting for unit tests...");
			Screen.NextLine();
			Screen.NextLine();

			// allocate space on stack for parameters
			uint esp = Native.AllocateStackSpace(MaxParameters * 4);

			Screen.Write("Stack @ ");
			Screen.Write((uint)esp, 16, 8);

			for (uint index = 0; index < MaxParameters; index++)
			{
				Native.Set32(esp + (index * 4), 0xFFFFFFFF);
			}

			Screen.NextLine();
			Screen.NextLine();

			uint row = Screen.Row;

			while (true)
			{
				if (testReady == 1)
				{
					Screen.Row = row;
					Screen.Column = 0;

					Screen.Write(++testCount, 10, 7);

					testReady = 0;
					testResultReady = 0;
					testResultReported = 0;

					// copy parameters into stack
					for (uint index = 0; index < testParameters; index++)
					{
						uint value = Native.Get32(Address.UnitTestStack + (index * 4));

						Native.Set32(esp + (index * 4), value);
					}

					switch (testResultType)
					{
						case 0:
							{
								Native.FrameCall(testMethodAddress);
								break;
							}
						case 1:
							{
								testResultU4 = Native.FrameCallRetU4(testMethodAddress);
								break;
							}
						case 2:
							{
								testResultU8 = Native.FrameCallRetU8(testMethodAddress);
								break;
							}
						case 3:
							{
								testResultU8 = Native.FrameCallRetR8(testMethodAddress);
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

			//testResultR8 = 0;
		}

		public static void SetUnitTestMethodParameter(uint index, uint value)
		{
			Native.Set32(Address.UnitTestStack + (index * 4), value);
		}

		public static void SetUnitTestMethodParameterCount(uint number)
		{
			testParameters = number;
		}

		public static void SetUnitTestMethodAddress(uint address)
		{
			testMethodAddress = address;
		}

		public static void SetUnitTestResultType(uint type)
		{
			testResultType = type;
		}

		public static void StartTest(int id)
		{
			testID = id;
			testResultReady = 0;
			testReady = 1;
		}

		public static void AbortUnitTest()
		{
			testReady = 0;

			// TODO
			//  Complex - the stack needs to be manipulated to re-enter EnterTestReadyLoop()
		}

		public static int GetTestID()
		{
			return testID;
		}

		public static ulong GetResults()
		{
			switch (testResultType)
			{
				case 0: return 0;
				case 1: return testResultU4;
				case 2: return testResultU8;
				case 3: return testResultU8;
				default: return 0;
			}
		}

		public static bool CheckResultsReady()
		{
			if (testReady == 0 && testResultReady == 1 && testResultReported == 0)
			{
				testResultReported = 1;
				return true;
			}
			return false;
		}
	}
}
