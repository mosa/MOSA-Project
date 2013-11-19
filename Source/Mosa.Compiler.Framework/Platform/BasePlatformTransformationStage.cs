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
using Mosa.Compiler.Metadata;
using System;
using System.IO;

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
			if (!operand.IsConstant || operand.StackType != StackTypeCode.F)
				return operand;

			int size, alignment;
			architecture.GetTypeRequirements(operand.Type, out size, out alignment);

			string name = String.Format("C_{0}", Guid.NewGuid());

			using (Stream stream = methodCompiler.Linker.Allocate(name, SectionKind.ROData, size, alignment))
			{
				using (BinaryWriter writer = new BinaryWriter(stream))
				{
					if (operand.Type.Type == CilElementType.R4)
					{
						writer.Write(operand.ConstantSingleFloatingPoint);
					}
					else if (operand.Type.Type == CilElementType.R8)
					{
						writer.Write(operand.ConstantDoubleFloatingPoint);
					}
				}
			}

			return Operand.CreateLabel(operand.Type, name);
		}

		#endregion Emit Constant Methods
	}
}