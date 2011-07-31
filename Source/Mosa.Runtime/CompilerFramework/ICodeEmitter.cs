/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Compiler.Linker;
using Mosa.Runtime.TypeSystem;
using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// Interface of a code emitter.
	/// </summary>
	public interface ICodeEmitter : IDisposable
	{
		/// <summary>
		/// Initializes the specified emittter.
		/// </summary>
		/// <param name="compiler">The compiler.</param>
		/// <param name="codeStream">The code stream.</param>
		/// <param name="linker">The linker.</param>
		void Initialize(IMethodCompiler compiler, Stream codeStream, IAssemblyLinker linker);

		/// <summary>
		/// Emits a label into the code stream.
		/// </summary>
		/// <param name="label">The label name to emit.</param>
		void Label(int label);

		/// <summary>
		/// Resolves the patches.
		/// </summary>
		void ResolvePatches();

		/// <summary>
		/// Gets the position.
		/// </summary>
		/// <param name="label">The label.</param>
		/// <returns></returns>
		long GetPosition(int label);
	}
}

