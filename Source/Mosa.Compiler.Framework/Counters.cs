// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework;

/// <summary>
/// Counters
/// </summary>
public sealed class Counters
{
	private readonly Dictionary<string, Counter> Entries = new();
	private readonly object _lock = new();

	public void Reset()
	{
		Entries.Clear();
	}

	public void Update(string name, int count)
	{
		UpdateCounter(name, count, false);
	}

	public void Set(string name, int count)
	{
		UpdateCounter(name, count, true);
	}

	public void Update(Counter counter)
	{
		UpdateCounter(counter.Name, counter.Count, false);
	}

	public void Update(Counters counters)
	{
		foreach (var counter in counters.Entries.Values)
		{
			UpdateCounter(counter.Name, counter.Count, false);
		}
	}

	private void UpdateCounter(string name, int count, bool reset = false)
	{
		lock (_lock)
		{
			if (Entries.TryGetValue(name, out var counter))
			{
				if (reset)
				{
					counter.Set(count);
				}
				else
				{
					counter.Increment(count);
				}
			}
			else
			{
				Entries.Add(name, new Counter(name, count));
			}
		}
	}

	public List<Counter> GetCounters()
	{
		var list = new List<Counter>();

		foreach (var counter in Entries)
		{
			list.Add(counter.Value);
		}

		return list;
	}

	public override string ToString() => $"Counts = {Entries.Count}";
}
