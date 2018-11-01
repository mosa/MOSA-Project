// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Platform.Intel;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// GetCR2
	/// </summary>
	internal sealed class GetCR2 : GetControlRegisterBase
	{
		public GetCR2()
			: base(ControlRegister.CR2)
		{
		}
	}
}
