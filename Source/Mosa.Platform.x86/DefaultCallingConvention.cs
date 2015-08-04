// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Platform;

namespace Mosa.Platform.x86
{
	/// <summary>
	/// Implements the default calling convention for x86.
	/// </summary>
	public class DefaultCallingConvention : BaseCallingConvention32Bit
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="DefaultCallingConvention"/>.
		/// </summary>
		/// <param name="architecture">The architecture of the calling convention.</param>
		public DefaultCallingConvention(BaseArchitecture architecture)
			: base(architecture)
		{
			scratchRegister = GeneralPurposeRegister.EDX;
			return32BitRegister = GeneralPurposeRegister.EAX;
			return64BitRegister = GeneralPurposeRegister.EDX;
			returnFloatingPointRegister = SSE2Register.XMM0;
		}

		#endregion Construction
	}
}
