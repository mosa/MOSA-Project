// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.MosaTypeSystem;

public sealed class MosaField : MosaUnit, IEquatable<MosaField>
{
	public MosaFieldAttributes FieldAttributes { get; private set; }

	public MosaType? DeclaringType { get; private set; }

	public MosaType? FieldType { get; private set; }

	public bool IsLiteral { get; private set; }

	public bool IsStatic { get; private set; }

	public bool HasDefault { get; private set; }

	public uint? Offset { get; private set; }

	public byte[]? Data { get; private set; }

	public bool HasOpenGenericParams { get; private set; }

	internal MosaField()
	{
	}

	internal override MosaField Clone()
	{
		return (MosaField)base.Clone();
	}

	public bool Equals(MosaField? other)
	{
		return SignatureComparer.Equals(FieldType, other?.FieldType);
	}

	public class Mutator : MosaUnit.MutatorBase
	{
		private readonly MosaField internalField;

		internal Mutator(MosaField field)
			: base(field)
		{
			internalField = field;
		}

		public MosaFieldAttributes FieldAttributes { set => internalField.FieldAttributes = value; }

		public MosaType? DeclaringType { set => internalField.DeclaringType = value; }

		public MosaType? FieldType { set => internalField.FieldType = value; }

		public bool IsLiteral { set => internalField.IsLiteral = value; }

		public bool IsStatic { set => internalField.IsStatic = value; }

		public bool HasDefault { set => internalField.HasDefault = value; }

		public uint? Offset { set => internalField.Offset = value; }

		public byte[] Data { set => internalField.Data = value; }

		public bool HasOpenGenericParams { set => internalField.HasOpenGenericParams = value; }

		public override void Dispose()
		{
			if (internalField.FieldType != null)
			{
				internalField.FullName = string.Concat(internalField.DeclaringType?.FullName, "::", internalField.Name, " ", internalField.FieldType.FullName);
				internalField.ShortName = string.Concat(internalField.Name, " : ", internalField.FieldType.ShortName);
			}
		}
	}
}
