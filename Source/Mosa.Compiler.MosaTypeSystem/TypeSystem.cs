// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using System.Collections.Generic;

namespace Mosa.Compiler.MosaTypeSystem
{
	/// <summary>
	///
	/// </summary>
	public class TypeSystem
	{
		public BuiltInTypes BuiltIn { get; private set; }

		private MosaModule corLib;

		public MosaModule CorLib
		{
			get { return corLib; }
			set
			{
				corLib = value;
				BuiltIn = new BuiltInTypes(this, corLib);
			}
		}

		public IEnumerable<MosaType> AllTypes
		{
			get
			{
				foreach (var module in Modules)
				{
					foreach (var type in module.Types.Values)
					{
						yield return type;
					}
				}
			}
		}

		public IList<MosaModule> Modules { get; private set; }

		public MosaModule LinkerModule { get; private set; }

		public MosaType DefaultLinkerType { get; private set; }

		public MosaMethod EntryPoint { get; private set; }

		internal ITypeSystemController Controller { get; private set; }

		private IMetadata metadata;

		private TypeSystem(IMetadata metadata)
		{
			this.metadata = metadata;
			Modules = new List<MosaModule>();
		}

		public static TypeSystem Load(IMetadata metadata)
		{
			TypeSystem result = new TypeSystem(metadata);
			result.Load();
			return result;
		}

		private void Load()
		{
			Controller = new TypeSystemController(this);

			LinkerModule = Controller.CreateModule();
			using (var module = Controller.MutateModule(LinkerModule))
			{
				module.Name = "@Linker";
				module.Assembly = "@Linker";
				module.IsLinkerGenerated = true;
			}
			Modules.Add(LinkerModule);

			DefaultLinkerType = Controller.CreateType();
			using (var type = Controller.MutateType(DefaultLinkerType))
			{
				type.Module = LinkerModule;
				type.Name = "Default";
				type.IsLinkerGenerated = true;
				type.TypeCode = MosaTypeCode.ReferenceType;
			}
			Controller.AddType(DefaultLinkerType);

			metadata.Initialize(this, Controller);
			metadata.LoadMetadata();

			if (CorLib == null)
				throw new AssemblyLoadException();
		}

		private Dictionary<Tuple<MosaModule, string, string>, MosaType> typeLookup = new Dictionary<Tuple<MosaModule, string, string>, MosaType>();

		public MosaType GetTypeByName(string @namespace, string name)
		{
			foreach (var module in Modules)
			{
				MosaType result;
				if (typeLookup.TryGetValue(Tuple.Create(module, @namespace, name), out result))
					return result;
			}

			return null;
		}

		public MosaType GetTypeByName(MosaModule module, string @namespace, string name)
		{
			MosaType result;
			if (typeLookup.TryGetValue(Tuple.Create(module, @namespace, name), out result))
				return result;

			return null;
		}

		public MosaModule GetModuleByAssembly(string name)
		{
			foreach (var module in Modules)
			{
				if (module.Assembly == name)
					return module;
			}

			return null;
		}

		public string LookupUserString(MosaModule module, uint token)
		{
			return metadata.LookupUserString(module, token);
		}

		public MosaMethod CreateLinkerMethod(string methodName, MosaType returnType, IList<MosaParameter> parameters)
		{
			if (parameters == null)
				parameters = new List<MosaParameter>();

			MosaMethod result = Controller.CreateMethod();
			using (var mosaType = Controller.MutateType(DefaultLinkerType))
				mosaType.Methods.Add(result);
			using (var mosaMethod = Controller.MutateMethod(result))
			{
				mosaMethod.Module = LinkerModule;
				mosaMethod.DeclaringType = DefaultLinkerType;
				mosaMethod.Name = methodName;
				mosaMethod.Signature = new MosaMethodSignature(returnType, parameters);

				mosaMethod.IsStatic = true;
				mosaMethod.HasThis = false;
				mosaMethod.HasExplicitThis = false;
				mosaMethod.IsLinkerGenerated = true;

				return result;
			}
		}

		private class TypeSystemController : ITypeSystemController
		{
			private TypeSystem typeSystem;
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

			public MosaType CreateType(MosaType source = null)
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
					MosaType result = source.Clone();
					result.ID = id++;
					return result;
				}
			}

			public MosaMethod CreateMethod(MosaMethod source = null)
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
					MosaMethod result = source.Clone();
					result.ID = id++;
					return result;
				}
			}

			public MosaField CreateField(MosaField source = null)
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
					MosaField result = source.Clone();
					result.ID = id++;
					return result;
				}
			}

			public MosaProperty CreateProperty(MosaProperty source = null)
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
					MosaProperty result = source.Clone();
					result.ID = id++;
					return result;
				}
			}

			public MosaParameter CreateParameter(MosaParameter source = null)
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
					MosaParameter result = source.Clone();
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

			public MosaParameter.Mutator MutateParameter(MosaParameter Parameter)
			{
				return new MosaParameter.Mutator(Parameter);
			}

			public void AddModule(MosaModule module)
			{
				typeSystem.Modules.Add(module);
			}

			public void AddType(MosaType type)
			{
				if (!type.Module.Types.ContainsKey(type.ID))
					type.Module.Types.Add(type.ID, type);

				if (string.IsNullOrEmpty(type.Signature))   // i.e. elementary types
					typeSystem.typeLookup[Tuple.Create(type.Module, type.Namespace, type.ShortName)] = type;
				else
					typeSystem.typeLookup[Tuple.Create(type.Module, type.Namespace, type.ShortName + type.Signature)] = type;
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
}
