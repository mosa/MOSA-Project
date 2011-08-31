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

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.CompilerFramework.Operands;
using Mosa.Compiler.Common;
using Mosa.Compiler.Linker;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.TypeSystem;

using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// Base code emitter.
	/// </summary>
	public class BaseCodeEmitter : IDisposable
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
		protected IMethodCompiler compiler;

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
		protected IAssemblyLinker linker;

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
		/// <param name="linker">The linker used to resolve external addresses.</param>
		public void Initialize(IMethodCompiler compiler, Stream codeStream, IAssemblyLinker linker)
		{
			Debug.Assert(null != compiler, @"MachineCodeEmitter needs a method compiler.");
			if (compiler == null)
				throw new ArgumentNullException(@"compiler");
			Debug.Assert(null != codeStream, @"MachineCodeEmitter needs a code stream.");
			if (codeStream == null)
				throw new ArgumentNullException(@"codeStream");
			Debug.Assert(null != linker, @"MachineCodeEmitter needs a linker.");
			if (linker == null)
				throw new ArgumentNullException(@"linker");

			this.compiler = compiler;
			this.codeStream = codeStream;
			this.codeStreamBasePosition = codeStream.Position;
			this.linker = linker;
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
		public long GetPosition(int label)
		{
			return labels[label];
		}

		/// <summary>
		/// Gets the current position.
		/// </summary>
		/// <value>The current position.</value>
		public long CurrentPosition
		{
			get { return codeStream.Position; }
		}

		#endregion // ICodeEmitter Members

	}
}
