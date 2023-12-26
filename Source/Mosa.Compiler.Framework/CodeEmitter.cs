// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.Linker;

namespace Mosa.Compiler.Framework;

/// <summary>
/// Base code emitter.
/// </summary>
public sealed partial class CodeEmitter
{
	#region Data Members

	/// <summary>
	/// The stream used to write machine code bytes to.
	/// </summary>
	private readonly Stream CodeStream;

	/// <summary>
	/// Holds the linker used to resolve externals.
	/// </summary>
	private readonly MosaLinker Linker;

	/// <summary>
	/// Gets the name of the method.
	/// </summary>
	public string MethodName;

	/// <summary>
	/// Patches we need to perform.
	/// </summary>
	private readonly List<LabelPatch> Patches = new();

	#endregion Data Members

	#region Properties

	/// <summary>
	/// Gets the current position.
	/// </summary>
	/// <value>The current position.</value>
	public int CurrentPosition => (int)CodeStream.Position;

	/// <summary>
	/// List of labels that were emitted.
	/// </summary>
	public Dictionary<int, int> Labels { get; set; }

	public OpcodeEncoder OpcodeEncoder { get; set; }

	#endregion Properties

	#region Constructor

	/// <summary>
	/// Initializes a new instance of <see cref="CodeEmitter" />.
	/// </summary>
	/// <param name="methodName">Name of the method.</param>
	/// <param name="linker">The linker.</param>
	/// <param name="codeStream">The stream the machine code is written to.</param>
	public CodeEmitter(string methodName, MosaLinker linker, Stream codeStream, OpcodeEncoder opcodeEncoder)
	{
		Debug.Assert(codeStream != null);
		Debug.Assert(linker != null);

		MethodName = methodName;
		Linker = linker;
		CodeStream = codeStream;
		OpcodeEncoder = opcodeEncoder;

		Labels = new Dictionary<int, int>();

		opcodeEncoder.SetEmitter(this);
	}

	#endregion Constructor

	#region Members

	/// <summary>
	/// Emits a label into the code stream.
	/// </summary>
	/// <param name="label">The label name to emit.</param>
	public void Label(int label)
	{
		/*
		 * Labels are used to resolve branches inside a procedure. Branches outside
		 * of procedures are handled differently, t.b.d.
		 *
		 * So we store the current instruction offset with the label info to be able to
		 * resolve jumps to this location.
		 *
		 */

		Debug.Assert(!Labels.ContainsKey(label));

		// Add this label to the label list, so we can resolve the jump later on
		Labels.Add(label, (int)CodeStream.Position);
	}

	private bool TryGetLabel(int label, out int position)
	{
		return Labels.TryGetValue(label, out position);
	}

	private void AddPatch(int label, int position, int bitPosition, LabelPatchType labelPatchType)
	{
		Patches.Add(new LabelPatch(label, position, bitPosition, labelPatchType));
	}

	public void ResolvePatches(TraceLog trace)
	{
		// Save the current position
		var currentPosition = CodeStream.Position;

		foreach (var patch in Patches)
		{
			if (!TryGetLabel(patch.Label, out var labelPosition))
			{
				throw new ArgumentException("Missing label while resolving patches.", $"label={labelPosition}");
			}

			patch.Patch(CodeStream, labelPosition);

			trace?.Log($"Patch L_{patch.Label:X5} @ {patch.Position} ");
		}

		// Reset the position
		CodeStream.Position = currentPosition;
	}

	#endregion Members

	#region Write Methods

	/// <summary>
	/// Writes the byte.
	/// </summary>
	/// <param name="data">The data.</param>
	public void WriteByte(byte data)
	{
		CodeStream.WriteByte(data);
	}

	/// <summary>
	/// Writes the specified data.
	/// </summary>
	/// <param name="data">The data.</param>
	public void Write(byte[] data)
	{
		CodeStream.Write(data, 0, data.Length);
	}

	#endregion Write Methods

	#region Emit Methods

	public void EmitLink(int position, Operand symbolOperand, int patchOffset, long referenceOffset, byte patchBitOffset, byte patchBitSize, byte patchValueShift)
	{
		position += patchOffset;

		if (symbolOperand.IsLabel)
		{
			Linker.Link(
				LinkType.AbsoluteAddress,
				MethodName,
				position,
				symbolOperand.Name,
				referenceOffset,
				patchBitOffset,
				patchBitSize,
				patchValueShift
			);
		}
		else if (symbolOperand.IsStaticField)
		{
			Linker.Link(
				LinkType.AbsoluteAddress,
				MethodName,
				position,
				symbolOperand.Field.FullName,
				referenceOffset,
				patchBitOffset,
				patchBitSize,
				patchValueShift
			);
		}
	}

	public void EmitForwardLink32(int offset)
	{
		Linker.Link(
			LinkType.AbsoluteAddress,
			MethodName,
			CurrentPosition,
			MethodName,
			CurrentPosition + offset,
			0,
			32,
			0
		);
	}

	#endregion Emit Methods

	public void EmitRelative24x4(Operand symbolOperand)
	{
		Linker.Link(
			LinkType.RelativeOffset,
			MethodName,
			CodeStream.Position,
			symbolOperand.Name,
			-4,
			8,
			24,
			4
		);
	}

	public void EmitRelative32(Operand symbolOperand)
	{
		Linker.Link(
			LinkType.RelativeOffset,
			MethodName,
			CodeStream.Position,
			symbolOperand.Name,
			-4,
			0,
			32,
			0
		);
	}

	public void EmitRelative64(Operand symbolOperand)
	{
		Linker.Link(
			LinkType.RelativeOffset,
			MethodName,
			CodeStream.Position,
			symbolOperand.Name,
			-8,
			0,
			64,
			0
		);
	}

	internal int EmitRelative(int label, int position, int bitPosition, LabelPatchType labelPatchType)
	{
		AddPatch(label, position, bitPosition, labelPatchType);

		return 0;
	}
}
