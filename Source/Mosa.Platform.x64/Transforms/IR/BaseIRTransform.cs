﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Transforms.IR
{
	public abstract class BaseIRTransform : BaseX64Transform
	{
		public BaseIRTransform(BaseInstruction instruction, TransformType type, bool log = false)
			: base(instruction, type, log)
		{ }

		#region Helpers

		//

		#endregion Helpers
	}
}
