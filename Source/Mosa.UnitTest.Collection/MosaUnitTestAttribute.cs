using System;

namespace Mosa.UnitTest.Collection
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class MosaUnitTestAttribute : Attribute
	{
		public String Series { get; set; }

		public String ParamSeries1 { get; set; }
		public String ParamSeries2 { get; set; }
		public String ParamSeries3 { get; set; }
		public String ParamSeries4 { get; set; }
		public String ParamSeries5 { get; set; }

		public object Param1 { get; set; }
		public object Param2 { get; set; }
		public object Param3 { get; set; }
		public object Param4 { get; set; }
		public object Param5 { get; set; }
	}
}
