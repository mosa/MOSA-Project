/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Scott Balmos <sbalmos@fastmail.fm>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using Mosa.Compiler.Linker;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Base code emitter.
	/// </summary>
	public class BaseCodeEmitter : IDisposable, ICodeEmitter
	{

		#region Types

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
			public Patch(int label, long position)
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
			public long Position;

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

		#endregion // Types

		#region Data members

		/// <summary>
		/// The compiler that is generating the code.
		/// </summary>
		protected BaseMethodCompiler compiler;

		/// <summary>
		/// The stream used to write machine code bytes to.
		/// </summary>
		protected Stream codeStream;

		/// <summary>
		/// The position that the code stream starts.
		/// </summary>
		protected long codeStreamBasePosition;

		/// <summary>
		/// List of labels that were emitted.
		/// </summary>
		protected readonly Dictionary<int, long> labels = new Dictionary<int, long>();

		/// <summary>
		/// Holds the linker used to resolve externals.
		/// </summary>
		protected ILinker linker;

		/// <summary>
		/// List of literal patches we need to perform.
		/// </summary>
		protected readonly List<Patch> literals = new List<Patch>();

		/// <summary>
		/// Patches we need to perform.
		/// </summary>
		protected readonly List<Patch> patches = new List<Patch>();

		#endregion

		#region IDisposable Members

		/// <summary>
		/// Completes emitting the code of a method.
		/// </summary>
		public void Dispose()
		{
			// Flush the stream - we're not responsible for disposing it, as it belongs
			// to another component that gave it to the code generator.
			codeStream.Flush();
		}

		#endregion // IDisposable Members

		#region ICodeEmitter Members

		/// <summary>
		/// Initializes a new instance of <see cref="BaseCodeEmitter"/>.
		/// </summary>
		/// <param name="compiler">The compiler.</param>
		/// <param name="codeStream">The stream the machine code is written to.</param>
		void ICodeEmitter.Initialize(BaseMethodCompiler compiler, Stream codeStream)
		{
			Debug.Assert(null != compiler, @"MachineCodeEmitter needs a method compiler.");
			if (compiler == null)
				throw new ArgumentNullException(@"compiler");
			Debug.Assert(null != codeStream, @"MachineCodeEmitter needs a code stream.");
			if (codeStream == null)
				throw new ArgumentNullException(@"codeStream");

			this.compiler = compiler;
			this.codeStream = codeStream;
			this.codeStreamBasePosition = codeStream.Position;
			this.linker = compiler.Linker;
		}

		void ICodeEmitter.ResolvePatches()
		{
			// Save the current position
			long currentPosition = codeStream.Position;

			foreach (Patch p in patches)
			{
				long labelPosition;
				if (!labels.TryGetValue(p.Label, out labelPosition))
				{
					throw new ArgumentException(@"Missing label while resolving patches.", @"label");
				}

				codeStream.Position = p.Position;

				// Compute relative branch offset
				int relOffset = (int)labelPosition - ((int)p.Position + 4);

				// Write relative offset to stream
				byte[] bytes = BitConverter.GetBytes(relOffset);
				codeStream.Write(bytes, 0, bytes.Length);
			}

			patches.Clear();

			// Reset the position
			codeStream.Position = currentPosition;
		}

		/// <summary>
		/// Emits a label into the code stream.
		/// </summary>
		/// <param name="label">The label name to emit.</param>
		void ICodeEmitter.Label(int label)
		{
			/*
			 * Labels are used to resolve branches inside a procedure. Branches outside
			 * of procedures are handled differently, t.b.d. 
			 * 
			 * So we store the current instruction offset with the label info to be able to 
			 * resolve jumps to this location.
			 *
			 */

			long labelPosition;
			if (labels.TryGetValue(label, out labelPosition))
			{
				if (labelPosition != codeStream.Position)
					throw new ArgumentException(@"Label already defined for another code point.", @"label");
			}

			// Add this label to the label list, so we can resolve the jump later on
			labels.Add(label, codeStream.Position);

			//Debug.WriteLine("LABEL: " + label.ToString() + " @" + codeStream.Position.ToString());
		}

		/// <summary>
		/// Gets the position.
		/// </summary>
		/// <param name="label">The label.</param>
		/// <returns></returns>
		long ICodeEmitter.GetPosition(int label)
		{
			return labels[label];
		}

		/// <summary>
		/// Gets the current position.
		/// </summary>
		/// <value>The current position.</value>
		long ICodeEmitter.CurrentPosition
		{
			get { return codeStream.Position; }
		}

		#endregion // ICodeEmitter Members

		#region Code Generation Members

		/// <summary>
		/// Writes the byte.
		/// </summary>
		/// <param name="data">The data.</param>
		public void WriteByte(byte data)
		{
			codeStream.WriteByte(data);
		}

		/// <summary>
		/// Writes the byte.
		/// </summary>
		/// <param name="buffer">The buffer.</param>
		/// <param name="offset">The offset.</param>
		/// <param name="count">The count.</param>
		public void Write(byte[] buffer, int offset, int count)
		{
			codeStream.Write(buffer, offset, count);
		}

		#endregion

	}
}
