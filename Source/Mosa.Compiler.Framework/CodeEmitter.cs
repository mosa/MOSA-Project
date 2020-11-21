// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Linker;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Base code emitter.
	/// </summary>
	public sealed class CodeEmitter
	{
		#region Patch Type

		/// <summary>
		/// Patch
		/// </summary>
		private struct Patch
		{
			/// <summary>
			/// Patch label
			/// </summary>
			public int Label;

			/// <summary>
			/// The patch's position in the stream
			/// </summary>
			public int Position;

			/// <summary>
			/// The patch size
			/// </summary>
			public int Size;

			/// <summary>
			/// Initializes a new instance of the <see cref="Patch"/> struct.
			/// </summary>
			/// <param name="label">The label.</param>
			/// <param name="position">The position.</param>
			public Patch(int label, int position, int size)
			{
				Label = label;
				Position = position;
				Size = size;
			}

			/// <summary>
			/// Returns a <see cref="System.String"/> that represents this instance.
			/// </summary>
			/// <returns>
			/// A <see cref="System.String"/> that represents this instance.
			/// </returns>
			public override string ToString()
			{
				return "[@" + Position.ToString() + " -> " + Label.ToString() + "]";
			}
		}

		#endregion Patch Type

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
		private readonly List<Patch> Patches = new List<Patch>();

		#endregion Data Members

		#region Properties

		/// <summary>
		/// Gets the current position.
		/// </summary>
		/// <value>The current position.</value>
		public int CurrentPosition { get { return (int)CodeStream.Position; } }

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

		private void AddPatch(int label, int position, int size)
		{
			Patches.Add(new Patch(label, position, size));
		}

		public void ResolvePatches()
		{
			// Save the current position
			long currentPosition = CodeStream.Position;

			foreach (var p in Patches)
			{
				if (!TryGetLabel(p.Label, out int labelPosition))
				{
					throw new ArgumentException("Missing label while resolving patches.", "label=" + labelPosition.ToString());
				}

				CodeStream.Position = p.Position;

				// Compute relative branch offset
				int relOffset = labelPosition - (p.Position + 4);

				// Write relative offset to stream
				var bytes = BitConverter.GetBytes(relOffset);
				CodeStream.Write(bytes, 4 - p.Size, p.Size);
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

		public void EmitLink(int position, PatchType patchType, Operand symbolOperand, int patchOffset, int referenceOffset)
		{
			position += patchOffset;

			if (symbolOperand.IsLabel)
			{
				Linker.Link(LinkType.AbsoluteAddress, patchType, MethodName, position, symbolOperand.Name, referenceOffset);
			}
			else if (symbolOperand.IsStaticField)
			{
				Linker.Link(LinkType.AbsoluteAddress, patchType, MethodName, position, symbolOperand.Field.FullName, referenceOffset);
			}
			else if (symbolOperand.IsSymbol)
			{
				Linker.Link(LinkType.AbsoluteAddress, patchType, MethodName, position, symbolOperand.Name, referenceOffset);
			}
		}

		public void EmitForwardLink(int offset)
		{
			Linker.Link(LinkType.AbsoluteAddress, PatchType.I32, MethodName, CurrentPosition, MethodName, CurrentPosition + offset);
		}

		#endregion Emit Methods

		public void EmitRelative24(Operand symbolOperand)
		{
			// TODO
			Linker.Link(
				LinkType.RelativeOffset,
				PatchType.I24o8,
				MethodName,
				CodeStream.Position,
				symbolOperand.Name,
				-4
			);
		}

		public void EmitRelative32(Operand symbolOperand)
		{
			Linker.Link(
				LinkType.RelativeOffset,
				PatchType.I32,
				MethodName,
				CodeStream.Position,
				symbolOperand.Name,
				-4
			);
		}

		public void EmitRelative64(Operand symbolOperand)
		{
			Linker.Link(
				LinkType.RelativeOffset,
				PatchType.I64,
				MethodName,
				CodeStream.Position,
				symbolOperand.Name,
				-8
			);
		}

		public int EmitRelative(int label, int offset, int size)
		{
			if (TryGetLabel(label, out int position))
			{
				// Yes, calculate the relative offset
				return position - (int)CodeStream.Position - offset;
			}
			else
			{
				// Forward jump, we can't resolve yet so store a patch
				AddPatch(label, (int)CodeStream.Position, size);

				return 0;
			}
		}
	}
}
