// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Platform;

namespace Mosa.Platform.ESP32
{
	/// <summary>
	/// BaseTransformationStage
	/// </summary>
	public abstract class BaseTransformationStage : BasePlatformTransformationStage
	{
		protected override string Platform { get { return "ESP32"; } }
	}
}
