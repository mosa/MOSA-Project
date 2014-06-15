/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

namespace Mosa.Compiler.Common
{
	public struct Metadata
	{
		public const string AssemblyListTable = "<$>AssemblyListTable";
		public const string MethodLookupTable = "<$>MethodLookupTable";
		public const string NameString = "$Name";

		public const string AssemblyDefinition = "$AssemblyDef";
		public const string TypeDefinition = "$TypeDef";
		public const string MethodDefinition = "$MethodDef";
		public const string FieldDefinition = "$FieldDef";

		public const string CustomAttributeListTable = "$CustomAttributeListTable";
		public const string CustomAttributeTable = "$CustomAttributeTable";
		public const string CustomAttributeArgument = "$CustomAttributeArgument";

		public const string InterfaceMethodTable = "$IMethodTable$";

		public const string InterfaceSlotTable = "$InterfaceSlotTable";
		public const string InterfaceBitmap = "$InterfaceBitmap";
	}
}