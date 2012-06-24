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

using Mosa.Compiler.Metadata;

namespace Mosa.Compiler.Framework.Platform
{

	/// <summary>
	/// 
	/// </summary>
	public abstract class BasePlatformTransformationStage : BaseCodeTransformationStage
	{

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

		#endregion // Common Internals
	}
}
