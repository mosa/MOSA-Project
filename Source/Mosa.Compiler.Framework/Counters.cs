// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework;

/// <summary>
/// Counters
/// </summary>
public sealed class Counters
{
	private readonly Dictionary<string, int> Entries = new();
	private readonly object _lock = new();

	public void Reset()
	{
		Entries.Clear();
	}

	public void Update(string name, int count)
	{
		lock (_lock)
		{
			UpdateSkipLock(name, count);
		}
	}

	private void UpdateSkipLock(string name, int count)
	{
		if (Entries.TryGetValue(name, out var current))
		{
			Entries[name] = current + count;
		}
		else
		{
			Entries.Add(name, count);
		}
	}

	public void NewCountSkipLock(string name, int count)
	{
		Entries.Add(name, count);
	}

	public List<string> Export(string prefex = null)
	{
		var counts = new List<string>();

		foreach (var item in Entries)
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
			foreach (var entry in counters.Entries)
			{
				UpdateSkipLock(entry.Key, entry.Value);
			}
		}
	}

	public List<Counter> GetCounters()
	{
		var list = new List<Counter>();

		foreach (var counter in Entries)
		{
			list.Add(new Counter(counter.Key, counter.Value));
		}

		return list;
	}

	public override string ToString() => $"Counts = {Entries.Count}";
}
