/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Platforms.x86
{
	/// <summary>
	/// x86 OpCode
	/// </summary>
	public struct OpCode
	{
		/// <summary>
		/// Byte code
		/// </summary>
		public byte[] Code;

		/// <summary>
		/// Register field to extend the operation
		/// </summary>
		public byte? RegField;

		/// <summary>
		/// Initializes a new instance of the <see cref="OpCode"/> struct.
		/// </summary>
		/// <param name="code">The corresponding opcodes</param>
		/// <param name="regField">Additonal parameter field</param>
		public OpCode(byte[] code, byte? regField)
		{
			Code = code;
			RegField = regField;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OpCode"/> struct.
		/// </summary>
		/// <param name="code">The corresponding opcodes</param>
		public OpCode(byte[] code)
		{
			Code = code;
			RegField = null;
		}
	}
}
