// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.UnitTest.Collection;
using Mosa.UnitTest.Engine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Mosa.Workspace.UnitTest.Debug
{
	internal static partial class UnitTestSystem
	{
		public static Type CombinationType = Assembly.Load("Mosa.UnitTest.Numbers").GetTypes().First(t => t.Name == "Combinations");
		public static Type SeriesType = Assembly.Load("Mosa.UnitTest.Numbers").GetTypes().First(t => t.Name == "Series");

		private static readonly UnitTestEngine unitTestEngine = new UnitTestEngine();

		public static void Start()
		{
			var stopwatch = new Stopwatch();

			unitTestEngine.Initialize();

			stopwatch.Start();

			Console.WriteLine("Discovering Unit Tests...");

			var unitTests = DiscoverUnitTests();

			Console.WriteLine("Found Tests: " + unitTests.Count.ToString());
			Console.WriteLine("Time: " + stopwatch.ElapsedMilliseconds + " ms");

			Console.WriteLine("Executing Unit Tests...");

			Execute(unitTests);

			stopwatch.Stop();
			Console.WriteLine("Time: " + stopwatch.ElapsedMilliseconds + " ms");

			((IDisposable)(unitTestEngine)).Dispose();
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
							Method = method,
							FullMethodName = fullMethodName,
							Values = paramValues,
							UnitTestAttribute = attribute,
							Skipped = false
						};

						try
						{
							unitTest.Expected = unitTest.Method.Invoke(null, unitTest.Values);
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

						unitTests.Add(unitTest);
					}
				}
			}

			return unitTests;
		}

		private static void Execute(List<UnitTest> unitTests)
		{
			foreach (var unitTest in unitTests)
			{
				if (!unitTest.Skipped)
				{
					unitTest.Result = unitTest.Values.Length == 0
						? unitTestEngine.Execute(unitTest.FullMethodName)
						: unitTestEngine.Execute(unitTest.FullMethodName, unitTest.Values);
					unitTest.Passed = unitTest.Result.Equals(unitTest.Expected);
				}

				if (unitTest.Skipped)
				{
					Console.Write("[Skipped] ");
				}
				else
				{
					if (unitTest.Passed)
						Console.Write("[Passed] ");
					else
						Console.Write("[FAILED] ");
				}

				Console.Write(unitTest.FullMethodName.Substring(25));
				Console.Write("(" + unitTest.Values.ToFormattedString() + ")");
				Console.Write(": " + unitTest.Expected.ToFormattedString());
				Console.WriteLine(" => " + unitTest.Result.ToFormattedString());
			}
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

				var property = CombinationType.GetProperty(s) ?? SeriesType.GetProperty(s);

				var values = property.GetValue("Value");

				foreach (var p in ((IEnumerable<object[]>)values))
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
				var property = CombinationType.GetProperty(unitTest.Series)
							?? SeriesType.GetProperty(unitTest.Series);

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
	}
}
