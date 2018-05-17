// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Counters
	/// </summary>
	public sealed class Counters
	{
		private Dictionary<string, int> counters = new Dictionary<string, int>();
		private readonly object _lock = new object();

		public void Reset()
		{
			counters.Clear();
		}

		public void Increment(string name, int count)
		{
			lock (_lock)
			{
				if (counters.ContainsKey(name))
					counters[name] += count;
				else
					counters.Add(name, count);
			}
		}

		public void Update(string name, int count)
		{
			lock (_lock)
			{
				if (counters.ContainsKey(name))
					counters[name] = count;
				else
					counters.Add(name, count);
			}
		}

		public void UpdateNoLock(string name, int count)
		{
			if (counters.ContainsKey(name))
				counters[name] = count;
			else
				counters.Add(name, count);
		}

		public IList<string> Export()
		{
			var counts = new List<string>();

			if (counters != null)
			{
				foreach (var item in counters)
				{
					counts.Add(item.Key + ": " + item.Value.ToString());
				}
			}

			return counts;
		}

		public void Merge(Counters counters)
		{
			if (counters.counters != null)
			{
				foreach (var item in counters.counters)
				{
					Increment(item.Key, item.Value);
				}
			}
		}
	}
}
