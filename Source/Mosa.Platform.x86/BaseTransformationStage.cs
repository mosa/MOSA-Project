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

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Platform;
using Mosa.Compiler.Linker;
using Mosa.Compiler.Metadata;
using System;
using System.IO;

namespace Mosa.Platform.x86
{
	/// <summary>
	///
	/// </summary>
	public abstract class BaseTransformationStage : BasePlatformTransformationStage
	{
		protected override string Platform { get { return "x86"; } }

		public static X86Instruction GetMove(Operand Destination, Operand Source)
		{
			//Debug.Assert(Destination.Type.Type == Source.Type.Type);

			if (Source.Type.Type == CilElementType.R4)
			{
				return X86.Movss;
			}
			else if (Source.Type.Type == CilElementType.R8)
			{
				return X86.Movsd;
			}
			else
			{
				return X86.Mov;
			}
		}

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
		/// <param name="dop">The operand to emit as a constant.</param>
		/// <returns>An operand, which represents the reference to the read-only constant.</returns>
		/// <remarks>
		/// This function checks if the given operand needs to be moved into the read-only data
		/// section of the executable. For x86 this concerns only floating point operands, as these
		/// can't be specified in inline assembly.<para/>
		/// This function checks if the given operand needs to be moved and rewires the operand, if
		/// it must be moved.
		/// </remarks>
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
						writer.Write((float)operand.Value);
					}
					else if (operand.Type.Type == CilElementType.R8)
					{
						writer.Write((double)operand.Value);
					}
				}
			}

			return Operand.CreateLabel(operand.Type, name);
		}

		#endregion Emit Constant Methods
	}
}