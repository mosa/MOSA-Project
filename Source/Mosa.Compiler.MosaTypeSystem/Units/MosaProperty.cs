/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

using System;

namespace Mosa.Compiler.MosaTypeSystem
{
	public class MosaProperty : MosaUnit, IEquatable<MosaProperty>
	{
		public MosaPropertyAttributes PropertyAttributes { get; private set; }

		public MosaType DeclaringType { get; private set; }

		public MosaType PropertyType { get; private set; }

		public MosaType InterfaceType { get; private set; }

		public MosaMethod GetterMethod
		{
			get
			{
				return this.DeclaringType.FindMethodByName("get_" + this.Name);
			}
		}

		public MosaMethod SetterMethod
		{
			get
			{
				return this.DeclaringType.FindMethodByName("set_" + this.Name);
			}
		}

		internal MosaProperty()
		{
		}

		internal MosaProperty Clone()
		{
			return (MosaProperty)base.MemberwiseClone();
		}

		public bool Equals(MosaProperty other)
		{
			return SignatureComparer.Equals(this.PropertyType, other.PropertyType);
		}

		public class Mutator : MosaUnit.MutatorBase
		{
			private MosaProperty property;

			internal Mutator(MosaProperty property)
				: base(property)
			{
				this.property = property;
			}

			public MosaPropertyAttributes PropertyAttributes { set { property.PropertyAttributes = value; } }

			public MosaType DeclaringType { set { property.DeclaringType = value; } }

			public MosaType PropertyType { set { property.PropertyType = value; } }

			public MosaType InterfaceType { set { property.InterfaceType = value; } }

			public override void Dispose()
			{
				if (property.PropertyType != null)
				{
					property.FullName = string.Concat(property.PropertyType.FullName, " ", property.DeclaringType.FullName, "::", property.Name);
					property.ShortName = string.Concat(property.Name, " : ", property.PropertyType.ShortName);
				}
			}
		}
	}
}