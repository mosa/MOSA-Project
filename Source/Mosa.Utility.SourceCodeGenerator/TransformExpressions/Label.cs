// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Utility.SourceCodeGenerator.TransformExpressions
{
	public partial class Label
	{
		public string Name { get; }

		public Label(string name)
		{
			Name = name;
		}

		public List<LabelPosition> Positions { get; } = new List<LabelPosition>();

		public void Add(int nodeNbr, int operandIndex)
		{
			Positions.Add(new LabelPosition(nodeNbr, operandIndex));
		}
	}
}
