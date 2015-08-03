// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	///
	/// </summary>
	internal sealed class GetCR4 : GetControlRegisterBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="GetCR4"/> class.
		/// </summary>
		public GetCR4()
			: base(ControlRegister.CR4)
		{
		}
	}
}