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
using System.Diagnostics;
using System.Text;

namespace Mosa.Compiler.MosaTypeSystem
{
	public class MosaTypeResolver
	{
		public IList<MosaAssembly> Assemblies { get; private set; }

		public IList<MosaType> Types { get; private set; }

		public IList<MosaMethod> Methods { get; private set; }

		public MosaAssembly InternalAssembly { get; private set; }

		public MosaAssembly LinkerAssembly { get; private set; }

		public MosaAssembly GenericAssembly { get; private set; }

		public BuiltInTypes BuiltIn { get; internal set; }

		public MosaType DefaultLinkerType { get; internal set; }

		private Dictionary<string, MosaAssembly> assemblyLookup = new Dictionary<string, MosaAssembly>();
		private Dictionary<MosaAssembly, Dictionary<Token, MosaType>> typeLookup = new Dictionary<MosaAssembly, Dictionary<Token, MosaType>>();
		private Dictionary<MosaAssembly, Dictionary<Token, MosaMethod>> methodLookup = new Dictionary<MosaAssembly, Dictionary<Token, MosaMethod>>();
		private Dictionary<MosaAssembly, Dictionary<Token, MosaField>> fieldLookup = new Dictionary<MosaAssembly, Dictionary<Token, MosaField>>();
		private Dictionary<MosaType, MosaType> unmanagedPointerTypes = new Dictionary<MosaType, MosaType>();
		private Dictionary<MosaType, MosaType> managedPointerTypes = new Dictionary<MosaType, MosaType>();
		private Dictionary<MosaAssembly, Dictionary<HeapIndexToken, string>> userString = new Dictionary<MosaAssembly, Dictionary<HeapIndexToken, string>>();
		private Dictionary<MosaAssembly, UserStringHeap> userStringHeaps = new Dictionary<MosaAssembly, UserStringHeap>();
		private Dictionary<MosaType, MosaType> arrayTypes = new Dictionary<MosaType, MosaType>();
		private Dictionary<MosaType, List<KeyValuePair<List<MosaType>, MosaType>>> genericTypeLookup = new Dictionary<MosaType, List<KeyValuePair<List<MosaType>, MosaType>>>();
		private Dictionary<MosaMethod, List<KeyValuePair<List<MosaType>, MosaMethod>>> genericMethodLookup = new Dictionary<MosaMethod, List<KeyValuePair<List<MosaType>, MosaMethod>>>();
		private Queue<MosaType> delayedGenericResolution = new Queue<MosaType>();

		private MosaType[] var = new MosaType[255];
		private MosaType[] mvar = new MosaType[255];

		public MosaTypeResolver()
		{
			Types = new List<MosaType>();
			Methods = new List<MosaMethod>();
			BuiltIn = new BuiltInTypes();
			Assemblies = new List<MosaAssembly>();

			SetupInternalAssembly();
			SetupLinkerAssembly();
			SetupGenericAssembly();
		}

		private void SetupLinkerAssembly()
		{
			LinkerAssembly = new MosaAssembly("@Linker");
			AddAssembly(LinkerAssembly);
			DefaultLinkerType = CreateLinkerType("@Linker", "Default");
		}

		private void SetupGenericAssembly()
		{
			GenericAssembly = new MosaAssembly("@Generic");
			AddAssembly(GenericAssembly);
		}

