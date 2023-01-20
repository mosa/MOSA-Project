using System;

namespace Mosa.Compiler.MosaTypeSystem
{
	internal class TypeSystemController : ITypeSystemController
	{
		private readonly TypeSystem typeSystem;
		private uint id = 1;

		public TypeSystemController(TypeSystem typeSystem)
		{
			this.typeSystem = typeSystem;
		}

		public MosaModule CreateModule()
		{
			return new MosaModule()
			{
				ID = id++,
				TypeSystem = typeSystem
			};
		}

		public MosaType CreateType(MosaType? source = null)
		{
			if (source == null)
			{
				return new MosaType()
				{
					ID = id++,
					TypeSystem = typeSystem
				};
			}
			else
			{
				var result = source.Clone();
				result.ID = id++;
				return result;
			}
		}

		public MosaMethod CreateMethod(MosaMethod? source = null)
		{
			if (source == null)
			{
				return new MosaMethod()
				{
					ID = id++,
					TypeSystem = typeSystem
				};
			}
			else
			{
				var result = source.Clone();
				result.ID = id++;
				return result;
			}
		}

		public MosaField CreateField(MosaField? source = null)
		{
			if (source == null)
			{
				return new MosaField()
				{
					ID = id++,
					TypeSystem = typeSystem
				};
			}
			else
			{
				var result = source.Clone();
				result.ID = id++;
				return result;
			}
		}

		public MosaProperty CreateProperty(MosaProperty? source = null)
		{
			if (source == null)
			{
				return new MosaProperty()
				{
					ID = id++,
					TypeSystem = typeSystem
				};
			}
			else
			{
				var result = source.Clone();
				result.ID = id++;
				return result;
			}
		}

		public MosaParameter CreateParameter(MosaParameter? source = null)
		{
			if (source == null)
			{
				return new MosaParameter()
				{
					ID = id++,
					TypeSystem = typeSystem
				};
			}
			else
			{
				var result = source.Clone();
				result.ID = id++;
				return result;
			}
		}

		public MosaModule.Mutator MutateModule(MosaModule module)
		{
			return new MosaModule.Mutator(module);
		}

		public MosaType.Mutator MutateType(MosaType type)
		{
			return new MosaType.Mutator(type);
		}

		public MosaMethod.Mutator MutateMethod(MosaMethod method)
		{
			return new MosaMethod.Mutator(method);
		}

		public MosaField.Mutator MutateField(MosaField field)
		{
			return new MosaField.Mutator(field);
		}

		public MosaProperty.Mutator MutateProperty(MosaProperty property)
		{
			return new MosaProperty.Mutator(property);
		}

		public MosaParameter.Mutator MutateParameter(MosaParameter parameter)
		{
			return new MosaParameter.Mutator(parameter);
		}

		public void AddModule(MosaModule module)
		{
			typeSystem.Modules.Add(module);
		}

		public void AddType(MosaType type)
		{
			if (type.Module == null || type.FullName == null)
				throw new InvalidOperationException("Type's module or full name is null!");

			if (!type.Module.Types.ContainsKey(type.ID))
				type.Module.Types.Add(type.ID, type);

			typeSystem.TypeResolver.AddType(Tuple.Create(type.Module, type.FullName), type);
		}

		public void SetCorLib(MosaModule module)
		{
			typeSystem.CorLib = module;
		}

		public void SetEntryPoint(MosaMethod entryPoint)
		{
			typeSystem.EntryPoint = entryPoint;
		}
	}
}
