// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	///	This stage removes dead and empty blocks.
	/// </summary>
	public class DeadBlockStage : BaseTransformationStage
	{
		public DeadBlockStage() : base(false, true)
		{
		}
	}
}