		private void SetupInternalAssembly()
		{
			InternalAssembly = new MosaAssembly("@Internal");
			AddAssembly(InternalAssembly);

			BuiltIn.Void = CreateAddBuiltInType(CilElementType.Void, 0); BuiltIn.Void.IsVoid = true;
			BuiltIn.Boolean = CreateAddBuiltInType(CilElementType.Boolean, 1); BuiltIn.Boolean.IsBoolean = true;
			BuiltIn.Char = CreateAddBuiltInType(CilElementType.Char, 2); BuiltIn.Char.IsChar = true;
			BuiltIn.I1 = CreateAddBuiltInType(CilElementType.I1, 1); BuiltIn.I1.IsSignedByte = true;
			BuiltIn.U1 = CreateAddBuiltInType(CilElementType.U1, 1); BuiltIn.U1.IsUnsignedByte = true;
			BuiltIn.I2 = CreateAddBuiltInType(CilElementType.I2, 2); BuiltIn.I2.IsSignedShort = true;
			BuiltIn.U2 = CreateAddBuiltInType(CilElementType.U2, 2); BuiltIn.U2.IsUnsignedShort = true;
			BuiltIn.I4 = CreateAddBuiltInType(CilElementType.I4, 4); BuiltIn.I4.IsSignedInt = true;
			BuiltIn.U4 = CreateAddBuiltInType(CilElementType.U4, 4); BuiltIn.U4.IsUnsignedInt = true;
			BuiltIn.I8 = CreateAddBuiltInType(CilElementType.I8, 8); BuiltIn.I8.IsSignedLong = true;
			BuiltIn.U8 = CreateAddBuiltInType(CilElementType.U8, 8); BuiltIn.U8.IsUnsignedLong = true;
			BuiltIn.R4 = CreateAddBuiltInType(CilElementType.R4, 4); BuiltIn.R4.IsSingle = true;
			BuiltIn.R8 = CreateAddBuiltInType(CilElementType.R8, 8); BuiltIn.R8.IsDouble = true;
			BuiltIn.String = CreateAddBuiltInType(CilElementType.String, null); BuiltIn.String.IsString = true;
			BuiltIn.Object = CreateAddBuiltInType(CilElementType.Object, null); BuiltIn.Object.IsObject = true;
			BuiltIn.TypedByRef = CreateAddBuiltInType(CilElementType.TypedByRef, null); BuiltIn.TypedByRef.IsManagedPointerType = true;
			BuiltIn.Ptr = CreateAddBuiltInType(CilElementType.Ptr, null); BuiltIn.Ptr.IsUnmanagedPointerType = true;
			BuiltIn.I = CreateAddBuiltInType(CilElementType.I, null); BuiltIn.I.IsNativeSignedInteger = true;
			BuiltIn.U = CreateAddBuiltInType(CilElementType.U, null); BuiltIn.I.IsNativeUnsignedInteger = true;
		}

		private MosaType CreateBuiltInType(CilElementType cilElementType)
		{
			MosaType type = new MosaType(InternalAssembly);
			type.Name = cilElementType.ToString();
			type.FullName = cilElementType.ToString();
			type.IsBuiltInType = true;

			return type;
		}

		private MosaType CreateAddBuiltInType(CilElementType cilElementType, int? fixedSize)
		{
			var type = CreateBuiltInType(cilElementType);
			type.FixedSize = fixedSize;

			AddType(cilElementType, type);

			return type;
		}

