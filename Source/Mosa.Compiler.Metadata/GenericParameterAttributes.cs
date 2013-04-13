using System;

namespace Mosa.Compiler.Metadata
{
	[Flags]
	public enum GenericParameterAttributes : ushort
	{
		VarianceMask = 0x0003,
		NonVariant = 0x0000,
		Covariant = 0x0001,
		Contravariant = 0x0002,

		SpecialConstraintMask = 0x001c,
		ReferenceTypeConstraint = 0x0004,
		NotNullableValueTypeConstraint = 0x0008,
		DefaultConstructorConstraint = 0x0010
	}
}