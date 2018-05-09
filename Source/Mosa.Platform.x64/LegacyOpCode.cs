// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Platform.x64
{
	/// <summary>
	/// x64 OpCode
	/// </summary>
	public sealed class LegacyOpCode
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
		/// Initializes a new instance of the <see cref="LegacyOpCode"/> class.
		/// </summary>
		/// <param name="code">The corresponding opcodes</param>
		/// <param name="regField">Additional parameter field</param>
		internal LegacyOpCode(byte[] code, byte? regField)
		{
			Code = code;
			RegField = regField;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LegacyOpCode"/> struct.
		/// </summary>
		/// <param name="code">The corresponding opcodes</param>
		internal LegacyOpCode(byte[] code)
		{
			Code = code;
			RegField = null;
		}
	}
}
