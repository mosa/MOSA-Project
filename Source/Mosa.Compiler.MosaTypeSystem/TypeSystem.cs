// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.MosaTypeSystem.Metadata;
using System;
using System.Collections.Generic;

namespace Mosa.Compiler.MosaTypeSystem
{
	/// <summary>
	/// Type System
	/// </summary>
	public class TypeSystem
	{
		public BuiltInTypes BuiltIn { get; private set; }

		private MosaModule corLib;

		public MosaModule CorLib
		{
			get
			{
				return corLib;
			}
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

		public IList<MosaModule> Modules { get; }

		public MosaModule LinkerModule { get; private set; }

		public MosaType DefaultLinkerType { get; private set; }

		public MosaMethod EntryPoint { get; private set; }

		internal ITypeSystemController Controller { get; private set; }

		internal readonly CLRMetadata metadata;

		private TypeSystem(CLRMetadata metadata)
		{
			this.metadata = metadata;
			Modules = new List<MosaModule>();
		}

		public static TypeSystem Load(IMetadata metadata)
		{
			var result = new TypeSystem(metadata as CLRMetadata);
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
				module.IsCompilerGenerated = true;
			}
			Modules.Add(LinkerModule);

			DefaultLinkerType = Controller.CreateType();

			using (var type = Controller.MutateType(DefaultLinkerType))
			{
				type.Module = LinkerModule;
				type.Name = "Default";
				type.IsCompilerGenerated = true;
				type.TypeCode = MosaTypeCode.ReferenceType;
			}
			Controller.AddType(DefaultLinkerType);

			metadata.Initialize(this, Controller);
			metadata.LoadMetadata();

			if (CorLib == null)
				throw new AssemblyLoadException();
		}

		private readonly Dictionary<Tuple<MosaModule, string, string>, MosaType> typeLookup = new Dictionary<Tuple<MosaModule, string, string>, MosaType>();

		public MosaType GetTypeByName(string @namespace, string name)
		{
			foreach (var module in Modules)
			{
				if (typeLookup.TryGetValue(Tuple.Create(module, @namespace, name), out MosaType result))
					return result;
			}

			return null;
		}

		public MosaType GetTypeByName(MosaModule module, string @namespace, string name)
		{
			if (typeLookup.TryGetValue(Tuple.Create(module, @namespace, name), out MosaType result))
				return result;

			return null;
		}

		/// <summary>
		/// Get a type by fullName
		/// </summary>
		/// <param name="fullName">fullName like namespace.typeName</param>
		public MosaType GetTypeByName(string fullName)
		{
			if (string.IsNullOrEmpty(fullName))
				return null;

			string ns;
			string typeName;

			var indexOfLastDot = fullName.LastIndexOf('.');
			if (indexOfLastDot == -1)
			{
				// type is in no namespace
				ns = "";
				typeName = fullName;
			}
			else
			{
				ns = fullName.Substring(0, indexOfLastDot);
				typeName = fullName.Substring(indexOfLastDot + 1);
			}

			return GetTypeByName(ns, typeName);
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

		public MosaMethod CreateLinkerMethod(string methodName, MosaType returnType, bool hasThis = false, IList<MosaParameter> parameters = null)
		{
			return CreateLinkerMethod(DefaultLinkerType, methodName, returnType, hasThis, parameters);
		}

		public MosaMethod CreateLinkerMethod(MosaType type, string methodName, MosaType returnType, bool hasThis = false, IList<MosaParameter> parameters = null)
		{
			if (parameters == null)
				parameters = new List<MosaParameter>();

			var result = Controller.CreateMethod();

			using (var mosaType = Controller.MutateType(type))
			{
				mosaType.Methods.Add(result);
			}

			using (var mosaMethod = Controller.MutateMethod(result))
			{
				mosaMethod.Module = LinkerModule;
				mosaMethod.DeclaringType = DefaultLinkerType;
				mosaMethod.Name = methodName;
				mosaMethod.Signature = new MosaMethodSignature(returnType, parameters);
				mosaMethod.IsStatic = true;
				mosaMethod.HasThis = hasThis;
				mosaMethod.HasExplicitThis = false;
				mosaMethod.IsCompilerGenerated = true;

				return result;
			}
		}

		/// <summary>
		/// Creates the type of the linker.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		public MosaType CreateLinkerType(string name)
		{
			var mosaType = Controller.CreateType();

			using (var type = Controller.MutateType(mosaType))
			{
				type.Module = LinkerModule;
				type.Name = name;
				type.IsCompilerGenerated = true;
				type.TypeCode = MosaTypeCode.ReferenceType;
			}
			return mosaType;
		}

		public MosaParameter CreateParameter(MosaMethod method, string name, MosaType parameterType)
		{
			var mosaParameter = Controller.CreateParameter();

			using (var parameter = Controller.MutateParameter(mosaParameter))
			{
				parameter.Name = name;
				parameter.ParameterAttributes = MosaParameterAttributes.In;
				parameter.ParameterType = parameterType;
				parameter.DeclaringMethod = method;

				return mosaParameter;
			}
		}

		private class TypeSystemController : ITypeSystemController
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
					var result = source.Clone();
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
					var result = source.Clone();
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
					var result = source.Clone();
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
					var result = source.Clone();
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
				if (!type.Module.Types.ContainsKey(type.ID))
				{
					type.Module.Types.Add(type.ID, type);
				}

				typeSystem.typeLookup[Tuple.Create(type.Module, type.Namespace, type.ShortName)] = type;
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
