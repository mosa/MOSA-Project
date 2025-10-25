// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.UnitTests;

// This file is duplicated from Mosa.UnitTests due to both projects using
// different core libraries (and thus being incompatible with each other).
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class MosaUnitTestAttribute : Attribute
{
	public String Series { get; set; }

	public object Param1 { get; }

	public object Param2 { get; }

	public object Param3 { get; }

	public object Param4 { get; }

	public object Param5 { get; }

	public String ParamSeries1 { get; }

	public String ParamSeries2 { get; }

	public String ParamSeries3 { get; }

	public String ParamSeries4 { get; }

	public String ParamSeries5 { get; }

	public int ParamCount { get; }

	public MosaUnitTestAttribute()
	{
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

	public MosaUnitTestAttribute(string paramSeries1)
	{
		ParamCount = 1;
		ParamSeries1 = paramSeries1;
	}

	public MosaUnitTestAttribute(string paramSeries1, string paramSeries2)
	{
		ParamCount = 2;
		ParamSeries1 = paramSeries1;
		ParamSeries2 = paramSeries2;
	}

	public MosaUnitTestAttribute(string paramSeries1, string paramSeries2, string paramSeries3)
	{
		ParamCount = 3;
		ParamSeries1 = paramSeries1;
		ParamSeries2 = paramSeries2;
		ParamSeries3 = paramSeries3;
	}

	public MosaUnitTestAttribute(string paramSeries1, string paramSeries2, string paramSeries3, string paramSeries4)
	{
		ParamCount = 4;
		ParamSeries1 = paramSeries1;
		ParamSeries2 = paramSeries2;
		ParamSeries3 = paramSeries3;
		ParamSeries4 = paramSeries4;
	}

	public MosaUnitTestAttribute(string paramSeries1, string paramSeries2, string paramSeries3, string paramSeries4, string paramSeries5)
	{
		ParamCount = 5;
		ParamSeries1 = paramSeries1;
		ParamSeries2 = paramSeries2;
		ParamSeries3 = paramSeries3;
		ParamSeries4 = paramSeries4;
		ParamSeries5 = paramSeries5;
	}
}
