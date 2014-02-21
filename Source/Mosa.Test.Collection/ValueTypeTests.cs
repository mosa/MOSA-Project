
namespace Mosa.Test.Collection
{
	public static class ValueTypeTests
	{
		struct valuetype
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
		class wrapper
		{
			public valuetype content;
		}

		public static bool TestValueTypeVariable()
		{
			valuetype p = new valuetype();
			p.a = 1;
			p.b = 7;
			p.c = 21;
			p.d = 171;

			return p.a == 1 && p.b == 7 & p.c == 21 && p.d == 171;
		}

		static valuetype staticField;
		public static bool TestValueTypeStaticField()
		{
			valuetype p = new valuetype();
			p.a = 1;
			p.b = 7;
			p.c = 21;
			p.d = 171;

			staticField = p;
			return staticField.a == 1 && staticField.b == 7 & staticField.c == 21 && staticField.d == 171;
		}

		public static bool TestValueTypeInstanceField()
		{
			valuetype p = new valuetype();
			p.a = 1;
			p.b = 7;
			p.c = 21;
			p.d = 171;

			wrapper obj = new wrapper();
			obj.content = p;
			return obj.content.a == 1 && obj.content.b == 7 & obj.content.c == 21 && obj.content.d == 171;
		}

		static bool ParameterOk(valuetype p)
		{
			return p.a == 1 && p.b == 7 & p.c == 21 && p.d == 171;
		}
		public static bool TestValueTypeParameter()
		{
			valuetype p = new valuetype();
			p.a = 1;
			p.b = 7;
			p.c = 21;
			p.d = 171;

			return ParameterOk(p);
		}

		static valuetype GetValue()
		{
			valuetype p = new valuetype();
			p.a = 1;
			p.b = 7;
			p.c = 21;
			p.d = 171;
			return p;
		}
		public static bool TestValueTypeReturnValue()
		{
			valuetype p = GetValue();
			return p.a == 1 && p.b == 7 & p.c == 21 && p.d == 171;
		}

		static bool BoxOk(object box)
		{
			valuetype p = (valuetype)box;
			return p.a == 1 && p.b == 7 & p.c == 21 && p.d == 171;
		}
		public static bool TestValueTypeBox()
		{
			valuetype p = new valuetype();
			p.a = 1;
			p.b = 7;
			p.c = 21;
			p.d = 171;

			wrapper obj = new wrapper();
			obj.content = p;
			return obj.content.a == 1 && obj.content.b == 7 & obj.content.c == 21 && obj.content.d == 171;
		}

		public static bool TestValueTypeInstanceMethod()
		{
			valuetype p = new valuetype();
			p.a = 1;
			p.b = 7;
			p.c = 21;
			p.d = 171;

			return p.Check(1, 7, 21, 171);
		}

		public static bool TestValueTypeVirtualMethod()
		{
			valuetype p = new valuetype();
			p.a = 1;
			p.b = 7;
			p.c = 21;
			p.d = 171;

			return p.ToString() == "200";
		}

		static bool ByRefModify(ref valuetype p)
		{
			bool result = p.Check(3, 11, 41, 83);
			p.a = 1;
			p.b = 7;
			p.c = 21;
			p.d = 171;
			result &= p.Check(1, 7, 21, 171);

			valuetype d = p;
			d.a = 0;
			d.b = 0;
			d.c = 0;
			d.d = 0;
			result &= p.Check(1, 7, 21, 171);
			return result;
		}
		static bool ByRefOk(ref valuetype p)
		{
			return p.a == 1 && p.b == 7 & p.c == 21 && p.d == 171;
		}

		public static bool TestValueTypePassByRef()
		{
			valuetype p = new valuetype();
			p.a = 3;
			p.b = 11;
			p.c = 41;
			p.d = 83;

			return ByRefOk(ref p);
		}

		public static bool TestValueTypePassByRefModify()
		{
			valuetype p = new valuetype();
			p.a = 1;
			p.b = 7;
			p.c = 21;
			p.d = 171;

			return ByRefModify(ref p) && p.Check(3, 11, 41, 83);
		}

		public static bool TestValueTypeArray()
		{
			valuetype[] l = new valuetype[2];
			valuetype p = new valuetype();
			p.a = 3;
			p.b = 11;
			p.c = 41;
			p.d = 83;
			l[1] = p;

			return l[1].Check(3, 11, 41, 83);
		}

		public static bool TestValueTypeArrayByRef()
		{
			valuetype[] l = new valuetype[2];
			valuetype p = new valuetype();
			p.a = 1;
			p.b = 7;
			p.c = 21;
			p.d = 171;
			l[1] = p;

			return ByRefModify(ref p) && p.Check(3, 11, 41, 83);
		}
	}
}
