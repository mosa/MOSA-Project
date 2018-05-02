// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Platform
{
	/// <summary>
	/// Flag Status
	/// </summary>
	public enum FlagOutputState
	{
		Undefined,  // ?
		Set,        // 1
		Clear,      // 0
		Modified,   // X
		Unchanged,  // U
	}
}
