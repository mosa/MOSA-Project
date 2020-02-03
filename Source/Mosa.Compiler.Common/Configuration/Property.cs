// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.Common.Configuration
{
	public sealed class Property
	{
		public string Name { get; }

		public string Value { get; set; }

		public List<string> List { get; } = new List<string>();

		public int Level { get; }

		public Property(string name)
		{
			Name = name;
			Level = DetermineLevel();
		}

		public override string ToString()
		{
			if (Value == null)
				return $"{Name}=NULL";
			else
				return $"{Name}={Value}";
		}

		public bool IsValueTrue
		{
			get
			{
				if (string.IsNullOrEmpty(Value))
					return false;

				return Value.ToLower().StartsWith("true") || Value.ToLower().StartsWith("y") || Value.ToLower().StartsWith("on");
			}
		}

		public bool IsValueFalse
		{
			get
			{
				if (string.IsNullOrEmpty(Value))
					return false;

				return Value.ToLower().StartsWith("false") || Value.ToLower().StartsWith("n") || Value.ToLower().StartsWith("off");
			}
		}

		private int DetermineLevel()
		{
			int count = 0;

			foreach (var c in Name)
			{
				if (c == '.')
					count++;
			}

			return count;
		}
	}
}
