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
using System.Diagnostics;
using System.IO;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Linker;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;

using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platforms.x86
{

	/// <summary>
	/// 
	/// </summary>
	public abstract class BaseX86TransformationStage :
		CodeTransformationStage,
		IMethodCompilerStage,
		IPlatformTransformationStage
	{
		private readonly DataConverter LittleEndianBitConverter = DataConverter.LittleEndian;

		#region X86 Internals

		/// <summary>
		/// Determines whether the specified op is unsigned.
		/// </summary>
		/// <param name="op">The op.</param>
		/// <returns>
		/// 	<c>true</c> if the specified op is unsigned; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsUnsigned(Operand op)
		{
			return IsUByte(op) || IsUShort(op) || IsUInt(op) || IsChar(op);
		}

		/// <summary>
		/// Determines whether the specified op is signed.
		/// </summary>
		/// <param name="op">The op.</param>
		/// <returns>
		/// 	<c>true</c> if the specified op is signed; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsSigned(Operand op)
		{
			return IsSByte(op) || IsSShort(op) || IsInt(op);
		}

		/// <summary>
		/// Determines whether the specified op is byte.
		/// </summary>
		/// <param name="op">The op.</param>
		/// <returns>
		/// 	<c>true</c> if the specified op is byte; otherwise, <c>false</c>.
		/// </returns>
		private static bool IsByte(Operand op)
		{
			return IsSByte(op) || IsUByte(op);
		}

		/// <summary>
		/// Determines whether [is S byte] [the specified op].
		/// </summary>
		/// <param name="op">The op.</param>
		/// <returns>
		/// 	<c>true</c> if [is S byte] [the specified op]; otherwise, <c>false</c>.
		/// </returns>
		private static bool IsSByte(Operand op)
		{
			return (op.Type.Type == CilElementType.I1);
		}

		/// <summary>
		/// Determines whether [is U byte] [the specified op].
		/// </summary>
		/// <param name="op">The op.</param>
		/// <returns>
		/// 	<c>true</c> if [is U byte] [the specified op]; otherwise, <c>false</c>.
		/// </returns>
		private static bool IsUByte(Operand op)
		{
			return (op.Type.Type == CilElementType.U1);
		}

		/// <summary>
		/// Determines whether the specified op is short.
		/// </summary>
		/// <param name="op">The op.</param>
		/// <returns>
		/// 	<c>true</c> if the specified op is short; otherwise, <c>false</c>.
		/// </returns>
		private static bool IsShort(Operand op)
		{
			return IsSShort(op) || IsUShort(op);
		}

		/// <summary>
		/// Determines whether [is S short] [the specified op].
		/// </summary>
		/// <param name="op">The op.</param>
		/// <returns>
		/// 	<c>true</c> if [is S short] [the specified op]; otherwise, <c>false</c>.
		/// </returns>
		private static bool IsSShort(Operand op)
		{
			return (op.Type.Type == CilElementType.I2);
		}

		/// <summary>
		/// Determines whether [is U short] [the specified op].
		/// </summary>
		/// <param name="op">The op.</param>
		/// <returns>
		/// 	<c>true</c> if [is U short] [the specified op]; otherwise, <c>false</c>.
		/// </returns>
		private static bool IsUShort(Operand op)
		{
			return (op.Type.Type == CilElementType.U2);
		}

		/// <summary>
		/// Determines whether the specified op is char.
		/// </summary>
		/// <param name="op">The op.</param>
		/// <returns>
		/// 	<c>true</c> if the specified op is char; otherwise, <c>false</c>.
		/// </returns>
		private static bool IsChar(Operand op)
		{
			return (op.Type.Type == CilElementType.Char || IsUShort(op));
		}

		/// <summary>
		/// Determines whether the specified op is int.
		/// </summary>
		/// <param name="op">The op.</param>
		/// <returns>
		/// 	<c>true</c> if the specified op is int; otherwise, <c>false</c>.
		/// </returns>
		private static bool IsInt(Operand op)
		{
			return (op.Type.Type == CilElementType.I4);
		}

		/// <summary>
		/// Determines whether [is U int] [the specified op].
		/// </summary>
		/// <param name="op">The op.</param>
		/// <returns>
		/// 	<c>true</c> if [is U int] [the specified op]; otherwise, <c>false</c>.
		/// </returns>
		private static bool IsUInt(Operand op)
		{
			return (op.Type.Type == CilElementType.U4);
		}

		#endregion // X86 Internals
	}
}
