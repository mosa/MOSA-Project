/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Simon Wollwage (<mailto:simon_wollwage@yahoo.co.jp>)
 */

using System;
using Mosa.Runtime.CompilerFramework.CIL;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// Performs IR constant folding of arithmetic instructions to optimize
	/// the code down to fewer calculations.
	/// </summary>
	public sealed class StrengthReductionStage : CILStage, IMethodCompilerStage
	{

		#region IMethodCompilerStage

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		public override string Name
		{
			get { return @"Strength Reduction"; }
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

				while (!ctx.EndOfInstructions) {
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
			pipeline.InsertBefore<CilToIrTransformationStage>(this);
		}

		#endregion

		#region Methods

		/// <summary>
		/// Folds multiplication when one of the constants is zero
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void Mul(Context ctx)
		{
			bool multiplyByZero = false;

			if (ctx.Operand1 is ConstantOperand)
				if (IsValueZero(ctx.Result.Type.Type, ctx.Operand1 as ConstantOperand))
					multiplyByZero = true;

			if (ctx.Operand2 is ConstantOperand)
				if (IsValueZero(ctx.Result.Type.Type, ctx.Operand2 as ConstantOperand))
					multiplyByZero = true;

			if (multiplyByZero) {
				if (ctx.Result.Type.Type == Metadata.CilElementType.R4)
					Replace(ctx, new IR.MoveInstruction(ctx.Result, new ConstantOperand(ctx.Result.Type, 0)));
				else if (ctx.Result.Type.Type == Metadata.CilElementType.R8)
					Replace(ctx, new IR.MoveInstruction(ctx.Result, new ConstantOperand(ctx.Result.Type, 0)));
				else
					Replace(ctx, new IR.MoveInstruction(ctx.Result, new ConstantOperand(ctx.Result.Type, 0)));

				return;
			}

			if (ctx.Operand1 is ConstantOperand)
				if (IsValueOne(ctx.Result.Type.Type, ctx.Operand1 as ConstantOperand)) {
					Replace(ctx, new IR.MoveInstruction(ctx.Result, ctx.Operand2));
					return;
				}

			if (ctx.Operand2 is ConstantOperand)
				if (IsValueOne(ctx.Result.Type.Type, ctx.Operand2 as ConstantOperand)) {
					Replace(ctx, new IR.MoveInstruction(ctx.Result, ctx.Operand1));
					return;
				}

		}

		/// <summary>
		/// Determines whether the value is zero.
		/// </summary>
		/// <param name="cilElementType">Type of the cil element.</param>
		/// <param name="constantOperand">The constant operand.</param>
		/// <returns>
		/// 	<c>true</c> if the value is zero; otherwise, <c>false</c>.
		/// </returns>
		private static bool IsValueZero(Metadata.CilElementType cilElementType, ConstantOperand constantOperand)
		{
			switch (cilElementType) {
				case Metadata.CilElementType.Char:
					goto case Metadata.CilElementType.U2;
				case Metadata.CilElementType.U1:
					return (byte)(constantOperand.Value) == 0;
				case Metadata.CilElementType.U2:
					return (ushort)(constantOperand.Value) == 0;
				case Metadata.CilElementType.U4:
					return (int)(constantOperand.Value) == 0;
				case Metadata.CilElementType.I1:
					return (sbyte)(constantOperand.Value) == 0;
				case Metadata.CilElementType.I2:
					return (short)(constantOperand.Value) == 0;
				case Metadata.CilElementType.I4:
					return (int)(constantOperand.Value) == 0;
				case Metadata.CilElementType.R4:
					return (float)(constantOperand.Value) == 0;
				case Metadata.CilElementType.R8:
					return (double)(constantOperand.Value) == 0;
				case Metadata.CilElementType.I:
					goto case Metadata.CilElementType.I4;
				case Metadata.CilElementType.U:
					goto case Metadata.CilElementType.U4;
				default:
					goto case Metadata.CilElementType.I4;
			}
		}

		/// <summary>
		/// Determines whether the value is one.
		/// </summary>
		/// <param name="cilElementType">Type of the cil element.</param>
		/// <param name="constantOperand">The constant operand.</param>
		/// <returns>
		/// 	<c>true</c> if the value is one; otherwise, <c>false</c>.
		/// </returns>
		private static bool IsValueOne(Metadata.CilElementType cilElementType, ConstantOperand constantOperand)
		{
			switch (cilElementType) {
				case Metadata.CilElementType.Char:
					goto case Metadata.CilElementType.U2;
				case Metadata.CilElementType.U1:
					return (byte)(constantOperand.Value) == 1;
				case Metadata.CilElementType.U2:
					return (ushort)(constantOperand.Value) == 1;
				case Metadata.CilElementType.U4:
					return (int)(constantOperand.Value) == 1;
				case Metadata.CilElementType.I1:
					return (sbyte)(constantOperand.Value) == 1;
				case Metadata.CilElementType.I2:
					return (short)(constantOperand.Value) == 1;
				case Metadata.CilElementType.I4:
					return (int)(constantOperand.Value) == 1;
				case Metadata.CilElementType.R4:
					return (float)(constantOperand.Value) == 1;
				case Metadata.CilElementType.R8:
					return (double)(constantOperand.Value) == 1;
				case Metadata.CilElementType.I:
					goto case Metadata.CilElementType.I4;
				case Metadata.CilElementType.U:
					goto case Metadata.CilElementType.U4;
				default:
					goto case Metadata.CilElementType.I4;
			}
		}

		#endregion // Methods
	}
}
