// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;

namespace Mosa.Compiler.Framework;

/// <summary>
/// Label Patch
/// </summary>
internal partial class LabelPatch
{
	/// <summary>
	/// Patch label
	/// </summary>
	public readonly int Label;

	/// <summary>
	/// The patch's opcode start position
	/// </summary>
	public readonly int Start;

	/// <summary>
	/// The patch's position in the stream
	/// </summary>
	public readonly int Position;

	/// <summary>
	/// The patch's bit position
	/// </summary>
	public readonly int BitPosition;

	/// <summary>
	/// The patch's type (future)
	/// </summary>
	public readonly LabelPatchType Type;

	/// <summary>
	/// Initializes a new instance of the <see cref="LabelPatch"/> struct.
	/// </summary>
	/// <param name="label">The label.</param>
	/// <param name="position">The position.</param>
	public LabelPatch(int label, int start, int position, int bitPosition, LabelPatchType type)
	{
		Label = label;
		Start = start;
		Position = position;
		BitPosition = bitPosition;
		Type = type;
	}

	public long Patch(Stream stream, int labelPosition)
	{
		stream.Position = Position;

		// Compute relative branch offset
		var relOffset = labelPosition - Position;

		switch (Type)
		{
			case LabelPatchType.Patch_64Bits:
				stream.Write64(relOffset - 8);
				break;

			case LabelPatchType.Patch_32Bits:
				stream.Write32(relOffset - 4);
				break;

			case LabelPatchType.Patch_16Bits:
				stream.Write16((byte)(relOffset - 2));
				break;

			case LabelPatchType.Patch_8Bits:
				stream.WriteByte((byte)(relOffset - 1));
				break;

			case LabelPatchType.Patch_26Bits:
				{
					var data = stream.Read32();
					stream.Position = Position;
					stream.Write32((data & 0x3FFFFFF) | (relOffset - 8));
					break;
				}
			case LabelPatchType.Patch_26Bits_4x:
				{
					var data = stream.Read32();
					stream.Position = Position;
					stream.Write32((data & 0x3FFFFFF) | (relOffset - 8) >> 4);
					break;
				}
		}

		return relOffset;
	}

	/// <summary>
	/// Returns a <see cref="System.String"/> that represents this instance.
	/// </summary>
	/// <returns>
	/// A <see cref="System.String"/> that represents this instance.
	/// </returns>
	public override string ToString() => $"[{Position} -> {Label}]";
}
