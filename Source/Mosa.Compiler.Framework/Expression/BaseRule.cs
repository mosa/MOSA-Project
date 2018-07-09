// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosa.Compiler.Framework.Expression
{
	public class BaseRule
	{
		public virtual int ID { get; }

		public virtual string Name { get; }
		public virtual string Type { get; }

		public virtual string Match { get; }
		public virtual string Transform { get; }
		public virtual string Criteria { get; }

		public virtual string DefaultInstructionFamily { get; }
		public virtual string DefaultArchitectureFamily { get; }

		public virtual bool IsOptimization { get; } = false;
		public virtual bool IsTransformation { get; } = false;
	}
}
