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

	/// <summary>
	/// CIL provider token types according to ISO/IEC 23271:2006 (E).
	/// </summary>
	/// <remarks>
	/// These token types represent the tables in CIL provider. They are
	/// properly formatted to allow or'ing with a provider id to build
	/// a fully qualified provider token identifier.
	/// </remarks>
	public enum TokenTypes {
		Module					= 0x00000000,
		TypeRef					= 0x01000000,
		TypeDef					= 0x02000000,
		// 0x03000000 - undefined in ISO/IEC 23271:2006 (E)
		Field					= 0x04000000,
		// 0x05000000 - undefined in ISO/IEC 23271:2006 (E)
		MethodDef				= 0x06000000,
		// 0x07000000 - undefined in ISO/IEC 23271:2006 (E)
		Param					= 0x08000000,
		InterfaceImpl			= 0x09000000,
		MemberRef				= 0x0A000000,
		Constant				= 0x0B000000,
		CustomAttribute			= 0x0C000000,
		FieldMarshal			= 0x0D000000,
		DeclSecurity			= 0x0E000000,
		ClassLayout				= 0x0F000000,
		FieldLayout				= 0x10000000,
		StandAloneSig			= 0x11000000,
		EventMap				= 0x12000000,
		// 0x13000000 - undefined in ISO/IEC 23271:2006 (E)
		Event					= 0x14000000,
		PropertyMap				= 0x15000000,
		// 0x16000000 - undefined in ISO/IEC 23271:2006 (E)
		Property				= 0x17000000,
		MethodSemantics			= 0x18000000,
		MethodImpl				= 0x19000000,
		ModuleRef				= 0x1A000000,
		TypeSpec				= 0x1B000000,
		ImplMap					= 0x1C000000,
		FieldRVA				= 0x1D000000,
		// 0x1E000000 - undefined in ISO/IEC 23271:2006 (E)
		// 0x1F000000 - undefined in ISO/IEC 23271:2006 (E)
		Assembly				= 0x20000000,
		AssemblyProcessor		= 0x21000000,
		AssemblyOS				= 0x22000000,
		AssemblyRef				= 0x23000000,
		AssemblyRefProcessor	= 0x24000000,
		AssemblyRefOS			= 0x25000000,
		File					= 0x26000000,
		ExportedType			= 0x27000000,
		ManifestResource		= 0x28000000,
		NestedClass				= 0x29000000,
		GenericParam			= 0x2A000000,
		MethodSpec				= 0x2B000000,
		GenericParamConstraint	= 0x2C000000,

		MaxTable				= 0x2D000000,

        /// <summary>
        /// Special constant to represent a user string heap token.
        /// </summary>
        UserString                  = 0x70000000,

        /// <summary>
        /// Special constant to represent a string heap token.
        /// </summary>
        String                    = 0x71000000,

        /// <summary>
        /// Constant to represent a blob heap token.
        /// </summary>
        Blob                    = 0x72000000,

        /// <summary>
        /// Constant to represent a guid heap token.
        /// </summary>
        Guid                    = 0x73000000,

        /// <summary>
        /// Table identification mask.
        /// </summary>
		TableMask				= 0x7F000000,
		RowIndexMask			= 0x00FFFFFF
	}
}
