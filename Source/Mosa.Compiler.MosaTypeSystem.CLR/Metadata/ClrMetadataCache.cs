// Copyright (c) MOSA Project. Licensed under the New BSD License.

using dnlib.DotNet;
using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.MosaTypeSystem.CLR.Utils;
using Mosa.Compiler.MosaTypeSystem.Units;

namespace Mosa.Compiler.MosaTypeSystem.CLR.Metadata
{
	public class ClrMetadataCache
	{
		public Dictionary<string, MosaModule> Modules { get; }

		private readonly Dictionary<ScopedToken, MosaType> typeLookup = new();
		private readonly Dictionary<ScopedToken, MosaMethod> methodLookup = new();
		private readonly Dictionary<ScopedToken, MosaField> fieldLookup = new();
		private readonly Dictionary<ScopedToken, MosaProperty> propertyLookup = new();

		private uint stringIdCounter;
		private readonly Dictionary<uint, string> stringHeapLookup = new();

		public ClrMetadataCache()
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
			if (Modules.TryGetValue(name, out var result))
				return result;

			throw new CompilerException();
		}

		public void AddType(MosaType type)
		{
			var unitDesc = type.GetUnderlyingObject<UnitDesc<TypeDef, TypeSig>>();
			if (unitDesc == null)
				throw new InvalidOperationException("Underlying object (unit description) of type is null!");

			typeLookup.Add(unitDesc.Token, type);
		}

		public MosaType GetTypeByToken(ScopedToken token)
		{
			return typeLookup[token];
		}

		public void AddMethod(MosaMethod method)
		{
			var unitDesc = method.GetUnderlyingObject<UnitDesc<MethodDef, MethodSig>>();
			if (unitDesc == null)
				throw new InvalidOperationException("Underlying object (unit description) of method is null!");

			methodLookup.Add(unitDesc.Token, method);
		}

		public MosaMethod GetMethodByToken(ScopedToken token)
		{
			return methodLookup[token];
		}

		public void AddField(MosaField field)
		{
			var unitDesc = field.GetUnderlyingObject<UnitDesc<FieldDef, FieldSig>>();
			if (unitDesc == null)
				throw new InvalidOperationException("Underlying object (unit description) of field is null!");

			fieldLookup.Add(unitDesc.Token, field);
		}

		public MosaField GetFieldByToken(ScopedToken token)
		{
			return fieldLookup[token];
		}

		public void AddProperty(MosaProperty property)
		{
			var unitDesc = property.GetUnderlyingObject<UnitDesc<PropertyDef, PropertySig>>();
			if (unitDesc == null)
				throw new InvalidOperationException("Underlying object (unit description) of property is null!");

			propertyLookup.Add(unitDesc.Token, property);
		}

		public MosaProperty GetPropertyByToken(ScopedToken token)
		{
			return propertyLookup[token];
		}

		public uint GetStringId(string value)
		{
			uint id = 0;

			var found = false;

			foreach (var val in stringHeapLookup)
				if (val.Value == value)
				{
					id = val.Key;
					found = true;
					break;
				}

			if (!found)
			{
				id = stringIdCounter++;
				stringHeapLookup[id] = value;
			}

			return id;
		}

		public string GetStringById(uint id)
		{
			return stringHeapLookup[id];
		}
	}
}
