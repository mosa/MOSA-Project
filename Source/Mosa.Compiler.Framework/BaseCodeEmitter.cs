// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.Framework.Platform;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Base code emitter.
	/// </summary>
	public class BaseCodeEmitter
	{
		#region Patch Type

		/// <summary>
		/// Patch
		/// </summary>
		protected struct Patch
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="Patch"/> struct.
			/// </summary>
			/// <param name="label">The label.</param>
			/// <param name="position">The position.</param>
			public Patch(int label, int position)
			{
				Label = label;
				Position = position;
			}

			/// <summary>
			/// Patch label
			/// </summary>
			public int Label;

			/// <summary>
			/// The patch's position in the stream
			/// </summary>
			public int Position;

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
		protected Stream CodeStream;

		/// <summary>
		/// Holds the linker used to resolve externals.
		/// </summary>
		protected Linker.MosaLinker Linker;

		/// <summary>
		/// Gets the name of the method.
		/// </summary>
		protected string MethodName;

		/// <summary>
		/// Patches we need to perform.
		/// </summary>
		protected readonly List<Patch> Patches = new List<Patch>();

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

		#region Members

		/// <summary>
		/// Initializes a new instance of <see cref="BaseCodeEmitter" />.
		/// </summary>
		/// <param name="methodName">Name of the method.</param>
		/// <param name="linker">The linker.</param>
		/// <param name="codeStream">The stream the machine code is written to.</param>
		public void Initialize(string methodName, Linker.MosaLinker linker, Stream codeStream)
		{
			Debug.Assert(codeStream != null);
			Debug.Assert(linker != null);

			MethodName = methodName;
			Linker = linker;
			CodeStream = codeStream;

			// only necessary if method is being recompiled (due to inline optimization, for example)
			var symbol = linker.GetSymbol(MethodName);
			symbol.RemovePatches();

			Labels = new Dictionary<int, int>();

			OpcodeEncoder = new OpcodeEncoder(this);
		}

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

		protected bool TryGetLabel(int label, out int position)
		{
			return Labels.TryGetValue(label, out position);
		}

		protected void AddPatch(int label, int position)
		{
			Patches.Add(new Patch(label, position));
		}

		public virtual void ResolvePatches()
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
				CodeStream.Write(bytes, 0, bytes.Length);
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

		/// <summary>
		/// Emits the specified opcode.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public void Emit(BaseOpcodeEncoder opcode)
		{
			opcode.WriteTo(CodeStream);
		}

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
			Linker.Link(LinkType.AbsoluteAddress, PatchType.I4, MethodName, CurrentPosition, MethodName, CurrentPosition + offset);
		}

		#endregion Emit Methods

		public void EmitRelative32(Operand symbolOperand)
		{
			Linker.Link(
				LinkType.RelativeOffset,
				PatchType.I4,
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
				PatchType.I8,
				MethodName,
				CodeStream.Position,
				symbolOperand.Name,
				-8
			);
		}

		public int EmitRelative(int label, int offset)
		{
			if (TryGetLabel(label, out int position))
			{
				// Yes, calculate the relative offset
				return position - (int)CodeStream.Position - offset;
			}
			else
			{
				// Forward jump, we can't resolve yet so store a patch
				AddPatch(label, (int)CodeStream.Position);

				return 0;
			}
		}
	}
}
