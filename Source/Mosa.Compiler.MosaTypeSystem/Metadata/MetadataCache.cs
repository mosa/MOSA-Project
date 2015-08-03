// Copyright (c) MOSA Project. Licensed under the New BSD License.

using dnlib.DotNet;
using Mosa.Compiler.Common;
using System;
using System.Collections.Generic;

namespace Mosa.Compiler.MosaTypeSystem.Metadata
{
	public class MetadataCache
	{
		public IDictionary<string, MosaModule> Modules { get; private set; }

		public MosaModule LinkerModule { get; private set; }

		public MosaType DefaultLinkerType { get; private set; }

		private Dictionary<ScopedToken, MosaType> typeLookup = new Dictionary<ScopedToken, MosaType>();
		private Dictionary<ScopedToken, MosaMethod> methodLookup = new Dictionary<ScopedToken, MosaMethod>();
		private Dictionary<ScopedToken, MosaField> fieldLookup = new Dictionary<ScopedToken, MosaField>();
		private Dictionary<ScopedToken, MosaProperty> propertyLookup = new Dictionary<ScopedToken, MosaProperty>();

		private uint stringIdCounter;
		internal Dictionary<string, uint> stringHeapLookup = new Dictionary<string, uint>(StringComparer.Ordinal);
		internal Dictionary<uint, string> stringHeapLookup2 = new Dictionary<uint, string>();

		public MetadataCache()
		{
			Modules = new Dictionary<string, MosaModule>();
		}

		public void AddModule(MosaModule module)
		{
			Modules.Add(module.Name, module);
			//var desc = module.GetUnderlyingObject<UnitDesc<ModuleDef, object>>();
		}

		public MosaModule GetModuleByName(string name)
		{
			MosaModule result;
			if (Modules.TryGetValue(name, out result))
				return result;

			throw new InvalidCompilerException();
		}

		public void AddType(MosaType type)
		{
			typeLookup.Add(type.GetUnderlyingObject<UnitDesc<TypeDef, TypeSig>>().Token, type);
		}

		public MosaType GetTypeByToken(ScopedToken token)
		{
			return typeLookup[token];
		}

		public void AddMethod(MosaMethod method)
		{
			methodLookup.Add(method.GetUnderlyingObject<UnitDesc<MethodDef, MethodSig>>().Token, method);
		}

		public MosaMethod GetMethodByToken(ScopedToken token)
		{
			return methodLookup[token];
		}

		public void AddField(MosaField field)
		{
			fieldLookup.Add(field.GetUnderlyingObject<UnitDesc<FieldDef, FieldSig>>().Token, field);
		}

		public MosaField GetFieldByToken(ScopedToken token)
		{
			return fieldLookup[token];
		}

		public void AddProperty(MosaProperty property)
		{
			propertyLookup.Add(property.GetUnderlyingObject<UnitDesc<PropertyDef, PropertySig>>().Token, property);
		}

		public MosaProperty GetPropertyByToken(ScopedToken token)
		{
			return propertyLookup[token];
		}

		public uint GetStringId(string value)
		{
			uint id;
			if (!stringHeapLookup.TryGetValue(value, out id))
			{
				id = stringIdCounter++;
				stringHeapLookup[value] = id;
				stringHeapLookup2[id] = value;
			}
			return id;
		}

		public string GetStringById(uint id)
		{
			return stringHeapLookup2[id];
		}
	}
}