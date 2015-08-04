// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Framework.Stages;
using Mosa.Compiler.Linker;

namespace Mosa.TinyCPUSimulator.Adaptor
{
	public sealed class SimPowerUpStage : BaseCompilerStage
	{
		public readonly static string StartUpName = "StartUp";

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
			var typeInitializer = Compiler.PostCompilePipeline.FindFirst<TypeInitializerSchedulerStage>().TypeInitializerMethod;

			var basicBlocks = new BasicBlocks();
			var block = basicBlocks.CreateBlock();
			basicBlocks.AddHeaderBlock(block);
			var context = new Context(block);

			var entryPoint = Operand.CreateSymbolFromMethod(TypeSystem, typeInitializer);

			context.AppendInstruction(IRInstruction.Call, null, entryPoint);
			context.InvokeMethod = typeInitializer;

			var method = Compiler.CreateLinkerMethod(StartUpName);
			Compiler.CompileMethod(method, basicBlocks, 0);

			Linker.EntryPoint = Linker.GetSymbol(method.FullName, SectionKind.Text);
		}

		#endregion ICompilerStage Members
	}
}
