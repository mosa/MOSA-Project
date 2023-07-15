// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using Mosa.Runtime.x86;

namespace Mosa.Kernel.x86;

/// <summary>
/// Unit Test Runner
/// </summary>
public static class UnitTestRunner
{
	private static Pointer Stack;

	private const uint MaxParameters = 8; // max 32-bit parameters

	private static bool Ready;
	private static bool ResultReady;
	private static bool ResultReported;

	private static uint TestID;
	private static uint TestParameters;
	private static uint TestMethodAddress;
	private static uint TestResultType;

	private static ulong TestResult;

	public static void Setup()
	{
		Stack = new Pointer(Address.UnitTestStack);

		Ready = false;
		ResultReported = true;

		TestID = 0;
		TestParameters = 0;
		TestMethodAddress = 0;
		TestResult = 0;
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
			if (Ready)
			{
				Screen.Goto(row, 0);
				Screen.ClearRow();

				Screen.Write("Test #: ");
				Screen.Write(++testCount, 10, 7);

				TestResult = 0;
				ResultReady = false;
				ResultReported = false;
				Ready = false;

				// copy parameters into stack
				for (var index = 0; index < TestParameters; index++)
				{
					var value = Stack.Load32(index * 4);

					new Pointer(esp).Store32(index * 4, value);
				}

				switch (TestResultType)
				{
					case 0: Native.FrameCall(TestMethodAddress); break;
					case 1: TestResult = Native.FrameCallRetU4(TestMethodAddress); break;
					case 2: TestResult = Native.FrameCallRetU8(TestMethodAddress); break;
					case 3: TestResult = Native.FrameCallRetR8(TestMethodAddress); break;
					default: break;
				}

				ResultReady = true;

				Native.Int(255);
			}
		}
	}

	public static void SetUnitTestMethodParameter(uint index, uint value) => Stack.Store32(index * 4, value);

	public static void SetUnitTestMethodParameterCount(uint number) => TestParameters = number;

	public static void SetUnitTestMethodAddress(uint address) => TestMethodAddress = address;

	public static void SetUnitTestResultType(uint type) => TestResultType = type;

	public static void StartTest(uint id)
	{
		TestID = id;
		ResultReady = false;
		Ready = true;
	}

	public static bool IsReady() => ResultReported && !Ready;

	public static bool GetResult(out ulong result, out uint id)
	{
		result = TestResult;
		id = TestID;

		if (!Ready && ResultReady && !ResultReported)
		{
			ResultReported = true;
			return true;
		}

		return false;
	}
}
