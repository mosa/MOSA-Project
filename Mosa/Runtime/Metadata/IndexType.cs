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

namespace Mosa.Runtime.Metadata {
    /// <summary>
    /// 
    /// </summary>
	public enum IndexType {
		/// <summary>
		/// Index into TypeDef, TypeRef or TypeSpec tables.
		/// </summary>
		TypeDefOrRef = 0x00,

		/// <summary>
		/// Index into Field, Param or StackFrameIndex tables.
		/// </summary>
		HasConstant = 0x01,

		/// <summary>
		/// Index into MethodDef, Field, TypeRef, TypeDef, Param, InterfaceImpl, MemberRef, Module, Permission, StackFrameIndex, Event, StandAloneSig, ModuleRef, TypeSpec, Assembly, AssemblyRef, File, ExportedType or ManifestResource tables.
		/// </summary>
		HasCustomAttribute = 0x02,

		/// <summary>
		/// Index into Field or Param tables.
		/// </summary>
		HasFieldMarshal = 0x03,

		/// <summary>
		/// Index into TypeDef, MethodDef or Assembly tables.
		/// </summary>
		HasDeclSecurity = 0x04,

		/// <summary>
		/// Index into TypeDef, TypeRef, ModuleRef, MethodDef or TypeSpec tables.
		/// </summary>
		MemberRefParent = 0x05,

		/// <summary>
		/// Index into Event or StackFrameIndex tables.
		/// </summary>
		HasSemantics = 0x06,

		/// <summary>
		/// Index into MethodDef or MethodRef tables.
		/// </summary>
		MethodDefOrRef = 0x07,

		/// <summary>
		/// Index into Field or MethodDef tables.
		/// </summary>
		MemberForwarded = 0x08,

		/// <summary>
		/// Index into File, AssemblyRef or ExportedType tables.
		/// </summary>
		Implementation = 0x09,

		/// <summary>
		/// Index into MethodDef or MemberRef tables.
		/// </summary>
		CustomAttributeType = 0x0A,

		/// <summary>
		/// Index into Module, ModuleRef, AssemblyRef or TypeRef tables.
		/// </summary>
		ResolutionScope = 0x0B,

		/// <summary>
		/// Index into TypeDef or MethodDef tables.
		/// </summary>
		TypeOrMethodDef = 0x0C,

		/// <summary>
		/// Index into the string heap.
		/// </summary>
		StringHeap = 0x0D,

		/// <summary>
		/// Index into the guid heap.
		/// </summary>
		GuidHeap = 0x0E,

		/// <summary>
		/// Index into the blob heap.
		/// </summary>
		BlobHeap = 0x0F,

		/// <summary>
		/// Maximum index number.
		/// </summary>
		IndexCount = 0x10,

		/// <summary>
		/// Last table index number.
		/// </summary>
		CodedIndexCount = 0x0D
	}
}
