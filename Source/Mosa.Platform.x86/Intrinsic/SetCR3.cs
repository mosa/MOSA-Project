// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	///
	/// </summary>
	internal sealed class SetCR3 : SetControlRegisterBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SetCR3"/> class.
		/// </summary>
		public SetCR3()
			: base(ControlRegister.CR3)
		{
		}
	}
}
