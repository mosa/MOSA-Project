/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Ki (kiootic) <kiootic@gmail.com>
 */

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

		public bool IsLinkerGenerated { get; private set; }

		List<MosaCustomAttribute> customAttributes;
		public IList<MosaCustomAttribute> CustomAttributes { get; private set; }

		internal MosaUnit()
		{
			CustomAttributes = (customAttributes = new List<MosaCustomAttribute>()).AsReadOnly();
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
			foreach (var attr in customAttributes)
			{
				if (attr.Constructor.DeclaringType.FullName == fullName)
					return attr;
			}
			return null;
		}

		public abstract class MutatorBase : IDisposable
		{
			MosaUnit unit;
			internal MutatorBase(MosaUnit unit)
			{
				this.unit = unit;
			}

			public object UnderlyingObject { set { unit.UnderlyingObject = value; } }

			public string Name { set { unit.Name = value; } }

			public bool IsLinkerGenerated { set { unit.IsLinkerGenerated = value; } }

			public IList<MosaCustomAttribute> CustomAttributes { get { return unit.customAttributes; } }

			public abstract void Dispose();
		}
	}
}
