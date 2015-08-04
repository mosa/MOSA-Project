// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// This interface represents a stage of compilation.
	/// </summary>
	public interface ICompilerStage : IPipelineStage
	{
		/// <summary>
		/// Sets up the compiler stage.
		/// </summary>
		/// <param name="compiler">A <see cref="BaseCompiler" /> using the stage.</param>
		void Initialize(BaseCompiler compiler);

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void Execute();
	}
}
