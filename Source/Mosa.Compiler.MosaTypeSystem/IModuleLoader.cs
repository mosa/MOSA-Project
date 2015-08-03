// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.MosaTypeSystem
{
	public interface IModuleLoader
	{
		void LoadModuleFromFile(string file);

		IMetadata CreateMetadata();
	}
}