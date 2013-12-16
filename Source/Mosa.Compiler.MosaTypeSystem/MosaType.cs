/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Collections.Generic;

namespace Mosa.Compiler.MosaTypeSystem
{
	public class MosaType
	{
		public MosaAssembly Assembly { get; internal set; }

		public string Name { get; internal set; }

		public string FullName { get; internal set; }

		public string Namespace { get; internal set; }

		public MosaType BaseType { get; internal set; }

		public MosaType EnclosingType { get; internal set; }

		public MosaType GenericBaseType { get; internal set; }

		public bool IsValueType { get; internal set; }

		public bool IsDelegate { get; internal set; }

		public bool IsEnum { get; internal set; }

		public bool IsInterface { get; internal set; }

		public bool IsNested { get; internal set; }

		public bool IsExplicitLayout { get; internal set; }

		public bool IsModule { get; internal set; }

		public int Size { get; internal set; }

		public int PackingSize { get; internal set; }

		public IList<MosaMethod> Methods { get; internal set; }

		public IList<MosaField> Fields { get; internal set; }

		public IList<MosaType> Interfaces { get; internal set; }

		public IList<MosaAttribute> CustomAttributes { get; internal set; }

		public bool IsUnsignedByte { get; internal set; }

		public bool IsSignedByte { get; internal set; }

		public bool IsUnsignedShort { get; internal set; }

		public bool IsSignedShort { get; internal set; }

		public bool IsUnsignedInt { get; internal set; }

		public bool IsSignedInt { get; internal set; }

		public bool IsUnsignedLong { get; internal set; }

		public bool IsSignedLong { get; internal set; }

		public bool IsByte { get { return IsUnsignedByte || IsSignedByte; } }

		public bool IsShort { get { return IsUnsignedShort || IsSignedShort; } }

		public bool IsChar { get; internal set; }

		public bool IsInt { get { return IsUnsignedInt || IsSignedInt; } }

		public bool IsLong { get { return IsUnsignedLong || IsSignedLong; } }

		public bool IsBoolean { get; internal set; }

		public bool IsPointer { get; internal set; }

		public bool IsObject { get; internal set; }

		public bool IsFloatingPoint { get { return IsDouble || IsSingle; } }

		public bool IsDouble { get; internal set; }

		public bool IsSingle { get; internal set; }

		public bool IsInteger { get; internal set; }

		public bool IsSigned { get; internal set; }

		public bool IsUnsigned { get; internal set; }

		public bool IsVarFlag { get; internal set; }

		public bool IsMVarFlag { get; internal set; }

		public int VarOrMVarIndex { get; internal set; }

		public MosaType ElementType { get; internal set; }

		public bool IsManagedPointerType { get; internal set; }

		public bool IsUnmanagedPointerType { get; internal set; }

		public bool IsArrayType { get; internal set; }

		public bool IsVoid { get; internal set; }

		public bool IsBuiltInType { get; internal set; }

		public IList<MosaGenericParameter> GenericParameters { get; internal set; }

		public bool IsGeneric { get { return GenericParameters.Count != 0; } }

		public IList<MosaType> GenericParameterTypes { get; internal set; }
		
		public IDictionary<MosaMethod, MosaMethod> InheritanceOveride { get; internal set;}

		public MosaType(MosaAssembly assembly)
		{
			Assembly = assembly;

			IsUnsignedByte = false;
			IsSignedByte = false;
			IsUnsignedShort = false;
			IsSignedShort = false;
			IsUnsignedInt = false;
			IsSignedInt = false;
			IsUnsignedLong = false;
			IsSignedLong = false;
			IsChar = false;
			IsBoolean = false;
			IsPointer = false;
			IsObject = true;
			IsDouble = false;
			IsSingle = false;
			IsInteger = false;
			IsSigned = false;
			IsUnsigned = false;
			IsVarFlag = false;
			IsMVarFlag = false;
			IsManagedPointerType = false;
			IsUnmanagedPointerType = false;
			IsArrayType = false;
			IsBuiltInType = false;
			IsModule = false;
			IsVoid = false;

			Methods = new List<MosaMethod>();
			Fields = new List<MosaField>();
			Interfaces = new List<MosaType>();
			GenericParameters = new List<MosaGenericParameter>();
			CustomAttributes = new List<MosaAttribute>();
			GenericParameterTypes = new List<MosaType>();
			InheritanceOveride = new Dictionary<MosaMethod, MosaMethod>();
		}

		public void SetFlags()
		{
			IsSignedByte = FullName == "System.SByte";
			IsSignedShort = FullName == "System.Int16";
			IsSignedInt = FullName == "System.Int32";
			IsSignedLong = FullName == "System.Int64";
			IsUnsignedByte = FullName == "System.Byte";
			IsUnsignedShort = FullName == "System.UInt16";
			IsUnsignedInt = FullName == "System.UInt32";
			IsUnsignedLong = FullName == "System.UInt64";
			IsChar = FullName == "System.Char";
			IsBoolean = FullName == "System.Boolean";
			IsSingle = FullName == "System.Single";
			IsDouble = FullName == "System.Double";
			IsPointer = FullName == "System.Ptr";

			IsValueType = BaseType != null && (BaseType.IsValueType || FullName == "System.ValueType");
			IsDelegate = BaseType != null && (BaseType.IsDelegate || FullName == "System.Delegate");
			IsEnum = BaseType != null && (BaseType.IsEnum || FullName == "System.Enum");
			IsObject = BaseType != null && (BaseType.IsEnum || FullName == "System.Object");

			IsModule = FullName == "<Module>";
			IsVoid = FullName == "System.Void";
			//"System.IntPtr"
			//"System.UIntPtr"
			//"System.TypedByRef"
		}

		public override string ToString()
		{
			return FullName;
		}

		public bool Matches(MosaType type)
		{
			return type == this;
		}
	}
}