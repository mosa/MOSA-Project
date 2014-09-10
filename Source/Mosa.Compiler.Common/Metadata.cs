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
		public const string AssembliesTable = "<$>AssembliesTable";
		public const string MethodLookupTable = "<$>MethodLookupTable";
		public const string NameString = "$Name";

		public const string AssemblyDefinition = "$AssemblyDef";
		public const string TypeDefinition = "$TypeDef";
		public const string MethodDefinition = "$MethodDef";
		public const string FieldDefinition = "$FieldDef";
		public const string FieldsTable = "$FieldsTable";
		public const string PropertyDefinition = "$PropertyDef";
		public const string PropertiesTable = "$PropertiesTable";

		public const string CustomAttributesTable = "$CustomAttributesTable";
		public const string CustomAttribute = "$CustomAttribute";
		public const string CustomAttributeArgument = "$CustomAttributeArgument";

		public const string InterfaceMethodTable = "$IMethodTable$";

		public const string InterfaceSlotTable = "$InterfaceSlotTable";
		public const string InterfaceBitmap = "$InterfaceBitmap";

		public const string ProtectedRegionTable = "$ProtectedRegionTable";
	}
}
