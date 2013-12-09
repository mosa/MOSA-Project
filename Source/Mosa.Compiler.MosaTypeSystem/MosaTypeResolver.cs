using Mosa.Compiler.Common;
using Mosa.Compiler.Metadata;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Compiler.MosaTypeSystem
{
	public class MosaTypeResolver
	{
		public List<MosaType> Types { get; private set; }

		public List<MosaMethod> Methods { get; private set; }

		private Dictionary<string, MosaAssembly> assemblyLookup = new Dictionary<string, MosaAssembly>();
		private Dictionary<MosaAssembly, Dictionary<Token, MosaType>> typeLookup = new Dictionary<MosaAssembly, Dictionary<Token, MosaType>>();
		private Dictionary<MosaAssembly, Dictionary<Token, MosaMethod>> methodLookup = new Dictionary<MosaAssembly, Dictionary<Token, MosaMethod>>();
		private Dictionary<MosaAssembly, Dictionary<Token, MosaField>> fieldLookup = new Dictionary<MosaAssembly, Dictionary<Token, MosaField>>();

		private MosaType[] var = new MosaType[255];
		private MosaType[] mvar = new MosaType[255];

		public MosaAssembly InternalAssembly { get; private set; }

		public MosaAssembly InternalGenericsAssembly { get; private set; }

		public MosaTypeResolver()
		{
			Types = new List<MosaType>();
			Methods = new List<MosaMethod>();

			SetupInternalAssembly();
			SetupInternalGenericsAssembly();
		}

		private void SetupInternalAssembly()
		{
			InternalAssembly = new MosaAssembly("@Internal");
			AddAssembly(InternalAssembly);

			CreateAddBuiltInType(InternalAssembly, CilElementType.Void);
			CreateAddBuiltInType(InternalAssembly, CilElementType.Boolean);
			CreateAddBuiltInType(InternalAssembly, CilElementType.Char);
			CreateAddBuiltInType(InternalAssembly, CilElementType.I1);
			CreateAddBuiltInType(InternalAssembly, CilElementType.U1);
			CreateAddBuiltInType(InternalAssembly, CilElementType.I2);
			CreateAddBuiltInType(InternalAssembly, CilElementType.U2);
			CreateAddBuiltInType(InternalAssembly, CilElementType.I4);
			CreateAddBuiltInType(InternalAssembly, CilElementType.U4);
			CreateAddBuiltInType(InternalAssembly, CilElementType.I8);
			CreateAddBuiltInType(InternalAssembly, CilElementType.U8);
			CreateAddBuiltInType(InternalAssembly, CilElementType.R4);
			CreateAddBuiltInType(InternalAssembly, CilElementType.R8);
			CreateAddBuiltInType(InternalAssembly, CilElementType.String);
			CreateAddBuiltInType(InternalAssembly, CilElementType.Object);
			CreateAddBuiltInType(InternalAssembly, CilElementType.I);
			CreateAddBuiltInType(InternalAssembly, CilElementType.U);
			CreateAddBuiltInType(InternalAssembly, CilElementType.TypedByRef);
			CreateAddBuiltInType(InternalAssembly, CilElementType.Ptr);
		}

		private void SetupInternalGenericsAssembly()
		{
			InternalGenericsAssembly = new MosaAssembly("@InternalGenerics");
			AddAssembly(InternalGenericsAssembly);
		}

		private MosaType CreateBuiltInType(CilElementType cilElementType)
		{
			MosaType type = new MosaType(InternalAssembly);
			type.Name = "@" + cilElementType.ToString();
			type.FullName = "@" + cilElementType.ToString();
			type.IsBuiltInType = true;

			return type;
		}

		private void CreateAddBuiltInType(MosaAssembly assembly, CilElementType cilElementType)
		{
			var type = CreateBuiltInType(cilElementType);

			AddType(assembly, new Token((uint)cilElementType), type);
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
			var clone = new MosaType(InternalGenericsAssembly);

			clone.GenericBaseType = genericBaseType;
			clone.GenericParameterTypes = genericTypes;
			clone.Name = genericBaseType.Name;
			clone.FullName = genericBaseType.FullName;
			clone.Namespace = genericBaseType.Namespace;
			clone.BaseType = genericBaseType.BaseType;
			clone.EnclosingType = genericBaseType.EnclosingType;
			clone.IsUnsignedByte = genericBaseType.IsUnsignedByte;
			clone.IsSignedByte = genericBaseType.IsSignedByte;
			clone.IsUnsignedShort = genericBaseType.IsUnsignedShort;
			clone.IsSignedShort = genericBaseType.IsSignedShort;
			clone.IsUnsignedInt = genericBaseType.IsUnsignedInt;
			clone.IsSignedInt = genericBaseType.IsSignedInt;
			clone.IsUnsignedLong = genericBaseType.IsUnsignedLong;
			clone.IsSignedLong = genericBaseType.IsSignedLong;
			clone.IsChar = genericBaseType.IsChar;
			clone.IsBoolean = genericBaseType.IsBoolean;
			clone.IsPointer = genericBaseType.IsPointer;
			clone.IsObject = genericBaseType.IsObject;
			clone.IsDouble = genericBaseType.IsDouble;
			clone.IsSingle = genericBaseType.IsSingle;
			clone.IsInteger = genericBaseType.IsInteger;
			clone.IsSigned = genericBaseType.IsSigned;
			clone.IsUnsigned = genericBaseType.IsUnsigned;
			clone.IsVarFlag = genericBaseType.IsVarFlag;
			clone.IsMVarFlag = genericBaseType.IsMVarFlag;
			clone.IsManagedPointerType = genericBaseType.IsManagedPointerType;
			clone.IsUnmanagedPointerType = genericBaseType.IsUnmanagedPointerType;
			clone.IsArrayType = genericBaseType.IsArrayType;
			clone.IsBuiltInType = genericBaseType.IsBuiltInType;
			clone.Size = genericBaseType.Size;
			clone.PackingSize = genericBaseType.PackingSize;
			clone.VarOrMVarIndex = genericBaseType.VarOrMVarIndex;
			clone.ElementType = genericBaseType.ElementType;

			var genericTypeNames = new StringBuilder();

			foreach (var genericType in genericTypes)
			{
				genericTypeNames.Append(genericType.FullName);
				genericTypeNames.Append(", ");
			}

			genericTypeNames.Length = genericTypeNames.Length - 2;
			clone.FullName = clone.FullName + '<' + genericTypeNames.ToString() + '>';

			foreach (var m in genericBaseType.Methods)
			{
				var cloneMethod = ResolveGenericMethod(m, clone);
				clone.Methods.Add(cloneMethod);
			}

			foreach (var f in genericBaseType.Fields)
			{
				var cloneField = ResolveGenericField(f, clone);
				clone.Fields.Add(cloneField);
			}

			foreach (var m in genericBaseType.Interfaces)
			{
				clone.Interfaces.Add(m);
			}

			clone.SetFlags();

			return clone;
		}

		private static MosaMethod ResolveGenericMethod(MosaMethod method, MosaType declaringType)
		{
			var clone = new MosaMethod();

			clone.DeclaringType = declaringType;
			clone.Name = method.Name;
			clone.MethodName = method.MethodName;
			clone.IsAbstract = method.IsAbstract;
			clone.IsGeneric = method.IsGeneric;
			clone.IsStatic = method.IsStatic;
			clone.HasThis = method.HasThis;
			clone.HasExplicitThis = method.HasExplicitThis;
			clone.ReturnType = method.ReturnType;
			clone.IsInternal = method.IsInternal;
			clone.IsNoInlining = method.IsNoInlining;
			clone.IsSpecialName = method.IsSpecialName;
			clone.IsRTSpecialName = method.IsRTSpecialName;
			clone.IsVirtual = method.IsVirtual;
			clone.IsPInvokeImpl = method.IsPInvokeImpl;
			clone.IsNewSlot = method.IsNewSlot;
			clone.IsFinal = method.IsFinal;
			clone.Rva = method.Rva;

			foreach (var p in method.Parameters)
			{
				var cloneParameter = ResolveGenericParameter(p, declaringType);
				clone.Parameters.Add(cloneParameter);
			}

			foreach (var a in method.CustomAttributes)
			{
				clone.CustomAttributes.Add(a);
			}

			if (method.ReturnType.IsVarFlag || method.ReturnType.IsMVarFlag)
			{
				clone.ReturnType = declaringType.GenericParameterTypes[method.ReturnType.VarOrMVarIndex];
			}

			clone.SetName();

			return clone;
		}

		private static MosaField ResolveGenericField(MosaField field, MosaType declaringType)
		{
			var clone = new MosaField();

			clone.DeclaringType = declaringType;
			clone.FieldType = field.FieldType;
			clone.Name = field.Name;
			clone.FullName = field.FullName;
			clone.CustomAttributes = field.CustomAttributes;
			clone.IsLiteralField = field.IsLiteralField;
			clone.IsStaticField = field.IsStaticField;
			clone.RVA = clone.RVA;

			if (field.FieldType.IsVarFlag || field.FieldType.IsMVarFlag)
			{
				clone.FieldType = declaringType.GenericParameterTypes[field.FieldType.VarOrMVarIndex];
			}

			return clone;
		}

		private static MosaParameter ResolveGenericParameter(MosaParameter parameter, MosaType declaringType)
		{
			if (!(parameter.Type.IsVarFlag || parameter.Type.IsMVarFlag))
				return parameter;

			var clone = new MosaParameter();

			clone.Type = declaringType.GenericParameterTypes[parameter.Type.VarOrMVarIndex];
			clone.Position = parameter.Position;
			clone.Name = parameter.Name;
			clone.IsIn = parameter.IsIn;
			clone.IsOut = parameter.IsOut;

			return clone;
		}
	}
}