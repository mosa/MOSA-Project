// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.MosaTypeSystem
{
	public class MosaCustomAttribute
	{
		public struct Argument
		{
			public Argument(MosaType type, object value)
			{
				this.Type = type;
				this.Value = value;
			}

			public readonly MosaType Type;
			public readonly object Value;
		}

		public class NamedArgument
		{
			public NamedArgument(string name, bool isField, Argument arg)
			{
				this.Name = name;
				this.IsField = isField;
				this.Argument = arg;
			}

			public string Name { get; private set; }

			public bool IsField { get; private set; }

			public Argument Argument { get; set; }
		}

		public MosaMethod Constructor { get; private set; }

		public Argument[] Arguments { get; private set; }

		public NamedArgument[] NamedArguments { get; private set; }

		public MosaCustomAttribute(MosaMethod ctor, Argument[] args, NamedArgument[] namedArgs)
		{
			Constructor = ctor;
			Arguments = args;
			NamedArguments = namedArgs;
		}
	}

	public class MosaCustomAttributeList : List<MosaCustomAttribute>
	{
		// This implementation isn't perfect but covers most cases
		public override bool Equals(object obj)
		{
			if (!(obj is MosaCustomAttributeList))
				return false;

			if (object.ReferenceEquals(this, obj))
				return true;

			var customAttributeList = obj as MosaCustomAttributeList;

			if (customAttributeList.Count != this.Count)
				return false;

			for (int i = 0; i < this.Count; i++)
			{
				if (!customAttributeList[i].Constructor.DeclaringType.Equals(this[i].Constructor.DeclaringType))
					return false;

				if (customAttributeList[i].Arguments.Length != this[i].Arguments.Length)
					return false;

				if (customAttributeList[i].NamedArguments.Length != this[i].NamedArguments.Length)
					return false;
			}

			return true;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}