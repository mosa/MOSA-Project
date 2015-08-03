// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.MosaTypeSystem
{
	public interface IMetadata
	{
		void Initialize(TypeSystem system, ITypeSystemController controller);

		void LoadMetadata();

		string LookupUserString(MosaModule module, uint id);
	}
}