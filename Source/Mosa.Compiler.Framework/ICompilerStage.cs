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
		/// Executes the pre compile phase
		/// </summary>
		void ExecutePreCompile();

		/// <summary>
		/// Executes the post compile phase
		/// </summary>
		void ExecutePostCompile();
	}
}
