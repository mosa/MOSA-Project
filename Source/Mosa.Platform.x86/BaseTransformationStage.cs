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
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Platform;
using Mosa.Compiler.Linker;
using Mosa.Compiler.Metadata;

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

		#region Emit Constant Methods

		/// <summary>
		/// Emits the constant operands.
		/// </summary>
		/// <param name="context">The context.</param>
		protected void EmitOperandConstants(Context context)
		{
			if (context.OperandCount > 0)
				context.Operand1 = EmitConstant(context.Operand1);
			else if (context.OperandCount > 1)
				context.Operand2 = EmitConstant(context.Operand2);
			else if (context.OperandCount > 2)
				context.Operand3 = EmitConstant(context.Operand3);
		}

		/// <summary>
		/// Emits the constant operands.
		/// </summary>
		/// <param name="context">The context.</param>
		protected void EmitResultConstants(Context context)
		{
			if (context.ResultCount == 1)
				context.Result = EmitConstant(context.Result);
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
		protected Operand EmitConstant(Operand cop)
		{
			if (!cop.IsConstant)
				return cop;

			if (!(cop.StackType == StackTypeCode.F || cop.StackType == StackTypeCode.Int64))
				return cop;

			int size, alignment;
			architecture.GetTypeRequirements(cop.Type, out size, out alignment);

			string name = String.Format("C_{0}", Guid.NewGuid());
			using (Stream stream = methodCompiler.Linker.Allocate(name, SectionKind.ROData, size, alignment))
			{
				using (BinaryWriter writer = new BinaryWriter(stream))
				{
					switch (cop.Type.Type)
					{
						case CilElementType.R4: writer.Write((float)cop.Value); break;
						case CilElementType.R8: writer.Write((double)cop.Value); break;
						case CilElementType.I8: writer.Write((long)cop.Value); break;
						case CilElementType.U8: writer.Write((ulong)cop.Value); break;
						default: throw new NotSupportedException();
					}
				}
			}
			// FIXME: Attach the label operand to the linker symbol
			// FIXME: Rename the operand to SymbolOperand
			// FIXME: Use the provided name to link
			return Operand.CreateLabel(cop.Type, name);
		}

		#endregion // Emit Methods

	}
}
