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

using Mosa.Runtime.CompilerFramework.CIL;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// Performs IR constant folding of arithmetic instructions to optimize
	/// the code down to fewer calculations.
	/// </summary>
	public sealed class ILConstantFoldingStage : CILStage, IMethodCompilerStage
	{

		#region IMethodCompilerStage

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		public override string Name
		{
			get { return @"IL Constant Folding"; }
		}

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		/// <param name="compiler">The compiler context to perform processing in.</param>
		void IMethodCompilerStage.Run(IMethodCompiler compiler)
		{
			if (null == compiler)
				throw new ArgumentNullException(@"compiler");
			IBasicBlockProvider blockProvider = (IBasicBlockProvider)compiler.GetPreviousStage(typeof(IBasicBlockProvider));
			if (null == blockProvider)
				throw new InvalidOperationException(@"Instruction stream must have been split to basic Blocks.");

			foreach (BasicBlock block in blockProvider.Blocks) {
				Context ctx = new Context(block);
				for (ctx.Index = 0; ctx.Index < block.Instructions.Count; ctx.Index++) {
					block.Instructions[ctx.Index].Visit(this, ctx);
				}
			}
		}

		/// <summary>
		/// Adds this stage to the given pipeline.
		/// </summary>
		/// <param name="pipeline">The pipeline to add this stage to.</param>
		public override void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline)
		{
			pipeline.InsertBefore<IL.CilToIrTransformationStage>(this);
		}
		#endregion

		#region Methods

		/// <summary>
		/// Folds multiplication with 2 constants
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void Mul(Context ctx)
		{
			if (ctx.Operand1 is ConstantOperand && ctx.Operand2 is ConstantOperand) {
				int result = 0;
				float fresult = 0.0f; ;
				double dresult = 0.0;
				switch (ctx.Result.Type.Type) {
					case Mosa.Runtime.Metadata.CilElementType.Char:
						goto case Mosa.Runtime.Metadata.CilElementType.U2;
					case Mosa.Runtime.Metadata.CilElementType.U1:
						result = ((byte)(ctx.Operand1 as ConstantOperand).Value) * ((byte)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.U2:
						result = ((ushort)(ctx.Operand1 as ConstantOperand).Value) * ((ushort)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.U4:
						result = (int)(((uint)(ctx.Operand1 as ConstantOperand).Value) * ((uint)(ctx.Operand2 as ConstantOperand).Value));
						break;
					case Mosa.Runtime.Metadata.CilElementType.I1:
						result = ((sbyte)(ctx.Operand1 as ConstantOperand).Value) * ((sbyte)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I2:
						result = ((short)(ctx.Operand1 as ConstantOperand).Value) * ((short)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I4:
						result = ((int)(ctx.Operand1 as ConstantOperand).Value) * ((int)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.R4:
						fresult = ((float)(ctx.Operand1 as ConstantOperand).Value) * ((float)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.R8:
						dresult = ((double)(ctx.Operand1 as ConstantOperand).Value) * ((double)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I:
						goto case Mosa.Runtime.Metadata.CilElementType.I4;
					case Mosa.Runtime.Metadata.CilElementType.U:
						goto case Mosa.Runtime.Metadata.CilElementType.U4;
					default:
						goto case Mosa.Runtime.Metadata.CilElementType.I4;
				}
				if (ctx.Result.Type.Type == Mosa.Runtime.Metadata.CilElementType.R4)
					Replace(ctx, new IR.MoveInstruction(ctx.Result, new ConstantOperand(ctx.Result.Type, fresult)));
				else if (ctx.Result.Type.Type == Mosa.Runtime.Metadata.CilElementType.R8)
					Replace(ctx, new IR.MoveInstruction(ctx.Result, new ConstantOperand(ctx.Result.Type, dresult)));
				else
					Replace(ctx, new IR.MoveInstruction(ctx.Result, new ConstantOperand(ctx.Result.Type, result)));
			}
		}

		/// <summary>
		/// Folds divisions with 2 constants
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void Div(Context ctx)
		{
			if (ctx.Operand1 is ConstantOperand && ctx.Operand2 is ConstantOperand) {
				int result = 0;
				float fresult = 0.0f; ;
				double dresult = 0.0;
				switch (ctx.Result.Type.Type) {
					case Mosa.Runtime.Metadata.CilElementType.Char:
						goto case Mosa.Runtime.Metadata.CilElementType.U2;
					case Mosa.Runtime.Metadata.CilElementType.U1:
						result = ((byte)(ctx.Operand1 as ConstantOperand).Value) / ((byte)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.U2:
						result = ((ushort)(ctx.Operand1 as ConstantOperand).Value) / ((ushort)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.U4:
						result = (int)(((uint)(ctx.Operand1 as ConstantOperand).Value) / ((uint)(ctx.Operand2 as ConstantOperand).Value));
						break;
					case Mosa.Runtime.Metadata.CilElementType.I1:
						result = ((sbyte)(ctx.Operand1 as ConstantOperand).Value) / ((sbyte)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I2:
						result = ((short)(ctx.Operand1 as ConstantOperand).Value) / ((short)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I4:
						result = ((int)(ctx.Operand1 as ConstantOperand).Value) / ((int)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.R4:
						fresult = ((float)(ctx.Operand1 as ConstantOperand).Value) / ((float)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.R8:
						dresult = ((double)(ctx.Operand1 as ConstantOperand).Value) / ((double)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I:
						goto case Mosa.Runtime.Metadata.CilElementType.I4;
					case Mosa.Runtime.Metadata.CilElementType.U:
						goto case Mosa.Runtime.Metadata.CilElementType.U4;
					default:
						goto case Mosa.Runtime.Metadata.CilElementType.I4;
				}
				if (ctx.Result.Type.Type == Mosa.Runtime.Metadata.CilElementType.R4)
					Replace(ctx, new IR.MoveInstruction(ctx.Result, new ConstantOperand(ctx.Result.Type, fresult)));
				else if (ctx.Result.Type.Type == Mosa.Runtime.Metadata.CilElementType.R8)
					Replace(ctx, new IR.MoveInstruction(ctx.Result, new ConstantOperand(ctx.Result.Type, dresult)));
				else
					Replace(ctx, new IR.MoveInstruction(ctx.Result, new ConstantOperand(ctx.Result.Type, result)));
			}
		}

		/// <summary>
		/// Folds the remainder of 2 constants
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void Rem(Context ctx)
		{
			if (ctx.Operand1 is ConstantOperand && ctx.Operand2 is ConstantOperand) {
				int result = 0;
				float fresult = 0.0f; ;
				double dresult = 0.0;
				switch (ctx.Result.Type.Type) {
					case Mosa.Runtime.Metadata.CilElementType.Char:
						goto case Mosa.Runtime.Metadata.CilElementType.U2;
					case Mosa.Runtime.Metadata.CilElementType.U1:
						result = ((byte)(ctx.Operand1 as ConstantOperand).Value) % ((byte)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.U2:
						result = ((ushort)(ctx.Operand1 as ConstantOperand).Value) % ((ushort)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.U4:
						result = (int)(((uint)(ctx.Operand1 as ConstantOperand).Value) % ((uint)(ctx.Operand2 as ConstantOperand).Value));
						break;
					case Mosa.Runtime.Metadata.CilElementType.I1:
						result = ((sbyte)(ctx.Operand1 as ConstantOperand).Value) % ((sbyte)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I2:
						result = ((short)(ctx.Operand1 as ConstantOperand).Value) % ((short)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I4:
						result = ((int)(ctx.Operand1 as ConstantOperand).Value) % ((int)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.R4:
						fresult = ((float)(ctx.Operand1 as ConstantOperand).Value) % ((float)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.R8:
						dresult = ((double)(ctx.Operand1 as ConstantOperand).Value) % ((double)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I:
						goto case Mosa.Runtime.Metadata.CilElementType.I4;
					case Mosa.Runtime.Metadata.CilElementType.U:
						goto case Mosa.Runtime.Metadata.CilElementType.U4;
					default:
						goto case Mosa.Runtime.Metadata.CilElementType.I4;
				}
				if (ctx.Result.Type.Type == Mosa.Runtime.Metadata.CilElementType.R4)
					Replace(ctx, new IR.MoveInstruction(ctx.Result, new ConstantOperand(ctx.Result.Type, fresult)));
				else if (ctx.Result.Type.Type == Mosa.Runtime.Metadata.CilElementType.R8)
					Replace(ctx, new IR.MoveInstruction(ctx.Result, new ConstantOperand(ctx.Result.Type, dresult)));
				else
					Replace(ctx, new IR.MoveInstruction(ctx.Result, new ConstantOperand(ctx.Result.Type, result)));
			}
		}

		/// <summary>
		/// Folds additions with 2 constants
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void Add(Context ctx)
		{
			if (ctx.Operand1 is ConstantOperand && ctx.Operand2 is ConstantOperand) {
				int result = 0;
				float fresult = 0.0f; ;
				double dresult = 0.0;
				switch (ctx.Result.Type.Type) {
					case Mosa.Runtime.Metadata.CilElementType.Char:
						goto case Mosa.Runtime.Metadata.CilElementType.U2;
					case Mosa.Runtime.Metadata.CilElementType.U1:
						result = ((byte)(ctx.Operand1 as ConstantOperand).Value) + ((byte)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.U2:
						result = ((ushort)(ctx.Operand1 as ConstantOperand).Value) + ((ushort)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.U4:
						result = (int)(((uint)(ctx.Operand1 as ConstantOperand).Value) + ((uint)(ctx.Operand2 as ConstantOperand).Value));
						break;
					case Mosa.Runtime.Metadata.CilElementType.I1:
						result = ((sbyte)(ctx.Operand1 as ConstantOperand).Value) + ((sbyte)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I2:
						result = ((short)(ctx.Operand1 as ConstantOperand).Value) + ((short)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I4:
						result = ((int)(ctx.Operand1 as ConstantOperand).Value) + ((int)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.R4:
						fresult = ((float)(ctx.Operand1 as ConstantOperand).Value) + ((float)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.R8:
						dresult = ((double)(ctx.Operand1 as ConstantOperand).Value) + ((double)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I:
						goto case Mosa.Runtime.Metadata.CilElementType.I4;
					case Mosa.Runtime.Metadata.CilElementType.U:
						goto case Mosa.Runtime.Metadata.CilElementType.U4;
					default:
						goto case Mosa.Runtime.Metadata.CilElementType.I4;
				}
				if (ctx.Result.Type.Type == Mosa.Runtime.Metadata.CilElementType.R4)
					Replace(ctx, new IR.MoveInstruction(ctx.Result, new ConstantOperand(ctx.Result.Type, fresult)));
				else if (ctx.Result.Type.Type == Mosa.Runtime.Metadata.CilElementType.R8)
					Replace(ctx, new IR.MoveInstruction(ctx.Result, new ConstantOperand(ctx.Result.Type, dresult)));
				else
					Replace(ctx, new IR.MoveInstruction(ctx.Result, new ConstantOperand(ctx.Result.Type, result)));
			}
		}

		/// <summary>
		/// Folds substractions with 2 constants
		/// </summary>
		/// <param name="ctx">The context.</param>
		public override void Sub(Context ctx)
		{
			if (ctx.Operand1 is ConstantOperand && ctx.Operand2 is ConstantOperand) {
				int result = 0;
				float fresult = 0.0f; ;
				double dresult = 0.0;
				switch (ctx.Result.Type.Type) {
					case Mosa.Runtime.Metadata.CilElementType.Char:
						goto case Mosa.Runtime.Metadata.CilElementType.U2;
					case Mosa.Runtime.Metadata.CilElementType.U1:
						result = ((byte)(ctx.Operand1 as ConstantOperand).Value) - ((byte)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.U2:
						result = ((ushort)(ctx.Operand1 as ConstantOperand).Value) - ((ushort)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.U4:
						result = (int)(((uint)(ctx.Operand1 as ConstantOperand).Value) - ((uint)(ctx.Operand2 as ConstantOperand).Value));
						break;
					case Mosa.Runtime.Metadata.CilElementType.I1:
						result = ((sbyte)(ctx.Operand1 as ConstantOperand).Value) - ((sbyte)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I2:
						result = ((short)(ctx.Operand1 as ConstantOperand).Value) - ((short)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I4:
						result = ((int)(ctx.Operand1 as ConstantOperand).Value) - ((int)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.R4:
						fresult = ((float)(ctx.Operand1 as ConstantOperand).Value) - ((float)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.R8:
						dresult = ((double)(ctx.Operand1 as ConstantOperand).Value) - ((double)(ctx.Operand2 as ConstantOperand).Value);
						break;
					case Mosa.Runtime.Metadata.CilElementType.I:
						goto case Mosa.Runtime.Metadata.CilElementType.I4;
					case Mosa.Runtime.Metadata.CilElementType.U:
						goto case Mosa.Runtime.Metadata.CilElementType.U4;
					default:
						goto case Mosa.Runtime.Metadata.CilElementType.I4;
				}
				if (ctx.Result.Type.Type == Mosa.Runtime.Metadata.CilElementType.R4)
					Replace(ctx, new IR.MoveInstruction(ctx.Result, new ConstantOperand(ctx.Result.Type, fresult)));
				else if (ctx.Result.Type.Type == Mosa.Runtime.Metadata.CilElementType.R8)
					Replace(ctx, new IR.MoveInstruction(ctx.Result, new ConstantOperand(ctx.Result.Type, dresult)));
				else
					Replace(ctx, new IR.MoveInstruction(ctx.Result, new ConstantOperand(ctx.Result.Type, result)));
			}
		}

		#endregion // Methods
	}
}
