/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

namespace Mosa.Compiler.Framework
{

	/// <summary>
	/// Represents a compilation stage during compilation of the JIT or AOT compilers.
	/// </summary>
	/// <remarks>
	/// Compilation stages are typically realized as individual components acting on fragments
	/// of a .NET IL binary. The JIT compiler uses a defined subset of the AOT compilation stages
	/// in order to provide fast code generation. Both compilers share a single compilation model 
	/// consisting of a set of individual stages forming a compilation pipeline. The pipeline 
	/// model allows for flexible configuration of the capabilities of either compiler. Individual
	/// stages may represent IL decoding, machine code generation, register allocation, emitting
	/// machine code and finally storing it in a binary. This provides for a highly modular and
	/// extensible compilation model.
	/// <para />
	/// In addition to the IMethodCompilerStage interface other compilation stage specific interfaces
	/// must be implemented.
	/// </remarks>
	public interface IMethodCompilerStage : IPipelineStage
	{
		/// <summary>
		/// Setup stage specific processing on the compiler context.
		/// </summary>
		/// <param name="methodCompiler">The compiler context to perform processing in.</param>
		void Setup(IMethodCompiler methodCompiler);

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void Run();

	}
}
