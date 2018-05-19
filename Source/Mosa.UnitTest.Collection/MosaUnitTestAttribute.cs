using System;

namespace Mosa.UnitTest.Collection
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class MosaUnitTestAttribute : Attribute
	{
		public String Series { get; set; }

		public object Param1 { get; set; }
		public object Param2 { get; set; }
		public object Param3 { get; set; }
		public object Param4 { get; set; }
		public object Param5 { get; set; }

		public String ParamSeries1 { get; set; }
		public String ParamSeries2 { get; set; }
		public String ParamSeries3 { get; set; }
		public String ParamSeries4 { get; set; }
		public String ParamSeries5 { get; set; }

		public int ParamCount { get; set; }

		public MosaUnitTestAttribute()
		{
		}

		public MosaUnitTestAttribute(String series)
		{
			Series = series;
		}

		public MosaUnitTestAttribute(object param1)
		{
			ParamCount = 1;
			Param1 = param1;
		}

		public MosaUnitTestAttribute(object param1, object param2)
		{
			ParamCount = 2;
			Param1 = param1;
			Param2 = param2;
		}

		public MosaUnitTestAttribute(object param1, object param2, object param3)
		{
			ParamCount = 3;
			Param1 = param1;
			Param2 = param2;
			Param3 = param3;
		}

		public MosaUnitTestAttribute(object param1, object param2, object param3, object param4)
		{
			ParamCount = 4;
			Param1 = param1;
			Param2 = param2;
			Param3 = param3;
			Param4 = param4;
		}

		public MosaUnitTestAttribute(object param1, object param2, object param3, object param4, object param5)
		{
			ParamCount = 5;
			Param1 = param1;
			Param2 = param2;
			Param3 = param3;
			Param4 = param4;
			Param5 = param5;
		}
	}
}
