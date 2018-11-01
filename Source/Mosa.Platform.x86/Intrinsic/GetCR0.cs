// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Platform.Intel;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// GetCR0
	/// </summary>
	internal sealed class GetCR0 : GetControlRegisterBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="GetCR0"/> class.
		/// </summary>
		public GetCR0()
			: base(ControlRegister.CR0)
		{
		}
	}
}
