// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.FinalTweak
{
	/// <summary>
	/// FinalTweak Transformation List
	/// </summary>
	public static class FixedRegistersTransforms
	{
		public static readonly List<BaseTransformation> List = new List<BaseTransformation>
		{
			//new Call(),
			new Mov32(),
			new MovLoad16(),
			new MovLoad8(),
			new Movsd(),
			new Movss(),
			new MovStore16(),
			new MovStore8(),
			new Movsx16To32(),
			new Movsx8To32(),
			new Movzx16To32(),
			new Movzx8To32(),
			new Setcc(),
		};
	}
}
