/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Ki (kiootic) <kiootic@gmail.com>
 */

using System;
using System.Collections.Generic;
using dnlib.DotNet;
using dnlib.DotNet.MD;
using Mosa.Compiler.Common;

namespace Mosa.Compiler.MosaTypeSystem
{
	internal class MosaTypeLoader
	{
		public TypeSystem TypeSystem { get; private set; }

		// Key is (Type namespace, Type full name, Module name)
		private Dictionary<Tuple<string, string, string>, MosaType> types = new Dictionary<Tuple<string, string, string>, MosaType>();

		private Queue<IResolvable> resolveQueue = new Queue<IResolvable>();

		private MosaType[] var = new MosaType[0x100];
		private MosaType[] mvar = new MosaType[0x100];

		public MosaTypeLoader(TypeSystem typeSystem)
		{
			this.TypeSystem = typeSystem;
		}

		public MosaModule Load(ModuleDefMD moduleDef)
		{
			MosaModule module = new MosaModule(moduleDef);
			TypeSystem.Resolver.AddModule(module);

			foreach (var typeDef in moduleDef.GetTypes())
			{
				Load(module, typeDef);
			}

			return module;
		}

		void Load(MosaModule module, TypeDef typeDef)
		{
			MosaType type = new MosaType(module, typeDef);

			TypeSystem.Resolver.AddMetadataType(type);
			types.Add(Tuple.Create(typeDef.ReflectionNamespace, typeDef.ReflectionFullName, typeDef.DefinitionAssembly.FullName), type);
			resolveQueue.Enqueue(type);

			foreach (var fieldDef in typeDef.Fields)
			{
				MosaField field = new MosaField(module, type, fieldDef);
				TypeSystem.Resolver.AddField(field);
				type.Fields.Add(field);
				resolveQueue.Enqueue(field);
			}

			foreach (var methodDef in typeDef.Methods)
			{
				MosaMethod method = new MosaMethod(module, type, methodDef);
				TypeSystem.Resolver.AddMethod(method);
				type.Methods.Add(method);
				resolveQueue.Enqueue(method);
			}
		}

		internal void EnqueueForResolve(IResolvable obj)
		{
			resolveQueue.Enqueue(obj);
		}

		public void Resolve()
		{
			foreach (MosaModule module in TypeSystem.AllModules)
			{
				if (module.InternalModule == null)
					continue;

				ModuleDefMD moduleDef = module.InternalModule;
				uint methodImplCount = moduleDef.TablesStream.Get(Table.MethodImpl).Rows;
				for (uint i = 0; i < methodImplCount; i++)
				{
					var methodImpl = moduleDef.TablesStream.ReadMethodImplRow(i + 1);
					MosaType type = TypeSystem.Resolver.GetTypeByToken(new ScopedToken(moduleDef, new MDToken(Table.TypeDef, methodImpl.Class)));

					MethodDef body = moduleDef.ResolveMethodDefOrRef(methodImpl.MethodBody).ResolveMethod();
					MethodDef decl = moduleDef.ResolveMethodDefOrRef(methodImpl.MethodDeclaration).ResolveMethod();

					MosaMethod mosaBody = TypeSystem.Resolver.GetMethodByToken(new ScopedToken((ModuleDefMD)body.Module, body.MDToken));
					MosaMethod mosaDecl = TypeSystem.Resolver.GetMethodByToken(new ScopedToken((ModuleDefMD)decl.Module, decl.MDToken));

					type.InheritanceOveride.Add(mosaDecl, mosaBody);
				}
			}
			while (resolveQueue.Count > 0)
			{
				resolveQueue.Dequeue().Resolve(this);
			}
			foreach (MosaModule module in TypeSystem.AllModules)
			{
				if (module.InternalModule != null && module.InternalModule.IsManagedEntryPointValid)
					module.EntryPoint = TypeSystem.Resolver.GetMethodByToken(new ScopedToken(module.InternalModule, module.InternalModule.ManagedEntryPoint.MDToken));
			}
		}

		Dictionary<IAssembly, string> assemblyNameCache = new Dictionary<IAssembly, string>();
		Tuple<string, string, string> CreateKey(TypeSig sig)
		{
			string assemblyFullName;
			if (sig is FnPtrSig)
				assemblyFullName = TypeSystem.Resolver.LinkerModule.Name;
			else if (!assemblyNameCache.TryGetValue(sig.DefinitionAssembly, out assemblyFullName))
			{
				assemblyFullName = TypeSystem.CorLib.InternalModule.Context.AssemblyResolver.Resolve(sig.DefinitionAssembly, sig.Module).FullName;
				assemblyNameCache[sig.DefinitionAssembly] = assemblyFullName;
			}
			return Tuple.Create(sig.ReflectionNamespace, sig.ReflectionFullName, assemblyFullName);
		}

