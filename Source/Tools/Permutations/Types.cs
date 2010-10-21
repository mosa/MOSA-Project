using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Tools.Permutations
{
	static class Types
	{
		public static IList<string> GetTypes()
		{
			IList<string> types = new List<string>();

			types.Add("U1");
			types.Add("I1");
			types.Add("U2");
			types.Add("I2");
			types.Add("U4");
			types.Add("I4");
			types.Add("U8");
			types.Add("I8");

			return types;
		}

		public static string ToString(string op)
		{
			switch (op)
			{
				case "U1": return "byte";
				case "I1": return "sbyte";
				case "U2": return "ushort";
				case "I2": return "short";
				case "U4": return "uint";
				case "I4": return "int";
				case "U8": return "ulong";
				case "I8": return "long";
				default: return string.Empty;
			}
		}
	}
}
