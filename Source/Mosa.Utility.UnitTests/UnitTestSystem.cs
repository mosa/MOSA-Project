// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Mosa.Compiler.Common.Configuration;
using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.MosaTypeSystem;
using Mosa.Utility.Configuration;

namespace Mosa.Utility.UnitTests;

public static class UnitTestSystem
{
	public static Type CombinationType = Assembly.Load("Mosa.Utility.UnitTests").GetTypes().First(t => t.Name == "Combinations");
	public static Type SeriesType = Assembly.Load("Mosa.Utility.UnitTests").GetTypes().First(t => t.Name == "Series2");

	public static int Start(string[] args)
	{
		var mosaSettings = new MosaSettings();
		mosaSettings.LoadArguments(args);

		var stopwatch = new Stopwatch();
		stopwatch.Start();

		Console.WriteLine("Discovering Unit Tests...");

		var discoveredUnitTests = Discovery.DiscoverUnitTests(mosaSettings.UnitTestFilter);

		Console.WriteLine($"Found Tests: {discoveredUnitTests.Count} in {(stopwatch.ElapsedMilliseconds / 1000.0).ToString("F2")} secs");
		Console.WriteLine();
		Console.WriteLine("Starting Unit Test Engine...");

		var unitTestEngine = new UnitTestEngine(mosaSettings);

		if (unitTestEngine.IsAborted)
		{
			Console.WriteLine("ERROR: Compilation aborted!");
			return 1;
		}

		var unitTests = PrepareUnitTest(discoveredUnitTests, unitTestEngine.TypeSystem, unitTestEngine.Linker);

		var executeStart = stopwatch.ElapsedMilliseconds;

		Execute(unitTests, unitTestEngine);
		stopwatch.Stop();

		Console.WriteLine("Unit Testing: " + ((stopwatch.ElapsedMilliseconds - executeStart) / 1000.0).ToString("F2") + " secs");
		Console.WriteLine("Total: " + stopwatch.ElapsedMilliseconds / 1000.0 + " secs");

		unitTestEngine.Terminate();

		Console.WriteLine();

		var failures = 0;
		var passed = 0;
		var skipped = 0;
		var incomplete = 0;

		foreach (var unitTest in unitTests)
		{
			switch (unitTest.Status)
			{
				case UnitTestStatus.Passed: passed++; break;
				case UnitTestStatus.Skipped: skipped++; break;
				case UnitTestStatus.Pending: incomplete++; break;
				case UnitTestStatus.FailedByCrash:
				case UnitTestStatus.Failed:
					failures++;
					Console.WriteLine(OutputUnitTestResult(unitTest));
					break;
			}
		}

		if (failures != 0)
			Console.WriteLine();

		Console.WriteLine("Unit Test Results:");
		Console.WriteLine($"  Passed:     {passed}");
		Console.WriteLine($"  Skipped:    {skipped}");
		Console.WriteLine($"  Incomplete: {incomplete}");
		Console.WriteLine($"  Failures:   {failures}");
		Console.WriteLine($"  Total:      {passed + skipped + failures + incomplete}");
		Console.WriteLine();

		if (unitTestEngine.IsAborted)
		{
			Console.WriteLine("ERROR: Unit tests aborted due to failures!");
			return 1;
		}

		if (failures + incomplete == 0)
		{
			Console.WriteLine("All unit tests passed successfully!");
			return 0;
		}
		else
		{
			Console.WriteLine("ERROR: Failures occurred in the unit tests!");
			return failures + incomplete;
		}
	}

	private static List<UnitTest> PrepareUnitTest(List<UnitTestInfo> discoveredUnitTests, TypeSystem typeSystem, MosaLinker linker)
	{
		var unitTests = new List<UnitTest>(discoveredUnitTests.Count);

		var id = 0;

		foreach (var unitTestInfo in discoveredUnitTests)
		{
			LinkerMethodInfo linkerMethodInfo;

			try
			{
				linkerMethodInfo = Linker.GetMethodInfo(typeSystem, linker, unitTestInfo);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"ERROR: Unable to resolve method: {unitTestInfo.FullMethodName}");

				throw;
			}

			var unitTest = new UnitTest(unitTestInfo, linkerMethodInfo);

			unitTest.SerializedUnitTest = SerializeUnitTestMessage(unitTest);
			unitTest.UnitTestID = ++id;

			unitTests.Add(unitTest);
		}

