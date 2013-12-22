/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Common;
using Mosa.Compiler.Metadata;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Compiler.MosaTypeSystem
{
	public class MosaTypeResolver
	{
		public IList<MosaAssembly> Assemblies { get; private set; }

		public IList<MosaType> Types { get; private set; }

		public IList<MosaMethod> Methods { get; private set; }

		public MosaAssembly InternalAssembly { get; private set; }

		public MosaAssembly InternalGenericsAssembly { get; private set; }

		public BuiltInTypes BuiltIn { get; internal set; }

		private Dictionary<string, MosaAssembly> assemblyLookup = new Dictionary<string, MosaAssembly>();
		private Dictionary<MosaAssembly, Dictionary<Token, MosaType>> typeLookup = new Dictionary<MosaAssembly, Dictionary<Token, MosaType>>();
		private Dictionary<MosaAssembly, Dictionary<Token, MosaMethod>> methodLookup = new Dictionary<MosaAssembly, Dictionary<Token, MosaMethod>>();
		private Dictionary<MosaAssembly, Dictionary<Token, MosaField>> fieldLookup = new Dictionary<MosaAssembly, Dictionary<Token, MosaField>>();

		private MosaType[] var = new MosaType[255];
		private MosaType[] mvar = new MosaType[255];

		public MosaTypeResolver()
		{
			Types = new List<MosaType>();
			Methods = new List<MosaMethod>();
			BuiltIn = new BuiltInTypes();
			Assemblies = new List<MosaAssembly>();

			SetupInternalAssembly();
			SetupInternalGenericsAssembly();
		}

		private void SetupInternalGenericsAssembly()
		{
			InternalGenericsAssembly = new MosaAssembly("@InternalGenerics");
			AddAssembly(InternalGenericsAssembly);
		}

		private void SetupInternalAssembly()
		{
			InternalAssembly = new MosaAssembly("@Internal");
			AddAssembly(InternalAssembly);

			BuiltIn.Void = CreateAddBuiltInType(CilElementType.Void); BuiltIn.Void.IsVoid = true;
			BuiltIn.Boolean = CreateAddBuiltInType(CilElementType.Boolean); BuiltIn.Void.IsBoolean = true;
			BuiltIn.Char = CreateAddBuiltInType(CilElementType.Char); BuiltIn.Void.IsChar = true;
			BuiltIn.I1 = CreateAddBuiltInType(CilElementType.I1); BuiltIn.Void.IsSignedByte = true;
			BuiltIn.U1 = CreateAddBuiltInType(CilElementType.U1); BuiltIn.Void.IsUnsignedByte = true;
			BuiltIn.I2 = CreateAddBuiltInType(CilElementType.I2); BuiltIn.Void.IsSignedShort = true;
			BuiltIn.U2 = CreateAddBuiltInType(CilElementType.U2); BuiltIn.Void.IsUnsignedShort = true;
			BuiltIn.I4 = CreateAddBuiltInType(CilElementType.I4); BuiltIn.Void.IsSignedInt = true;
			BuiltIn.U4 = CreateAddBuiltInType(CilElementType.U4); BuiltIn.Void.IsUnsignedInt = true;
			BuiltIn.I8 = CreateAddBuiltInType(CilElementType.I8); BuiltIn.Void.IsSignedLong = true;
			BuiltIn.U8 = CreateAddBuiltInType(CilElementType.U8); BuiltIn.Void.IsUnsignedLong= true;
			BuiltIn.R4 = CreateAddBuiltInType(CilElementType.R4); BuiltIn.Void.IsSingle = true;
			BuiltIn.R8 = CreateAddBuiltInType(CilElementType.R8); BuiltIn.Void.IsDouble = true;
			BuiltIn.String = CreateAddBuiltInType(CilElementType.String); BuiltIn.Void.IsString = true;
			BuiltIn.Object = CreateAddBuiltInType(CilElementType.Object); BuiltIn.Void.IsObject = true;
			BuiltIn.I = CreateAddBuiltInType(CilElementType.I);
			BuiltIn.U = CreateAddBuiltInType(CilElementType.U);
			BuiltIn.TypedByRef = CreateAddBuiltInType(CilElementType.TypedByRef); 
			BuiltIn.Ptr = CreateAddBuiltInType(CilElementType.Ptr); BuiltIn.Void.IsPointer = true;
		}

		private MosaType CreateBuiltInType(CilElementType cilElementType)
		{
			MosaType type = new MosaType(InternalAssembly);
			type.Name = "@" + cilElementType.ToString();
			type.FullName = "@" + cilElementType.ToString();
			type.IsBuiltInType = true;

			return type;
		}

		private MosaType CreateAddBuiltInType(CilElementType cilElementType)
		{
			var type = CreateBuiltInType(cilElementType);

			AddType(cilElementType, type);

			return type;
		}

		public void AddAssembly(MosaAssembly assembly)
		{
			assemblyLookup.Add(assembly.Name, assembly);
			typeLookup.Add(assembly, new Dictionary<Token, MosaType>());
			methodLookup.Add(assembly, new Dictionary<Token, MosaMethod>());
			fieldLookup.Add(assembly, new Dictionary<Token, MosaField>());
		}

		public MosaAssembly GetAssemblyByName(string name)
		{
			MosaAssembly assembly;

			if (assemblyLookup.TryGetValue(name, out assembly))
			{
				return assembly;
			}

			throw new InvalidCompilerException();
		}

		public void AddType(MosaAssembly assembly, Token token, MosaType type)
		{
			Types.Add(type);
			typeLookup[assembly].Add(token, type);
		}

		public void AddType(CilElementType elementType, MosaType type)
		{
			AddType(InternalAssembly, new Token((uint)elementType), type);
		}

		public MosaType GetTypeByElementType(CilElementType elementType)
		{
			return GetTypeByToken(InternalAssembly, new Token((uint)elementType));
		}

		public MosaType GetTypeByElementType(CilElementType? elementType)
		{
			return GetTypeByToken(InternalAssembly, new Token((uint)elementType.Value));
		}

		public MosaType GetTypeByToken(MosaAssembly assembly, Token token)
		{
			return typeLookup[assembly][token];
		}

		public MosaType GetTypeByName(MosaAssembly assembly, string @namespace, string name)
		{
			foreach (var type in typeLookup[assembly].Values)
			{
				if (type.Name.Equals(name) && type.Namespace.Equals(@namespace))
					return type;
			}

			throw new AssemblyLoadException();
		}

		public MosaType GetTypeByName(MosaAssembly assembly, string fullname)
		{
			int dot = fullname.LastIndexOf(".");

			if (dot >= 0)
			{
				return GetTypeByName(assembly, fullname.Substring(0, dot), fullname.Substring(dot + 1));
			}

			throw new AssemblyLoadException();
		}

		public bool CheckTypeExists(MosaAssembly assembly, Token token)
		{
			return typeLookup[assembly].ContainsKey(token);
		}

		public void AddMethod(MosaAssembly assembly, Token token, MosaMethod method)
		{
			Methods.Add(method);
			methodLookup[assembly].Add(token, method);
		}

		public MosaMethod GetMethodByToken(MosaAssembly assembly, Token token)
		{
			return methodLookup[assembly][token];
		}

		public bool CheckFieldExists(MosaAssembly assembly, Token token)
		{
			return fieldLookup[assembly].ContainsKey(token);
		}

		public void AddField(MosaAssembly assembly, Token token, MosaField field)
		{
			fieldLookup[assembly].Add(token, field);
		}

		public MosaField GetFieldByToken(MosaAssembly assembly, Token token)
		{
			return fieldLookup[assembly][token];
		}

		public MosaType GetVarType(int index)
		{
			MosaType type = var[index];

			if (type == null)
			{
				type = new MosaType(InternalAssembly);
				type.Name = "VAR#" + index.ToString();
				type.FullName = type.Name;
				type.IsVarFlag = true;
				type.VarOrMVarIndex = index;
				var[index] = type;
			}

			return type;
		}

		public MosaType GetMVarType(int index)
		{
			MosaType type = var[index];

			if (type == null)
			{
				type = new MosaType(InternalAssembly);
				type.Name = "MVAR#" + index.ToString();
				type.FullName = type.Name;
				type.IsMVarFlag = true;
				type.VarOrMVarIndex = index;
				var[index] = type;
			}

			return type;
		}

		private Dictionary<MosaType, MosaType> unmanagedPointerTypes = new Dictionary<MosaType, MosaType>();

		public MosaType GetUnmanagedPointerType(MosaType element)
		{
			MosaType type;

			if (unmanagedPointerTypes.TryGetValue(element, out type))
				return type;

			type = new MosaType(InternalAssembly);
			type.FullName = element.FullName + "*";
			type.Name = element.Name + "*";
			type.IsUnmanagedPointerType = true;
			type.ElementType = element;
			type.SetFlags();

			unmanagedPointerTypes.Add(element, type);

			return type;
		}

		private Dictionary<MosaType, MosaType> managedPointerTypes = new Dictionary<MosaType, MosaType>();

		public MosaType GetManagedPointerType(MosaType element)
		{
			MosaType type;

			if (managedPointerTypes.TryGetValue(element, out type))
				return type;

			type = new MosaType(InternalAssembly);
			type.FullName = element.FullName + "&";
			type.Name = element.Name + "&";
			type.IsManagedPointerType = true;
			type.ElementType = element;
			type.SetFlags();

			managedPointerTypes.Add(element, type);

			return type;
		}

		private Dictionary<MosaType, MosaType> arrayTypes = new Dictionary<MosaType, MosaType>();

		public MosaType GetArrayType(MosaType element)
		{
			MosaType type;

			if (arrayTypes.TryGetValue(element, out type))
				return type;

			type = new MosaType(InternalAssembly);
			type.FullName = element.FullName + "[]";
			type.Name = element.Name + "[]";
			type.IsArrayType = true;
			type.ElementType = element;
			type.SetFlags();

			arrayTypes.Add(element, type);

			return type;
		}

		private Dictionary<MosaType, List<KeyValuePair<List<MosaType>, MosaType>>> generics = new Dictionary<MosaType, List<KeyValuePair<List<MosaType>, MosaType>>>();

		public MosaType FindGeneric(MosaType genericBaseType, List<MosaType> genericTypes)
		{
			List<KeyValuePair<List<MosaType>, MosaType>> genericVariations = null;

			if (!generics.TryGetValue(genericBaseType, out genericVariations))
				return null;

			foreach (var genericPair in genericVariations)
			{
				var types = genericPair.Key;

				if (types.Count != genericTypes.Count)
					continue;

				bool match = true;
				for (int i = 0; i < types.Count; i++)
				{
					if (!types[i].Matches(genericTypes[i]))
					{
						match = false;
						break;
					}
				}

				if (!match)
					continue;

				return genericPair.Value;
			}

			return null;
		}

		public void StoreGeneric(MosaType genericBaseType, List<MosaType> genericTypes, MosaType genericType)
		{
			List<KeyValuePair<List<MosaType>, MosaType>> genericVariations;

			if (!generics.TryGetValue(genericBaseType, out genericVariations))
			{
				genericVariations = new List<KeyValuePair<List<MosaType>, MosaType>>();
				generics.Add(genericBaseType, genericVariations);
			}

			genericVariations.Add(new KeyValuePair<List<MosaType>, MosaType>(genericTypes, genericType));
		}

		public MosaType ResolveGenericType(MosaType genericBaseType, List<MosaType> genericTypes)
		{
			var genericType = FindGeneric(genericBaseType, genericTypes);

			if (genericType != null)
				return genericType;

			genericType = CreateGenericType(genericBaseType, genericTypes);

			StoreGeneric(genericBaseType, genericTypes, genericType);

			return genericType;
		}

		public MosaType CreateGenericType(MosaType genericBaseType, List<MosaType> genericTypes)
		{
			var generic = new MosaType(InternalGenericsAssembly);

			generic.GenericBaseType = genericBaseType;
			generic.GenericParameterTypes = genericTypes;
			generic.Name = genericBaseType.Name;
			generic.FullName = genericBaseType.FullName;
			generic.Namespace = genericBaseType.Namespace;
			generic.BaseType = genericBaseType.BaseType;
			generic.EnclosingType = genericBaseType.EnclosingType;
			generic.IsUnsignedByte = genericBaseType.IsUnsignedByte;
			generic.IsSignedByte = genericBaseType.IsSignedByte;
			generic.IsUnsignedShort = genericBaseType.IsUnsignedShort;
			generic.IsSignedShort = genericBaseType.IsSignedShort;
			generic.IsUnsignedInt = genericBaseType.IsUnsignedInt;
			generic.IsSignedInt = genericBaseType.IsSignedInt;
			generic.IsUnsignedLong = genericBaseType.IsUnsignedLong;
			generic.IsSignedLong = genericBaseType.IsSignedLong;
			generic.IsChar = genericBaseType.IsChar;
			generic.IsBoolean = genericBaseType.IsBoolean;
			generic.IsPointer = genericBaseType.IsPointer;
			generic.IsObject = genericBaseType.IsObject;
			generic.IsDouble = genericBaseType.IsDouble;
			generic.IsSingle = genericBaseType.IsSingle;
			generic.IsInteger = genericBaseType.IsInteger;
			generic.IsSigned = genericBaseType.IsSigned;
			generic.IsUnsigned = genericBaseType.IsUnsigned;
			generic.IsVarFlag = genericBaseType.IsVarFlag;
			generic.IsMVarFlag = genericBaseType.IsMVarFlag;
			generic.IsManagedPointerType = genericBaseType.IsManagedPointerType;
			generic.IsUnmanagedPointerType = genericBaseType.IsUnmanagedPointerType;
			generic.IsArrayType = genericBaseType.IsArrayType;
			generic.IsBuiltInType = genericBaseType.IsBuiltInType;
			generic.Size = genericBaseType.Size;
			generic.PackingSize = genericBaseType.PackingSize;
			generic.VarOrMVarIndex = genericBaseType.VarOrMVarIndex;
			generic.ElementType = genericBaseType.ElementType;

			var genericTypeNames = new StringBuilder();

			foreach (var genericType in genericTypes)
			{
				genericTypeNames.Append(genericType.FullName);
				genericTypeNames.Append(", ");
			}

			genericTypeNames.Length = genericTypeNames.Length - 2;
			generic.FullName = generic.FullName + '<' + genericTypeNames.ToString() + '>';

			foreach (var m in genericBaseType.Methods)
			{
				var cloneMethod = ResolveGenericMethod(m, generic);
				generic.Methods.Add(cloneMethod);
			}

			foreach (var f in genericBaseType.Fields)
			{
				var cloneField = ResolveGenericField(f, generic);
				generic.Fields.Add(cloneField);
			}

			foreach (var m in genericBaseType.Interfaces)
			{
				if (m.GenericParameterTypes.Count == 0)
				{
					generic.Interfaces.Add(m);
				}
				else
				{
					var genericInterface = ResolveGenericType(m.GenericBaseType, genericTypes);
					generic.Interfaces.Add(genericInterface);
				}
			}

			generic.SetFlags();

			return generic;
		}

		private static MosaMethod ResolveGenericMethod(MosaMethod method, MosaType declaringType)
		{
			var generic = new MosaMethod();

			generic.DeclaringType = declaringType;
			generic.Name = method.Name;
			generic.MethodName = method.MethodName;
			generic.IsAbstract = method.IsAbstract;
			generic.IsGeneric = method.IsGeneric;
			generic.IsStatic = method.IsStatic;
			generic.HasThis = method.HasThis;
			generic.HasExplicitThis = method.HasExplicitThis;
			generic.ReturnType = method.ReturnType;
			generic.IsInternal = method.IsInternal;
			generic.IsNoInlining = method.IsNoInlining;
			generic.IsSpecialName = method.IsSpecialName;
			generic.IsRTSpecialName = method.IsRTSpecialName;
			generic.IsVirtual = method.IsVirtual;
			generic.IsPInvokeImpl = method.IsPInvokeImpl;
			generic.IsNewSlot = method.IsNewSlot;
			generic.IsFinal = method.IsFinal;
			generic.Rva = method.Rva;
			generic.Code = method.Code;
			generic.CodeAssembly = method.CodeAssembly;
			generic.IsCILGenerated = method.IsCILGenerated;

			foreach (var p in method.Parameters)
			{
				var cloneParameter = ResolveGenericParameter(p, declaringType);
				generic.Parameters.Add(cloneParameter);
			}

			foreach (var a in method.CustomAttributes)
			{
				generic.CustomAttributes.Add(a);
			}

			if (method.ReturnType.IsVarFlag || method.ReturnType.IsMVarFlag)
			{
				generic.ReturnType = declaringType.GenericParameterTypes[method.ReturnType.VarOrMVarIndex];
			}

			generic.SetName();

			return generic;
		}

		private static MosaField ResolveGenericField(MosaField field, MosaType declaringType)
		{
			var generic = new MosaField();

			generic.DeclaringType = declaringType;
			generic.FieldType = field.FieldType;
			generic.Name = field.Name;
			generic.FullName = field.FullName;
			generic.CustomAttributes = field.CustomAttributes;
			generic.IsLiteralField = field.IsLiteralField;
			generic.IsStaticField = field.IsStaticField;
			generic.RVA = generic.RVA;

			if (field.FieldType.IsVarFlag || field.FieldType.IsMVarFlag)
			{
				generic.FieldType = declaringType.GenericParameterTypes[field.FieldType.VarOrMVarIndex];
			}

			return generic;
		}

		private static MosaParameter ResolveGenericParameter(MosaParameter parameter, MosaType declaringType)
		{
			if (!(parameter.Type.IsVarFlag || parameter.Type.IsMVarFlag))
				return parameter;

			var generic = new MosaParameter();

			generic.Type = declaringType.GenericParameterTypes[parameter.Type.VarOrMVarIndex];
			generic.Position = parameter.Position;
			generic.Name = parameter.Name;
			generic.IsIn = parameter.IsIn;
			generic.IsOut = parameter.IsOut;

			return generic;
		}
	}
}