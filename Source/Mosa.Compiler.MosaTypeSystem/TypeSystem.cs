// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics.CodeAnalysis;
using Mosa.Compiler.Common.Exceptions;

namespace Mosa.Compiler.MosaTypeSystem;

/// <summary>
/// Type System
/// </summary>
public class TypeSystem
{
	[NotNull]
	public BuiltInTypes? BuiltIn { get; private set; }

	private MosaModule? corLib;

	[NotNull]
	public MosaModule? CorLib
	{
		get
		{
			if (corLib == null)
				throw new AssemblyLoadException();

			return corLib;
		}
		set
		{
			corLib = value;
			if (corLib == null)
				throw new AssemblyLoadException();

			BuiltIn = new BuiltInTypes(TypeResolver, corLib);
		}
	}

	public IEnumerable<MosaType> AllTypes
	{
		get
		{
			foreach (var module in Modules)
			{
				var types = module?.Types.Values;
				if (types == null)
					throw new InvalidOperationCompilerException("Types list is null");

				foreach (var type in types)
				{
					yield return type;
				}
			}
		}
	}

	public IList<MosaModule> Modules { get; }

	[NotNull]
	public MosaModule? LinkerModule { get; private set; }

	[NotNull]
	public MosaType? DefaultLinkerType { get; private set; }

	public MosaMethod? EntryPoint { get; internal set; }

	[NotNull]
	public ITypeSystemController? Controller { get; private set; }

	public ITypeResolver TypeResolver { get; }

	private readonly IMetadata metadata;

	private TypeSystem(IMetadata metadata, ITypeResolver typeResolver)
	{
		this.metadata = metadata;
		TypeResolver = typeResolver;
		Modules = new List<MosaModule>();
	}

	public static TypeSystem Load(IMetadata metadata, ITypeResolver typeResolver)
	{
		var result = new TypeSystem(metadata, typeResolver);
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

	/// <summary>
	/// Get a type by fullName
	/// </summary>
	/// <param name="fullName">fullName like namespace.typeName</param>
	public MosaType? GetTypeByName(string fullName)
	{
		return string.IsNullOrEmpty(fullName) ? null : TypeResolver.GetTypeByName(Modules, fullName);
	}

	public MosaType? GetTypeByName(MosaModule module, string fullName)
	{
		return string.IsNullOrEmpty(fullName) ? null : TypeResolver.GetTypeByName(module, fullName);
	}

	public MosaModule? GetModuleByAssembly(string name)
	{
		foreach (var module in Modules)
		{
			if (module?.Assembly == name)
				return module;
		}

		return null;
	}

	public string? LookupUserString(MosaModule module, uint token)
	{
		return metadata.LookupUserString(module, token);
	}

	public MosaMethod CreateLinkerMethod(string methodName, MosaType returnType, bool hasThis = false, IList<MosaParameter>? parameters = null)
	{
		return CreateLinkerMethod(DefaultLinkerType, methodName, returnType, hasThis, parameters);
	}

	public MosaMethod CreateLinkerMethod(MosaType type, string methodName, MosaType returnType, bool hasThis = false, IList<MosaParameter>? parameters = null)
	{
		parameters ??= new List<MosaParameter>();

		var result = Controller.CreateMethod();

		using (var mosaType = Controller.MutateType(type))
		{
			mosaType.Methods?.Add(result);
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

		using var type = Controller.MutateType(mosaType);
		type.Module = LinkerModule;
		type.Name = name;
		type.IsCompilerGenerated = true;
		type.TypeCode = MosaTypeCode.ReferenceType;
		return mosaType;
	}

	public MosaParameter CreateParameter(MosaMethod method, string name, MosaType parameterType)
	{
		var mosaParameter = Controller.CreateParameter();

		using var parameter = Controller.MutateParameter(mosaParameter);
		parameter.Name = name;
		parameter.ParameterAttributes = MosaParameterAttributes.In;
		parameter.ParameterType = parameterType;
		parameter.DeclaringMethod = method;

		return mosaParameter;
	}
}
