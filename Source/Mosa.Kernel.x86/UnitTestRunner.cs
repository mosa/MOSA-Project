// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using Mosa.Runtime.x86;

namespace Mosa.Kernel.x86;

/// <summary>
/// Unit Test Runner
/// </summary>
public static class UnitTestRunner
{
	private const uint MaxParameters = 8; // max 32-bit parameters

	private static int testReady;
	private static int testResultReady;
	private static int testResultReported;

	private static uint testID;
	private static uint testParameters;
	private static uint testMethodAddress;
	private static uint testResultType;

	private static ulong testResult;

	public static void Setup()
	{
		testReady = 0;
		testResult = 0;
		testResultReported = 1;

		testID = 0;
		testParameters = 0;
		testMethodAddress = 0;
	}

	public static void EnterTestReadyLoop()
	{
		var testCount = 0u;

		Screen.Write("Waiting for unit tests...");
		Screen.NextLine();
		Screen.NextLine();

		// allocate space on stack for parameters
		var esp = Native.AllocateStackSpace(MaxParameters * 4);

		Screen.Write("Stack @ ");
		Screen.Write(esp, 16, 8);

		Screen.NextLine();
		Screen.NextLine();

		var row = Screen.Row;

		while (true)
		{
			if (testReady == 1)
			{
				Screen.Goto(row, 0);
				Screen.ClearRow();

				Screen.Write("Test #: ");
				Screen.Write(++testCount, 10, 7);

				testResult = 0;
				testResultReady = 0;
				testResultReported = 0;
				testReady = 0;

				// copy parameters into stack
				for (var index = 0; index < testParameters; index++)
				{
					var value = new Pointer(Address.UnitTestStack).Load32(index * 4);

					new Pointer(esp).Store32(index * 4, value);
				}

				switch (testResultType)
				{
					case 0: Native.FrameCall(testMethodAddress); break;
					case 1: testResult = Native.FrameCallRetU4(testMethodAddress); break;
					case 2: testResult = Native.FrameCallRetU8(testMethodAddress); break;
					case 3: testResult = Native.FrameCallRetR8(testMethodAddress); break;
					default: break;
				}

				testResultReady = 1;

				Native.Int(255);
			}
		}
	}

	public static void SetUnitTestMethodParameter(uint index, uint value)
	{
		new Pointer(Address.UnitTestStack).Store32(index * 4, value);
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

	public static void StartTest(uint id)
	{
		testID = id;
		testResultReady = 0;
		testReady = 1;
	}

	public static bool IsReady()
	{
		return testResultReported == 1 && testReady == 0;
	}

	public static bool GetResult(out ulong result, out uint id)
	{
		result = testResult;
		id = testID;

		if (testReady == 0 && testResultReady == 1 && testResultReported == 0)
		{
			testResultReported = 1;
			return true;
		}

		return false;
	}
}
