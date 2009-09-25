/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:simon_wollwage@yahoo.co.jp>)
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// Performs IR constant folding of arithmetic instructions to optimize
	/// the code down to fewer calculations.
	/// </summary>
	public sealed class IRConstantFoldingStage : IR2.IRStage, IMethodCompilerStage
	{

		/// <summary>
		/// Folds logical ANDs with 2 constants
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void LogicalAndInstruction(Context ctx)
		{
			if (ctx.Operand2 is ConstantOperand && ctx.Operand3 is ConstantOperand) {
				int result = 0;
				switch (ctx.Result.Type.Type) {
					case Metadata.CilElementType.Char:
						goto case Metadata.CilElementType.U2;
					case Metadata.CilElementType.U1:
						result = ((byte)(ctx.Operand2 as ConstantOperand).Value) & ((byte)(ctx.Operand3 as ConstantOperand).Value);
						break;
					case Metadata.CilElementType.U2:
						result = ((ushort)(ctx.Operand2 as ConstantOperand).Value) & ((ushort)(ctx.Operand3 as ConstantOperand).Value);
						break;
					case Metadata.CilElementType.U4:
						result = (int)(((uint)(ctx.Operand2 as ConstantOperand).Value) & ((uint)(ctx.Operand3 as ConstantOperand).Value));
						break;
					case Mosa.Runtime.Metadata.CilElementType.I1:
						result = ((sbyte)(ctx.Operand2 as ConstantOperand).Value) & ((sbyte)(ctx.Operand3 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I2:
						result = ((short)(ctx.Operand2 as ConstantOperand).Value) & ((short)(ctx.Operand3 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I4:
						result = ((int)(ctx.Operand2 as ConstantOperand).Value) & ((int)(ctx.Operand3 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I:
						goto case Mosa.Runtime.Metadata.CilElementType.I4;
					case Mosa.Runtime.Metadata.CilElementType.U:
						goto case Mosa.Runtime.Metadata.CilElementType.U4;
					default:
						goto case Mosa.Runtime.Metadata.CilElementType.I4;
				}
				ctx.SetInstruction(IR2.Instruction.MoveInstruction, ctx.Result, new ConstantOperand(ctx.Result.Type, result));
			}
		}


		/// <summary>
		/// Folds logical ORs with 2 constants
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void LogicalOrInstruction(Context ctx)
		{
			if (ctx.Operand2 is ConstantOperand && ctx.Operand3 is ConstantOperand) {
				int result = 0;
				switch (ctx.Result.Type.Type) {
					case Mosa.Runtime.Metadata.CilElementType.Char:
						goto case Mosa.Runtime.Metadata.CilElementType.U2;
					case Mosa.Runtime.Metadata.CilElementType.U1:
						result = ((byte)(ctx.Operand2 as ConstantOperand).Value) | ((byte)(ctx.Operand3 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.U2:
						result = ((ushort)(ctx.Operand2 as ConstantOperand).Value) | ((ushort)(ctx.Operand3 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.U4:
						result = (int)(((uint)(ctx.Operand2 as ConstantOperand).Value) | ((uint)(ctx.Operand3 as ConstantOperand).Value));
						break;
					case Mosa.Runtime.Metadata.CilElementType.I1:
						result = (sbyte)(((uint)(sbyte)(ctx.Operand2 as ConstantOperand).Value) | ((uint)(sbyte)(ctx.Operand3 as ConstantOperand).Value));
						break;
					case Mosa.Runtime.Metadata.CilElementType.I2:
						result = ((short)(ctx.Operand2 as ConstantOperand).Value) | ((short)(ctx.Operand3 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I4:
						result = ((int)(ctx.Operand2 as ConstantOperand).Value) | ((int)(ctx.Operand3 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I:
						goto case Mosa.Runtime.Metadata.CilElementType.I4;
					case Mosa.Runtime.Metadata.CilElementType.U:
						goto case Mosa.Runtime.Metadata.CilElementType.U4;
					default:
						goto case Mosa.Runtime.Metadata.CilElementType.I4;
				}
				ctx.SetInstruction(IR2.Instruction.MoveInstruction, ctx.Result, new ConstantOperand(ctx.Result.Type, result));
			}
		}

		/// <summary>
		/// Folds logical XORs with 2 constants
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void LogicalXorInstruction(Context ctx)
		{
			if (ctx.Operand2 is ConstantOperand && ctx.Operand3 is ConstantOperand) {
				int result = 0;
				switch (ctx.Result.Type.Type) {
					case Mosa.Runtime.Metadata.CilElementType.Char:
						goto case Mosa.Runtime.Metadata.CilElementType.U2;
					case Mosa.Runtime.Metadata.CilElementType.U1:
						result = ((byte)(ctx.Operand2 as ConstantOperand).Value) ^ ((byte)(ctx.Operand3 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.U2:
						result = ((ushort)(ctx.Operand2 as ConstantOperand).Value) ^ ((ushort)(ctx.Operand3 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.U4:
						result = (int)(((uint)(ctx.Operand2 as ConstantOperand).Value) ^ ((uint)(ctx.Operand3 as ConstantOperand).Value));
						break;
					case Mosa.Runtime.Metadata.CilElementType.I1:
						result = ((sbyte)(ctx.Operand2 as ConstantOperand).Value) ^ ((sbyte)(ctx.Operand3 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I2:
						result = ((short)(ctx.Operand2 as ConstantOperand).Value) ^ ((short)(ctx.Operand3 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I4:
						result = ((int)(ctx.Operand2 as ConstantOperand).Value) ^ ((int)(ctx.Operand3 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I:
						goto case Mosa.Runtime.Metadata.CilElementType.I4;
					case Mosa.Runtime.Metadata.CilElementType.U:
						goto case Mosa.Runtime.Metadata.CilElementType.U4;
					default:
						goto case Mosa.Runtime.Metadata.CilElementType.I4;
				}
				ctx.SetInstruction(IR2.Instruction.MoveInstruction, ctx.Result, new ConstantOperand(ctx.Result.Type, result));
			}
		}

		#region IMethodCompilerStage

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		public override string Name
		{
			get { return @"Constant Folding"; }
		}

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		/// <param name="compiler">The compiler context to perform processing in.</param>
		void IMethodCompilerStage.Run(IMethodCompiler compiler)
		{
			if (compiler == null)
				throw new ArgumentNullException(@"compiler");

			IBasicBlockProvider blockProvider = (IBasicBlockProvider)compiler.GetPreviousStage(typeof(IBasicBlockProvider));

			if (blockProvider == null)
				throw new InvalidOperationException(@"Instruction stream must be split to basic Blocks.");

			// Retrieve the instruction provider and the instruction set
			InstructionSet instructionset = (compiler.GetPreviousStage(typeof(IInstructionsProvider)) as IInstructionsProvider).InstructionSet;

			foreach (BasicBlock block in blockProvider.Blocks) {
				Context ctx = new Context(instructionset, block);

				while (!ctx.EndOfInstruction) {
					ctx.Instruction.Visit(this, ctx);
					ctx.Forward();
				}
			}
		}

		/// <summary>
		/// Adds this stage to the given pipeline.
		/// </summary>
		/// <param name="pipeline">The pipeline to add this stage to.</param>
		public override void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline)
		{
			pipeline.InsertBefore<CIL.CilToIrTransformationStage>(this);
		}

		#endregion

	}
}
