// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;

namespace Mosa.Compiler.MosaTypeSystem;

public interface IModuleLoader : IDisposable
{
	public void AddSearchPaths(List<string>? paths)
	{
		if (paths == null)
			return;

		foreach (var path in paths)
		{
			AddSearchPath(path);
		}
	}

	/// <summary>
	/// Loads the module from files.
	/// </summary>
	/// <param name="files">The files.</param>
	public void LoadModuleFromFiles(IList<string> files)
	{
		foreach (var file in files)
		{
			LoadModuleFromFile(file);
		}
	}

	/// <summary>
	/// Loads the module.
	/// </summary>
	/// <param name="file">The file path of the module to load.</param>
	void LoadModuleFromFile(string file);

	/// <summary>
	/// Appends the given path to the assembly search path.
	/// </summary>
	/// <param name="path">The path to append to the assembly search path.</param>
	void AddSearchPath(string path);

	IMetadata CreateMetadata();
}
