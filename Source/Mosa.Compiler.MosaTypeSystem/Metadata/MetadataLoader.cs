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
using Mosa.Compiler.Common;

namespace Mosa.Compiler.MosaTypeSystem.Metadata
{
	internal class MetadataLoader
	{
		private class TypeSigComparer : EqualityComparer<TypeSig>
		{
			public override bool Equals(TypeSig x, TypeSig y)
			{
				return new SigComparer().Equals(x, y);
			}

			public override int GetHashCode(TypeSig obj)
			{
				return new SigComparer().GetHashCode(obj);
			}
		}

		private Dictionary<TypeSig, MosaType> typeCache = new Dictionary<TypeSig, MosaType>(new TypeSigComparer());
		private MosaType[] mvar = new MosaType[0x100];
		private MosaType[] var = new MosaType[0x100];

		public IList<MosaUnit> LoadedUnits { get; private set; }

		public MosaModule CorLib { get; private set; }

		CLRMetadata metadata;
		public MetadataLoader(CLRMetadata metadata)
		{
			this.metadata = metadata;
			LoadedUnits = new List<MosaUnit>();
		}

		public MosaModule Load(ModuleDef moduleDef)
		{
			MosaModule mosaModule = metadata.Controller.CreateModule();
			using (var module = metadata.Controller.MutateModule(mosaModule))
			{
				module.UnderlyingObject = new UnitDesc<ModuleDef, object>(moduleDef, moduleDef, null);
				module.Name = moduleDef.Name;
				module.Assembly = moduleDef.Assembly.Name;

				foreach (var typeDef in moduleDef.GetTypes())
				{
					Load(mosaModule, typeDef);
				}
			}
			metadata.Controller.AddModule(mosaModule);
			metadata.Cache.AddModule(mosaModule);
			LoadedUnits.Add(mosaModule);

			if (moduleDef.Assembly.IsCorLib())
				CorLib = mosaModule;

			return mosaModule;
		}

		void Load(MosaModule module, TypeDef typeDef)
		{
			TypeSig typeSig = typeDef.ToTypeSig();
			MosaType mosaType = metadata.Controller.CreateType();
			using (var type = metadata.Controller.MutateType(mosaType))
			{
				type.Module = module;
				type.UnderlyingObject = new UnitDesc<TypeDef, TypeSig>(typeDef.Module, typeDef, typeSig);

				type.Namespace = typeDef.Namespace;
				type.Name = typeDef.Name;

				type.IsInterface = typeDef.IsInterface;
				type.IsEnum = typeDef.IsEnum;
				type.IsDelegate =
					typeDef.BaseType != null && typeDef.BaseType.DefinitionAssembly.IsCorLib() &&
					(typeDef.BaseType.FullName == "System.Delegate" || typeDef.BaseType.FullName == "System.MulticastDelegate");
				type.IsModule = typeDef.IsGlobalModuleType;

				type.IsExplicitLayout = typeDef.IsExplicitLayout;
				if (typeDef.IsExplicitLayout)
				{
					type.ClassSize = (int)typeDef.ClassSize;
					type.PackingSize = typeDef.PackingSize;
				}
				type.TypeCode = (MosaTypeCode)typeSig.ElementType;

				// Load members

				foreach (var fieldDef in typeDef.Fields)
				{
					MosaField mosaField = metadata.Controller.CreateField();

					using (var field = metadata.Controller.MutateField(mosaField))
						LoadField(mosaType, field, fieldDef);

					type.Fields.Add(mosaField);
					metadata.Cache.AddField(mosaField);
					LoadedUnits.Add(mosaField);
				}

				foreach (var methodDef in typeDef.Methods)
				{
					MosaMethod mosaMethod = metadata.Controller.CreateMethod();

					using (var method = metadata.Controller.MutateMethod(mosaMethod))
						LoadMethod(mosaType, method, methodDef);

					type.Methods.Add(mosaMethod);
					metadata.Cache.AddMethod(mosaMethod);
					LoadedUnits.Add(mosaMethod);
				}
			}
			typeCache[typeSig] = mosaType;
			metadata.Controller.AddType(mosaType);
			metadata.Cache.AddType(mosaType);
			LoadedUnits.Add(mosaType);
		}

