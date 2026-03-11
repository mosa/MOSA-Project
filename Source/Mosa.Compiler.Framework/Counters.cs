// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework;

/// <summary>
/// Counters
/// </summary>
public sealed class Counters
{
	private readonly Dictionary<string, Counter> Entries = new();

	private readonly Compiler Compiler;

	private readonly object _lock = new();

	private readonly string Name;

	public Counters(Compiler compiler, string name = null)
	{
		Compiler = compiler;
		Name = name;
	}

	public void Reset()
	{
		//var lockTimer = Stopwatch.StartNew();
		lock (_lock)
		{
			//Compiler.LockMonitor.RecordLockWait(Name, lockTimer);

			Entries.Clear();
		}
	}

	public void Update(string name, int count)
	{
		Update(name, count, false);
	}

	public void Set(string name, int count)
	{
		Update(name, count, true);
	}

	public void Update(Counter counter)
	{
		Update(counter.Name, counter.Count, false);
	}

	public void Update(Counters counters)
	{
		//var lockTimer = Stopwatch.StartNew();
		lock (_lock)
		{
			//Compiler.LockMonitor.RecordLockWait(Name, lockTimer);

			foreach (var counter in counters.Entries.Values)
			{
				UpdateInLock(counter.Name, counter.Count, false);
			}
		}
	}

	private void Update(string name, int count, bool reset = false)
	{
		//var lockTimer = Stopwatch.StartNew();
		lock (_lock)
		{
			//Compiler.LockMonitor.RecordLockWait(Name, lockTimer);

			UpdateInLock(name, count, reset);
		}
	}

	private void UpdateInLock(string name, int count, bool reset)
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

	public List<Counter> GetCounters()
	{
		var list = new List<Counter>();

		foreach (var counter in Entries)
		{
			list.Add(counter.Value);
		}

		return list;
	}

	public List<Counter> GetSortedCounters()
	{
		var list = GetCounters();
		list.Sort((left, right) => string.CompareOrdinal(left.Name, right.Name));
		return list;
	}

	public override string ToString() => $"Counts = {Entries.Count}";
}
