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

using System;
using System.IO;
using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Operands;
using Mosa.Compiler.Linker;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Platform;

namespace Mosa.Platform.x86
{

	/// <summary>
	/// 
	/// </summary>
	public abstract class BaseTransformationStage : BasePlatformTransformationStage
	{

		#region Data members

		#endregion // Data members

		#region IPipelineStage Members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		public override string Name { get { return "x86." + this.GetType().Name; } }

		#endregion // IPipelineStage Members

		#region Emit Methods

		/// <summary>
		/// Emits the constant operands.
		/// </summary>
		/// <param name="ctx">The context.</param>
		protected void EmitOperandConstants(Context ctx)
		{
			if (ctx.OperandCount > 0)
				ctx.Operand1 = EmitConstant(ctx.Operand1);
			else if (ctx.OperandCount > 1)
				ctx.Operand2 = EmitConstant(ctx.Operand2);
			else if (ctx.OperandCount > 2)
				ctx.Operand3 = EmitConstant(ctx.Operand3);
		}

		/// <summary>
		/// Emits the constant operands.
		/// </summary>
		/// <param name="ctx">The context.</param>
		protected void EmitResultConstants(Context ctx)
		{
			if (ctx.ResultCount == 1)
				ctx.Result = EmitConstant(ctx.Result);
		}

		/// <summary>
		/// This function emits a constant variable into the read-only data section.
		/// </summary>
		/// <param name="op">The operand to emit as a constant.</param>
		/// <returns>An operand, which represents the reference to the read-only constant.</returns>
		/// <remarks>
		/// This function checks if the given operand needs to be moved into the read-only data
		/// section of the executable. For x86 this concerns only floating point operands, as these
		/// can't be specified in inline assembly.<para/>
		/// This function checks if the given operand needs to be moved and rewires the operand, if
		/// it must be moved.
		/// </remarks>
		protected Operand EmitConstant(Operand op)
		{
			ConstantOperand cop = op as ConstantOperand;
			if (cop != null && (cop.StackType == StackTypeCode.F || cop.StackType == StackTypeCode.Int64))
			{
				int size, alignment;
				architecture.GetTypeRequirements(cop.Type, out size, out alignment);

				string name = String.Format("C_{0}", Guid.NewGuid());
				using (Stream stream = methodCompiler.Linker.Allocate(name, SectionKind.ROData, size, alignment))
				{
					using (BinaryWriter writer = new BinaryWriter(stream))
					{
						switch (cop.Type.Type)
						{
							case CilElementType.R4:
								writer.Write((float)cop.Value);
								break;

							case CilElementType.R8:
								writer.Write((double)cop.Value);
								break;

							case CilElementType.I8:
								writer.Write((long)cop.Value);
								break;

							case CilElementType.U8:
								writer.Write((ulong)cop.Value);
								break;
							default:
								throw new NotSupportedException();
						}
					}
				}
				// FIXME: Attach the label operand to the linker symbol
				// FIXME: Rename the operand to SymbolOperand
				// FIXME: Use the provided name to link
				LabelOperand lop = new LabelOperand(cop.Type, name);
				op = lop;
			}
			return op;
		}

		#endregion // Emit Methods

	}
}