		void LoadField(MosaType declType, MosaField.Mutator field, FieldDef fieldDef)
		{
			FieldSig fieldSig = fieldDef.FieldSig;
			field.Module = declType.Module;
			field.UnderlyingObject = new UnitDesc<FieldDef, FieldSig>(fieldDef.Module, fieldDef, fieldSig);

			field.DeclaringType = declType;
			field.Name = fieldDef.Name;

			field.IsLiteral = fieldDef.IsLiteral;
			field.IsStatic = fieldDef.IsStatic;
			field.HasDefault = fieldDef.HasDefault;
			field.Offset = fieldDef.FieldOffset;
			field.Data = fieldDef.InitialValue;
		}

		void LoadMethod(MosaType declType, MosaMethod.Mutator method, MethodDef methodDef)
		{
			MethodSig methodSig = methodDef.MethodSig;
			method.Module = declType.Module;
			method.UnderlyingObject = new UnitDesc<MethodDef, MethodSig>(methodDef.Module, methodDef, methodSig);

			method.DeclaringType = declType;
			method.Name = methodDef.Name;

			method.IsAbstract = methodDef.IsAbstract;
			method.IsStatic = methodDef.IsStatic;
			method.HasThis = methodDef.HasThis;
			method.HasExplicitThis = methodDef.ExplicitThis;
			method.IsInternalCall = methodDef.IsInternalCall;
			method.IsNoInlining = methodDef.IsNoInlining;
			method.IsSpecialName = methodDef.IsSpecialName;
			method.IsRTSpecialName = methodDef.IsRuntimeSpecialName;
			method.IsVirtual = methodDef.IsVirtual;
			method.IsNewSlot = methodDef.IsNewSlot;
			method.IsFinal = methodDef.IsFinal;
			method.IsSpecialName = methodDef.IsSpecialName;
			if (methodDef.HasImplMap)
				method.ExternMethod = methodDef.ImplMap.Module.Name;
		}

		public MosaType GetType(TypeSig typeSig)
		{
			MosaType result;
			if (typeCache.TryGetValue(typeSig, out result))
				return result;

			result = Load(typeSig);
			typeCache[typeSig] = result;
			return result;
		}

		public MosaType GetTypeThrow(TypeSig typeSig)
		{
			MosaType result;
			if (typeCache.TryGetValue(typeSig, out result))
				return result;

			throw new AssemblyLoadException();
		}

