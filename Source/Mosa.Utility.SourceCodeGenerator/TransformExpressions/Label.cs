// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Utility.SourceCodeGenerator.TransformExpressions
{
	public sealed class Label
	{
		public string Name { get; }

		public bool IsInResult { get; set; }

		public Label(string name)
		{
			Name = name;
			IsInResult = false;
		}

		public List<LabelPosition> Positions { get; } = new List<LabelPosition>();

		public void Add(int nodeNbr, int operandIndex)
		{
			Positions.Add(new LabelPosition(nodeNbr, operandIndex));
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
