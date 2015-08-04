// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	///
	/// </summary>
	internal sealed class GetCR2 : GetControlRegisterBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="GetCR2"/> class.
		/// </summary>
		public GetCR2()
			: base(ControlRegister.CR2)
		{
		}
	}
}
