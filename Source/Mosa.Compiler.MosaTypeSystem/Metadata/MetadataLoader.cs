﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using dnlib.DotNet;
using Mosa.Compiler.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

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

		private readonly Dictionary<TypeSig, MosaType> typeCache = new Dictionary<TypeSig, MosaType>(new TypeSigComparer());
		private readonly MosaType[] mvar = new MosaType[0x100];
		private readonly MosaType[] var = new MosaType[0x100];
		private ClassOrValueTypeSig szHelperEnumeratorSig = null;
		private ClassOrValueTypeSig iListSig = null;
		private UnitDesc<MethodDef, MethodSig>[] szHelperMethods = null;

		public IList<MosaUnit> LoadedUnits { get; }

		public MosaModule CorLib { get; private set; }

		private readonly CLRMetadata metadata;

		public MetadataLoader(CLRMetadata metadata)
		{
			this.metadata = metadata;
			LoadedUnits = new List<MosaUnit>();
		}

		public MosaModule Load(ModuleDef moduleDef)
		{
			var mosaModule = metadata.Controller.CreateModule();

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

		private void Load(MosaModule module, TypeDef typeDef)
		{
			var typeSig = typeDef.ToTypeSig();

			// Check to see if its one of our classes we need for SZ Arrays
			if (typeDef.Name.Contains("SZGenericArrayEnumerator`1"))
				szHelperEnumeratorSig = typeSig as ClassOrValueTypeSig;
			else if (typeDef.Name.Contains("IList`1"))
				iListSig = typeSig as ClassOrValueTypeSig;

			var mosaType = metadata.Controller.CreateType();
			using (var type = metadata.Controller.MutateType(mosaType))
			{
				type.Module = module;
				type.UnderlyingObject = new UnitDesc<TypeDef, TypeSig>(typeDef.Module, typeDef, typeSig);

				type.Namespace = typeDef.Namespace;
				type.Name = typeDef.Name;

				type.IsInterface = typeDef.IsInterface;
				type.IsEnum = typeDef.IsEnum;
				type.IsDelegate = typeDef.BaseType != null
					&& typeDef.BaseType.DefinitionAssembly.IsCorLib()
					&& (typeDef.BaseType.FullName == "System.Delegate" || typeDef.BaseType.FullName == "System.MulticastDelegate");
				type.IsModule = typeDef.IsGlobalModuleType;

				type.IsExplicitLayout = typeDef.IsExplicitLayout;
				if (typeDef.IsValueType)
				{
					if (typeDef.PackingSize > 0 && typeDef.PackingSize != ushort.MaxValue)
						type.PackingSize = typeDef.PackingSize;
					if (typeDef.ClassSize > 0 && typeDef.ClassSize != uint.MaxValue)
						type.ClassSize = (int)typeDef.ClassSize;
				}
				type.TypeAttributes = (MosaTypeAttributes)typeDef.Attributes;
				type.TypeCode = (MosaTypeCode)typeSig.ElementType;

				// Load members
				foreach (var fieldDef in typeDef.Fields)
				{
					var mosaField = metadata.Controller.CreateField();

					using (var field = metadata.Controller.MutateField(mosaField))
					{
						LoadField(mosaType, field, fieldDef);
					}

					type.Fields.Add(mosaField);
					metadata.Cache.AddField(mosaField);
					LoadedUnits.Add(mosaField);
				}

				foreach (var methodDef in typeDef.Methods)
				{
					var mosaMethod = metadata.Controller.CreateMethod();

					using (var method = metadata.Controller.MutateMethod(mosaMethod))
					{
						LoadMethod(mosaType, method, methodDef);
					}

					type.Methods.Add(mosaMethod);
					metadata.Cache.AddMethod(mosaMethod);
					LoadedUnits.Add(mosaMethod);
				}

				foreach (var propertyDef in typeDef.Properties)
				{
					var mosaProperty = metadata.Controller.CreateProperty();

					using (var property = metadata.Controller.MutateProperty(mosaProperty))
					{
						LoadProperty(mosaType, property, propertyDef);
					}

					type.Properties.Add(mosaProperty);
					metadata.Cache.AddProperty(mosaProperty);
					LoadedUnits.Add(mosaProperty);
				}
			}
			typeCache[typeSig] = mosaType;
			metadata.Controller.AddType(mosaType);
			metadata.Cache.AddType(mosaType);
			LoadedUnits.Add(mosaType);

			if (typeDef.Name.Contains("SZArrayHelper"))
			{
				szHelperMethods = mosaType
					.Methods
					.Select(x => x.GetUnderlyingObject<UnitDesc<MethodDef, MethodSig>>())
					.ToArray();
			}
		}

		private void LoadField(MosaType declType, MosaField.Mutator field, FieldDef fieldDef)
		{
			var fieldSig = fieldDef.FieldSig;
			field.UnderlyingObject = new UnitDesc<FieldDef, FieldSig>(fieldDef.Module, fieldDef, fieldSig);

			field.DeclaringType = declType;
			field.Name = fieldDef.Name;

			field.IsLiteral = fieldDef.IsLiteral;
			field.IsStatic = fieldDef.IsStatic;
			field.HasDefault = fieldDef.HasDefault;
			field.Offset = fieldDef.FieldOffset;
			field.Data = fieldDef.InitialValue;
			field.FieldAttributes = (MosaFieldAttributes)fieldDef.Attributes;
		}

		private void LoadMethod(MosaType declType, MosaMethod.Mutator method, MethodDef methodDef)
		{
			var methodSig = methodDef.MethodSig;
			method.Module = declType.Module;
			method.UnderlyingObject = new UnitDesc<MethodDef, MethodSig>(methodDef.Module, methodDef, methodSig);

			method.DeclaringType = declType;
			method.Name = methodDef.Name;

			method.MethodAttributes = (MosaMethodAttributes)methodDef.Attributes;
			method.IsAbstract = methodDef.IsAbstract;
			method.IsStatic = methodDef.IsStatic;
			method.HasThis = methodDef.HasThis;
			method.HasExplicitThis = methodDef.ExplicitThis;
			method.IsInternalCall = methodDef.IsInternalCall;
			method.IsNoInlining = methodDef.IsNoInlining;
			method.IsAggressiveInlining = methodDef.IsAggressiveInlining;
			method.IsSpecialName = methodDef.IsSpecialName;
			method.IsRTSpecialName = methodDef.IsRuntimeSpecialName;
			method.IsVirtual = methodDef.IsVirtual;
			method.IsNewSlot = methodDef.IsNewSlot;
			method.IsFinal = methodDef.IsFinal;
			method.IsSpecialName = methodDef.IsSpecialName;

			if (methodDef.HasImplMap)
			{
				method.IsExternal = true;
				method.ExternMethodModule = methodDef.ImplMap.Module.Name;
				method.ExternMethodName = methodDef.ImplMap.Name;
			}
		}

		private void LoadProperty(MosaType declType, MosaProperty.Mutator property, PropertyDef propertyDef)
		{
			var propertySig = propertyDef.PropertySig;
			property.UnderlyingObject = new UnitDesc<PropertyDef, PropertySig>(propertyDef.Module, propertyDef, propertySig);

			property.DeclaringType = declType;
			property.Name = propertyDef.Name;

			property.PropertyAttributes = (MosaPropertyAttributes)propertyDef.Attributes;
		}

		public MosaType GetType(TypeSig typeSig)
		{
			if (typeCache.TryGetValue(typeSig, out MosaType result))
				return result;

			result = Load(typeSig);
			typeCache[typeSig] = result;
			return result;
		}

		public MosaType GetTypeThrow(TypeSig typeSig)
		{
			if (typeCache.TryGetValue(typeSig, out MosaType result))
				return result;

			throw new AssemblyLoadException();
		}

		private MosaType Load(TypeSig typeSig)
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
					var fnPtr = ((FnPtrSig)typeSig).MethodSig;
					var returnType = GetType(fnPtr.RetType);
					var pars = new List<MosaParameter>();
					for (int i = 0; i < fnPtr.Params.Count; i++)
					{
						var parameter = metadata.Controller.CreateParameter();

						using (var mosaParameter = metadata.Controller.MutateParameter(parameter))
						{
							mosaParameter.Name = "A_" + i;
							mosaParameter.ParameterAttributes = MosaParameterAttributes.In;
							mosaParameter.ParameterType = GetType(fnPtr.Params[i]);
						}

						pars.Add(parameter);
					}
					return metadata.TypeSystem.ToFnPtr(new MosaMethodSignature(returnType, pars));
				}
				else
				{
					throw new NotSupportedException();
				}
			}
			else    // Non-leaf signature
			{
				var elementType = GetType(typeSig.Next);
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
							modType.ElementType = elementType;
						}
						break;

					case ElementType.Pinned:
						result = elementType;    // Pinned types are indicated in MosaLocal
						return result;           // Don't add again to controller

					case ElementType.SZArray:
						result = elementType.ToSZArray();
						using (var arrayType = metadata.Controller.MutateType(result))
						{
							arrayType.UnderlyingObject = elementType.GetUnderlyingObject<UnitDesc<TypeDef, TypeSig>>().Clone(typeSig);

							if (!typeSig.Next.HasOpenGenericParameter())
							{
								var typeDesc = elementType.GetUnderlyingObject<UnitDesc<TypeDef, TypeSig>>();
								GetType(new GenericInstSig(szHelperEnumeratorSig, typeSig.Next));
								GetType(new GenericInstSig(iListSig, typeSig.Next));
								foreach (var method in szHelperMethods)
								{
									var methodSpec = new MethodSpecUser(method.Definition, new GenericInstMethodSig(typeDesc.Signature));
									LoadGenericMethodInstance(methodSpec, new GenericArgumentResolver());
								}
							}
						}

						if (!typeSig.Next.HasOpenGenericParameter())
							metadata.Resolver.EnqueueForArrayResolve(result);
						return result;

					case ElementType.Array:
						var array = (ArraySig)typeSig;
						var arrayInfo = new MosaArrayInfo(array.LowerBounds, array.Rank, array.Sizes);
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

		private MosaType LoadGenericParam(GenericSig sig)
		{
			//Debug.Assert(false, sig.FullName);
			var pars = sig.IsTypeVar ? var : mvar;
			var type = pars[sig.Number];

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
					genericParam.HasOpenGenericParams = true;
					genericParam.UnderlyingObject = new UnitDesc<TypeDef, TypeSig>(null, null, sig);
				}
				pars[sig.Number] = type;
				metadata.Controller.AddType(type);
			}

			return type;
		}

		private MosaType LoadGenericTypeInstanceSig(GenericInstSig typeSig)
		{
			//Debug.Assert(false, typeSig.FullName);
			var origin = GetType(typeSig.GenericType);
			var result = metadata.Controller.CreateType(origin);
			var desc = result.GetUnderlyingObject<UnitDesc<TypeDef, TypeSig>>();

			using (var resultType = metadata.Controller.MutateType(result))
			{
				resultType.UnderlyingObject = desc.Clone(typeSig);
				resultType.ElementType = origin;

				foreach (var genericArg in typeSig.GenericArguments)
				{
					resultType.GenericArguments.Add(GetType(genericArg));
				}

				metadata.Resolver.EnqueueForResolve(result);

				var resolver = new GenericArgumentResolver();
				resolver.PushTypeGenericArguments(typeSig.GenericArguments);

				for (int i = 0; i < result.Methods.Count; i++)
				{
					var method = metadata.Controller.CreateMethod(result.Methods[i]);

					using (var mosaMethod = metadata.Controller.MutateMethod(method))
					{
						mosaMethod.DeclaringType = result;
						mosaMethod.UnderlyingObject = method.GetUnderlyingObject<UnitDesc<MethodDef, MethodSig>>();
					}

					resultType.Methods[i] = method;
					metadata.Resolver.EnqueueForResolve(method);
				}

				for (int i = 0; i < result.Fields.Count; i++)
				{
					var field = metadata.Controller.CreateField(result.Fields[i]);

					using (var mosaField = metadata.Controller.MutateField(field))
					{
						mosaField.DeclaringType = result;
						mosaField.UnderlyingObject = field.GetUnderlyingObject<UnitDesc<FieldDef, FieldSig>>();
					}

					resultType.Fields[i] = field;
					metadata.Resolver.EnqueueForResolve(field);
				}

				for (int i = 0; i < result.Properties.Count; i++)
				{
					var property = metadata.Controller.CreateProperty(result.Properties[i]);

					var newSig = property.GetPropertySig().Clone();
					newSig.RetType = resolver.Resolve(newSig.RetType);
					using (var mosaProperty = metadata.Controller.MutateProperty(property))
					{
						mosaProperty.DeclaringType = result;
						mosaProperty.UnderlyingObject = property.GetUnderlyingObject<UnitDesc<PropertyDef, PropertySig>>();
					}

					resultType.Properties[i] = property;
					metadata.Resolver.EnqueueForResolve(property);
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
			var declType = GetType(resolver.Resolve(method.DeclaringType.ToTypeSig()));

			MDToken token;
			if (method is MethodDef)
				token = ((MethodDef)method).MDToken;
			else
				token = ((MemberRef)method).ResolveMethodThrow().MDToken;

			MosaMethod mosaMethod = null;
			UnitDesc<MethodDef, MethodSig> desc = null;
			foreach (var m in declType.Methods)
			{
				desc = m.GetUnderlyingObject<UnitDesc<MethodDef, MethodSig>>();
				if (desc.Token.Token == token)
				{
					mosaMethod = m;
					break;
				}
			}

			if (mosaMethod == null)
				throw new AssemblyLoadException();

			var genericArgs = new List<TypeSig>();
			foreach (var genericArg in genericArguments)
			{
				genericArgs.Add(resolver.Resolve(genericArg));
			}

			resolver.PushMethodGenericArguments(genericArgs);

			// Check for existing generic method instance
			var newSig = resolver.Resolve(method.MethodSig);

			// Need to make sure we pop otherwise it will cause bugs
			resolver.PopMethodGenericArguments();

			var comparer = new SigComparer();
			foreach (var m in declType.Methods)
			{
				var mDesc = m.GetUnderlyingObject<UnitDesc<MethodDef, MethodSig>>();
				if (mDesc.Definition != desc.Definition || !comparer.Equals(mDesc.Signature, newSig))
					continue;

				if (newSig.ContainsGenericParameter || newSig.GenParamCount > 0)
					continue;

				if (m.GenericArguments.Count != genericArgs.Count)
					continue;

				if (m.GenericArguments.Count > 0)
				{
					var failedGenericArgumentMatch = false;
					for (var i = 0; i < m.GenericArguments.Count; i++)
					{
						if (comparer.Equals(genericArguments[i], m.GenericArguments[i].GetTypeSig()))
							continue;

						failedGenericArgumentMatch = true;
						break;
					}

					if (failedGenericArgumentMatch)
						continue;
				}

				return m;
			}

			mosaMethod = metadata.Controller.CreateMethod(mosaMethod);

			using (var _mosaMethod = metadata.Controller.MutateMethod(mosaMethod))
			{
				bool hasOpening = mosaMethod.DeclaringType.HasOpenGenericParams;
				foreach (var genericArg in genericArgs)
				{
					hasOpening |= genericArg.HasOpenGenericParameter();
					_mosaMethod.GenericArguments.Add(GetType(genericArg));
				}

				_mosaMethod.UnderlyingObject = desc.Clone(newSig);
				_mosaMethod.DeclaringType = declType;

				_mosaMethod.HasOpenGenericParams = hasOpening;
			}

			using (var decl = metadata.Controller.MutateType(declType))
				decl.Methods.Add(mosaMethod);

			metadata.Resolver.EnqueueForResolve(mosaMethod);

			return mosaMethod;
		}
	}
}
