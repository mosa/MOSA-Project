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
				Type = type;
				Value = value;
			}

			public readonly MosaType Type;
			public readonly object Value;
		}

		public class NamedArgument
		{
			public NamedArgument(string name, bool isField, Argument arg)
			{
				Name = name;
				IsField = isField;
				Argument = arg;
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
		public MosaCustomAttributeList() : base()
		{
		}

		public MosaCustomAttributeList(IEnumerable<MosaCustomAttribute> collection) : base(collection)
		{
		}

		// This implementation isn't perfect but covers most cases
		public override bool Equals(object obj)
		{
			if (!(obj is MosaCustomAttributeList))
				return false;

			if (ReferenceEquals(this, obj))
				return true;

			var customAttributeList = obj as MosaCustomAttributeList;

			if (customAttributeList.Count != Count)
				return false;

			for (int i = 0; i < Count; i++)
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
