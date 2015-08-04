// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	///
	/// </summary>
	internal sealed class SetCR0 : SetControlRegisterBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SetCR0"/> class.
		/// </summary>
		public SetCR0()
			: base(ControlRegister.CR0)
		{
		}
	}
}
