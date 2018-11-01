// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Platform.Intel;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// GetCR4
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
