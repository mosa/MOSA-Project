/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

using System;

namespace Mosa.Compiler.Metadata
{
	[Flags]
	public enum TypeAttributes : uint
	{
		VisibilityMask = 0x00000007,
		NotPublic = 0x00000000,
		Public = 0x00000001,
		NestedPublic = 0x00000002,
		NestedPrivate = 0x00000003,
		NestedFamily = 0x00000004,
		NestedAssembly = 0x00000005,
		NestedFamANDAssem = 0x00000006,
		NestedFamORAssem = 0x00000007,
		LayoutMask = 0x00000018,
		AutoLayout = 0x00000000,
		SequentialLayout = 0x00000008,
		ExplicitLayout = 0x00000010,
		ClassSemanticsMask = 0x00000020,
		Class = 0x00000000,
		Interface = 0x00000020,
		Abstract = 0x00000080,
		Sealed = 0x00000100,
		SpecialName = 0x00000400,
		Import = 0x00001000,
		Serializable = 0x00002000,
		StringFormatMask = 0x00030000,
		AnsiClass = 0x00000000,
		UnicodeClass = 0x00010000,
		AutoClass = 0x00020000,
		BeforeFieldInit = 0x00100000,
		RTSpecialName = 0x00000800,
		HasSecurity = 0x00040000,
		Forwarder = 0x00200000,
	}
}