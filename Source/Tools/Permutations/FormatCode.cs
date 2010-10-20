using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mosa.Tools.Permutations
{
	public static class FormatCode
	{
		public static IList<string> Format(IList<string> lines)
		{
			return Format(lines, 0);
		}

		public static IList<string> Format(IList<string> lines, int startIndent)
		{
			List<string> newlines = new List<string>(lines.Count);

			int level = startIndent;

			foreach (string line in lines)
			{
				foreach (string part in line.Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
				{
					string tabs = string.Empty.PadLeft(level, '\t');

					if (part.Contains("{"))
					{
						level++;
					}
					else
						if (part.Contains("}"))
						{
							level--;
							tabs = string.Empty.PadLeft(level, '\t');
						}

					newlines.Add(tabs + part);
				}
			}

			return newlines;
		}
	}
}
