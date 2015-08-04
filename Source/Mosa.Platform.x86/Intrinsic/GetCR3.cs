// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	///
	/// </summary>
	internal sealed class GetCR3 : GetControlRegisterBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="GetCR3"/> class.
		/// </summary>
		public GetCR3()
			: base(ControlRegister.CR3)
		{
		}
	}
}
