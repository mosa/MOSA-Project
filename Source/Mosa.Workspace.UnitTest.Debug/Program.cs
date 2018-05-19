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
	internal static class Program
	{
		private static readonly UnitTestEngine unitTestEngine = new UnitTestEngine();

		public class UnitTest
		{
			public string FullMethodName { get; set; }
			public MethodInfo Method { get; set; }
			public MosaUnitTestAttribute UnitTestAttribute { get; set; }
			public object[] ParameterValues { get; set; }
			public object Expected { get; set; }
			public object Result { get; set; }
			public bool Skipped { get; set; }
			public bool Passed { get; set; }
		}

		private static void Main()
		{
			var stopwatch = new Stopwatch();

			unitTestEngine.Initialize();

			stopwatch.Start();

			var unitTests = CreateUnitTests();

			Console.WriteLine("Tests: " + unitTests.Count.ToString());
			Console.WriteLine("Time: " + stopwatch.ElapsedMilliseconds + " ms");

			ExecuteUnitTests(unitTests);

			stopwatch.Stop();
			Console.WriteLine("Time: " + stopwatch.ElapsedMilliseconds + " ms");

			((IDisposable)(unitTestEngine)).Dispose();
		}

		private static List<UnitTest> CreateUnitTests()
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
					if (attribute.Series == null)
					{
						var unitTest = new UnitTest()
						{
							Method = method,
							FullMethodName = fullMethodName,
							ParameterValues = null,
							UnitTestAttribute = attribute,
						};

						unitTest.Skipped = false;

						try
						{
							unitTest.Expected = unitTest.Method.Invoke(null, unitTest.ParameterValues);
						}
						catch (Exception e)
						{
							unitTest.Skipped = true;
						}

						if (!unitTest.Skipped)
							unitTests.Add(unitTest);
					}
					else if (attribute.Series != null)
					{
						var type = Assembly.Load("Mosa.UnitTest.Numbers").GetTypes().First(t => t.Name == "Combinations");

						var seriesProperty = type.GetProperty(attribute.Series);

						var value = seriesProperty.GetValue("Value");

						var parameters = ((IEnumerable<object[]>)value).ToArray();

						foreach (var param in parameters)
						{
							var unitTest = new UnitTest()
							{
								Method = method,
								FullMethodName = fullMethodName,
								ParameterValues = param,
								UnitTestAttribute = attribute,
							};

							unitTest.Skipped = false;

							try
							{
								unitTest.Expected = unitTest.Method.Invoke(null, unitTest.ParameterValues);
							}
							catch (Exception e)
							{
								unitTest.Skipped = true;
							}

							if (!unitTest.Skipped)
								unitTests.Add(unitTest);
						}
					}
				}
			}

			return unitTests;
		}

		private static void ExecuteUnitTests(List<UnitTest> unitTests)
		{
			foreach (var unitTest in unitTests)
			{

				if (!unitTest.Skipped)
				{
					unitTest.Result = unitTest.ParameterValues == null
						? unitTestEngine.Execute(unitTest.FullMethodName)
						: unitTestEngine.Execute(unitTest.FullMethodName, unitTest.ParameterValues);
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
				Console.Write("(");

				if (unitTest.ParameterValues != null)
				{
					bool first = true;
					foreach (var param in unitTest.ParameterValues)
					{
						if (first)
						{
							first = false;
						}
						else
						{
							Console.Write(", ");
						}

						Console.Write(param.ToString());
					}
				}

				Console.Write(")");

				Console.Write(" Expected: " + unitTest.Expected);
				Console.WriteLine(" => " + unitTest.Result);
			}
		}
	}
}
