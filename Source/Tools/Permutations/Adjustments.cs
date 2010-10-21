using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Tools.Permutations
{
	public static class Adjustments
	{

		public static IList<string> CommentOut(IList<string> lines, string match)
		{
			for (int i = 0; i < lines.Count; i++)
			{
				if (lines[i].Contains(match))
				{
					int pos = lines[i].IndexOf(match);

					lines[i] = lines[i].Insert(pos, @"//") + @" // Adjustment";
				}
			}

			return lines;
		}

		//public static string CommentOut(string lines, string match)
		//{
		//    int pos = lines.IndexOf(match);

		//    if (pos >= 0)
		//        return lines.Insert(pos, @"//");

		//    return lines;
		//}

		public static string CommentOut(string lines, string match, string comment)
		{
			return lines.Replace(match, @"//" + match + @" // " + comment);
		}

	}
}
