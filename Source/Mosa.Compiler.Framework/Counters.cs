// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.Framework;

/// <summary>
/// Counters
/// </summary>
public sealed class Counters
{
	private readonly Dictionary<string, int> Entry = new Dictionary<string, int>();
	private readonly object _lock = new object();

	public void Reset()
	{
		Entry.Clear();
	}

	public void Update(string name, int count)
	{
		lock (_lock)
		{
			UpdateSkipLock(name, count);
		}
	}

	public void UpdateSkipLock(string name, int count)
	{
		if (Entry.TryGetValue(name, out int current))
		{
			Entry[name] = current + count;
		}
		else
		{
			Entry.Add(name, count);
		}
	}

	public void NewCountSkipLock(string name, int count)
	{
		Entry.Add(name, count);
	}

	public List<string> Export(string prefex = null)
	{
		var counts = new List<string>();

		foreach (var item in Entry)
		{
			if (prefex == null)
				counts.Add($"{item.Key}: {item.Value}");
			else
				counts.Add($"{prefex}{item.Key}: {item.Value}");
		}

		return counts;
	}

	public void Merge(Counters counters)
	{
		lock (_lock)
		{
			foreach (var entry in counters.Entry)
			{
				UpdateSkipLock(entry.Key, entry.Value);
			}
		}
	}

	public override string ToString()
	{
		return $"Counts = {Entry.Count}";
	}
}
