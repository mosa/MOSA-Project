// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using System.Text;
using Mosa.Compiler.Framework.RegisterAllocator.RedBlackTree;

namespace Mosa.Compiler.Framework.RegisterAllocator;

public sealed class RegisterTrack
{
	public readonly PhysicalRegister Register;

	private readonly IntervalTree<LiveInterval> Intervals = new IntervalTree<LiveInterval>();

	public readonly bool IsReserved;

	public bool IsFloatingPoint => Register.IsFloatingPoint;

	public bool IsInteger => Register.IsInteger;

	public RegisterTrack(PhysicalRegister register, bool reserved)
	{
		Register = register;
		IsReserved = reserved;
	}

	public void Add(LiveInterval liveInterval)
	{
		Debug.Assert(!Intervals.Contains(liveInterval.StartValue, liveInterval.EndValue));

		Intervals.Add(liveInterval.StartValue, liveInterval.EndValue, liveInterval);

		liveInterval.LiveIntervalTrack = this;
	}

	public void Evict(LiveInterval liveInterval)
	{
		Intervals.Remove(liveInterval.StartValue, liveInterval.EndValue);

		Debug.Assert(!Intervals.Contains(liveInterval.StartValue, liveInterval.EndValue));

		liveInterval.LiveIntervalTrack = null;
	}

	public void Evict(List<LiveInterval> liveIntervals)
	{
		foreach (var interval in liveIntervals)
		{
			Evict(interval);
		}
	}

	public bool Intersects(LiveInterval liveInterval)
	{
		return Intervals.Contains(liveInterval.StartValue, liveInterval.EndValue);
	}

	public bool Intersects(SlotIndex slotIndex)
	{
		return Intervals.Contains(slotIndex.Index);
	}

	public LiveInterval GetLiveIntervalAt(SlotIndex slotIndex)
	{
		return Intervals.SearchFirstOverlapping(slotIndex.Index);
	}

	public List<LiveInterval> GetIntersections(LiveInterval liveInterval)
	{
		return Intervals.Search(liveInterval.StartValue, liveInterval.EndValue);
	}

	public override string ToString() => Register.ToString();

	public string ToString2()
	{
		var sb = new StringBuilder();

		sb.Append(Register);
		sb.Append(' ');

		foreach (var interval in Intervals)
		{
			sb.Append(interval);
			sb.Append(", ");
		}

		sb.Length -= 2;

		return sb.ToString();
	}
}
