using Mosa.UnitTest.Collection;
using System;
using System.Text;

namespace Mosa.Workspace.UnitTest.Debug
{
	public static class UnitTestHelper
	{
		public static object[] GetParams(this MosaUnitTestAttribute unitTest)
		{
			object[] values = null;

			switch (unitTest.ParamCount)
			{
				case 1: values = new object[] { unitTest.Param1 }; break;
				case 2: values = new object[] { unitTest.Param1, unitTest.Param2 }; break;
				case 3: values = new object[] { unitTest.Param1, unitTest.Param2, unitTest.Param3 }; break;
				case 4: values = new object[] { unitTest.Param1, unitTest.Param2, unitTest.Param3, unitTest.Param4 }; break;
				case 5: values = new object[] { unitTest.Param1, unitTest.Param2, unitTest.Param3, unitTest.Param4, unitTest.Param5 }; break;
			}

			return values;
		}

		public static string ToFormattedString(this object o)
		{
			if (o is Byte | o is UInt16 | o is UInt32 | o is UInt64)
			{
				return String.Format("0x{0:X}", o);
			}
			else if (o is SByte | o is Int16 | o is Int32 | o is Int64)
			{
				return String.Format("0x{0:X}", o);
			}
			else if (o is Char)
			{
				return String.Format("0x{0:X}", Convert.ToUInt32(o));
			}
			else if (o is Boolean)
			{
				return ((bool)o) ? "True" : "False";
			}

			return o.ToString();
		}

		public static string ToFormmattedString(this object[] objects)
		{
			var sb = new StringBuilder();

			if (objects != null)
			{
				bool first = true;
				foreach (var param in objects)
				{
					if (first)
					{
						first = false;
					}
					else
					{
						sb.Append(", ");
					}

					sb.Append(param.ToFormattedString());
				}
			}

			return sb.ToString();
		}
	}
}
