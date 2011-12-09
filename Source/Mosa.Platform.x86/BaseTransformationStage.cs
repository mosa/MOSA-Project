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
using Mosa.Compiler.Framework.Operands;
using Mosa.Compiler.Linker;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86
{

	/// <summary>
	/// 
	/// </summary>
	public abstract class BaseTransformationStage : BaseCodeTransformationStage
	{

		#region Data members

		protected DataConverter Converter = DataConverter.LittleEndian;

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
			if (ctx.ResultCount > 0)
				ctx.Result = EmitConstant(ctx.Result);
			else if (ctx.OperandCount > 1)
				ctx.Result2 = EmitConstant(ctx.Result2);
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
					byte[] buffer;

					switch (cop.Type.Type)
					{
						case CilElementType.R4:
							buffer = Converter.GetBytes((float)cop.Value);
							break;

						case CilElementType.R8:
							buffer = Converter.GetBytes((double)cop.Value);
							break;

						case CilElementType.I8:
							buffer = Converter.GetBytes((long)cop.Value);
							break;

						case CilElementType.U8:
							buffer = Converter.GetBytes((ulong)cop.Value);
							break;
						default:
							throw new NotSupportedException();
					}

					stream.Write(buffer, 0, buffer.Length);
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

		#region X86 Internals

		/// <summary>
		/// Determines whether the specified operand is 32 bits.
		/// </summary>
		/// <param name="operand">The operand.</param>
		/// <returns></returns>
		protected static bool Is32Bit(Operand operand)
		{
			return (operand.Type.Type == CilElementType.U4)
				|| (operand.Type.Type == CilElementType.I4)
				|| IsPointer(operand)
				|| IsObject(operand);
		}

		/// <summary>
		/// Determines whether the specified operand is unsigned.
		/// </summary>
		/// <param name="operand">The operand.</param>
		/// <returns>
		/// 	<c>true</c> if the specified operand is unsigned; otherwise, <c>false</c>.
		/// </returns>
		protected static bool IsUnsigned(Operand operand)
		{
			return IsUByte(operand) || IsUShort(operand) || IsUInt(operand) || IsChar(operand);
		}

		/// <summary>
		/// Determines whether the specified operand is signed.
		/// </summary>
		/// <param name="operand">The operand.</param>
		/// <returns>
		/// 	<c>true</c> if the specified operand is signed; otherwise, <c>false</c>.
		/// </returns>
		protected static bool IsSigned(Operand operand)
		{
			return IsSByte(operand) || IsSShort(operand) || IsInt(operand) || IsPointer(operand);
		}

		/// <summary>
		/// Determines whether the specified operand is byte.
		/// </summary>
		/// <param name="operand">The operand.</param>
		/// <returns>
		/// 	<c>true</c> if the specified operand is byte; otherwise, <c>false</c>.
		/// </returns>
		private static bool IsByte(Operand operand)
		{
			return IsSByte(operand) || IsUByte(operand);
		}

		/// <summary>
		/// Determines whether [is S byte] [the specified operand].
		/// </summary>
		/// <param name="operand">The operand.</param>
		/// <returns>
		/// 	<c>true</c> if [is S byte] [the specified operand]; otherwise, <c>false</c>.
		/// </returns>
		private static bool IsSByte(Operand operand)
		{
			return (operand.Type.Type == CilElementType.I1);
		}

		/// <summary>
		/// Determines whether [is U byte] [the specified operand].
		/// </summary>
		/// <param name="operand">The operand.</param>
		/// <returns>
		/// 	<c>true</c> if [is U byte] [the specified operand]; otherwise, <c>false</c>.
		/// </returns>
		private static bool IsUByte(Operand operand)
		{
			return (operand.Type.Type == CilElementType.U1);
		}

		/// <summary>
		/// Determines whether the specified operand is short.
		/// </summary>
		/// <param name="operand">The operand.</param>
		/// <returns>
		/// 	<c>true</c> if the specified operand is short; otherwise, <c>false</c>.
		/// </returns>
		private static bool IsShort(Operand operand)
		{
			return IsSShort(operand) || IsUShort(operand);
		}

		/// <summary>
		/// Determines whether [is S short] [the specified operand].
		/// </summary>
		/// <param name="operand">The operand.</param>
		/// <returns>
		/// 	<c>true</c> if [is S short] [the specified operand]; otherwise, <c>false</c>.
		/// </returns>
		private static bool IsSShort(Operand operand)
		{
			return (operand.Type.Type == CilElementType.I2);
		}

		/// <summary>
		/// Determines whether [is U short] [the specified operand].
		/// </summary>
		/// <param name="operand">The operand.</param>
		/// <returns>
		/// 	<c>true</c> if [is U short] [the specified operand]; otherwise, <c>false</c>.
		/// </returns>
		private static bool IsUShort(Operand operand)
		{
			return (operand.Type.Type == CilElementType.U2);
		}

		/// <summary>
		/// Determines whether the specified operand is char.
		/// </summary>
		/// <param name="operand">The operand.</param>
		/// <returns>
		/// 	<c>true</c> if the specified operand is char; otherwise, <c>false</c>.
		/// </returns>
		private static bool IsChar(Operand operand)
		{
			return (operand.Type.Type == CilElementType.Char || IsUShort(operand));
		}

		/// <summary>
		/// Determines whether the specified operand is int.
		/// </summary>
		/// <param name="operand">The operand.</param>
		/// <returns>
		/// 	<c>true</c> if the specified operand is int; otherwise, <c>false</c>.
		/// </returns>
		private static bool IsInt(Operand operand)
		{
			return (operand.Type.Type == CilElementType.I4);
		}

		/// <summary>
		/// Determines whether [is U int] [the specified operand].
		/// </summary>
		/// <param name="operand">The operand.</param>
		/// <returns>
		/// 	<c>true</c> if [is U int] [the specified operand]; otherwise, <c>false</c>.
		/// </returns>
		private static bool IsUInt(Operand operand)
		{
			return (operand.Type.Type == CilElementType.U4);
		}

		/// <summary>
		/// Determines whether the specified operand is an object reference.
		/// </summary>
		/// <param name="operand">The operand.</param>
		/// <returns>
		/// 	<c>true</c> if the specified operand is an object reference; otherwise, <c>false</c>.
		/// </returns>
		private static bool IsObject(Operand operand)
		{
			return operand.StackType == StackTypeCode.O;
		}

		/// <summary>
		/// Determines whether the specified operand is a pointer.
		/// </summary>
		/// <param name="operand">The operand.</param>
		/// <returns>
		/// 	<c>true</c> if the specified operand is a pointer; otherwise, <c>false</c>.
		/// </returns>
		private static bool IsPointer(Operand operand)
		{
			return operand.Type.Type == CilElementType.Ptr;
		}

		#endregion // X86 Internals
	}
}
