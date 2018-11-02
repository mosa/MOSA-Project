// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Platform.Intel;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// SetCR4
	/// </summary>
	internal sealed class SetCR4 : SetControlRegisterBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SetCR4"/> class.
		/// </summary>
		public SetCR4()
			: base(ControlRegister.CR4)
		{
		}
	}
}
