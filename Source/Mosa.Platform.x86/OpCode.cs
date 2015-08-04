// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Platform.x86
{
	/// <summary>
	/// x86 OpCode
	/// </summary>
	public class OpCode
	{
		/// <summary>
		/// Byte code
		/// </summary>
		internal byte[] Code;

		/// <summary>
		/// Register field to extend the operation
		/// </summary>
		internal byte? RegField;

		/// <summary>
		/// Initializes a new instance of the <see cref="OpCode"/> class.
		/// </summary>
		/// <param name="code">The corresponding opcodes</param>
		/// <param name="regField">Additional parameter field</param>
		internal OpCode(byte[] code, byte? regField)
		{
			Code = code;
			RegField = regField;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OpCode"/> struct.
		/// </summary>
		/// <param name="code">The corresponding opcodes</param>
		internal OpCode(byte[] code)
		{
			Code = code;
			RegField = null;
		}
	}
}
