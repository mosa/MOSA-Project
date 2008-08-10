/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the GNU GPL v3, with Classpath Linking Exception
 * Licensed under the terms of the New BSD License for exclusive use by the Ensemble OS Project
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.Metadata {
	public enum GenericParamAttributes {
		VarianceMask = 0x03,
		None = 0x00,
		Covariant = 0x01,
		Contravariant = 0x02,
		SpecialConstraintMask = 0x1C,
		ReferenceTypeConstraint = 0x04,
		NotNullableValueTypeConstraint = 0x08,
		DefaultConstructorConstraint = 0x10
	}
}
