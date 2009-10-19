/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.Metadata {
    /// <summary>
    /// 
    /// </summary>
	public enum GenericParamAttributes {
        /// <summary>
        /// 
        /// </summary>
		VarianceMask = 0x03,
        /// <summary>
        /// 
        /// </summary>
		None = 0x00,
        /// <summary>
        /// 
        /// </summary>
		Covariant = 0x01,
        /// <summary>
        /// 
        /// </summary>
		Contravariant = 0x02,
        /// <summary>
        /// 
        /// </summary>
		SpecialConstraintMask = 0x1C,
        /// <summary>
        /// 
        /// </summary>
		ReferenceTypeConstraint = 0x04,
        /// <summary>
        /// 
        /// </summary>
		NotNullableValueTypeConstraint = 0x08,
        /// <summary>
        /// 
        /// </summary>
		DefaultConstructorConstraint = 0x10
	}
}
