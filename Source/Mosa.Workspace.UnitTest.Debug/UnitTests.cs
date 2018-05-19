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
	internal static partial class UnitTests
	{
		private static readonly UnitTestEngine unitTestEngine = new UnitTestEngine();

		public static void Start()
		{
			var stopwatch = new Stopwatch();

			unitTestEngine.Initialize();

			stopwatch.Start();

			var unitTests = CreateUnitTests();

			Console.WriteLine("Tests: " + unitTests.Count.ToString());
			Console.WriteLine("Time: " + stopwatch.ElapsedMilliseconds + " ms");

			Execute(unitTests);

			stopwatch.Stop();
			Console.WriteLine("Time: " + stopwatch.ElapsedMilliseconds + " ms");

			((IDisposable)(unitTestEngine)).Dispose();
		}

		private static List<UnitTest> CreateUnitTests()
		{
			var combinationType = Assembly.Load("Mosa.UnitTest.Numbers").GetTypes().First(t => t.Name == "Combinations");
			var seriesType = Assembly.Load("Mosa.UnitTest.Numbers").GetTypes().First(t => t.Name == "Series");

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
						var parameterValues = attribute.GetParams();

						var unitTest = new UnitTest()
						{
							Method = method,
							FullMethodName = fullMethodName,
							ParameterValues = parameterValues,
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
						var property = combinationType.GetProperty(attribute.Series)
							?? seriesType.GetProperty(attribute.Series);

						var value = property.GetValue("Value");

						foreach (var param in ((IEnumerable<object[]>)value).ToArray())
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

		private static void Execute(List<UnitTest> unitTests)
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
				Console.Write("(" + unitTest.ParameterValues.ToFormmattedString() + ")");
				Console.Write(": " + unitTest.Expected.ToFormattedString());
				Console.WriteLine(" => " + unitTest.Result.ToFormattedString());
			}
		}
	}
}
