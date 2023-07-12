// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTests.ValueType;

public static class ValueTypeTests
{
	private struct ValueTypeTest
	{
		public byte a;
		public short b;
		public int c;
		public long d;

		public bool Check(byte _a, short _b, int _c, long _d)
		{
			return a == _a && b == _b & c == _c && d == _d;
		}

		public override string ToString()
		{
			return ((int)(a + b + c + d)).ToString();
		}
	}

	private class Wrapper
	{
		public ValueTypeTest content;
	}

	private struct ValueWrapper
	{
		public ValueTypeTest content;
	}

	[MosaUnitTest]
	public static bool TestValueTypeVariable()
	{
		var p = new ValueTypeTest();
		p.a = 1;
		p.b = 7;
		p.c = 21;
		p.d = 171;
		return p.a == 1 && p.b == 7 & p.c == 21 && p.d == 171;
	}

	private static ValueTypeTest staticField;

	[MosaUnitTest]
	public static bool TestValueTypeStaticField()
	{
		var p = new ValueTypeTest();
		p.a = 1;
		p.b = 7;
		p.c = 21;
		p.d = 171;

		staticField = p;
		return staticField.a == 1 && staticField.b == 7 & staticField.c == 21 && staticField.d == 171;
	}

	[MosaUnitTest]
	public static bool TestValueTypeInstanceField()
	{
		var p = new ValueTypeTest();
		p.a = 1;
		p.b = 7;
		p.c = 21;
		p.d = 171;

		Wrapper obj = new Wrapper();
		obj.content = p;
		return obj.content.a == 1 && obj.content.b == 7 & obj.content.c == 21 && obj.content.d == 171;
	}

	[MosaUnitTest]
	public static bool TestNestedValueTypeField()
	{
		var p = new ValueTypeTest();
		p.a = 1;
		p.b = 7;
		p.c = 21;
		p.d = 171;

		ValueWrapper val = new ValueWrapper();
		val.content = p;

		var r = val.content;
		return r.a == 1 && r.b == 7 & r.c == 21 && r.d == 171;
	}

	private static bool ParameterOk(ValueTypeTest p)
	{
		return p.a == 1 && p.b == 7 & p.c == 21 && p.d == 171;
	}

	[MosaUnitTest]
	public static bool TestValueTypeParameter()
	{
		var p = new ValueTypeTest();
		p.a = 1;
		p.b = 7;
		p.c = 21;
		p.d = 171;

		return ParameterOk(p);
	}

	private static ValueTypeTest GetValue()
	{
		var p = new ValueTypeTest();
		p.a = 1;
		p.b = 7;
		p.c = 21;
		p.d = 171;
		return p;
	}

	[MosaUnitTest]
	public static bool TestValueTypeReturnValue()
	{
		var p = GetValue();
		return p.a == 1 && p.b == 7 & p.c == 21 && p.d == 171;
	}

	private static bool BoxOk(object box)
	{
		var p = (ValueTypeTest)box;
		return p.a == 1 && p.b == 7 & p.c == 21 && p.d == 171;
	}

	[MosaUnitTest]
	public static bool TestValueTypeBox()
	{
		var p = new ValueTypeTest();
		p.a = 1;
		p.b = 7;
		p.c = 21;
		p.d = 171;

		Wrapper obj = new Wrapper();
		obj.content = p;
		return obj.content.a == 1 && obj.content.b == 7 & obj.content.c == 21 && obj.content.d == 171;
	}

	[MosaUnitTest]
	public static bool TestValueTypeInstanceMethod()
	{
		var p = new ValueTypeTest();
		p.a = 1;
		p.b = 7;
		p.c = 21;
		p.d = 171;

		return p.Check(1, 7, 21, 171);
	}

	[MosaUnitTest]
	public static bool TestValueTypeVirtualMethod()
	{
		var p = new ValueTypeTest();
		p.a = 1;
		p.b = 7;
		p.c = 21;
		p.d = 171;

		return p.ToString() == "202";
	}

	private static bool ByRefModify(ref ValueTypeTest p)
	{
		var result = p.Check(3, 11, 41, 83);
		p.a = 1;
		p.b = 7;
		p.c = 21;
		p.d = 171;
		result &= p.Check(1, 7, 21, 171);

		var d = p;
		d.a = 0;
		d.b = 0;
		d.c = 0;
		d.d = 0;
		result &= p.Check(1, 7, 21, 171);
		return result;
	}

	private static bool ByRefOk(ref ValueTypeTest p)
	{
		return p.a == 1 && p.b == 7 & p.c == 21 && p.d == 171;
	}

	[MosaUnitTest]
	public static bool TestValueTypePassByRef()
	{
		var p = new ValueTypeTest();
		p.a = 1;
		p.b = 7;
		p.c = 21;
		p.d = 171;

		return ByRefOk(ref p);
	}

	[MosaUnitTest]
	public static bool TestValueTypePassByRefModify()
	{
		var p = new ValueTypeTest();
		p.a = 3;
		p.b = 11;
		p.c = 41;
		p.d = 83;

		return ByRefModify(ref p) && p.Check(1, 7, 21, 171);
	}

	[MosaUnitTest]
	public static bool TestValueTypeArray()
	{
		var l = new ValueTypeTest[2];
		var p = new ValueTypeTest();
		p.a = 3;
		p.b = 11;
		p.c = 41;
		p.d = 83;
		l[1] = p;

		return l[1].Check(3, 11, 41, 83);
	}

	[MosaUnitTest]
	public static bool TestValueTypeArrayByRef()
	{
		var l = new ValueTypeTest[2];
		var p = new ValueTypeTest();
		p.a = 3;
		p.b = 11;
		p.c = 41;
		p.d = 83;
		l[1] = p;

		return ByRefModify(ref l[1]) && l[1].Check(1, 7, 21, 171);
	}
}
