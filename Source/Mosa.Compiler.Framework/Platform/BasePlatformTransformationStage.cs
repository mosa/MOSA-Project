/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Scott Balmos <sbalmos@fastmail.fm>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Linker;

namespace Mosa.Compiler.Framework.Platform
{
	/// <summary>
	///
	/// </summary>
	public abstract class BasePlatformTransformationStage : BaseCodeTransformationStage
	{
		protected virtual string Platform { get { return "Generic"; } }

		#region IPipelineStage Members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		public override string Name { get { return Platform + "." + base.Name; } }

		#endregion IPipelineStage Members

		#region Emit Constant Methods

		/// <summary>
		/// Emits the constant operands.
		/// </summary>
		/// <param name="context">The context.</param>
		protected void EmitFloatingPointConstants(Context context)
		{
			if (context.OperandCount > 0)
				context.Operand1 = EmitFloatingPointConstant(context.Operand1);
			if (context.OperandCount > 1)
				context.Operand2 = EmitFloatingPointConstant(context.Operand2);
			if (context.OperandCount > 2)
				context.Operand3 = EmitFloatingPointConstant(context.Operand3);
		}

		/// <summary>
		/// This function emits a constant variable into the read-only data section.
		/// </summary>
		/// <param name="operand">The operand.</param>
		/// <returns>
		/// An operand, which represents the reference to the read-only constant.
		/// </returns>
		protected Operand EmitFloatingPointConstant(Operand operand)
		{
			if (!operand.IsConstant || !operand.IsR)
				return operand;

			LinkerSymbol symbol;

			if (operand.IsR4)
			{
				symbol = MethodCompiler.Linker.GetConstantSymbol(operand.ConstantSingleFloatingPoint);
			}
			else
			{
				symbol = MethodCompiler.Linker.GetConstantSymbol(operand.ConstantDoubleFloatingPoint);
			}

			return Operand.CreateLabel(operand.Type, symbol.Name);
		}

		#endregion Emit Constant Methods
	}
}