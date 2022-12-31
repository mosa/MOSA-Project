// Copyright (c) MOSA Project. Licensed under the New BSD License.

using dnlib.DotNet;
using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.MosaTypeSystem.CLR.Utils;

namespace Mosa.Compiler.MosaTypeSystem.CLR.Metadata
{
	public class ClrMetadataCache
	{
		public Dictionary<string, MosaModule?> Modules { get; }

		public MosaModule LinkerModule { get; }

		public MosaType DefaultLinkerType { get; }

		private readonly Dictionary<ScopedToken, MosaType> typeLookup = new();
		private readonly Dictionary<ScopedToken, MosaMethod> methodLookup = new();
		private readonly Dictionary<ScopedToken, MosaField> fieldLookup = new();
		private readonly Dictionary<ScopedToken, MosaProperty> propertyLookup = new();

		private uint stringIdCounter;
		private readonly Dictionary<string, uint> stringHeapLookup = new(StringComparer.Ordinal);
		private readonly Dictionary<uint, string> stringHeapLookup2 = new();

		public ClrMetadataCache()
		{
			Modules = new Dictionary<string, MosaModule?>();
		}

		public void AddModule(MosaModule? module)
		{
			Modules.Add(module?.Name, module);

			//var desc = module.GetUnderlyingObject<UnitDesc<ModuleDef, object>>();
		}

		public MosaModule GetModuleByName(string name)
		{
			if (Modules.TryGetValue(name, out var result))
				return result;

			throw new CompilerException();
		}

		public void AddType(MosaType? type)
		{
			typeLookup.Add(type.GetUnderlyingObject<UnitDesc<TypeDef, TypeSig>>().Token, type);
		}

		public MosaType GetTypeByToken(ScopedToken token)
		{
			return typeLookup[token];
		}

		public void AddMethod(MosaMethod? method)
		{
			methodLookup.Add(method.GetUnderlyingObject<UnitDesc<MethodDef, MethodSig>>().Token, method);
		}

		public MosaMethod GetMethodByToken(ScopedToken token)
		{
			return methodLookup[token];
		}

		public void AddField(MosaField? field)
		{
			fieldLookup.Add(field.GetUnderlyingObject<UnitDesc<FieldDef, FieldSig>>().Token, field);
		}

		public MosaField GetFieldByToken(ScopedToken token)
		{
			return fieldLookup[token];
		}

		public void AddProperty(MosaProperty? property)
		{
			propertyLookup.Add(property.GetUnderlyingObject<UnitDesc<PropertyDef, PropertySig>>().Token, property);
		}

		public MosaProperty GetPropertyByToken(ScopedToken token)
		{
			return propertyLookup[token];
		}

		public uint GetStringId(string value)
		{
			if (!stringHeapLookup.TryGetValue(value, out var id))
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
