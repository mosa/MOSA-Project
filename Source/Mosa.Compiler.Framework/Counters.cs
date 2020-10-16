// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Counters
	/// </summary>
	public sealed class Counters
	{
		private readonly Dictionary<string, int> counters = new Dictionary<string, int>();
		private readonly object _lock = new object();

		public void Reset()
		{
			counters.Clear();
		}

		public void Update(string name, int count)
		{
			lock (_lock)
			{
				if (counters.TryGetValue(name, out int current))
				{
					counters.Remove(name);
					counters.Add(name, count + current);
				}
				else
				{
					counters.Add(name, count);
				}
			}
		}

		public void UpdateSkipLock(string name, int count)
		{
			if (counters.TryGetValue(name, out int current))
			{
				counters.Remove(name);
				counters.Add(name, count + current);
			}
			else
			{
				counters.Add(name, count);
			}
		}

		public void NewCountSkipLock(string name, int count)
		{
			counters.Add(name, count);
		}

		public List<string> Export()
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
			foreach (var item in counters.counters)
			{
				Update(item.Key, item.Value);
			}
		}
	}
}
