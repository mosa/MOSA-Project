// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Utility.SourceCodeGenerator.TransformExpressions
{
	public class Method
	{
		public string MethodName { get; set; }

		public bool IsNegated { get; set; } = false;

		public List<Operand> Parameters { get; } = new List<Operand>();

		public override string ToString()
		{
			return $"{MethodName}";
		}
	}
}
