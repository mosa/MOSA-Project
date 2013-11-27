using Mosa.Compiler.Common;
using Mosa.Compiler.Metadata;
using System.Collections.Generic;

namespace Mosa.Compiler.MosaTypeSystem
{
	public class MosaTypeResolver
	{
		private List<MosaType> types = new List<MosaType>();
		private List<MosaMethod> methods = new List<MosaMethod>();

		private Dictionary<string, MosaAssembly> assemblyLookup = new Dictionary<string, MosaAssembly>();
		private Dictionary<MosaAssembly, Dictionary<Token, MosaType>> typeLookup = new Dictionary<MosaAssembly, Dictionary<Token, MosaType>>();
		private Dictionary<MosaAssembly, Dictionary<Token, MosaMethod>> methodLookup = new Dictionary<MosaAssembly, Dictionary<Token, MosaMethod>>();
		private Dictionary<MosaAssembly, Dictionary<Token, MosaField>> fieldLookup = new Dictionary<MosaAssembly, Dictionary<Token, MosaField>>();

		private MosaType[] var = new MosaType[255];
		private MosaType[] mvar = new MosaType[255];

		public MosaTypeResolver()
		{
			SetupInternalAssembly();
		}

		public void SetupInternalAssembly()
		{
			var assembly = new MosaAssembly("@Internal");
			AddAssembly(assembly);

			CreateAddBuiltInType(assembly, CilElementType.Void);
			CreateAddBuiltInType(assembly, CilElementType.Boolean);
			CreateAddBuiltInType(assembly, CilElementType.Char);
			CreateAddBuiltInType(assembly, CilElementType.I1);
			CreateAddBuiltInType(assembly, CilElementType.U1);
			CreateAddBuiltInType(assembly, CilElementType.I2);
			CreateAddBuiltInType(assembly, CilElementType.U2);
			CreateAddBuiltInType(assembly, CilElementType.I4);
			CreateAddBuiltInType(assembly, CilElementType.U4);
			CreateAddBuiltInType(assembly, CilElementType.I8);
			CreateAddBuiltInType(assembly, CilElementType.U8);
			CreateAddBuiltInType(assembly, CilElementType.R4);
			CreateAddBuiltInType(assembly, CilElementType.R8);
			CreateAddBuiltInType(assembly, CilElementType.String);
			CreateAddBuiltInType(assembly, CilElementType.Object);
			CreateAddBuiltInType(assembly, CilElementType.I);
			CreateAddBuiltInType(assembly, CilElementType.U);
			CreateAddBuiltInType(assembly, CilElementType.TypedByRef);
			CreateAddBuiltInType(assembly, CilElementType.Ptr);
		}

		private MosaType CreateBuiltInType(CilElementType cilElementType)
		{
			MosaType type = new MosaType();
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
			types.Add(type);
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
			methods.Add(method);
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
				type = new MosaType();
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
				type = new MosaType();
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

			type = new MosaType();
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

			type = new MosaType();
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

			type = new MosaType();
			type.FullName = element.FullName + "[]";
			type.Name = element.Name + "[]";
			type.IsArrayType = true;
			type.ElementType = element;
			type.SetFlags();

			arrayTypes.Add(element, type);

			return type;
		}
	}
}