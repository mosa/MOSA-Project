// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework;

/// <summary>
/// Stores metadata for a single GC safepoint within a compiled method.
/// </summary>
public sealed class SafePointEntry
{
	/// <summary>
	/// Method-relative byte offset at which this safepoint occurs.
	/// </summary>
	public int Offset { get; set; }

	/// <summary>
	/// Bitmap of CPU registers holding live object references or managed pointers at this safepoint.
	/// Bit N corresponds to the register whose <see cref="PhysicalRegister.Index"/> equals N.
	/// </summary>
	public uint RegisterBitmap { get; set; }

	/// <summary>
	/// For each set bit in <see cref="RegisterBitmap"/>, the corresponding bit here encodes the GC type:
	/// 0 = object reference, 1 = managed pointer.
	/// </summary>
	public uint TypeBitmap { get; set; }
}
