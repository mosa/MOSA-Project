// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;

namespace Mosa.Compiler.Framework;

/// <summary>
/// Label Patch
/// </summary>
internal class LabelPatch
{
	/// <summary>
	/// Patch label
	/// </summary>
	public readonly int Label;

	/// <summary>
	/// The patch's position in the stream
	/// </summary>
	public readonly int Position;

	/// <summary>
	/// The patch's bit position
	/// </summary>
	public readonly int BitPosition;

	/// <summary>
	/// The patch's type
	/// </summary>
	public readonly LabelPatchType Type;

	/// <summary>
	/// Initializes a new instance of the <see cref="LabelPatch" /> struct.
	/// </summary>
	/// <param name="label">The label.</param>
	/// <param name="position">The position.</param>
	/// <param name="bitPosition"></param>
	/// <param name="type"></param>
	public LabelPatch(int label, int position, int bitPosition, LabelPatchType type)
	{
		Label = label;
		Position = position;
		BitPosition = bitPosition;
		Type = type;
	}

	public long Patch(Stream stream, int labelPosition)
	{
		// Compute relative branch offset
		var relOffset = labelPosition - Position;

		switch (Type)
		{
			case LabelPatchType.Patch_8Bits: PatchValue(stream, Position, relOffset, 8, -1, 0); break;
			case LabelPatchType.Patch_16Bits: PatchValue(stream, Position, relOffset, 16, -2, 0); break;
			case LabelPatchType.Patch_26Bits: PatchValue(stream, Position, relOffset, 26, -8, 0); break;
			case LabelPatchType.Patch_32Bits: PatchValue(stream, Position, relOffset, 32, -4, 0); break;
			case LabelPatchType.Patch_64Bits: PatchValue(stream, Position, relOffset, 64, -8, 0); break;
			case LabelPatchType.Patch_24Bits_4x: PatchValue(stream, Position, relOffset, 24, -8, 4); break;
			case LabelPatchType.Patch_26Bits_4x: PatchValue(stream, Position, relOffset, 26, -8, 4); break;
		}

		return relOffset;
	}

	public static void PatchValue(Stream stream, int position, long value, byte patchSize, int patchVaueOffset, byte patchValueShift = 0)
	{
		stream.Position = position;

		value = (value + patchVaueOffset) >> patchValueShift;

		switch (patchSize)
		{
			case 64:
				stream.Write64((uint)value);
				break;

			case 32:
				stream.Write32((uint)value);
				break;

			case 16:
				stream.Write16((ushort)value);
				break;

			case 8:
				stream.WriteByte((byte)value);
				break;

			case 26:
				{
					var data = stream.Read32();
					stream.Position = position;
					stream.Write32((data & 0x3FFFFFF) | (int)value);
					break;
				}
		}
	}

	public override string ToString() => $"[{Position} -> {Label}]";
}
