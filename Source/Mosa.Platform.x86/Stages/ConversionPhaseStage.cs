// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	/// This stages converts generic x86 instructions to specific load/store-type instructions.
	/// This is a temporary stage until all the previous stages use the load/store-type instructions
	/// </summary>
	public sealed class ConversionPhaseStage : BaseTransformationStage
	{
		protected override void PopulateVisitationDictionary()
		{
			visitationDictionary[X86.Mov] = Mov;
			visitationDictionary[X86.Movsx] = Movsx;
			visitationDictionary[X86.Movzx] = Movzx;
			visitationDictionary[X86.Movsd] = Movsd;
			visitationDictionary[X86.Movss] = Movss;
			visitationDictionary[X86.Movups] = Movups;
			visitationDictionary[X86.Movaps] = Movaps;
		}

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
		/// Visitation function for <see cref="IX86Visitor.Movsx"/> instructions.
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

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Movsd"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public void Movsd(Context context)
		{
			if (context.Result.IsCPURegister && context.Operand1.IsMemoryAddress)
			{
				context.SetInstruction(X86.MovsdLoad,
					context.Result,
					(context.Operand1.IsLabel || context.Operand1.IsSymbol || context.Operand1.IsField)
						? context.Operand1
						: Operand.CreateCPURegister(context.Operand1.Type, context.Operand1.EffectiveOffsetBase),
					Operand.CreateConstant(MethodCompiler.TypeSystem, (int)context.Operand1.Displacement)
				);
			}

			if (context.Result.IsMemoryAddress && (context.Operand1.IsCPURegister /*|| context.Operand1.IsConstant*/))
			{
				context.SetInstruction(X86.MovsdStore,
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
		/// Visitation function for <see cref="IX86Visitor.Movss" /> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public void Movss(Context context)
		{
			if (context.Result.IsCPURegister && context.Operand1.IsMemoryAddress)
			{
				context.SetInstruction(X86.MovssLoad,
					context.Result,
					(context.Operand1.IsLabel || context.Operand1.IsSymbol || context.Operand1.IsField)
						? context.Operand1
						: Operand.CreateCPURegister(context.Operand1.Type, context.Operand1.EffectiveOffsetBase),
					Operand.CreateConstant(MethodCompiler.TypeSystem, (int)context.Operand1.Displacement)
				);
			}

			if (context.Result.IsMemoryAddress && (context.Operand1.IsCPURegister /*|| context.Operand1.IsConstant*/))
			{
				context.SetInstruction(X86.MovssStore,
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
		/// Visitation function for <see cref="IX86Visitor.Movups" /> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public void Movaps(Context context)
		{
			if (context.Result.IsCPURegister && context.Operand1.IsMemoryAddress)
			{
				context.SetInstruction(X86.MovapsLoad,
					context.Result,
					(context.Operand1.IsLabel || context.Operand1.IsSymbol || context.Operand1.IsField)
						? context.Operand1
						: Operand.CreateCPURegister(context.Operand1.Type, context.Operand1.EffectiveOffsetBase),
					Operand.CreateConstant(MethodCompiler.TypeSystem, (int)context.Operand1.Displacement)
				);
			}
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Movups" /> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public void Movups(Context context)
		{
			if (context.Result.IsCPURegister && context.Operand1.IsMemoryAddress)
			{
				context.SetInstruction(X86.MovupsLoad,
					context.Result,
					(context.Operand1.IsLabel || context.Operand1.IsSymbol || context.Operand1.IsField)
						? context.Operand1
						: Operand.CreateCPURegister(context.Operand1.Type, context.Operand1.EffectiveOffsetBase),
					Operand.CreateConstant(MethodCompiler.TypeSystem, (int)context.Operand1.Displacement)
				);
			}

			if (context.Result.IsMemoryAddress && (context.Operand1.IsCPURegister /*|| context.Operand1.IsConstant*/))
			{
				context.SetInstruction(X86.MovupsStore,
					null,
					(context.Result.IsLabel || context.Result.IsSymbol || context.Result.IsField)
						? context.Result
						: Operand.CreateCPURegister(context.Result.Type, context.Result.EffectiveOffsetBase),
					Operand.CreateConstant(MethodCompiler.TypeSystem, (int)context.Result.Displacement),
					context.Operand1
				);
			}
		}

		#endregion Visitation Methods
	}
}
