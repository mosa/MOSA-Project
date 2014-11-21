/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.InternalTrace;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework
{
	public interface ICompiler
	{
		BaseCompiler Compiler { get; set; }

		CompilerOptions CompilerOptions { get; set; }

		TypeSystem TypeSystem { get; set; }

		CompilerTrace CompilerTrace { get; set; }

		void Begin();

		void Compile();

		void CompilerType(MosaType type);

		void CompilerMethod(MosaMethod method);

		void ResolveSymbols();

		void FinalizeOutput();
	}
}