		public void AddAssembly(MosaAssembly assembly)
		{
			Assemblies.Add(assembly);
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

		internal void AddType(MosaType type)
		{
			Types.Add(type);

			if (type.GenericBaseType != null)
			{
				delayedGenericResolution.Enqueue(type);
			}
		}

		internal void AddType(MosaAssembly assembly, Token token, MosaType type)
		{
			typeLookup[assembly].Add(token, type);

			if (Types.Contains(type))
				return;

			AddType(type);
		}

		internal void AddLinkerType(MosaType type)
		{
			AddType(type);
		}

		internal void AddType(CilElementType elementType, MosaType type)
		{
			AddType(InternalAssembly, new Token((uint)elementType), type);
		}

		internal MosaType CreateLinkerType(string @namespace, string name)
		{
			var type = new MosaType(LinkerAssembly);
			type.Name = name;
			type.Namespace = @namespace;
			type.IsLinkerGenerated = true;
			type.FullName = @namespace + "." + name;

			AddLinkerType(type);

			return type;
		}

		public MosaType GetTypeByElementType(CilElementType elementType)
		{
			return GetTypeByToken(InternalAssembly, new Token((uint)elementType));
		}

		public MosaType GetTypeByElementType(CilElementType? elementType)
		{
			return GetTypeByToken(InternalAssembly, new Token((uint)elementType.Value));
		}

		internal MosaType GetTypeByToken(MosaAssembly assembly, Token token)
		{
			return typeLookup[assembly][token];
		}

		public MosaType GetTypeByToken(MosaAssembly assembly, Token token, MosaMethod baseMethod)
		{
			var type = typeLookup[assembly][token];

			Debug.Assert(!type.IsMVarFlag);

			if (type.IsOpenGenericType)
			{
				type = ResolveGenericType(baseMethod.DeclaringType, baseMethod.DeclaringType.GenericArguments);
			}

			return type;
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

		public bool CheckMethodExists(MosaAssembly assembly, Token token)
		{
			return methodLookup[assembly].ContainsKey(token);
		}

		internal void AddMethod(MosaAssembly assembly, Token token, MosaMethod method)
		{
			methodLookup[assembly].Add(token, method);

			if (Methods.Contains(method))
				return;

			Methods.Add(method);
		}

		internal void AddLinkerMethod(MosaMethod method)
		{
			Methods.Add(method);
			method.DeclaringType.Methods.Add(method);
		}

		internal MosaMethod CreateLinkerMethod(MosaType declaringType, string name, MosaType returnType, IList<MosaType> parameters)
		{
			var method = new MosaMethod();

			method.Name = name;
			method.DeclaringType = declaringType;
			method.ReturnType = returnType;
			method.IsStatic = true;
			method.IsCILGenerated = false;
			method.HasExplicitThis = false;
			method.HasThis = false;

			if (parameters != null)
			{
				for (int index = 0; index < parameters.Count; index++)
				{
					var param = new MosaParameter();

					param.Type = parameters[index];
					param.IsIn = false;
					param.IsOut = false;
					param.Position = index;
					param.Name = "P" + index.ToString();

					method.Parameters.Add(param);
				}
			}

			method.SetName();
			method.SetOpenGeneric();

			AddLinkerMethod(method);

			return method;
		}

		internal MosaMethod GetMethodByToken(MosaAssembly assembly, Token token)
		{
			return methodLookup[assembly][token];
		}

		public MosaMethod GetMethodByToken(MosaAssembly assembly, Token token, List<MosaType> genericArguments)
		{
			var method = methodLookup[assembly][token];

			if (method.IsOpenGenericType)
			{
				method = ResolveGenericMethod(method, genericArguments);
			}

			return method;
		}

		public bool CheckFieldExists(MosaAssembly assembly, Token token)
		{
			return fieldLookup[assembly].ContainsKey(token);
		}

		internal void AddField(MosaAssembly assembly, Token token, MosaField field)
		{
			Debug.Assert(field.DeclaringType.IsBaseGeneric || field.DeclaringType.GenericParameters.Count == field.DeclaringType.GenericArguments.Count);
			fieldLookup[assembly].Add(token, field);
		}

		public MosaField GetFieldByToken(MosaAssembly assembly, Token token)
		{
			return fieldLookup[assembly][token];
		}

		public MosaField GetFieldByToken(MosaAssembly assembly, Token token, List<MosaType> genericArguments)
		{
			var field = fieldLookup[assembly][token];

			Debug.Assert(!field.Type.IsMVarFlag);

			if (field.DeclaringType.IsOpenGenericType)
			{
				var type = ResolveGenericType(field.DeclaringType, genericArguments, null);

				ResolveDelayedGenerics();

				foreach (var f in type.Fields)
				{
					if (f.Name == field.Name)
						return f;
				}

				throw new InvalidCompilerException();
			}

			return field;
		}

		internal MosaType GetVarType(int index)
		{
			MosaType type = var[index];

			if (type == null)
			{
				type = new MosaType(InternalAssembly);
				type.Name = "!" + index.ToString();
				type.FullName = type.Name;
				type.IsVarFlag = true;
				type.VarOrMVarIndex = index;
				var[index] = type;
			}

			return type;
		}

		internal MosaType GetMVarType(int index)
		{
			MosaType type = mvar[index];

			if (type == null)
			{
				type = new MosaType(InternalAssembly);
				type.Name = "!!" + index.ToString();
				type.FullName = type.Name;
				type.IsMVarFlag = true;
				type.VarOrMVarIndex = index;
				mvar[index] = type;
			}

			return type;
		}

		public MosaType GetUnmanagedPointerType(MosaType element)
		{
			MosaType type;

			if (unmanagedPointerTypes.TryGetValue(element, out type))
				return type;

			type = new MosaType(InternalAssembly);
			type.FullName = "Ptr-" + element.FullName;
			type.Name = "Ptr-" + element.Name;
			type.ElementType = element;
			type.IsUnmanagedPointerType = true;
			type.IsBuiltInType = true;
			type.SetFlags();
			type.SetOpenGeneric();

			unmanagedPointerTypes.Add(element, type);

			return type;
		}

		public MosaType GetManagedPointerType(MosaType element)
		{
			MosaType type;

			if (managedPointerTypes.TryGetValue(element, out type))
				return type;

			type = new MosaType(InternalAssembly);
			type.FullName = "Ptr+" + element.FullName;
			type.Name = "Ptr+" + element.Name;
			type.ElementType = element;
			type.IsManagedPointerType = true;
			type.IsBuiltInType = true;
			type.SetFlags();
			type.SetOpenGeneric();

			managedPointerTypes.Add(element, type);

			return type;
		}

		public MosaType GetArrayType(MosaType element)
		{
			MosaType type;

			if (arrayTypes.TryGetValue(element, out type))
				return type;

			type = new MosaType(InternalAssembly);
			type.FullName = "Array-" + element.FullName;
			type.Name = "Array-" + element.Name;
			type.IsArray = true;
			type.ElementType = element;
			type.IsBuiltInType = true;
			type.SetFlags();
			type.SetOpenGeneric();

			arrayTypes.Add(element, type);

			return type;
		}

		private MosaType CreateNewElementType(MosaType baseType, MosaType elementType)
		{
			if (baseType.IsArray)
			{
				return GetArrayType(elementType);
			}
			else if (baseType.IsUnmanagedPointerType)
			{
				return GetUnmanagedPointerType(elementType);
			}
			else if (baseType.IsManagedPointerType)
			{
				return GetManagedPointerType(elementType);
			}

			throw new InvalidCompilerException();
		}

		private MosaType FindGenericType(MosaType genericType, List<MosaType> genericArguments)
		{
			//Debug.Write("FIND : " + genericType.FullName + " <");
			//foreach (var a in genericArguments)
			//{
			//	Debug.Write(a.FullName + ", ");
			//}
			//Debug.Write("> ");

			List<KeyValuePair<List<MosaType>, MosaType>> argumentPairs = null;

			if (!genericTypeLookup.TryGetValue(genericType.GenericBaseType, out argumentPairs))
			{
				//Debug.WriteLine("==> NONE");
				return null;
			}

			foreach (var genericPair in argumentPairs)
			{
				var types = genericPair.Key;

				if (types.Count != genericArguments.Count)
					continue;

				bool match = true;
				for (int i = 0; i < types.Count; i++)
				{
					if (!types[i].Matches(genericArguments[i]))
					{
						match = false;
						break;
					}
				}

				if (!match)
					continue;

				//Debug.WriteLine("==> RETURNED: " + genericPair.Value);
				return genericPair.Value;
			}

			//Debug.WriteLine("==> NONE");
			return null;
		}

		private void StoreGenericType(MosaType genericType, List<MosaType> genericArguments)
		{
			//Debug.Write("STORE: " + genericType.FullName + " <");
			//foreach (var a in genericArguments)
			//{
			//	Debug.Write(a.FullName + ", ");
			//}
			//Debug.WriteLine("> BASE: " + genericType.GenericBaseType.FullName);

			List<KeyValuePair<List<MosaType>, MosaType>> argumentPairs;

			if (!genericTypeLookup.TryGetValue(genericType.GenericBaseType, out argumentPairs))
			{
				argumentPairs = new List<KeyValuePair<List<MosaType>, MosaType>>();
				genericTypeLookup.Add(genericType.GenericParentType, argumentPairs);
			}

			argumentPairs.Add(new KeyValuePair<List<MosaType>, MosaType>(genericArguments, genericType));
		}

		public MosaType ResolveGenericType(MosaType genericType, List<MosaType> genericArguments)
		{
			var resolvedGenericType = FindGenericType(genericType, genericArguments);

			if (resolvedGenericType != null)
			{
				return resolvedGenericType;
			}

			resolvedGenericType = CreateGenericType(genericType, genericArguments);
			
			AddType(resolvedGenericType);

			StoreGenericType(resolvedGenericType, genericArguments);

			return resolvedGenericType;
		}

		public MosaType CreateGenericType(MosaType genericBaseType, List<MosaType> genericArguments)
		{
			var generic = new MosaType(GenericAssembly);

			generic.GenericParentType = genericBaseType;
			generic.GenericBaseType = genericBaseType.GenericBaseType;
			generic.GenericArguments = genericArguments;
			generic.Name = genericBaseType.Name;
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
			generic.IsObject = genericBaseType.IsObject;
			generic.IsDouble = genericBaseType.IsDouble;
			generic.IsSingle = genericBaseType.IsSingle;
			generic.IsVarFlag = genericBaseType.IsVarFlag;
			generic.IsMVarFlag = genericBaseType.IsMVarFlag;
			generic.IsManagedPointerType = genericBaseType.IsManagedPointerType;
			generic.IsUnmanagedPointerType = genericBaseType.IsUnmanagedPointerType;
			generic.IsArray = genericBaseType.IsArray;
			generic.IsBuiltInType = genericBaseType.IsBuiltInType;
			generic.IsInterface = genericBaseType.IsInterface;
			generic.Size = genericBaseType.Size;
			generic.PackingSize = genericBaseType.PackingSize;
			generic.VarOrMVarIndex = genericBaseType.VarOrMVarIndex;
			generic.ElementType = genericBaseType.ElementType;
			generic.IsNativeSignedInteger = genericBaseType.IsNativeSignedInteger;
			generic.IsNativeUnsignedInteger = genericBaseType.IsNativeUnsignedInteger;
			generic.GenericParameters = genericBaseType.GenericParameters;
			generic.SetOpenGeneric();

			var genericTypeNames = new StringBuilder();

			foreach (var genericType in genericArguments)
			{
				genericTypeNames.Append(genericType.FullName);
				genericTypeNames.Append(", ");
			}

			genericTypeNames.Length = genericTypeNames.Length - 2;
			generic.FullName = generic.Namespace + "." + generic.Name + '<' + genericTypeNames.ToString() + '>';

			//Debug.WriteLine("CREATED: " + generic.FullName);

			generic.SetFlags();

			return generic;
		}

		internal void AddGenericMethods(MosaType generic)
		{
			if (generic.AreMethodsAssigned)
				return;

			if (!generic.GenericBaseType.AreMethodsAssigned)
				return;

			generic.AreMethodsAssigned = true;

			foreach (var method in generic.GenericBaseType.Methods)
			{
				var cloneMethod = CreateGenericMethod(method, generic, null);
				generic.Methods.Add(cloneMethod);
			}
		}

		internal void AddGenericFields(MosaType generic)
		{
			if (generic.AreFieldsAssigned)
				return;

			if (!generic.GenericBaseType.AreFieldsAssigned)
				return;

			generic.AreFieldsAssigned = true;

			foreach (var field in generic.GenericBaseType.Fields)
			{
				var cloneField = ResolveGenericField(field, generic);
				generic.Fields.Add(cloneField);
			}
		}

		internal void AddGenericInterfaces(MosaType generic)
		{
			if (generic.AreInterfacesAssigned)
				return;

			if (!generic.GenericBaseType.AreInterfacesAssigned)
				return;

			generic.AreInterfacesAssigned = true;

			foreach (var @interface in generic.GenericBaseType.Interfaces)
			{
				if (@interface.GenericArguments.Count == 0)
				{
					generic.Interfaces.Add(@interface);
				}
				else
				{
					var genericInterface = ResolveGenericType(@interface, generic.GenericArguments, null);
					generic.Interfaces.Add(genericInterface);
				}
			}
		}

		private MosaMethod FindGenericMethod(MosaMethod genericMethod, List<MosaType> genericArguments)
		{
			List<KeyValuePair<List<MosaType>, MosaMethod>> argumentPairs = null;

			if (!genericMethodLookup.TryGetValue(genericMethod.GenericBaseMethod, out argumentPairs))
				return null;

			foreach (var genericPair in argumentPairs)
			{
				var types = genericPair.Key;

				if (types.Count != genericArguments.Count)
					continue;

				bool match = true;
				for (int i = 0; i < types.Count; i++)
				{
					if (!types[i].Matches(genericArguments[i]))
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

		private void StoreGenericMethod(MosaMethod genericMethod, List<MosaType> genericArguments)
		{
			List<KeyValuePair<List<MosaType>, MosaMethod>> argumentPairs;

			if (!genericMethodLookup.TryGetValue(genericMethod.GenericBaseMethod, out argumentPairs))
			{
				argumentPairs = new List<KeyValuePair<List<MosaType>, MosaMethod>>();
				genericMethodLookup.Add(genericMethod.GenericBaseMethod, argumentPairs);
			}

			argumentPairs.Add(new KeyValuePair<List<MosaType>, MosaMethod>(genericArguments, genericMethod));
		}

		public MosaMethod ResolveGenericMethod(MosaMethod genericMethod, List<MosaType> genericArguments)
		{
			var resolvedGenericMethod = FindGenericMethod(genericMethod, genericArguments);

			if (resolvedGenericMethod != null)
			{
				return resolvedGenericMethod;
			}

			var genericType = ResolveGenericType(genericMethod.DeclaringType, genericArguments, null);

			resolvedGenericMethod = CreateGenericMethod(genericMethod, genericType, genericArguments);

			StoreGenericMethod(resolvedGenericMethod, genericArguments);

			return resolvedGenericMethod;
		}

		private MosaMethod CreateGenericMethod(MosaMethod method, MosaType declaringType, List<MosaType> genericMethodArguments)
		{
			var generic = new MosaMethod();

			generic.DeclaringType = declaringType;
			generic.Name = method.Name;
			generic.MethodName = method.MethodName;

			generic.GenericParentMethod = method;
			generic.GenericBaseMethod = method.GenericBaseMethod ?? method;

			generic.IsAbstract = method.IsAbstract;
			generic.IsBaseGeneric = method.IsBaseGeneric;
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
			generic.ReturnType = ResolveGenericType(method.ReturnType, declaringType.GenericArguments, genericMethodArguments);

			foreach (var parameter in method.Parameters)
			{
				var cloneParameter = ResolveGenericParameter(parameter, declaringType.GenericArguments, genericMethodArguments);
				generic.Parameters.Add(cloneParameter);
			}

			foreach (var attribute in method.CustomAttributes)
			{
				generic.CustomAttributes.Add(attribute);
			}

			generic.SetName();
			generic.SetOpenGeneric();

			foreach (var localVariable in method.LocalVariables)
			{
				var local = ResolveGenericType(localVariable, declaringType.GenericArguments, genericMethodArguments);

				generic.LocalVariables.Add(local);
			}

			return generic;
		}

		private MosaField ResolveGenericField(MosaField field, MosaType declaringType)
		{
			var generic = new MosaField();

			generic.DeclaringType = declaringType;
			generic.Type = ResolveGenericType(field.Type, declaringType.GenericArguments, null);
			generic.Name = field.Name;
			generic.FullName = generic.DeclaringType.FullName + "." + field.Name;
			generic.CustomAttributes = field.CustomAttributes;
			generic.IsLiteralField = field.IsLiteralField;
			generic.IsStaticField = field.IsStaticField;
			generic.Offset = field.Offset;
			generic.RVA = field.RVA;
			generic.HasRVA = field.HasRVA;
			generic.Data = generic.Data;

			return generic;
		}

		public MosaType ResolveGenericType(MosaType type, List<MosaType> genericArguments, List<MosaType> genericMethodArguments)
		{
			if (type.IsVarFlag)
			{
				type = genericArguments[type.VarOrMVarIndex];
			}
			else if (type.IsMVarFlag)
			{
				type = genericMethodArguments[type.VarOrMVarIndex];
			}
			else if (type.IsOpenGenericType)
			{
				List<MosaType> arguments = new List<MosaType>(type.GenericParameters.Count);

				foreach (var typeArgument in type.GenericArguments)
				{
					var argument = typeArgument;

					if (argument.IsVarFlag)
					{
						argument = genericArguments[argument.VarOrMVarIndex];
					}

					arguments.Add(argument);
				}

				type = ResolveGenericType(type, arguments);
			}

			if (type.HasElement && type.ElementType.IsVarFlag)
			{
				type = CreateNewElementType(type, genericArguments[type.VarOrMVarIndex]);
			}

			return type;
		}

		private MosaParameter ResolveGenericParameter(MosaParameter parameter, List<MosaType> genericArguments, List<MosaType> genericMethodArguments)
		{
			var type = ResolveGenericType(parameter.Type, genericArguments, genericMethodArguments);

			if (type == parameter.Type)
				return parameter;

			var generic = new MosaParameter();
			generic.Type = type;
			generic.Position = parameter.Position;
			generic.Name = parameter.Name;
			generic.IsIn = parameter.IsIn;
			generic.IsOut = parameter.IsOut;

			return generic;
		}

		internal void AddUserStringHeap(MosaAssembly assembly, UserStringHeap userStringHeap)
		{
			userStringHeaps.Add(assembly, userStringHeap);
		}

		/// <summary>
		/// Gets the string.
		/// </summary>
		/// <param name="assembly">The assembly.</param>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public string GetUserString(MosaAssembly assembly, HeapIndexToken token)
		{
			string value;

			if (userString.ContainsKey(assembly))
			{
				if (userString[assembly].TryGetValue(token, out value))
					return value;
			}
			else
			{
				userString.Add(assembly, new Dictionary<HeapIndexToken, string>());
			}

			value = userStringHeaps[assembly].ReadString(token);

			userString[assembly].Add(token, value);

			return value;
		}

		public void ResolveDelayedGenerics()
		{
			while (delayedGenericResolution.Count != 0)
			{
				var genericType = delayedGenericResolution.Dequeue();

				if (!ResolveDelayedGeneric(genericType))
				{
					delayedGenericResolution.Enqueue(genericType);
				}
			}
		}

		private bool ResolveDelayedGeneric(MosaType genericType)
		{
			AddGenericMethods(genericType);
			AddGenericFields(genericType);
			AddGenericInterfaces(genericType);

			return genericType.AreMethodsAssigned && genericType.AreFieldsAssigned && genericType.AreInterfacesAssigned;
		}
	}
}