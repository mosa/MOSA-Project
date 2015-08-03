// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	///
	/// </summary>
	internal sealed class SetCR2 : SetControlRegisterBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SetCR2"/> class.
		/// </summary>
		public SetCR2()
			: base(ControlRegister.CR2)
		{
		}
	}
}
