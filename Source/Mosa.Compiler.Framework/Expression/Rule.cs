// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosa.Compiler.Framework.Expression
{
	public class Rule
	{
		public int ID { get; set; }

		public string Name { get; set; }
		public string Type { get; set; }

		public string Match { get; set; }
		public string Transform { get; set; }
		public string Criteria { get; set; }

		public string DefaultInstructionFamily { get; set; }
		public string DefaultArchitectureFamily { get; set; }

		public bool IsOptimization { get; set; } = false;
		public bool IsTransformation { get; set; } = false;
	}
}
