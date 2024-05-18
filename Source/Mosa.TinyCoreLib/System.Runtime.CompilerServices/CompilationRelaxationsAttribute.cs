namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Module | AttributeTargets.Class | AttributeTargets.Method)]
public class CompilationRelaxationsAttribute : Attribute
{
	public int CompilationRelaxations
	{
		get
		{
			throw null;
		}
	}

	public CompilationRelaxationsAttribute(int relaxations)
	{
	}

	public CompilationRelaxationsAttribute(CompilationRelaxations relaxations)
	{
	}
}
