/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Framework.Stages;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.TinyCPUSimulator.Adaptor
{
	public sealed class SimPowerUpStage : BaseCompilerStage, ICompilerStage, IPipelineStage
	{
		public readonly string StartUpName = "StartUp"; // FullName = Mosa.Tools.Compiler.LinkerGenerated.<$>PowerUp

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="SimPowerUpStage"/> class.
		/// </summary>
		public SimPowerUpStage()
		{
		}

		#endregion Construction

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
			TypeInitializerSchedulerStage typeInitializerSchedulerStage = compiler.Pipeline.FindFirst<TypeInitializerSchedulerStage>();

			BasicBlocks basicBlocks = new BasicBlocks();
			InstructionSet instructionSet = new InstructionSet(25);
			Context context = instructionSet.CreateNewBlock(basicBlocks);
			basicBlocks.AddHeaderBlock(context.BasicBlock);

			Operand entryPoint = Operand.CreateSymbolFromMethod(typeSystem, typeInitializerSchedulerStage.TypeInitializerMethod);

			context.AppendInstruction(IRInstruction.Call, null, entryPoint);
			context.MosaMethod = typeInitializerSchedulerStage.TypeInitializerMethod;
			//context.AppendInstruction(IRInstruction.Break);

			MosaMethod method = compiler.CreateLinkerMethod(StartUpName);
			compiler.CompileMethod(method, basicBlocks, instructionSet);

			linker.EntryPoint = linker.GetSymbol(method.FullName);
		}

		#endregion ICompilerStage Members
	}
}