		internal MosaType GetType(TypeSig typeSig)
		{
			if (typeSig.GetElementSig() is GenericSig)
				return Load(typeSig);

			var key = CreateKey(typeSig);
			MosaType result;
			if (types.TryGetValue(key, out result))
				return result;

			result = Load(typeSig);
			types[key] = result;
			return result;
		}

		internal MosaType GetTypeThrow(TypeSig typeSig)
		{
			if (typeSig.GetElementSig() is GenericSig)
				return Load(typeSig);

			var key = CreateKey(typeSig);
			MosaType result;
			if (types.TryGetValue(key, out result))
				return result;

			throw new AssemblyLoadException();
		}

		MosaType GetVarType(uint index)
		{
			MosaType type = var[index];

			if (type == null)
			{
				type = new MosaType(TypeSystem.Resolver.LinkerModule, null, new GenericVar(index));
				var[index] = type;
			}

			return type;
		}

		MosaType GetMVarType(uint index)
		{
			MosaType type = mvar[index];

			if (type == null)
			{
				type = new MosaType(TypeSystem.Resolver.LinkerModule, null, new GenericMVar(index));
				mvar[index] = type;
			}

			return type;
		}

		MosaType Load(TypeSig typeSig)
		{
			if (typeSig is LeafSig)
			{
				if (typeSig is TypeDefOrRefSig)
				{
					return GetTypeThrow(typeSig);
				}
				else if (typeSig is GenericInstSig)
				{
					return LoadGenericTypeInstanceSig((GenericInstSig)typeSig);
				}
				else if (typeSig is GenericSig)
				{
					GenericSig genericParam = (GenericSig)typeSig;
					return genericParam.IsTypeVar ? GetVarType(genericParam.Number) : GetMVarType(genericParam.Number);
				}
				else if (typeSig is FnPtrSig)
				{
					return LoadFnPointerSig((FnPtrSig)typeSig);
				}
				else
					throw new NotSupportedException();
			}
			else	// Non-leaf signature
			{
				MosaType elementType = GetType(typeSig.Next);
				MosaType result;
				switch (typeSig.ElementType)
				{
					case ElementType.Ptr:
					case ElementType.ByRef:
						result = LoadPointerSig(elementType, typeSig);
						break;

					case ElementType.CModReqd:
					case ElementType.CModOpt:
					case ElementType.Pinned:
						return LoadModifierSig(elementType, typeSig);    // signature is identical to elem. type so don't add it

					case ElementType.Array:
					case ElementType.SZArray:
						result = LoadArraySig(elementType, typeSig);
						break;

					default:
						throw new AssemblyLoadException();
				}
				if (!result.HasOpenGenericParams)
					TypeSystem.Resolver.AddNewType(result);
				return result;
			}
		}

		MosaType LoadPointerSig(MosaType elemType, TypeSig typeSig)
		{
			// Pointers have nothing defined
			MosaType type = elemType.Clone();
			type.UpdateSignature(typeSig);
			type.ElementType = elemType;

			type.Methods.Clear();
			type.Fields.Clear();
			type.Interfaces.Clear();

			type.InheritanceOveride.Clear();

			return type;
		}

		MosaType LoadModifierSig(MosaType elemType, TypeSig typeSig)
		{
			// Basically same as element type
			MosaType type = elemType.Clone();
			type.UpdateSignature(typeSig);
			type.ElementType = elemType;

			return type;
		}

		MosaType LoadArraySig(MosaType elemType, TypeSig typeSig)
		{
			// See Partition II 14.2 Arrays

			MosaType array = types[Tuple.Create("System", "System.Array", TypeSystem.CorLib.InternalModule.Assembly.FullName)];
			if (!array.Resolved)
				((IResolvable)array).Resolve(this);

			MosaType type = array.Clone();   // Copy from System.Array
			type.UpdateSignature(typeSig);
			type.ElementType = elemType;

			// Remove Methods & Fields
			type.Methods.Clear();
			type.Fields.Clear();

			// Add three array accessors as defined in standard (Get, Set, Address)

			// TODO: Add them

			return type;
		}

		MosaType LoadFnPointerSig(FnPtrSig sig)
		{
			MosaType type = new MosaType(TypeSystem.Resolver.LinkerModule, string.Format("FnPtr[0x{0:x8}]", sig.MethodSig), "");
			type.UpdateSignature(sig);

			return type;
		}

