// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.MosaTypeSystem;

public interface ITypeResolver
{
	void AddType(Tuple<MosaModule, string> key, MosaType? value);

	MosaType ResolveType(MosaModule module, BuiltInType type);

	MosaType ResolveType(MosaModule module, MosaTypeCode type);

	MosaType? GetTypeByName(IList<MosaModule> modules, string fullName);

	MosaType? GetTypeByName(MosaModule module, string fullName);
}
