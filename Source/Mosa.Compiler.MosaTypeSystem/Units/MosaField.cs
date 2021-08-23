// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Compiler.MosaTypeSystem
{
	public sealed class MosaField : MosaUnit, IEquatable<MosaField>
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

		override internal MosaField Clone()
		{
			return (MosaField)base.Clone();
		}

		public bool Equals(MosaField other)
		{
			return SignatureComparer.Equals(FieldType, other.FieldType);
		}

		public class Mutator : MosaUnit.MutatorBase
		{
			private readonly MosaField field;

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
					field.FullName = string.Concat(field.DeclaringType.FullName, "::", field.Name, " ", field.FieldType.FullName);
					field.ShortName = string.Concat(field.Name, " : ", field.FieldType.ShortName);
				}
			}
		}
	}
}