		MosaType Load(TypeSig typeSig)
		{
			if (typeSig is LeafSig)
			{
				if (typeSig is TypeDefOrRefSig)
				{
					throw new AssemblyLoadException();  // Should have been loaded in MetadataLoader
				}
				else if (typeSig is GenericInstSig)
				{
					return LoadGenericTypeInstanceSig((GenericInstSig)typeSig);
				}
				else if (typeSig is GenericSig)
				{
					return LoadGenericParam((GenericSig)typeSig);
				}
				else if (typeSig is FnPtrSig)
				{
					MethodSig fnPtr = (MethodSig)((FnPtrSig)typeSig).MethodSig;
					MosaType returnType = GetType(fnPtr.RetType);
					List<MosaParameter> pars = new List<MosaParameter>();
					for (int i = 0; i < fnPtr.Params.Count; i++)
						pars.Add(new MosaParameter("A_" + i, GetType(fnPtr.Params[i])));
					return metadata.TypeSystem.ToFnPtr(new MosaMethodSignature(returnType, pars));
				}
				else
					throw new NotSupportedException();
			}
			else    // Non-leaf signature
			{
				MosaType elementType = GetType(typeSig.Next);
				MosaType result;
				switch (typeSig.ElementType)
				{
					case ElementType.Ptr:
						result = elementType.ToUnmanagedPointer();
						using (var ptrType = metadata.Controller.MutateType(result))
							ptrType.UnderlyingObject = elementType.GetUnderlyingObject<UnitDesc<TypeDef, TypeSig>>().Clone(typeSig);
						break;

					case ElementType.ByRef:
						result = elementType.ToManagedPointer();
						using (var ptrType = metadata.Controller.MutateType(result))
							ptrType.UnderlyingObject = elementType.GetUnderlyingObject<UnitDesc<TypeDef, TypeSig>>().Clone(typeSig);
						break;

					case ElementType.CModReqd:
					case ElementType.CModOpt:
						result = metadata.Controller.CreateType(elementType);
						using (var modType = metadata.Controller.MutateType(result))
						{
							modType.Modifier = GetType(((ModifierSig)typeSig).Modifier.ToTypeSig());
							modType.UnderlyingObject = elementType.GetUnderlyingObject<UnitDesc<TypeDef, TypeSig>>().Clone(typeSig);
						}
						break;

					case ElementType.Pinned:
						result = elementType;    // Pinned types are indicated in MosaLocal
						return result;           // Don't add again to controller

					case ElementType.SZArray:
						result = elementType.ToSZArray();
						using (var arrayType = metadata.Controller.MutateType(result))
							arrayType.UnderlyingObject = elementType.GetUnderlyingObject<UnitDesc<TypeDef, TypeSig>>().Clone(typeSig);
						break;

					case ElementType.Array:
						ArraySig array = (ArraySig)typeSig;
						MosaArrayInfo arrayInfo = new MosaArrayInfo(array.LowerBounds, array.Rank, array.Sizes);
						result = elementType.ToArray(arrayInfo);
						using (var arrayType = metadata.Controller.MutateType(result))
							arrayType.UnderlyingObject = elementType.GetUnderlyingObject<UnitDesc<TypeDef, TypeSig>>().Clone(typeSig);
						break;

					default:
						throw new AssemblyLoadException();

				}
				metadata.Controller.AddType(result);
				return result;
			}
		}

		MosaType LoadGenericParam(GenericSig sig)
		{
			MosaType[] pars = sig.IsTypeVar ? var : mvar;
			MosaType type = pars[sig.Number];

			if (type == null)
			{
				type = metadata.Controller.CreateType();
				using (var genericParam = metadata.Controller.MutateType(type))
				{
					genericParam.Module = metadata.TypeSystem.LinkerModule;
					genericParam.Namespace = "";
					genericParam.Name = (sig.IsTypeVar ? "!" : "!!") + sig.Number;
					genericParam.TypeCode = (MosaTypeCode)sig.ElementType;
					genericParam.GenericParamIndex = (int)sig.Number;
					genericParam.UnderlyingObject = new UnitDesc<TypeDef, TypeSig>(null, null, sig);
				}
				pars[sig.Number] = type;
			}

			return type;
		}

