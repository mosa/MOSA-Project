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

		public bool IsStruct { get; internal set; }

		public bool IsUnsignedByte { get; internal set; }

		public bool IsSignedByte { get; internal set; }

		public bool IsUnsignedShort { get; internal set; }

		public bool IsSignedShort { get; internal set; }

		public bool IsUnsignedInt { get; internal set; }

		public bool IsSignedInt { get; internal set; }

		public bool IsUnsignedLong { get; internal set; }

		public bool IsSignedLong { get; internal set; }

		public bool IsChar { get; internal set; }

		public bool IsBoolean { get; internal set; }

		public bool IsObject { get; internal set; }

		public bool IsString { get; internal set; }

		public bool IsDouble { get; internal set; }

		public bool IsSingle { get; internal set; }

		public bool IsPointer { get { return IsUnmanagedPointerType || IsManagedPointerType; } }

		public bool IsByte { get { return IsUnsignedByte || IsSignedByte; } }

		public bool IsShort { get { return IsUnsignedShort || IsSignedShort; } }

		public bool IsInt { get { return IsUnsignedInt || IsSignedInt; } }

		public bool IsLong { get { return IsUnsignedLong || IsSignedLong; } }

		public bool IsFloatingPoint { get { return IsDouble || IsSingle; } }

		public bool IsInteger { get { return IsSigned || IsUnsigned; } }

		public bool IsSigned { get { return IsSignedByte || IsSignedShort || IsSignedInt || IsSignedLong || IsNativeSignedInteger; } }

		public bool IsUnsigned { get { return IsUnsignedByte || IsUnsignedShort || IsUnsignedInt || IsUnsignedLong || IsNativeUnsignedInteger; } }

		public bool IsVarFlag { get; internal set; }

		public bool IsMVarFlag { get; internal set; }

		public int VarOrMVarIndex { get; internal set; }

		public MosaType ElementType { get; internal set; }

		public bool HasElement { get { return ElementType != null; } }

		public bool IsManagedPointerType { get; internal set; }

		public bool IsUnmanagedPointerType { get; internal set; }

		public bool IsArray { get; internal set; }

		public bool IsVoid { get; internal set; }

		public bool IsBuiltInType { get; internal set; }

		public IList<MosaGenericParameter> GenericParameters { get; internal set; }

		public bool IsBaseGeneric { get; internal set; }

		public List<MosaType> GenericArguments { get; internal set; }

		public IDictionary<MosaMethod, MosaMethod> InheritanceOveride { get; internal set; }

		public bool IsLinkerGenerated { get; internal set; }

		public bool IsNativeSignedInteger { get; internal set; }

		public bool IsNativeUnsignedInteger { get; internal set; }

		public bool IsNativeInteger { get { return IsNativeSignedInteger || IsNativeUnsignedInteger; } }

		internal int? FixedSize { get; set; }

		public bool IsOpenGenericType { get; internal set; }

		internal bool AreMethodsAssigned { get; set; }

		internal bool AreFieldsAssigned { get; set; }

		internal bool AreInterfacesAssigned { get; set; }

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
			IsObject = false;
			IsDouble = false;
			IsSingle = false;
			IsVarFlag = false;
			IsMVarFlag = false;
			IsManagedPointerType = false;
			IsUnmanagedPointerType = false;
			IsArray = false;
			IsBuiltInType = false;
			IsModule = false;
			IsVoid = false;
			IsLinkerGenerated = false;
			IsString = false;
			IsNativeSignedInteger = false;
			IsNativeUnsignedInteger = false;
			IsBaseGeneric = false;

			Methods = new List<MosaMethod>();
			Fields = new List<MosaField>();
			Interfaces = new List<MosaType>();
			GenericParameters = new List<MosaGenericParameter>();
			CustomAttributes = new List<MosaAttribute>();
			GenericArguments = new List<MosaType>();
			InheritanceOveride = new Dictionary<MosaMethod, MosaMethod>();

			AreMethodsAssigned = false;
			AreFieldsAssigned = false;
			AreInterfacesAssigned = false;
		}

		public void SetFlags()
		{
			IsObject = (BaseType != null && BaseType.IsObject) || FullName == "System.Object";
			IsValueType = FullName == "System.ValueType";
			IsStruct = (BaseType != null && BaseType.IsValueType);
			IsDelegate = (BaseType != null && BaseType.IsDelegate) || FullName == "System.Delegate";
			IsEnum = (BaseType != null && BaseType.IsEnum) || FullName == "System.Enum";
			IsModule = Name == "<Module>" && Namespace == string.Empty;
		}

		public override string ToString()
		{
			return FullName;
		}

		public bool Matches(MosaType type)
		{
			return type == this;
		}

		internal static bool IsOpenGeneric(MosaType type)
		{
			if (type.IsVarFlag || type.IsMVarFlag)
				return true;

			if (!type.HasElement)
				return false;

			return IsOpenGeneric(type.ElementType);
		}

		internal void SetOpenGeneric()
		{
			IsOpenGenericType = IsOpenGeneric(this);

			foreach (var param in GenericArguments)
			{
				IsOpenGenericType = MosaType.IsOpenGeneric(param);

				if (IsOpenGenericType)
					return;
			}

			IsOpenGenericType = false;
		}
	}
}