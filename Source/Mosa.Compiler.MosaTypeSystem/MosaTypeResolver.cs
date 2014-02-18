/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Ki (kiootic) <kiootic@gmail.com>
 */

using System.Collections.Generic;
using dnlib.DotNet;
using Mosa.Compiler.Common;

namespace Mosa.Compiler.MosaTypeSystem
{
	public class MosaTypeResolver
	{
		public IDictionary<string, MosaModule> Modules { get; private set; }

		public MosaModule LinkerModule { get; private set; }

		public MosaType DefaultLinkerType { get; private set; }

		private Dictionary<ScopedToken, MosaType> typeLookup = new Dictionary<ScopedToken, MosaType>();
		private Dictionary<ScopedToken, MosaMethod> methodLookup = new Dictionary<ScopedToken, MosaMethod>();
		private Dictionary<ScopedToken, MosaField> fieldLookup = new Dictionary<ScopedToken, MosaField>();

		private Dictionary<GenericInstSig, MosaType> genericTypeInsts;
		private Dictionary<Tuple<MethodSig, GenericInstMethodSig>, MosaMethod> genericMethodInsts;

		internal Dictionary<Tuple<ModuleDefMD, string>, uint> stringHeapLookup = new Dictionary<Tuple<ModuleDefMD, string>, uint>();

		class GenericTypeInstComparer : IEqualityComparer<GenericInstSig>
		{
			public bool Equals(GenericInstSig x, GenericInstSig y)
			{
				return new SigComparer().Equals(x, y);
			}

			public int GetHashCode(GenericInstSig obj)
			{
				return new SigComparer().GetHashCode(obj);
			}
		}

		class GenericMethodInstComparer : IEqualityComparer<Tuple<MethodSig, GenericInstMethodSig>>
		{
			public bool Equals(Tuple<MethodSig, GenericInstMethodSig> x, Tuple<MethodSig, GenericInstMethodSig> y)
			{
				SigComparer comparer = new SigComparer();
				return comparer.Equals(x.Item1, y.Item2) && comparer.Equals(x.Item2, y.Item2);
			}

			public int GetHashCode(Tuple<MethodSig, GenericInstMethodSig> obj)
			{
				SigComparer comparer = new SigComparer();
				return comparer.GetHashCode(obj.Item1) + comparer.GetHashCode(obj.Item2);
			}
		}

		public MosaTypeResolver()
		{
			Modules = new Dictionary<string, MosaModule>();
			SetupLinkerModule();

			genericTypeInsts = new Dictionary<GenericInstSig, MosaType>(new GenericTypeInstComparer());
			genericMethodInsts = new Dictionary<Tuple<MethodSig, GenericInstMethodSig>, MosaMethod>(new GenericMethodInstComparer());
		}

		private void SetupLinkerModule()
		{
			LinkerModule = new MosaModule("@Linker");
			AddModule(LinkerModule);
			DefaultLinkerType = CreateLinkerType("@Linker", "Default");
		}

		class USLookupHook : IStringDecrypter
		{
			MosaTypeResolver resolver;
			ModuleDefMD module;

			public USLookupHook(MosaTypeResolver resolver, ModuleDefMD module)
			{
				this.resolver = resolver;
				this.module = module;
			}

			public string ReadUserString(uint token)
			{
				string result = module.USStream.ReadNoNull(token & 0xffffff);
				resolver.stringHeapLookup[Tuple.Create(module, result)] = token;
				return result;
			}
		}

		internal void AddModule(MosaModule module)
		{
			Modules.Add(module.Name, module);
			if (module.InternalModule != null)
				module.InternalModule.StringDecrypter = new USLookupHook(this, module.InternalModule);
		}

		public MosaModule GetModuleByName(string name)
		{
			MosaModule result;
			if (Modules.TryGetValue(name, out result))
				return result;

			throw new InvalidCompilerException();
		}

		Tuple<string, string> CreateKey(TypeSig typeSig)
		{
			return Tuple.Create(typeSig.ReflectionNamespace, typeSig.ReflectionFullName);
		}

		// Add a type existed in metadata
		internal void AddMetadataType(MosaType type)
		{
			type.Module.Types.Add(CreateKey(type.TypeSignature), type);
			typeLookup.Add(type.Token, type);
		}

		// Add a type created by compiler (e.g. generic instances, linker types)
		internal void AddNewType(MosaType type)
		{
			type.Module.Types.Add(CreateKey(type.TypeSignature), type);
		}

		internal MosaType CreateLinkerType(string @namespace, string name)
		{
			var type = new MosaType(LinkerModule, name, @namespace);
			type.IsLinkerGenerated = true;

			AddNewType(type);

			return type;
		}

		internal MosaType GetTypeByToken(ScopedToken token)
		{
			return typeLookup[token];
		}

		public MosaType GetTypeByName(MosaModule module, string @namespace, string name)
		{
			MosaType result;
			if (module.Types.TryGetValue(Tuple.Create(@namespace, @namespace + "." + name), out result))
				return result;

			throw new AssemblyLoadException();
		}

		public MosaType GetTypeByName(MosaModule module, string fullname)
		{
			int dot = fullname.LastIndexOf(".");

			if (dot >= 0)
			{
				return GetTypeByName(module, fullname.Substring(0, dot), fullname.Substring(dot + 1));
			}

			throw new AssemblyLoadException();
		}

		public bool CheckTypeExists(MosaModule module, MDToken token)
		{
			return typeLookup.ContainsKey(new ScopedToken(module.InternalModule, token));
		}

		public bool CheckMethodExists(MosaModule module, MDToken token)
		{
			return methodLookup.ContainsKey(new ScopedToken(module.InternalModule, token));
		}

		internal void AddMethod(MosaMethod method)
		{
			methodLookup.Add(method.Token, method);
		}

		void AddLinkerMethod(MosaMethod method)
		{
			method.IsLinkerGenerated = true;
			method.DeclaringType.Methods.Add(method);
		}

		internal MosaMethod CreateLinkerMethod(MosaType declaringType, string name, MosaType returnType, IList<MosaType> parameters)
		{
			List<TypeSig> parameterSigs = new List<TypeSig>();
			if (parameters != null)
			{
				foreach (var parameter in parameters)
					parameterSigs.Add(parameter.TypeSignature);
			}

			var method = new MosaMethod(
				declaringType.Module,
				declaringType,
				name,
				MethodSig.CreateStatic(returnType.TypeSignature, parameterSigs.ToArray()));

			AddLinkerMethod(method);

			return method;
		}

		internal MosaMethod GetMethodByToken(ScopedToken token)
		{
			return methodLookup[token];
		}

		public bool CheckFieldExists(MosaModule module, MDToken token)
		{
			return fieldLookup.ContainsKey(new ScopedToken(module.InternalModule, token));
		}

		internal void AddField(MosaField field)
		{
			fieldLookup.Add(field.Token, field);
		}

		void AddLinkerField(MosaField field)
		{
			field.DeclaringType.Fields.Add(field);
		}

		internal MosaField GetFieldByToken(ScopedToken token)
		{
			return fieldLookup[token];
		}
	}
}