// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework;

/// <summary>
/// Label Patch
/// </summary>
internal class LabelPatch
{
	internal enum PatchType
	{
		Normal,
	}

	/// <summary>
	/// Patch label
	/// </summary>
	public readonly int Label;

	/// <summary>
	/// The patch's position in the stream
	/// </summary>
	public readonly int Position;

	/// <summary>
	/// The patch's type (future)
	/// </summary>
	public readonly PatchType Type;

	/// <summary>
	/// The patch size
	/// </summary>
	public readonly int Size;

	// Future
	// [Opcode-----]
	// <-----------> Opcode Length
	// <--> Offset (from label start)
	//     <---> Size (in bits)
	// Type
	//   Normal = Simple
	//   4x = Reduce Bits by removing last 4
	// or
	//	 Intel_8Bits
	//   Intel_32Bits
	//   Intel_64Bits
	//   ARM32_24Bits
	//   ARM64_26Bits
	//   ARM64_26Bits_4x

	/// <summary>
	/// Initializes a new instance of the <see cref="LabelPatch"/> struct.
	/// </summary>
	/// <param name="label">The label.</param>
	/// <param name="position">The position.</param>
	public LabelPatch(int label, int position, int size, PatchType type = PatchType.Normal)
	{
		Label = label;
		Position = position;
		Size = size;
		Type = type;
	}

	/// <summary>
	/// Returns a <see cref="System.String"/> that represents this instance.
	/// </summary>
	/// <returns>
	/// A <see cref="System.String"/> that represents this instance.
	/// </returns>
	public override string ToString() => $"[{Position} -> {Label}]";
}
