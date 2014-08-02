﻿/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Ki (kiootic) <kiootic@gmail.com>
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

using System;

namespace Mosa.Compiler.MosaTypeSystem
{
	public class MosaField : MosaUnit, IEquatable<MosaField>
	{
		public MosaFieldAttributes FieldAttributes { get; private set; }

		public MosaType DeclaringType { get; private set; }

		public MosaType FieldType { get; private set; }

		public bool IsLiteral { get; private set; }

		public bool IsStatic { get; private set; }

		public bool HasDefault { get; private set; }

		public uint? Offset { get; private set; }

		public byte[] Data { get; private set; }

		public bool HasOpenGenericParams { get; private set; }

		internal MosaField()
		{
		}

		internal MosaField Clone()
		{
			return (MosaField)base.MemberwiseClone();
		}

		public bool Equals(MosaField other)
		{
			return SignatureComparer.Equals(this.FieldType, other.FieldType);
		}

		public class Mutator : MosaUnit.MutatorBase
		{
			private MosaField field;

			internal Mutator(MosaField field)
				: base(field)
			{
				this.field = field;
			}

			public MosaFieldAttributes FieldAttributes { set { field.FieldAttributes = value; } }

			public MosaType DeclaringType { set { field.DeclaringType = value; } }

			public MosaType FieldType { set { field.FieldType = value; } }

			public bool IsLiteral { set { field.IsLiteral = value; } }

			public bool IsStatic { set { field.IsStatic = value; } }

			public bool HasDefault { set { field.HasDefault = value; } }

			public uint? Offset { set { field.Offset = value; } }

			public byte[] Data { set { field.Data = value; } }

			public bool HasOpenGenericParams { set { field.HasOpenGenericParams = value; } }

			public override void Dispose()
			{
				if (field.FieldType != null)
				{
					field.FullName = string.Concat(field.FieldType.FullName, " ", field.DeclaringType.FullName, "::", field.Name);
					field.ShortName = string.Concat(field.Name, " : ", field.FieldType.ShortName);
				}
			}
		}
	}
}