		MosaType LoadGenericTypeInstanceSig(GenericInstSig typeSig)
		{
			MosaType origin = GetType(typeSig.GenericType);
			MosaType result = metadata.Controller.CreateType(origin);
			var desc = result.GetUnderlyingObject<UnitDesc<TypeDef, TypeSig>>();

			using (var resultType = metadata.Controller.MutateType(result))
			{
				resultType.UnderlyingObject = desc.Clone(typeSig);

				foreach (var genericArg in typeSig.GenericArguments)
					resultType.GenericArguments.Add(GetType(genericArg));

				metadata.Resolver.EnqueueForResolve(result);

				GenericArgumentResolver resolver = new GenericArgumentResolver();
				resolver.PushTypeGenericArguments(typeSig.GenericArguments);

				for (int i = 0; i < result.Methods.Count; i++)
				{
					MosaMethod method = metadata.Controller.CreateMethod(result.Methods[i]);

					MethodSig newSig = resolver.Resolve(method.GetMethodSig());
					using (var mosaMethod = metadata.Controller.MutateMethod(method))
					{
						mosaMethod.DeclaringType = result;
						mosaMethod.UnderlyingObject = method.GetUnderlyingObject<UnitDesc<MethodDef, MethodSig>>().Clone(newSig);
					}

					resultType.Methods[i] = method;
					metadata.Resolver.EnqueueForResolve(method);
				}

				for (int i = 0; i < result.Fields.Count; i++)
				{
					MosaField field = metadata.Controller.CreateField(result.Fields[i]);

					FieldSig newSig = new FieldSig(resolver.Resolve(field.GetFieldSig().Type));
					using (var mosaField = metadata.Controller.MutateField(field))
					{
						mosaField.DeclaringType = result;
						mosaField.UnderlyingObject = field.GetUnderlyingObject<UnitDesc<FieldDef, FieldSig>>().Clone(newSig);
					}

					resultType.Fields[i] = field;
					metadata.Resolver.EnqueueForResolve(field);
				}

				resultType.HasOpenGenericParams = typeSig.HasOpenGenericParameter();
			}

			metadata.Controller.AddType(result);

			return result;
		}

		public MosaMethod LoadGenericMethodInstance(MethodSpec methodSpec, GenericArgumentResolver resolver)
		{
			return LoadGenericMethodInstance(methodSpec.Method, methodSpec.GenericInstMethodSig.GenericArguments, resolver);
		}

		public MosaMethod LoadGenericMethodInstance(IMethodDefOrRef method, IList<TypeSig> genericArguments, GenericArgumentResolver resolver)
		{
			MosaType declType = GetType(resolver.Resolve(method.DeclaringType.ToTypeSig()));

			if (declType.HasOpenGenericParams)
				throw new AssemblyLoadException();

			MDToken token;
			if (method is MethodDef)
				token = ((MethodDef)method).MDToken;
			else
				token = ((MemberRef)method).ResolveMethodThrow().MDToken;

			MosaMethod mosaMethod = null;
			foreach (var m in declType.Methods)
			{
				var desc = m.GetUnderlyingObject<UnitDesc<MethodDef, MethodSig>>();
				if (desc.Token.Token == token)
				{
					mosaMethod = m;
					break;
				}
			}

			if (mosaMethod == null)
				throw new AssemblyLoadException();

			List<TypeSig> genericArgs;

			genericArgs = new List<TypeSig>();
			foreach (var genericArg in genericArguments)
				genericArgs.Add(resolver.Resolve(genericArg));
			resolver.PushMethodGenericArguments(genericArgs);

			mosaMethod = metadata.Controller.CreateMethod(mosaMethod);

			using (var _mosaMethod = metadata.Controller.MutateMethod(mosaMethod))
			{
				bool hasOpening = mosaMethod.DeclaringType.HasOpenGenericParams;
				foreach (var genericArg in genericArguments)
				{
					var newGenericArg = resolver.Resolve(genericArg);
					hasOpening |= newGenericArg.HasOpenGenericParameter();
					_mosaMethod.GenericArguments.Add(GetType(newGenericArg));
				}

				var desc = mosaMethod.GetUnderlyingObject<UnitDesc<MethodDef, MethodSig>>();
				_mosaMethod.UnderlyingObject = desc = desc.Clone(resolver.Resolve(method.MethodSig));
				_mosaMethod.DeclaringType = declType;


				_mosaMethod.HasOpenGenericParams = hasOpening;
			}

			resolver.PopMethodGenericArguments();

			using (var decl = metadata.Controller.MutateType(declType))
				decl.Methods.Add(mosaMethod);

			metadata.Resolver.EnqueueForResolve(mosaMethod);

			return mosaMethod;
		}
	}
}
