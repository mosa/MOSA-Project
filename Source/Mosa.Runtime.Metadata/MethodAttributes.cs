/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */


using System;

namespace Mosa.Runtime.Metadata
{

	/// <summary>
	/// Method attributes according to ISO/IEC 23271:2006 (E), Partition II, §23.1.8 as used in method definition provider. 
	/// </summary>
	[Flags]
	public enum MethodAttributes : ushort
	{
		/// <summary>
		/// These 3 bits contain one of the following values: CompilerControlled, Private, FamAndAssem, Assem, Family, FamilyOrAssem, Public.
		/// </summary>
		MemberAccessMask = 0x0007,

		/// <summary>
		/// Member not referenceable
		/// </summary>
		CompilerControlled = 0x0000,

		/// <summary>
		/// Accessible only by the parent type
		/// </summary>
		Private = 0x0001,

		/// <summary>
		/// Accessible by sub-types only in this Assembly
		/// </summary>
		FamANDAssem = 0x0002,

		/// <summary>
		/// Accessibly by anyone in the Assembly
		/// </summary>
		Assem = 0x0003,

		/// <summary>
		/// Accessible only by type and sub-types
		/// </summary>
		Family = 0x0004,

		/// <summary>
		/// Accessibly by sub-types anywhere, plus anyone in assembly
		/// </summary>
		FamORAssem = 0x0005,

		/// <summary>
		/// Accessibly by anyone who has visibility to this scope
		/// </summary>
		Public = 0x0006,

		/// <summary>
		/// Defined on type, else per instance
		/// </summary>
		Static = 0x0010,

		/// <summary>
		/// Method cannot be overridden
		/// </summary>
		Final = 0x0020,

		/// <summary>
		/// Method is virtual
		/// </summary>
		Virtual = 0x0040,

		/// <summary>
		/// Method hides by name+buffer, else just by name
		/// </summary>
		HideBySig = 0x0080,

		/// <summary>
		/// Use this mask to retrieve vtable attributes
		/// </summary>
		VtableLayoutMask = 0x0100,

		/// <summary>
		/// Method reuses existing slot in vtable
		/// </summary>
		ReuseSlot = 0x0000,

		/// <summary>
		/// Method always gets a new slot in the vtable
		/// </summary>
		NewSlot = 0x0100,

		/// <summary>
		/// Method can only be overriden if also accessible
		/// </summary>
		Strict = 0x0200,

		/// <summary>
		/// Method does not provide an implementation
		/// </summary>
		Abstract = 0x0400,

		/// <summary>
		/// Method is special
		/// </summary>
		SpecialName = 0x0800,

		/// <summary>
		/// Implementation is forwarded through PInvoke
		/// </summary>
		PInvokeImpl = 0x2000,

		/// <summary>
		/// Reserved: shall be zero for conforming implementations
		/// </summary>
		UnmanagedExport = 0x0800,

		/// <summary>
		/// CLI provides 'special' behavior, depending upon the name of the method
		/// </summary>
		RTSpecialName = 0x1000,

		/// <summary>
		/// Method has security associate with it
		/// </summary>
		HasSecurity = 0x4000,

		/// <summary>
		/// Method calls another method containing security code.
		/// </summary>
		RequireSecObject = 0x8000
	}
}