		MosaType LoadGenericTypeInstanceSig(GenericInstSig typeSig)
		{
			MosaType origin = GetType(typeSig.GenericType);
			if (!origin.Resolved)
				((IResolvable)origin).Resolve(this);
			MosaType result = origin.Clone();
			result.UpdateSignature(typeSig);

			GenericArgumentResolver resolver = new GenericArgumentResolver();
			resolver.PushTypeGenericArguments(typeSig.GenericArguments);

			// Resolve infos
			if (result.BaseType != null)
				result.BaseType = GetType(resolver.Resolve(result.BaseType.TypeSignature));

			for (int i = 0; i < result.Interfaces.Count; i++)
			{
				result.Interfaces[i] = GetType(resolver.Resolve(result.Interfaces[i].TypeSignature));
			}

			// Resolve members
			Dictionary<MosaMethod, MosaMethod> resolvedMethodLookup = new Dictionary<MosaMethod, MosaMethod>();
			for (int i = 0; i < result.Methods.Count; i++)
			{
				MosaMethod method = result.Methods[i].Clone();
				MethodSig newSig = resolver.Resolve(method.MethodSignature);
				method.UpdateSignature(newSig, result);
				resolvedMethodLookup[result.Methods[i]] = method;
				result.Methods[i] = method;
				resolveQueue.Enqueue(method);
			}

			for (int i = 0; i < result.Fields.Count; i++)
			{
				MosaField field = result.Fields[i].Clone();
				FieldSig newSig = new FieldSig(resolver.Resolve(field.FieldSignature.Type));
				field.UpdateSignature(newSig, result);
				result.Fields[i] = field;
				resolveQueue.Enqueue(field);
			}

			foreach (var decl in new List<MosaMethod>(result.InheritanceOveride.Keys))
				result.InheritanceOveride[decl] = resolvedMethodLookup[result.InheritanceOveride[decl]];

			TypeSystem.Resolver.AddNewType(result);

			return result;
		}

		internal MosaMethod LoadGenericMethodInstance(MethodSpec methodSpec, GenericArgumentResolver resolver)
		{
			return LoadGenericMethodInstance(methodSpec.Method, methodSpec.GenericInstMethodSig.GenericArguments, resolver);
		}

		internal MosaMethod LoadGenericMethodInstance(IMethodDefOrRef method, IList<TypeSig> genericArguments, GenericArgumentResolver resolver)
		{
			MosaType declType = GetType(resolver.Resolve(method.DeclaringType.ToTypeSig()));
			MDToken token;
			if (method is MethodDef)
				token = ((MethodDef)method).MDToken;
			else
				token = ((MemberRef)method).ResolveMethodThrow().MDToken;

			MosaMethod mosaMethod = null;
			foreach (var m in declType.Methods)
			{
				if (m.Token.Token == token)
				{
					mosaMethod = m;
					break;
				}
			}

			if (mosaMethod == null)
				throw new AssemblyLoadException();

			List<TypeSig> genericArgs;

			if (declType.TypeSignature.IsGenericInstanceType)
			{
				genericArgs = new List<TypeSig>(((GenericInstSig)declType.TypeSignature).GenericArguments);
				for (int i = 0; i < genericArgs.Count; i++)
					genericArgs[i] = resolver.Resolve(genericArgs[i]);
				resolver.PushTypeGenericArguments(genericArgs);
			}

			genericArgs = new List<TypeSig>();
			foreach (var genericArg in genericArguments)
				genericArgs.Add(resolver.Resolve(genericArg));
			resolver.PushMethodGenericArguments(genericArgs);

			mosaMethod = mosaMethod.Clone();

			mosaMethod.Resolve(this, resolver);

			bool hasOpenGenericParams = false;
			foreach (var genericArg in genericArguments)
			{
				var arg = resolver.Resolve(genericArg);
				mosaMethod.GenericArguments.Add(GetType(arg));
				hasOpenGenericParams |= arg.HasOpenGenericParameter();
			}

			mosaMethod.UpdateSignature(resolver.Resolve(method.MethodSig), declType);
			mosaMethod.HasOpenGenericParams = hasOpenGenericParams;

			if (declType.TypeSignature.IsGenericInstanceType)
				resolver.PopTypeGenericArguments();

			resolver.PopMethodGenericArguments();

			declType.Methods.Add(mosaMethod);

			return mosaMethod;
		}
	}
}
