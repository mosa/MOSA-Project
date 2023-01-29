// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics.CodeAnalysis;
using Mosa.Compiler.Common.Exceptions;

namespace Mosa.Compiler.MosaTypeSystem.CLR.Metadata;

internal class ClrMetadata : IMetadata
{
	private readonly ClrModuleLoader moduleLoader;

	public ClrMetadata(ClrModuleLoader loader)
	{
		moduleLoader = loader;

		Cache = new ClrMetadataCache();
		Loader = new ClrMetadataLoader(this);
		Resolver = new ClrMetadataResolver(this);
	}

	[NotNull]
	public TypeSystem? TypeSystem { get; private set; }

	[NotNull]
	public ITypeSystemController? Controller { get; private set; }

	public ClrMetadataCache Cache { get; }

	public ClrMetadataLoader Loader { get; }

	public ClrMetadataResolver Resolver { get; }

	public void Initialize(TypeSystem system, ITypeSystemController controller)
	{
		TypeSystem = system;
		Controller = controller;
	}

	public void LoadMetadata()
	{
		foreach (var module in moduleLoader.Modules)
		{
			Loader.Load(module);
		}

		if (Loader.CorLib == null)
			throw new AssemblyLoadException();

		Controller.SetCorLib(Loader.CorLib);

		Resolver.Resolve();

		var modules = Cache?.Modules.Values;
		if (modules == null)
			throw new InvalidOperationException("Modules list is empty!");

		foreach (var module in modules)
		{
			if (module.EntryPoint != null)
			{
				Controller.SetEntryPoint(module.EntryPoint);
				break;
			}
		}
	}

	public string? LookupUserString(MosaModule module, uint id)
	{
		return Cache?.GetStringById(id);
	}
}
