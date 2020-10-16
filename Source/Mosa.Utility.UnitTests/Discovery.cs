// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;
using Mosa.UnitTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Mosa.Utility.UnitTests
{
	public class Discovery
	{
		public static Type CombinationType = Assembly.Load("Mosa.Utility.UnitTests").GetTypes().First(t => t.Name == "Combinations");
		public static Type SeriesType = Assembly.Load("Mosa.Utility.UnitTests").GetTypes().First(t => t.Name == "Series2");

		public static List<UnitTestInfo> DiscoverUnitTests()
		{
			var unitTests = new List<UnitTestInfo>();

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
						var unitTest = new UnitTestInfo()
						{
							MethodInfo = method,
							FullMethodName = fullMethodName,
							Values = paramValues,
							UnitTestAttribute = attribute,
						};

						unitTests.Add(unitTest);

						GetExpectedResult(unitTest);
					}
				}
			}

			return unitTests;
		}

		private static List<object[]> GetParameters(MosaUnitTestAttribute unitTest)
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

		private static IEnumerable<object> GetParamList(MosaUnitTestAttribute unitTest, int index)
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

		private static object GetParam(MosaUnitTestAttribute unitTest, int index)
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

		private static void GetExpectedResult(UnitTestInfo unitTest)
		{
			try
			{
				unitTest.Expected = unitTest.MethodInfo.Invoke(null, unitTest.Values);
			}
			catch (Exception e)
			{
				if (e.InnerException is DivideByZeroException || e.InnerException is OverflowException)
				{
					unitTest.Skip = true;
				}
				else
				{
					unitTest.Skip = true;
				}
			}
		}
	}
}
