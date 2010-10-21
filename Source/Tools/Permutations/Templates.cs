using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Tools.Permutations
{
	public class Templates
	{
		public static string Author = "[Test, Author(\"tgiphil\", \"phil@thinkedge.com\")]";

		public static IList<string> ReplaceWithExceptionOnZero(IList<string> values)
		{
			IList<string> newlist = new List<string>();

			foreach (string value in values)
				if (value == "0")
					newlist.Add(value + ", ExpectedException = typeof(DivideByZeroException)");
				else
					newlist.Add(value);

			return newlist;
		}

		public static IList<string> ReplaceWithExceptionOnLessThanZero(IList<string> values)
		{
			IList<string> newlist = new List<string>();

			foreach (string value in values)
				if (value.StartsWith("-"))
					newlist.Add(value + ", ExpectedException = typeof(OverflowException)");
				else
					newlist.Add(value);

			return newlist;
		}

		public static string CreateUnitTest(string basetype, string op, string code)
		{
			StringBuilder str = new StringBuilder();

			str.AppendLine("#region " + op);
			str.AppendLine();
			str.AppendLine(Author);
			str.AppendLine(AddNewLines(InsertBaseType(code, basetype)));
			str.AppendLine();
			str.AppendLine("#endregion // " + op);

			return str.ToString();
		}

		public static string CreateUnitTest(string basetype, string op, string code, string[] parts, IList<string>[] perms)
		{
			IList<string[]> permutations = Permutation.GetPermutation(perms);

			StringBuilder str = new StringBuilder();

			str.AppendLine("#region " + op);
			str.AppendLine();
			str.Append(CreateTestRow(permutations));
			str.AppendLine(Author);
			str.AppendLine(AddNewLines(
				InsertBaseType(
					InsertTypes(
						InsertParameters(code, parts)
					, parts)
				, basetype)
				)
			);
			str.AppendLine();
			str.AppendLine("#endregion // " + op);

			return str.ToString();
		}

		public static string SafeShiftType(string type)
		{
			if (type == "char") return "char";
			if (type == "uint") return "int";
			if (type == "ulong") return "int";
			if (type == "long") return "int";

			return type;
		}

		public static string CreateTestRow(IList<string[]> permutations)
		{
			StringBuilder str = new StringBuilder();

			foreach (string[] values in permutations)
			{
				bool comment = false;

				foreach (string part in values)
					if (part.Contains("Overflow"))
						comment = true;

				if (comment)
					str.Append("//");

				str.Append("[Row(");

				for (int index = 0; index < values.Length; index++)
				{
					if (index != 0)
						str.Append(", ");
					str.Append(values[index]);
				}

				str.AppendLine(")]");
			}

			return str.ToString();
		}

		public static string PrettyType(string type)
		{
			return char.ToUpper(type[0]).ToString() + type.Substring(1);
		}

		public static string ReplacePattern(string line, string basemask, string value)
		{
			return line.Replace("{" + basemask + "}", value).Replace("{" + basemask + "_}", value + "_").Replace("{_" + basemask + "}", "_" + value);
		}

		public static string InsertParameters(string line, string[] parts)
		{
			for (int i = 0; i < parts.Length; i++)
				line = line.Replace("{" + i.ToString() + "}", parts[i]);

			return line;
		}

		public static string InsertBaseType(string line, string basetype)
		{
			return ReplacePattern(line, "b", PrettyType(basetype));
		}

		public static string InsertTypes(string line, string[] types)
		{
			for (int i = 0; i < types.Length; i++)
				line = ReplacePattern(line, "t" + i.ToString(), PrettyType(types[i]));

			return line;
		}

		public static string AddNewLines(string line)
		{
			return line.Replace("  ", Environment.NewLine);
		}
	}
}