		return unitTests;
	}

	private static void Execute(List<UnitTest> unitTests, UnitTestEngine unitTestEngine)
	{
		unitTestEngine.QueueUnitTests(unitTests);

		unitTestEngine.WaitUntilComplete();
	}

	public static IntPtr GetMethodAddress(MosaMethod method, MosaLinker linker)
	{
		var symbol = linker.GetSymbol(method.FullName);

		if (symbol.VirtualAddress == 0)
		{
			Console.WriteLine(method.FullName);
		}

		return new IntPtr((long)symbol.VirtualAddress);
	}

	public static List<int> SerializeUnitTestMessage(UnitTest unitTest)
	{
		var address = unitTest.MosaMethodAddress.ToInt64();

		var cmd = new List<int>(4 + 4 + 4 + 4 + unitTest.MosaMethod.Signature.Parameters.Count)
		{
			(int)address,
			//(int)(address>>32),
			GetReturnResultType(unitTest.MosaMethod.Signature.ReturnType),
			0
		};

		foreach (var param in unitTest.Values)
		{
			AddParameters(cmd, param);
		}

		cmd[2] = cmd.Count - 3;

		return cmd;
	}

	private static int GetReturnResultType(MosaType type)
	{
		if (type.IsI1)
			return 1;
		else if (type.IsI2)
			return 1;
		else if (type.IsI4)
			return 1;
		else if (type.IsI8)
			return 2;
		else if (type.IsU1)
			return 1;
		else if (type.IsU2)
			return 1;
		else if (type.IsU4)
			return 1;
		else if (type.IsU8)
			return 2;
		else if (type.IsChar)
			return 1;
		else if (type.IsBoolean)
			return 1;
		else if (type.IsR4)
			return 3;
		else if (type.IsR8)
			return 3;
		else if (type.IsVoid)
			return 0;

		return 0;
	}

	private static void AddParameters(List<int> cmd, object parameter)
	{
		if (parameter == null || !(parameter is ValueType))
		{
			throw new InvalidProgramException();
		}

		if (parameter is Boolean)
		{
			cmd.Add((bool)parameter ? 1 : 0);
		}
		else if (parameter is Char)
		{
			cmd.Add((char)parameter);
		}
		else if (parameter is SByte)
		{
			cmd.Add((sbyte)parameter);
		}
		else if (parameter is Int16)
		{
			cmd.Add((short)parameter);
		}
		else if (parameter is int)
		{
			cmd.Add((int)parameter);
		}
		else if (parameter is Byte)
		{
			cmd.Add((byte)parameter);
		}
		else if (parameter is UInt16)
		{
			cmd.Add((ushort)parameter);
		}
		else if (parameter is UInt32)
		{
			cmd.Add((int)(uint)parameter);
		}
		else if (parameter is UInt64)
		{
			cmd.Add((int)(ulong)parameter);
			cmd.Add((int)((ulong)parameter >> 32));
		}
		else if (parameter is Int64)
		{
			cmd.Add((int)(long)parameter);
			cmd.Add((int)((long)parameter >> 32));
		}
		else if (parameter is Single)
		{
			var b = BitConverter.GetBytes((float)parameter);
			var u = BitConverter.ToUInt32(b, 0);
			cmd.Add((int)u);
		}
		else if (parameter is Double)
		{
			var b = BitConverter.GetBytes((double)parameter);
			var u = BitConverter.ToUInt64(b, 0);
			cmd.Add((int)(long)u);
			cmd.Add((int)((long)u >> 32));
		}
		else
		{
			throw new InvalidProgramException();
		}
	}

	public static void ParseResultData(UnitTest unitTest, ulong data)
	{
		var arr = BitConverter.GetBytes(data);

		unitTest.Result = GetResult(unitTest.MosaMethod.Signature.ReturnType, arr);
	}

	public static object GetResult(MosaType type, byte[] data)
	{
		if (type.IsI1)
		{
			return (sbyte)data[0];
		}
		else if (type.IsI2)
		{
			return (short)(data[0] | (data[1] << 8));
		}
		else if (type.IsI4)
		{
			return data[0] | (data[1] << 8) | (data[2] << 16) | (data[3] << 24);
		}
		else if (type.IsI8)
		{
			ulong low = (uint)(data[0] | (data[1] << 8) | (data[2] << 16) | (data[3] << 24));
			ulong high = (uint)(data[4] | (data[5] << 8) | (data[6] << 16) | (data[7] << 24));

			return (long)(low | (high << 32));
		}
		else if (type.IsU1)
		{
			return data[0];
		}
		else if (type.IsU2)
		{
			return (ushort)(data[0] | (data[1] << 8));
		}
		else if (type.IsU4)
		{
			return (uint)(data[0] | (data[1] << 8) | (data[2] << 16) | (data[3] << 24));
		}
		else if (type.IsU8)
		{
			ulong low = (uint)(data[0] | (data[1] << 8) | (data[2] << 16) | (data[3] << 24));
			ulong high = (uint)(data[4] | (data[5] << 8) | (data[6] << 16) | (data[7] << 24));

			return low | (high << 32);
		}
		else if (type.IsChar)
		{
			return (char)(data[0] | (data[1] << 8));
		}
		else if (type.IsBoolean)
		{
			return data[0] != 0;
		}
		else if (type.IsR4)
		{
			var value = new byte[8];

			for (var i = 0; i < 8; i++)
				value[i] = data[i];

			return BitConverter.ToSingle(value, 0);
		}
		else if (type.IsR8)
		{
			var value = new byte[8];

			for (var i = 0; i < 8; i++)
				value[i] = data[i];

			return BitConverter.ToDouble(value, 0);
		}
		else if (type.IsVoid)
		{
			return null;
		}

		return null;
	}

	public static string OutputUnitTestResult(UnitTest unitTest)
	{
		var sb = new StringBuilder();

		switch (unitTest.Status)
		{
			case UnitTestStatus.Failed: sb.Append("FAILED"); break;
			case UnitTestStatus.FailedByCrash: sb.Append("CRASHED"); break;
			case UnitTestStatus.Skipped: sb.Append("SKIPPED"); break;
			case UnitTestStatus.Passed: sb.Append("OK"); break;
			case UnitTestStatus.Pending: sb.Append("PENDING"); break;
		}

		sb.Append(": ");

		sb.Append($"{unitTest.MethodTypeName}::{unitTest.MethodName}");

		sb.Append('(');

		foreach (var param in unitTest.Values)
		{
			sb.Append(param);
			sb.Append(',');
		}

		if (unitTest.Values.Length > 0)
		{
			sb.Length--;
		}

		sb.Append(')');

		sb.Append($" Expected: {ObjectToString(unitTest.Expected)}");
		sb.Append($" Result: {ObjectToString(unitTest.Result)}");

		return sb.ToString();
	}

	private static string ObjectToString(object o)
	{
		if (o is null)
			return "NULL";
		else
			return o.ToString();
	}

	private static void Dump(List<UnitTest> unitTests)
	{
		var sb = new StringBuilder();

		foreach (var unitTest in unitTests)
		{
			sb.Append($"{unitTest.MethodTypeName}.{unitTest.MethodName}");

			sb.Append('(');

			foreach (var param in unitTest.Values)
			{
				sb.Append(ParamToString(param));
				sb.Append(',');
			}

			if (unitTest.Values.Length > 0)
			{
				sb.Length--;
			}

			sb.Append(");");
			sb.AppendLine();
		}

		Debug.WriteLine(sb.ToString());
	}

	private static string ParamToString(object parameter)
	{
		if (parameter is Boolean)
		{
			return (bool)parameter ? "false" : "true";
		}
		else if (parameter is Char)
		{
			var c = (char)parameter;

			if (Char.IsLetterOrDigit(c) || char.IsSymbol(c))
				return $"'{c}'";
			else
				return $"(char){(int)c}";
		}
		else if (parameter is SByte)
		{
			return $"{(sbyte)parameter}";
		}
		else if (parameter is Int16)
		{
			return $"{(short)parameter}";
		}
		else if (parameter is Int32)
		{
			return $"{(int)parameter}";
		}
		else if (parameter is Byte)
		{
			return $"{(byte)parameter}";
		}
		else if (parameter is UInt16)
		{
			return $"{(ushort)parameter}";
		}
		else if (parameter is UInt32)
		{
			return $"{(uint)parameter}U";
		}
		else if (parameter is UInt64)
		{
			return $"{(ulong)parameter}UL";
		}
		else if (parameter is Int64)
		{
			return $"{(long)parameter}L";
		}
		else if (parameter is Single)
		{
			var f = (float)parameter;

			if (Single.IsNaN(f))
				return "Single.NaN";
			else if (Single.IsNegativeInfinity(f))
				return "Single.NegativeInfinity";
			else if (Single.IsPositiveInfinity(f))
				return "Single.PositiveInfinity";
			else
				return $"{f}f";
		}
		else if (parameter is Double)
		{
			var d = (double)parameter;

			if (Double.IsNaN(d))
				return "Double.NaN";
			else if (Double.IsNegativeInfinity(d))
				return "Double.NegativeInfinity";
			else if (Double.IsPositiveInfinity(d))
				return "Double.PositiveInfinity";
			else
				return $"{d}d";
		}
		else
		{
			throw new InvalidProgramException();
		}
	}
}
