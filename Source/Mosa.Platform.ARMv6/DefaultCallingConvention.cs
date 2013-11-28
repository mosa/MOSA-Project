/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Platform;

namespace Mosa.Platform.ARMv6
{
	/// <summary>
	/// Implements the default calling convention for ARM v6.
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
			// FIXME:
			scratchRegister = GeneralPurposeRegister.R12;
			return32BitRegister = GeneralPurposeRegister.R1;
			return64BitRegister = GeneralPurposeRegister.R2;
			returnFloatingPointRegister = GeneralPurposeRegister.R1;
		}

		#endregion Construction
	}
}