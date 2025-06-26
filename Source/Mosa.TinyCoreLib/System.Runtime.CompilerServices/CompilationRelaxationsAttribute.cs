namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Module | AttributeTargets.Class | AttributeTargets.Method)]
public class CompilationRelaxationsAttribute(int relaxations) : Attribute
{
	public int CompilationRelaxations { get; } = relaxations;

	public CompilationRelaxationsAttribute(CompilationRelaxations relaxations)
		: this((int)relaxations) {}
}
