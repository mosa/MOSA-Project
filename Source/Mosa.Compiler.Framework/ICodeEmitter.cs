/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.IO;

using Mosa.Compiler.Linker;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Interface of a code emitter.
	/// </summary>
	public interface ICodeEmitter : IDisposable
	{
		/// <summary>
		/// Initializes the specified emitter.
		/// </summary>
		/// <param name="compiler">The compiler.</param>
		/// <param name="codeStream">The code stream.</param>
		void Initialize(BaseMethodCompiler compiler, Stream codeStream);

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

		/// <summary>
		/// Gets the current position.
		/// </summary>
		/// <value>The current position.</value>
		long CurrentPosition { get; }
	}
}

