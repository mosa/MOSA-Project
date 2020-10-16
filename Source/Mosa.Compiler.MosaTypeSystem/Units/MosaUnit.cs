// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;

namespace Mosa.Compiler.MosaTypeSystem
{
	public abstract class MosaUnit
	{
		public object UnderlyingObject { get; private set; }

		public uint ID { get; internal set; }

		public TypeSystem TypeSystem { get; internal set; }

		public string Name { get; private set; }

		public string FullName { get; internal set; }

		public string ShortName { get; internal set; }

		public bool IsCompilerGenerated { get; private set; }

		private readonly MosaCustomAttributeList customAttributes;

		public IList<MosaCustomAttribute> CustomAttributes { get; }

		internal MosaUnit()
		{
			CustomAttributes = (customAttributes = new MosaCustomAttributeList()).AsReadOnly();
			Name = "";
		}

		public T GetUnderlyingObject<T>()
		{
			return (T)UnderlyingObject;
		}

		public override string ToString()
		{
			return FullName;
		}

		public MosaCustomAttribute FindCustomAttribute(string fullName)
		{
			foreach (var attribute in customAttributes)
			{
				if (attribute.Constructor.DeclaringType.FullName == fullName)
				{
					return attribute;
				}
			}

			return null;
		}

		public abstract class MutatorBase : IDisposable
		{
			private readonly MosaUnit unit;

			internal MutatorBase(MosaUnit unit)
			{
				this.unit = unit;
			}

			public object UnderlyingObject { set { unit.UnderlyingObject = value; } }

			public string Name { set { unit.Name = value; } }

			public bool IsCompilerGenerated { set { unit.IsCompilerGenerated = value; } }

			public IList<MosaCustomAttribute> CustomAttributes { get { return unit.customAttributes; } }

			public abstract void Dispose();
		}
	}
}
