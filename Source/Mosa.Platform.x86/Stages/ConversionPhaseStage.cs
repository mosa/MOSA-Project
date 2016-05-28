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
			//visitationDictionary[X86.Mov] = Mov;

			//visitationDictionary[X86.Movsx] = Movsx;
			//visitationDictionary[X86.Movzx] = Movzx;
			//visitationDictionary[X86.Movss] = Movss;
			//visitationDictionary[X86.Movsd] = Movsd;
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
					context.Result,
					(context.Operand1.IsLabel || context.Operand1.IsSymbol || context.Operand1.IsField)
						? context.Operand1
						: Operand.CreateCPURegister(context.Operand1.Type, context.Operand1.EffectiveOffsetBase),
					Operand.CreateConstant(MethodCompiler.TypeSystem, (int)context.Operand1.Displacement));
			}
		}

		#endregion Visitation Methods
	}
}
