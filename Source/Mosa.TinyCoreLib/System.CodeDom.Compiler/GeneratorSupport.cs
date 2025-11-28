namespace System.CodeDom.Compiler;

[Flags]
public enum GeneratorSupport
{
	ArraysOfArrays = 1,
	EntryPointMethod = 2,
	GotoStatements = 4,
	MultidimensionalArrays = 8,
	StaticConstructors = 0x10,
	TryCatchStatements = 0x20,
	ReturnTypeAttributes = 0x40,
	DeclareValueTypes = 0x80,
	DeclareEnums = 0x100,
	DeclareDelegates = 0x200,
	DeclareInterfaces = 0x400,
	DeclareEvents = 0x800,
	AssemblyAttributes = 0x1000,
	ParameterAttributes = 0x2000,
	ReferenceParameters = 0x4000,
	ChainedConstructorArguments = 0x8000,
	NestedTypes = 0x10000,
	MultipleInterfaceMembers = 0x20000,
	PublicStaticMembers = 0x40000,
	ComplexExpressions = 0x80000,
	Win32Resources = 0x100000,
	Resources = 0x200000,
	PartialTypes = 0x400000,
	GenericTypeReference = 0x800000,
	GenericTypeDeclaration = 0x1000000,
	DeclareIndexerProperties = 0x2000000
}
