/*
* (c) 2008 MOSA - The Managed Operating System Alliance
*
* Licensed under the terms of the New BSD License.
*
* Authors:
*  Phil Garcia (tgiphil) <phil@thinkedge.com>
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Mosa.Test.System
{
	static class DelegateUtility
	{

		static public string GetDelegteName(object ret, params object[] parameters)
		{
			object[] total = new object[parameters.Length + 1];
			total[0] = ret;
			for (int i = 0; i < parameters.Length; i++)
				total[i + 1] = parameters[i];

			return GetDelegteName(total);
		}

		static public string GetDelegteName(object[] parameters)
		{
			if (parameters.Length == 0)
				return string.Empty;

			StringBuilder name = new StringBuilder();

			foreach (object o in parameters)
			{
				name.Append(GetTypePartName(o));
				name.Append("_");
			}

			name.Length = name.Length - 1;

			return name.ToString();
		}

		static string GetTypePartName(object t)
		{
			if (t == null) return "V";

			if (t is ValueType)
			{
				if (t is Boolean) return "B";
				if (t is Char) return "C";
				if (t is IntPtr) return "I";
				if (t is SByte) return "I1";
				if (t is Int16) return "I2";
				if (t is Int32) return "I4";
				if (t is Int64) return "I8";
				if (t is UIntPtr) return "U";
				if (t is Byte) return "U1";
				if (t is UInt16) return "U2";
				if (t is UInt32) return "U4";
				if (t is UInt64) return "U8";
				if (t is Single) return "R4";
				if (t is Double) return "R8";
			}

			if (t is String) return "S";

			return t.GetType().ToString();
		}

		static public string GetDelegatePrototype(object[] parameters)
		{
			if (parameters.Length == 0)
				return null;

			StringBuilder prototype = new StringBuilder();

			int count = 0;

			foreach (object o in parameters)
			{
				if (count == 0)
				{
					string ret = AddReturnMarshalAttribute(GetTypeFromPartName(GetTypePartName(o)));
					if (ret != null)
						prototype.AppendLine(ret);

					prototype.Append("delegate ");
					prototype.Append(GetTypeFromPartName(GetTypePartName(o)));
					prototype.Append(" ");
					prototype.Append(GetDelegteName(parameters));
					prototype.Append("(");
				}
				else
				{
					prototype.Append(AddMarshalAttribute(GetTypeFromPartName(GetTypePartName(o))));
					prototype.Append(" value" + count.ToString() + ", ");
				}
				count++;
			}

			if (count >= 2)
				prototype.Length = prototype.Length - 2;

			prototype.Append(");");

			return prototype.ToString();
		}

		static string AddReturnMarshalAttribute(string t)
		{
			if (t == "char")
				return "[return: MarshalAs(UnmanagedType.U2)]";
			else
				return null;
		}

		static string AddMarshalAttribute(string t)
		{
			if (t == "char")
				return "[MarshalAs(UnmanagedType.U2)]" + t;
			else
				return t;
		}

		static string GetTypeFromPartName(string p)
		{
			switch (p)
			{
				case "B": return "bool";
				case "C": return "char";
				case "I": return "IntPtr";
				case "I1": return "sbyte";
				case "I2": return "short";
				case "I4": return "int";
				case "I8": return "long";
				case "U": return "UIntPtr";
				case "U1": return "byte";
				case "U2": return "ushort";
				case "U4": return "uint";
				case "U8": return "ulong";
				case "R4": return "float";
				case "R8": return "double";
				case "S": return "string";
				case "V": return "void";
				default: return null;
			}
		}
	}
}
