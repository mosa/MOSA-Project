// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	///
	/// </summary>
	public class Counters
	{
		protected Dictionary<string, int> counters = new Dictionary<string, int>();

		public Counters()
		{ }

		public void UpdateCounter(string name, int count)
		{
			lock (counters)
			{
				if (counters.ContainsKey(name))
					counters[name] = counters[name] + count;
				else
					counters.Add(name, count);
			}
		}

		public IList<string> Export()
		{
			List<string> counts = new List<string>();

			foreach (var item in counters)
			{
				counts.Add(item.Key + ": " + item.Value.ToString());
			}

			return counts;
		}
	}
}