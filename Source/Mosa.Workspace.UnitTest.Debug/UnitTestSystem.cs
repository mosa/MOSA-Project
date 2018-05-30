// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Linker;
using Mosa.Compiler.MosaTypeSystem;
using Mosa.UnitTest.Collection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Mosa.Workspace.UnitTest.Debug
{
	public static class UnitTestSystem
	{
		public static Type CombinationType = Assembly.Load("Mosa.UnitTest.Numbers").GetTypes().First(t => t.Name == "Combinations");
		public static Type SeriesType = Assembly.Load("Mosa.UnitTest.Numbers").GetTypes().First(t => t.Name == "Series2");

		public static void Start()
		{
			var stopwatch = new Stopwatch();
			stopwatch.Start();

			Console.WriteLine("Discovering Unit Tests...");
			var unitTests = DiscoverUnitTests();
			Console.WriteLine("Found Tests: " + unitTests.Count.ToString());
			var elapsedDiscovery = stopwatch.ElapsedMilliseconds;
			Console.WriteLine("Elapsed: " + (elapsedDiscovery / 1000.0).ToString("F2") + " secs");
			Console.WriteLine();

			Console.WriteLine("Compiling Unit Tests...");
			var unitTestEngine = new UnitTestEngineV2();
			unitTestEngine.Initialize();
			unitTestEngine.Compile();
			var elapsedCompile = stopwatch.ElapsedMilliseconds - elapsedDiscovery;
			Console.WriteLine("Elapsed: " + (elapsedCompile / 1000.0).ToString("F2") + " secs");
			Console.WriteLine();

			Console.WriteLine("Preparing Unit Tests...");
			PrepareUnitTest(unitTests, unitTestEngine.TypeSystem, unitTestEngine.Linker);
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

			((IDisposable)(unitTestEngine)).Dispose();
		}

		private static void PrepareUnitTest(List<UnitTest> unitTests, TypeSystem typeSystem, BaseLinker linker)
		{
			int id = 0;

			foreach (var unitTest in unitTests)
			{
				unitTest.UnitTestID = ++id;

				ResolveExpectedResult(unitTest);
				ResolveName(unitTest);
				ResolveMosaMethod(unitTest, typeSystem);
				ResolveAddress(unitTest, linker);
				SerializeUnitTest(unitTest);
			}
		}

		private static void ResolveExpectedResult(UnitTest unitTest)
		{
			try
			{
				unitTest.Expected = unitTest.MethodInfo.Invoke(null, unitTest.Values);
			}
			catch (Exception e)
			{
				if (e.InnerException is DivideByZeroException || e.InnerException is OverflowException)
				{
					unitTest.Skipped = true;
				}
				else
				{
					unitTest.Skipped = true;
				}
			}
		}

		private static List<UnitTest> DiscoverUnitTests()
		{
			var unitTests = new List<UnitTest>();

			var assembly = typeof(MosaUnitTestAttribute).Assembly;

			var methods = assembly.GetTypes()
					  .SelectMany(t => t.GetMethods())
					  .Where(m => m.GetCustomAttributes(typeof(MosaUnitTestAttribute), false).Length > 0)
					  .ToArray();

			foreach (var method in methods)
			{
				var fullMethodName = method.DeclaringType.FullName + "." + method.Name;

				foreach (var attribute in method.GetCustomAttributes<MosaUnitTestAttribute>())
				{
					foreach (var paramValues in GetParameters(attribute))
					{
						var unitTest = new UnitTest()
						{
							MethodInfo = method,
							FullMethodName = fullMethodName,
							Values = paramValues,
							UnitTestAttribute = attribute,
							Skipped = false
						};

						unitTests.Add(unitTest);
					}
				}
			}

			return unitTests;
		}

		private static void Execute(List<UnitTest> unitTests, UnitTestEngineV2 unitTestEngine)
		{
			unitTestEngine.QueueUnitTests(unitTests);

			unitTestEngine.WaitUntilComplete();
		}

		public static object GetParam(MosaUnitTestAttribute unitTest, int index)
		{
			switch (index)
			{
				case 1: return unitTest.Param1 ?? unitTest.ParamSeries1;
				case 2: return unitTest.Param2 ?? unitTest.ParamSeries2;
				case 3: return unitTest.Param3 ?? unitTest.ParamSeries3;
				case 4: return unitTest.Param4 ?? unitTest.ParamSeries4;
				case 5: return unitTest.Param5 ?? unitTest.ParamSeries5;
			}

			return null;
		}

		public static IEnumerable<object> GetParamList(MosaUnitTestAttribute unitTest, int index)
		{
			var param = GetParam(unitTest, index);

			if (param is string)
			{
				string s = param as string;

				var property = SeriesType.GetProperty(s);

				var values = property.GetValue("Value");

				var val = (IEnumerable<object>)values;

				foreach (var p in val)
				{
					yield return p;
				}
			}
			else
			{
				yield return param;
			}
		}

		public static List<object[]> GetParameters(MosaUnitTestAttribute unitTest)
		{
			var list = new List<object[]>();

			if (unitTest.Series != null)
			{
				var property = CombinationType.GetProperty(unitTest.Series);

				var value = property.GetValue("Value");

				foreach (var param in ((IEnumerable<object[]>)value))
				{
					list.Add(param);
				}
			}
			else if (unitTest.ParamCount == 0)
			{
				list.Add(new object[] { });
			}
			else if (unitTest.ParamCount == 1)
			{
				foreach (var p1 in GetParamList(unitTest, 1))
				{
					list.Add(new object[] { p1 });
				}
			}
			else if (unitTest.ParamCount == 2)
			{
				foreach (var p1 in GetParamList(unitTest, 1))
				{
					foreach (var p2 in GetParamList(unitTest, 2))
					{
						list.Add(new object[] { p1, p2 });
					}
				}
			}
			else if (unitTest.ParamCount == 3)
			{
				foreach (var p1 in GetParamList(unitTest, 1))
				{
					foreach (var p2 in GetParamList(unitTest, 2))
					{
						foreach (var p3 in GetParamList(unitTest, 3))
						{
							list.Add(new object[] { p1, p2, p3 });
						}
					}
				}
			}
			else if (unitTest.ParamCount == 4)
			{
				foreach (var p1 in GetParamList(unitTest, 1))
				{
					foreach (var p2 in GetParamList(unitTest, 2))
					{
						foreach (var p3 in GetParamList(unitTest, 3))
						{
							foreach (var p4 in GetParamList(unitTest, 4))
							{
								list.Add(new object[] { p1, p2, p3, p4 });
							}
						}
					}
				}
			}
			else if (unitTest.ParamCount == 5)
			{
				foreach (var p1 in GetParamList(unitTest, 1))
				{
					foreach (var p2 in GetParamList(unitTest, 2))
					{
						foreach (var p3 in GetParamList(unitTest, 3))
						{
							foreach (var p4 in GetParamList(unitTest, 4))
							{
								foreach (var p5 in GetParamList(unitTest, 5))
								{
									list.Add(new object[] { p1, p2, p3, p4, p5 });
								}
							}
						}
					}
				}
			}

			return list;
		}

		public static void ResolveName(UnitTest unitTest)
		{
			string fullMethodName = unitTest.FullMethodName;

			int first = fullMethodName.LastIndexOf(".");
			int second = fullMethodName.LastIndexOf(".", first - 1);

			unitTest.MethodNamespaceName = fullMethodName.Substring(0, second);
			unitTest.MethodTypeName = fullMethodName.Substring(second + 1, first - second - 1);
			unitTest.MethodName = fullMethodName.Substring(first + 1);
		}

		public static void ResolveAddress(UnitTest unitTest, BaseLinker linker)
		{
			unitTest.MosaMethodAddress = GetMethodAddress(unitTest.MosaMethod, linker);
		}

		public static void ResolveMosaMethod(UnitTest unitTest, TypeSystem typeSystem)
		{
			unitTest.MosaMethod = FindMosaMethod(
				typeSystem,
				unitTest.MethodNamespaceName,
				unitTest.MethodTypeName,
				unitTest.MethodName,
				unitTest.Values);
		}

		public static MosaMethod FindMosaMethod(TypeSystem typeSystem, string ns, string type, string method, params object[] parameters)
		{
			foreach (var t in typeSystem.AllTypes)
			{
				if (t.Name != type)
					continue;

				if (!string.IsNullOrEmpty(ns) && t.Namespace != ns)
					continue;

				foreach (var m in t.Methods)
				{
					if (m.Name == method && m.Signature.Parameters.Count == parameters.Length)
					{
						return m;
					}
				}

				break;
			}

			return null;
		}

		public static IntPtr GetMethodAddress(MosaMethod method, BaseLinker linker)
		{
			var symbol = linker.GetSymbol(method.FullName, SectionKind.Text);

			return new IntPtr((long)symbol.VirtualAddress);
		}

		public static MosaMethod FindMethod(TypeSystem typeSystem, string ns, string type, string method, params object[] parameters)
		{
			foreach (var t in typeSystem.AllTypes)
			{
				if (t.Name != type)
					continue;

				if (!string.IsNullOrEmpty(ns) && t.Namespace != ns)
					continue;

				foreach (var m in t.Methods)
				{
					if (m.Name == method && m.Signature.Parameters.Count == parameters.Length)
					{
						return m;
					}
				}

				break;
			}

			return null;
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
	}
}
