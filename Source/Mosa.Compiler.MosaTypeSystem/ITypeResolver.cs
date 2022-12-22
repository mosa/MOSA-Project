namespace Mosa.Compiler.MosaTypeSystem;

public interface ITypeResolver
{
	MosaType ResolveType(TypeSystem typeSystem, MosaModule module, BuiltInType type);
}
