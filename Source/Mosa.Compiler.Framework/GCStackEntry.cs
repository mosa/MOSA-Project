// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework;

/// <summary>
/// Describes a GC-root stack slot and the code ranges over which it holds a live managed reference.
/// Used by the GC to scan threads paused at an arbitrary instruction (no safepoint required).
/// </summary>
public sealed class GCStackEntry
{
	/// <summary>
	/// Frame-relative byte offset of this stack slot.
	/// Negative for locals (below frame pointer), positive for parameters (above).
	/// </summary>
	public int StackOffset { get; set; }

	/// <summary>
	/// False = object reference; true = managed pointer.
	/// </summary>
	public bool IsManagedPointer { get; set; }

	/// <summary>
	/// Ordered, non-overlapping code ranges (method-relative start, length) over which this slot holds a live GC root.
	/// </summary>
	public List<(int Start, int Length)> LiveRanges { get; set; }
}
