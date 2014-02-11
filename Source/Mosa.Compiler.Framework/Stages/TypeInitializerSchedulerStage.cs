/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Schedules type initializers and creates a hidden mosacl_main method,
	/// which runs all type initializers in sequence.
	/// </summary>
	/// <remarks>
	/// Dependencies are not resolved, it is hoped that dependencies are resolved
	/// by the high-level language compiler by placing cctors in some order in
	/// metadata.
	/// </remarks>
	public sealed class TypeInitializerSchedulerStage : BaseCompilerStage, ICompilerStage, IPipelineStage
	{
		public readonly string TypeInitializerName = "AssemblyInit"; 

		#region Data Members

		/// <summary>
		/// The instruction set
		/// </summary>
		private InstructionSet instructionSet;

		/// <summary>
		/// Hold the current context
		/// </summary>
		private Context context;

		/// <summary>
		/// The basic blocks
		/// </summary>
		private BasicBlocks basicBlocks;

		#endregion Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="TypeInitializerSchedulerStage"/> class.
		/// </summary>
		public TypeInitializerSchedulerStage()
		{
			basicBlocks = new BasicBlocks();
			instructionSet = new InstructionSet(25);
			context = instructionSet.CreateNewBlock(basicBlocks);
			basicBlocks.AddHeaderBlock(context.BasicBlock);

			context.AppendInstruction(IRInstruction.Prologue);
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the intializer method.
		/// </summary>
		/// <value>The method.</value>
		public MosaMethod TypeInitializerMethod { get; private set; }

		#endregion Properties

		#region ICompilerStage Members

		void ICompilerStage.Setup(BaseCompiler compiler)
		{
			base.Setup(compiler);
		}

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void ICompilerStage.Run()
		{
			if (typeSystem.EntryMethod != null)
			{
				Schedule(typeSystem.EntryMethod);
			}

			context.AppendInstruction(IRInstruction.Epilogue);

			TypeInitializerMethod = compiler.CreateLinkerMethod(TypeInitializerName);

			compiler.CompileMethod(TypeInitializerMethod, basicBlocks, instructionSet);
		}

		#endregion ICompilerStage Members

		#region Methods

		/// <summary>
		/// Schedules the specified method for invocation in the main.
		/// </summary>
		/// <param name="method">The method.</param>
		public void Schedule(MosaMethod method)
		{
			Operand symbolOperand = Operand.CreateSymbolFromMethod(typeSystem,method);
			context.AppendInstruction(IRInstruction.Call, null, symbolOperand);
			context.InvokeMethod = method;
		}

		#endregion Methods
	}
}