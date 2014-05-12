using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mosa.Compiler.Common
{
	public struct Metadata
	{
		public const string AssemblyListTable = "$_$assemblylisttable";
		public const string MethodLookupTable = "$_$methodlookuptable";

		public const string AssemblyDefinition = "$assemblydef";
		public const string AssemblyName = "$assemblyname";

		public const string TypeDefinition = "$typedef";
		public const string TypeName = "$typename";

		public const string FieldDefinition = "$fielddef";
		public const string FieldName = "$fieldname";

		public const string MethodTable = "$methodtable";
		public const string InterfaceMethodTable = "$i_methodtable$";

		public const string InterfaceTable = "$interfacetable";
		public const string InterfaceBitmap = "$interfacebitmap";
	}
}
