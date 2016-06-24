// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	///
	/// </summary>
	public sealed class ConversionPhaseStage : BaseTransformationStage
	{
		protected override void PopulateVisitationDictionary()
		{
			visitationDictionary[X86.Mov] = Mov;
			visitationDictionary[X86.Movsx] = Movsx;
			visitationDictionary[X86.Movzx] = Movzx;

			//visitationDictionary[X86.Movss] = Movss;
			//visitationDictionary[X86.Movsd] = Movsd;
		}

		//protected override void Run()
		//{
		//	for (int index = 0; index < BasicBlocks.Count; index++)
		//	{
		//		for (var node = BasicBlocks[index].First; !node.IsBlockEndInstruction; node = node.Next)
		//		{
		//			if (node.IsEmpty)
		//				continue;

		//			instructionCount++;

		//			var ctx = new Context(node);

		//			if (ctx.ResultCount == 1 && ctx.OperandCount >= 1)
		//			{
		//				// Load
		//				if (ctx.Result.IsCPURegister && ctx.Operand1.IsMemoryAddress)
		//				{
		//					System.Diagnostics.Debug.WriteLine("Load\t" + ctx.Instruction.ToString() + "\t" + MethodCompiler.Method.ToString());
		//				}
		//			}
		//			if (ctx.ResultCount == 1)
		//			{
		//				// Store
		//				if (ctx.Result.IsMemoryAddress)
		//				{
		//					System.Diagnostics.Debug.WriteLine("Store\t" + ctx.Instruction.ToString() + "\t" + MethodCompiler.Method.ToString());
		//				}
		//			}

		//			VisitationDelegate visitationMethod;
		//			if (!visitationDictionary.TryGetValue(ctx.Instruction, out visitationMethod))
		//				continue;

		//			visitationMethod(ctx);
		//		}
		//	}
		//}

		#region Visitation Methods

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Mov"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public void Mov(Context context)
		{
			if (context.Result.IsCPURegister && context.Operand1.IsMemoryAddress)
			{
				context.SetInstruction(X86.MovLoad,

					//context.Size,
					context.Result,
					(context.Operand1.IsLabel || context.Operand1.IsSymbol || context.Operand1.IsField)
						? context.Operand1
						: Operand.CreateCPURegister(context.Operand1.Type, context.Operand1.EffectiveOffsetBase),
					Operand.CreateConstant(MethodCompiler.TypeSystem, (int)context.Operand1.Displacement)
				);
			}

			if (context.Result.IsMemoryAddress && (context.Operand1.IsCPURegister /*|| context.Operand1.IsConstant*/))
			{
				context.SetInstruction(X86.MovStore,
					null,
					(context.Result.IsLabel || context.Result.IsSymbol || context.Result.IsField)
						? context.Result
						: Operand.CreateCPURegister(context.Result.Type, context.Result.EffectiveOffsetBase),
					Operand.CreateConstant(MethodCompiler.TypeSystem, (int)context.Result.Displacement),
					context.Operand1
				);
			}
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Movzx"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public void Movzx(Context context)
		{
			if (context.Result.IsCPURegister && context.Operand1.IsMemoryAddress)
			{
				context.SetInstruction(X86.MovzxLoad,
					context.Size,
					context.Result,
					(context.Operand1.IsLabel || context.Operand1.IsSymbol || context.Operand1.IsField)
						? context.Operand1
						: Operand.CreateCPURegister(context.Operand1.Type, context.Operand1.EffectiveOffsetBase),
					Operand.CreateConstant(MethodCompiler.TypeSystem, (int)context.Operand1.Displacement)
				);
			}
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Movzx"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public void Movsx(Context context)
		{
			if (context.Result.IsCPURegister && context.Operand1.IsMemoryAddress)
			{
				context.SetInstruction(X86.MovsxLoad,
					context.Size,
					context.Result,
					(context.Operand1.IsLabel || context.Operand1.IsSymbol || context.Operand1.IsField)
						? context.Operand1
						: Operand.CreateCPURegister(context.Operand1.Type, context.Operand1.EffectiveOffsetBase),
					Operand.CreateConstant(MethodCompiler.TypeSystem, (int)context.Operand1.Displacement)
				);
			}
		}

		#endregion Visitation Methods
	}
}
