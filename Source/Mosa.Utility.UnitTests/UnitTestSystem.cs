// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.MosaTypeSystem;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Mosa.Utility.UnitTests
{
	public static class UnitTestSystem
	{
		public static Type CombinationType = Assembly.Load("Mosa.Utility.UnitTests").GetTypes().First(t => t.Name == "Combinations");
		public static Type SeriesType = Assembly.Load("Mosa.Utility.UnitTests").GetTypes().First(t => t.Name == "Series2");

		public static int Start(bool display)
		{
			var stopwatch = new Stopwatch();
			stopwatch.Start();

			Console.WriteLine("Discovering Unit Tests...");
			var discoveredUnitTests = Discovery.DiscoverUnitTests();
			Console.WriteLine("Found Tests: " + discoveredUnitTests.Count.ToString());
			var elapsedDiscovery = stopwatch.ElapsedMilliseconds;
			Console.WriteLine("Elapsed: " + (elapsedDiscovery / 1000.0).ToString("F2") + " secs");
			Console.WriteLine();

			Console.WriteLine("Starting Unit Test Engine...");
			var unitTestEngine = new UnitTestEngine(display);
			var elapsedCompile = stopwatch.ElapsedMilliseconds - elapsedDiscovery;
			Console.WriteLine("Elapsed: " + (elapsedCompile / 1000.0).ToString("F2") + " secs");
			Console.WriteLine();

			Console.WriteLine("Preparing Unit Tests...");
			var unitTests = PrepareUnitTest(discoveredUnitTests, unitTestEngine.TypeSystem, unitTestEngine.Linker);
			var elapsedPreparing = stopwatch.ElapsedMilliseconds - elapsedDiscovery - elapsedCompile;
			Console.WriteLine("Elapsed: " + (elapsedPreparing / 1000.0).ToString("F2") + " secs");
			Console.WriteLine();

			Console.WriteLine("Executing Unit Tests...");
			Execute(unitTests, unitTestEngine);
			var elapsedExecuting = stopwatch.ElapsedMilliseconds - elapsedDiscovery - elapsedCompile - elapsedPreparing;
			Console.WriteLine("Elapsed: " + (elapsedExecuting / 1000.0).ToString("F2") + " secs");
			Console.WriteLine();

			stopwatch.Stop();

			Console.WriteLine("Total Elapsed: " + (stopwatch.ElapsedMilliseconds / 1000.0) + " secs");

			unitTestEngine.Terminate();

			int failures = 0;
			int passed = 0;
			int skipped = 0;

			foreach (var unitTest in unitTests)
			{
				if (unitTest.Status == UnitTestStatus.Passed)
				{
					passed++;
					continue;
				}

				if (unitTest.Status == UnitTestStatus.Skipped)
				{
					skipped++;
					continue;
				}

				failures++;
				Console.WriteLine(OutputUnitTestResult(unitTest));
			}

			Console.WriteLine();
			Console.WriteLine("Unit Test Results:");
			Console.WriteLine($"   Passed:   {passed}");
			Console.WriteLine($"   Skipped:  {skipped}");
			Console.WriteLine($"   Failures: {failures}");
			Console.WriteLine($"   Total:    {passed + skipped + failures}");
			Console.WriteLine();

			if (failures == 0)
			{
				Console.WriteLine("All unit tests passed successfully!");
			}
			else
			{
				Console.WriteLine("Failures occurred in the unit tests!");
			}

			return failures;
		}

		private static List<UnitTest> PrepareUnitTest(List<UnitTestInfo> discoveredUnitTests, TypeSystem typeSystem, MosaLinker linker)
		{
			var unitTests = new List<UnitTest>(discoveredUnitTests.Count);

			int id = 0;

			foreach (var unitTestInfo in discoveredUnitTests)
			{
				var linkerMethodInfo = Linker.GetMethodInfo(typeSystem, linker, unitTestInfo);

				var unitTest = new UnitTest(unitTestInfo, linkerMethodInfo);

				unitTest.SerializedUnitTest = SerializeUnitTestMessage(unitTest);
				unitTest.UnitTestID = id++;

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

		public static void SerializeUnitTest(UnitTest unitTest)
		{
			unitTest.SerializedUnitTest = SerializeUnitTestMessage(unitTest);
		}

		public static IList<int> SerializeUnitTestMessage(UnitTest unitTest)
		{
			var cmd = new List<int>(4 + 4 + 4 + unitTest.MosaMethod.Signature.Parameters.Count)
				{
					(int)unitTest.MosaMethodAddress,
					GetReturnResultType(unitTest.MosaMethod.Signature.ReturnType),
					0
				};

			foreach (var parm in unitTest.Values)
			{
				AddParameters(cmd, parm);
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
			if ((parameter == null) || !(parameter is ValueType))
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
				cmd.Add((int)(sbyte)parameter);
			}
			else if (parameter is Int16)
			{
				cmd.Add((int)(short)parameter);
			}
			else if (parameter is int)
			{
				cmd.Add((int)(int)parameter);
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
				cmd.Add((int)((uint)parameter));
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
				cmd.Add((int)((long)u));
				cmd.Add((int)((long)u >> 32));
			}
			else
			{
				throw new InvalidProgramException();
			}
		}

		public static void ParseResultData(UnitTest unitTest, List<byte> data)
		{
			unitTest.Result = GetResult(unitTest.MosaMethod.Signature.ReturnType, data);
		}

		public static object GetResult(MosaType type, List<byte> data)
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

				for (int i = 0; i < 8; i++)
					value[i] = data[i];

				return BitConverter.ToSingle(value, 0);
			}
			else if (type.IsR8)
			{
				var value = new byte[8];

				for (int i = 0; i < 8; i++)
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

			sb.Append("(");

			foreach (var param in unitTest.Values)
			{
				sb.Append(param.ToString());
				sb.Append(",");
			}

			if (unitTest.Values.Length > 0)
			{
				sb.Length--;
			}

			sb.Append(")");

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
	}
}
