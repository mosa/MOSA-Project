// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using Mosa.Compiler.Framework.Transform;

namespace Mosa.Platform.x86.Transform.FinalTweak
{
	/// <summary>
	/// Transformations
	/// </summary>
	public static class Transforms
	{
		public static readonly List<BaseTransformation> FinalTweakList = new List<BaseTransformation>
		{
			//new Call(),
			new MovLoad16(),
			new Movsd(),
			new MovStore16(),
			new Movzx16To32(),
			new Movzx8To32(),
			new Setcc(),
		};
	}
}
