/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using Mosa.Compiler.Metadata;
using Mosa.Compiler.TypeSystem;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// Interface of instruction decoders.
	/// </summary>
	public interface IInstructionDecoder
	{
		/// <summary>
		/// Gets the method compiler that is currently executing.
		/// </summary>
		BaseMethodCompiler Compiler { get; }

		/// <summary>
		/// Gets the RuntimeMethod being compiled.
		/// </summary>
		RuntimeMethod Method { get; }

		/// <summary>
		/// Gets the type system.
		/// </summary>
		/// <value>The type system.</value>
		ITypeModule TypeModule { get; }

		/// <summary>
		/// Gets the generic type patcher.
		/// </summary>
		IGenericTypePatcher GenericTypePatcher { get; }

		/// <summary>
		/// Decodes the byte value from the instruction stream
		/// </summary>
		/// <returns></returns>
		byte DecodeByte();

		/// <summary>
		/// Decodes the sbyte value from the instruction stream
		/// </summary>
		/// <returns></returns>
		sbyte DecodeSByte();

		/// <summary>
		/// Decodes the short value from the instruction stream
		/// </summary>
		/// <returns></returns>
		short DecodeShort();

		/// <summary>
		/// Decodes the ushort value from the instruction stream
		/// </summary>
		/// <returns></returns>
		ushort DecodeUShort();

		/// <summary>
		/// Decodes the int value from the instruction stream
		/// </summary>
		/// <returns></returns>
		int DecodeInt();

		/// <summary>
		/// Decodes the uint value from the instruction stream
		/// </summary>
		/// <returns></returns>
		uint DecodeUInt();

		/// <summary>
		/// Decodes the long value from the instruction stream
		/// </summary>
		/// <returns></returns>
		long DecodeLong();

		/// <summary>
		/// Decodes the float value from the instruction stream
		/// </summary>
		/// <returns></returns>
		float DecodeFloat();

		/// <summary>
		/// Decodes the double value from the instruction stream
		/// </summary>
		/// <returns></returns>
		double DecodeDouble();

		/// <summary>
		/// Decodes the tokentype from the instruction stream
		/// </summary>
		/// <returns></returns>
		Token DecodeTokenType();
	}
}
