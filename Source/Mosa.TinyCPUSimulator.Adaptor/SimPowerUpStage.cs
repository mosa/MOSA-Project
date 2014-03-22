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
	public sealed class SimPowerUpStage : BaseCompilerStage
	{
		public readonly string StartUpName = "StartUp";

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="SimPowerUpStage"/> class.
		/// </summary>
		public SimPowerUpStage()
		{
		}

		#endregion Construction

		#region ICompilerStage Members

		protected override void Run()
		{
			TypeInitializerSchedulerStage typeInitializerSchedulerStage = Compiler.Pipeline.FindFirst<TypeInitializerSchedulerStage>();

			BasicBlocks basicBlocks = new BasicBlocks();
			InstructionSet instructionSet = new InstructionSet(25);
			Context context = instructionSet.CreateNewBlock(basicBlocks);
			basicBlocks.AddHeaderBlock(context.BasicBlock);

			Operand entryPoint = Operand.CreateSymbolFromMethod(TypeSystem, typeInitializerSchedulerStage.TypeInitializerMethod);

			context.AppendInstruction(IRInstruction.Call, null, entryPoint);
			context.MosaMethod = typeInitializerSchedulerStage.TypeInitializerMethod;
			//context.AppendInstruction(IRInstruction.Break);

			MosaMethod method = Compiler.CreateLinkerMethod(StartUpName);
			Compiler.CompileMethod(method, basicBlocks, instructionSet);

			Linker.EntryPoint = Linker.GetSymbol(method.FullName);
		}

		#endregion ICompilerStage Members
	}
}