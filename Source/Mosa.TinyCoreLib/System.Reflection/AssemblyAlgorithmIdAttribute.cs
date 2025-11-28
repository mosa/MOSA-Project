using System.Configuration.Assemblies;

namespace System.Reflection;

[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
public sealed class AssemblyAlgorithmIdAttribute : Attribute
{
	[CLSCompliant(false)]
	public uint AlgorithmId
	{
		get
		{
			throw null;
		}
	}

	public AssemblyAlgorithmIdAttribute(System.Configuration.Assemblies.AssemblyHashAlgorithm algorithmId)
	{
	}

	[CLSCompliant(false)]
	public AssemblyAlgorithmIdAttribute(uint algorithmId)
	{
	}
}
