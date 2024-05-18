namespace System.Reflection;

[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
public sealed class AssemblyFlagsAttribute : Attribute
{
	public int AssemblyFlags
	{
		get
		{
			throw null;
		}
	}

	[CLSCompliant(false)]
	[Obsolete("AssemblyFlagsAttribute.Flags has been deprecated. Use AssemblyFlags instead.")]
	public uint Flags
	{
		get
		{
			throw null;
		}
	}

	[Obsolete("This constructor has been deprecated. Use AssemblyFlagsAttribute(AssemblyNameFlags) instead.")]
	public AssemblyFlagsAttribute(int assemblyFlags)
	{
	}

	public AssemblyFlagsAttribute(AssemblyNameFlags assemblyFlags)
	{
	}

	[CLSCompliant(false)]
	[Obsolete("This constructor has been deprecated. Use AssemblyFlagsAttribute(AssemblyNameFlags) instead.")]
	public AssemblyFlagsAttribute(uint flags)
	{
	}
}
