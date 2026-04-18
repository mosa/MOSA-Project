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
	/// False = managed pointer; true = object reference.
	/// </summary>
	public bool IsObject { get; set; }

	/// <summary>
	/// Method-relative byte offset where this GC root range begins.
	/// </summary>
	public int Start;

	/// <summary>
	/// Length in bytes of the live GC root range starting at <see cref="Start"/>.
	/// </summary>
	public int Length;
}
