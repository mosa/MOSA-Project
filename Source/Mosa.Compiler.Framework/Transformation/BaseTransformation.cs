// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Compiler.Framework.Transformation
{
	public abstract class BaseTransformation
	{
		public virtual BaseInstruction Instruction { get; private set; }

		public string Name { get; }

		public BaseTransformation()
		{
			Name = ExtractName();
		}

		private string ExtractName()
		{
			string name = GetType().FullName;

			int offset1 = name.IndexOf('.');
			int offset2 = name.IndexOf('.', offset1);
			int offset3 = name.IndexOf('.', offset2);
			int offset4 = name.IndexOf('.', offset3);

			return name.Substring(offset4 + 1);
		}

		public bool ValidateInstruction(Context context)
		{
			if (context.IsEmpty)
				return false;

			return context.Instruction == Instruction;
		}

		public abstract bool Match(Context context, TransformContext transformContext);

		public abstract void Transform(Context context, TransformContext transformContext);
	}
}
