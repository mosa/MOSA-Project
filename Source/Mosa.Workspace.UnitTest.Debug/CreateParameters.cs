using Mosa.UnitTest.Collection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Mosa.Workspace.UnitTest.Debug
{
	public static class CreateParameters
	{
		public static Type CombinationType = Assembly.Load("Mosa.UnitTest.Numbers").GetTypes().First(t => t.Name == "Combinations");
		public static Type SeriesType = Assembly.Load("Mosa.UnitTest.Numbers").GetTypes().First(t => t.Name == "Series");

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

		public static IEnumerable<object[]> GetParamList(MosaUnitTestAttribute unitTest, int index)
		{
			var param = GetParam(unitTest, index);

			if (param is string)
			{
				string s = param as string;

				var property = CombinationType.GetProperty(s) ?? SeriesType.GetProperty(s);

				var values = property.GetValue("Value");

				foreach (var p in ((IEnumerable<object[]>)values).ToArray())
				{
					yield return new object[] { p };
				}
			}
			else
			{
				yield return new object[] { param };
			}
		}

		public static List<object> GetParameters(MosaUnitTestAttribute unitTest)
		{
			var list = new List<object>();

			if (unitTest.ParamCount == 0)
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
