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

		#region Common Internals

		/// <summary>
		/// Determines whether the specified operand is 32 bits.
		/// </summary>
		/// <param name="operand">The operand.</param>
		/// <returns></returns>
		protected static bool Is32Bit(Operand operand)
		{
			return (operand.Type.Type == CilElementType.U4)
				|| (operand.Type.Type == CilElementType.I4)
				|| IsPointer(operand)		// FIXME: Only on 32-bit platforms is this correct
				|| IsObject(operand);		// FIXME: Only on 32-bit platforms is this correct
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

		#endregion Common Internals

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